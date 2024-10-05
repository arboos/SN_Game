using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPreset : MonoBehaviour
{
	public int currentTurnIndex;
	public List<Turn> presets;
	public List<GameObject> playedCards;
	[SerializeField] private PlayerProperties player;

	public int HP;
	public int MaxHP;
	public float DamageResistance;

	[SerializeField] private Image HPBar;
	[SerializeField] private int winReward;
	public TextMeshProUGUI outputField;
	[SerializeField] private HonestReactions honestReaction;
	[SerializeField] private GameObject nextTurn;

	public bool isDead = false;

	private void Start()
	{
		UpdateViewModels();
	}


	[Serializable]
	public struct Turn
	{
		public GameObject[] cards;
	}

	public void TakeDamage(int damage,float resistancePenetration)
	{
		if (damage > 0)
		{
			honestReaction.Shake(1);
			honestReaction.PlayAngry();
		}
		if (DamageResistance != 0)
		{
			DamageResistance = Mathf.Clamp(DamageResistance - resistancePenetration,-100,100);
			HP -= (int)(damage * DamageResistance);
			if (HP - (int)(damage * DamageResistance) <= 0)
			{
				HP = 0;
				UpdateViewModels();
				Debug.Log("L");
				honestReaction.Shake(5);
				Die();
				return;
			}
		}
		if (HP - damage <= 0)
		{
			HP = 0;
			UpdateViewModels();
			Debug.Log("L");
			Die();
			return;
		}
		HP -= damage;
		UpdateViewModels();
	}

	private void UpdateViewModels()
	{
		HPBar.fillAmount = HP / (float)MaxHP;
		CardPlacementSystem.Instance.textHP_Enemy.text = HP.ToString();
	}

	private void Die()
	{
		CardPlacementSystem.Instance.shop.gameObject.SetActive(true);
		CardPlacementSystem.Instance.shop.Open();
		MenuManager.Instance.winScreen.SetActive(true);
		PlayerProperties.Instance.fame += winReward;
		StopAllCoroutines();
		transform.parent.gameObject.SetActive(false);
	}

	public IEnumerator TakeTurn()
	{
		if (currentTurnIndex >= presets.Count) currentTurnIndex = 0;

		Turn turn = presets[currentTurnIndex];

		int damage = 0;
		float damageResistance = 0;
		float heal = 0;

		yield return new WaitForSeconds(1f);
		foreach (var cardObject in turn.cards)
		{
			playedCards.Add(Instantiate(cardObject, CardPlacementSystem.Instance.playboard.transform));
			outputField.text = "Enemy:\n";
			CardInfo card = cardObject.GetComponent<CardInfo>();
			damage += card.Damage;
			if (card.DamageResistance != 0)
			{
				damageResistance += (card.DamageResistance / 100f);
			}
			honestReaction.PlayNeutral();
			heal += card.Heal;
			if (damage > 0)
			{
				outputField.text += "Damage: " + damage.ToString() + "\n";
				honestReaction.PlayHappy();
			}
			honestReaction.PlayNeutral();
			if (damageResistance != 1)
			{
				outputField.text += "Damage resistance: " + ((damageResistance) * 100).ToString() + "%\n";
			}
			honestReaction.PlayNeutral();
			if (heal > 0)
			{
				outputField.text += "Healing: " + heal.ToString();
				honestReaction.PlayHappy();
			}
			honestReaction.PlayNeutral();
			yield return new WaitForSeconds(2f);
		}

		PlayerProperties.Instance.TakeDamage(damage);
		DamageResistance = 1 - damageResistance;
		currentTurnIndex++;

		int count = CardPlacementSystem.Instance.playboard.transform.childCount;
		for (int j = 0; j < count; j++)
		{
			Destroy(CardPlacementSystem.Instance.playboard.transform.GetChild(j).gameObject);
		}
		nextTurn.SetActive(true);
		CardPlacementSystem.Instance.turnBlocker.SetActive(false);
	}

}
