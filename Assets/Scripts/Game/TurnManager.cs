using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	public PlayerProperties player;
	[SerializeField] private EnemyPreset enemy;
	[SerializeField] private CardDeck cardsField;
	[SerializeField] private TextMeshProUGUI outputField;
	[Header("Visual Settings")]
	[SerializeField] private float compilationSpeed;

	private bool CheckCombination(int buffId) //temporary realisation. Possible changes in the future;
	{
		try
		{
			if (cardsField.cardsInDeck[buffId - 1].GetComponent<CardInfo>().CardType + 1
				== cardsField.cardsInDeck[buffId].GetComponent<CardInfo>().CardType)
			{
				return true;
			}
		}
		catch
		{
			
		}
		try
		{
			if (cardsField.cardsInDeck[buffId + 1].GetComponent<CardInfo>().CardType + 1
			== cardsField.cardsInDeck[buffId].GetComponent<CardInfo>().CardType)
			{
				return true;
			}
		}
		catch
		{
			Debug.Log("out of range!");
		}
		return false;
	}


	private IEnumerator CountValues()
	{
		var cards = cardsField.cardsInDeck;

		int Damage = 0;
		int DamageBuff = 0;
		float DamageResistance = 0;
		float DamageResistanceBuff = 0;
		int Heal = 0;
		int HealBuff = 0;
		foreach (GameObject cardObject in cards)
		{

			outputField.text = "Игрок:\n";
			CardInfo card = cardObject.GetComponent<CardInfo>();
			if (card.CardType == 2 || card.CardType == 4)
			{
				if (CheckCombination(cards.IndexOf(cardObject)))
				{
					DamageBuff += card.Damage;
					DamageResistanceBuff += card.DamageResistance;
					HealBuff += card.Heal;
				}
				continue;
			}
			Damage += card.Damage;
			if (card.DamageResistance != 0)
			{
				DamageResistance += (card.DamageResistance / 100f);
			}
			Heal += card.Heal;
			yield return new WaitForSeconds(compilationSpeed);
		}
		player.SetResistance(DamageResistance + DamageResistanceBuff);
		player.Heal(Heal + HealBuff);
		enemy.TakeDamage(Damage + DamageBuff);
		yield return new WaitForSeconds(compilationSpeed);
		CardPlacementSystem.Instance.EndTurn();
	}

	public void Compilate()
	{
		StartCoroutine(CountValues());
	}

	public void PreCompilate()
	{
		var cards = cardsField.cardsInDeck;
		int Damage = 0;
		int DamageBuff = 0;
		float DamageResistance = 0;
		float DamageResistanceBuff = 0;
		int Heal = 0;
		int HealBuff = 0;
		outputField.text = "Игрок:\n";
		foreach (GameObject cardObject in cards)
		{
			CardInfo card = cardObject.GetComponent<CardInfo>();
			if (card.CardType == 2 || card.CardType == 4)
			{
				if (CheckCombination(cards.IndexOf(cardObject)))
				{
					DamageBuff += card.Damage;
					DamageResistanceBuff += card.DamageResistance;
					HealBuff += card.Heal;
				}
				continue;
			}
			Damage += card.Damage;
			if (card.DamageResistance != 0)
			{
				DamageResistance += (card.DamageResistance / 100f);
			}
			Heal += card.Heal;

		}
		if (Damage > 0)
		{
			outputField.text += "Урон: " + Damage.ToString();
			if (DamageBuff != 0)
			{
				outputField.text += " +" + DamageBuff;
			}
			outputField.text += "\n";
		}
		if (DamageResistance != 1)
		{
			outputField.text += "Защита: " + ((DamageResistance) * 100).ToString() + "%";
			if (DamageResistanceBuff != 0)
			{
				outputField.text += " +" + DamageResistanceBuff + "%";
			}
			outputField.text += "\n";
		}
		if (Heal > 0)
		{
			outputField.text += "Лечение: " + Heal.ToString();
			if (HealBuff != 0)
			{
				outputField.text += " +" + HealBuff;
			}
			outputField.text += "\n";
		}
	}
}
