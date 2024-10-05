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
	//[SerializeField] private PlayerProperties player;

	public int HP;
	public int MaxHP;
	public int DamageResistance;

	[SerializeField] private Image HPBar;
	[SerializeField] private int winReward;
	public TextMeshProUGUI outputField;
	[SerializeField] private HonestReactions honestReaction;
	[SerializeField] private GameObject nextTurn;

	public bool isDead = false;

	private void Start()
	{
		winReward = HP;
		UpdateViewModels();
	}


	[Serializable]
	public struct Turn
	{
		public GameObject[] cards;
	}

	public void TakeDamage(int damage,int resistancePenetration)
	{
		if (damage > 0)
		{
			honestReaction.Shake(1);
			honestReaction.PlayAngry();
		}
		if (DamageResistance != 0)
		{
			DamageResistance = Mathf.Clamp(DamageResistance - resistancePenetration,-100,100);
			HP -= Mathf.Clamp(damage - DamageResistance,0,1000);
			UpdateViewModels();
			if (HP - Mathf.Clamp(damage - DamageResistance,0,1000) <= 0)
			{
				HP = 0;
				UpdateViewModels();
				Debug.Log("L");
				honestReaction.Shake(5);
				Die();
				return;
			}
			return;
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

	public void UpdateViewModels()
	{
		//HPBar.fillAmount = HP / (float)MaxHP;
		CardPlacementSystem.Instance.textHP_Enemy.text = "�����: "+ HP.ToString() +"\n������: "+DamageResistance;
	}

	private void Die()
	{
		PlayerProperties.Instance.fame += winReward;
		CardPlacementSystem.Instance.shop.gameObject.SetActive(true);
		CardPlacementSystem.Instance.shop.Open();
		MenuManager.Instance.winScreen.SetActive(true);
		PlayerProperties.Instance.audioSource.PlayOneShot(PlayerProperties.Instance.win);
		StopAllCoroutines();
		transform.parent.gameObject.SetActive(false);
	}

	public IEnumerator TakeTurn()
	{
		if (currentTurnIndex >= presets.Count) currentTurnIndex = 0;

		Turn turn = presets[currentTurnIndex];

		int damage = 0;
		int damageResistance = 0;
		float heal = 0;

		yield return new WaitForSeconds(1f);
		foreach (var cardObject in turn.cards)
		{
			var cardInstance = Instantiate(cardObject, CardPlacementSystem.Instance.playboard.transform);
			cardInstance.transform.GetChild(0).gameObject.SetActive(true);
			playedCards.Add(cardInstance);
			outputField.text = "����:\n";
			CardInfo card = cardObject.GetComponent<CardInfo>();
			damage += card.Damage;
			if (card.DamageResistance != 0)
			{
				damageResistance += card.DamageResistance;
			}
			honestReaction.PlayNeutral();
			//heal += card.Heal;
			if (damage > 0)
			{
				outputField.text += "����: " + damage.ToString() + "\n";
				honestReaction.PlayHappy();
			}
			honestReaction.PlayNeutral();
			if (damageResistance != 1)
			{
				outputField.text += "������: " + (damageResistance).ToString() + "\n";
			}
			honestReaction.PlayNeutral();
			if (heal > 0)
			{
				outputField.text += "�������: " + heal.ToString();
				honestReaction.PlayHappy();
			}
			honestReaction.PlayNeutral();
			yield return new WaitForSeconds(2f);
		}

		PlayerProperties.Instance.TakeDamage(damage);
		DamageResistance = damageResistance;
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
