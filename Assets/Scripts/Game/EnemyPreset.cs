using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPreset : MonoBehaviour
{
	public int currentTurnIndex;
	public List<Turn> presets;
	public List<GameObject> playedCards;

	public int HP;
	public int MaxHP;
	public int DamageResistance;

	[SerializeField] private Image HPBar;
	[SerializeField] private int winReward;
	public TextMeshProUGUI outputField;
	[SerializeField] private HonestReactions honestReaction;
	[SerializeField] private GameObject nextTurn;

	[SerializeField] private Transform cardSpawnPosition;

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
		CardPlacementSystem.Instance.textHP_Enemy.text = "Слава: "+ HP.ToString() +"\nЗащита: "+DamageResistance;
	}

	private async void Die()
	{
		StopAllCoroutines();
		isDead = true;
		await UniTask.Delay(TimeSpan.FromSeconds(2f));
		
		outputField.gameObject.SetActive(false);
		GameObject.Find("TurnManager").GetComponent<TurnManager>().outputField.gameObject.SetActive(false);

		CardPlacementSystem.Instance.endTurnGO.SetActive(false);
		CardPlacementSystem.Instance.nextTurnGO.SetActive(false);
		
		CardPlacementSystem.Instance.hand.SetActive(false);
		CardPlacementSystem.Instance.playboard.SetActive(false);
		
		CardPlacementSystem.Instance.playerDialog.gameObject.SetActive(true);
		CardPlacementSystem.Instance.playerDialog.StartText(CardPlacementSystem.Instance.playerPhrasesWin);

		await UniTask.Delay(TimeSpan.FromSeconds(CardPlacementSystem.Instance.playerPhrasesWin[0].Length * 0.1f + 2f));
		
		CardPlacementSystem.Instance.enemyDialog.gameObject.SetActive(true);
		CardPlacementSystem.Instance.enemyDialog.StartText(CardPlacementSystem.Instance.enemyPhrasesWin);

		await UniTask.Delay(TimeSpan.FromSeconds(CardPlacementSystem.Instance.enemyPhrasesWin[0].Length * 0.1f + 2f));
		
		PlayerProperties.Instance.fame += winReward;
		CardPlacementSystem.Instance.shop.gameObject.SetActive(true);
		CardPlacementSystem.Instance.shop.Open();
		MenuManager.Instance.winScreen.SetActive(true);
		PlayerProperties.Instance.audioSource.PlayOneShot(PlayerProperties.Instance.win);
		
		transform.parent.gameObject.SetActive(false);
	}
	
	public async UniTask MoveCard(GameObject card,  Vector3 destination)
	{
		Tween move = card.transform.DOMove(destination, 0.5f);
		await move.ToUniTask();
	}

	public IEnumerator TakeTurn()
	{
		if (currentTurnIndex >= presets.Count) currentTurnIndex = 0;

		Turn turn = presets[currentTurnIndex];

		int damage = 0;
		int damageResistance = 0;
		float heal = 0;
		if (isDead) { yield return null; }
		yield return new WaitForSeconds(0.5f);
		foreach (var cardObject in turn.cards)
		{
			if (isDead) { break; }
			var cardInstance = Instantiate(cardObject, cardSpawnPosition);
			
			float xPos = 90f;
			Vector3 movePos = new Vector3();

        
			movePos = CardPlacementSystem.Instance.playboard.transform.position + new Vector3((CardPlacementSystem.Instance.playboardDeck.cardsInDeck.Count+1) * 100f, 0, 0);


			MoveCard(cardInstance, movePos);
			yield return new WaitForSeconds(0.5f);
			
			cardInstance.transform.SetParent(CardPlacementSystem.Instance.playboard.transform);
			
			cardInstance.transform.GetChild(0).gameObject.SetActive(true);
			playedCards.Add(cardInstance);
			outputField.text = "Враг:\n";
			CardInfo card = cardObject.GetComponent<CardInfo>();
			damage += card.Damage;
			if (card.DamageResistance != 0)
			{
				damageResistance += card.DamageResistance;
			}
			honestReaction.PlayNeutral();
			if (damage > 0)
			{
				outputField.text += "Урон: " + damage.ToString() + "\n";
				honestReaction.PlayHappy();
			}
			honestReaction.PlayNeutral();
			if (damageResistance != 1)
			{
				outputField.text += "Защита: " + (damageResistance).ToString() + "\n";
			}
			honestReaction.PlayNeutral();
			if (heal > 0)
			{
				outputField.text += "Лечение: " + heal.ToString();
				honestReaction.PlayHappy();
			}
			honestReaction.PlayNeutral();
			yield return new WaitForSeconds(1f);
		}

		PlayerProperties.Instance.TakeDamage(damage);
		DamageResistance = damageResistance;
		currentTurnIndex++;

		yield return new WaitForSeconds(1f);
		
		int count = CardPlacementSystem.Instance.playboard.transform.childCount;
		for (int j = 0; j < count; j++)
		{
			MoveCard(CardPlacementSystem.Instance.playboard.transform.GetChild(0).gameObject, cardSpawnPosition.position);
			Destroy(CardPlacementSystem.Instance.playboard.transform.GetChild(0).gameObject, 0.5f);
			yield return new WaitForSeconds(0.6f);
			
		}
		nextTurn.SetActive(true);
		CardPlacementSystem.Instance.turnBlocker.SetActive(false);
	}

}
