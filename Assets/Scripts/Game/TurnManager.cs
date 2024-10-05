using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	//public PlayerProperties player;
	[SerializeField] private EnemyPreset enemy;
	[SerializeField] private CardDeck cardsField;
	public TextMeshProUGUI outputField;
	[Header("Visual Settings")]
	[SerializeField] private float compilationSpeed;

	private string CheckCombination(int buffId) //temporary realisation. Possible changes in the future;
	{
		try
		{
			if (cardsField.cardsInDeck[buffId].GetComponent<CardInfo>().CardType == 2
				&& cardsField.cardsInDeck[buffId - 1].GetComponent<CardInfo>().CardType == 1
				&& cardsField.cardsInDeck[buffId - 2].GetComponent<CardInfo>().CardType == 1)
			{
				return "heal";
			}
		}
		catch { }
		try
		{
			if (cardsField.cardsInDeck[buffId + 1].GetComponent<CardInfo>().CardType == 3
				&& cardsField.cardsInDeck[buffId].GetComponent<CardInfo>().CardType == 3)
			{
				return "-defence";
			}
		}
		catch { }
		try
		{
			if (cardsField.cardsInDeck[buffId - 1].GetComponent<CardInfo>().CardType == 3
				&& cardsField.cardsInDeck[buffId].GetComponent<CardInfo>().CardType == 4)
			{
				return "+damage";
			}
		}
		catch { }
		try
		{
			if (cardsField.cardsInDeck[buffId + 1].GetComponent<CardInfo>().CardType == 3
			&& cardsField.cardsInDeck[buffId].GetComponent<CardInfo>().CardType == 4)
			{
				return "+damage";
			}
		}
		catch { }
		try
		{
			if (cardsField.cardsInDeck[buffId - 1].GetComponent<CardInfo>().CardType == 1
				&& cardsField.cardsInDeck[buffId].GetComponent<CardInfo>().CardType == 2)
			{
				return "+defence";
			}
		}
		catch { }
		try
		{
			if (cardsField.cardsInDeck[buffId + 1].GetComponent<CardInfo>().CardType == 1
			&& cardsField.cardsInDeck[buffId].GetComponent<CardInfo>().CardType == 2)
			{
				return "+defence";
			}
		}
		catch { }
		return "";
	}


	private IEnumerator CountValues()
	{
		var cards = cardsField.cardsInDeck;

		int Damage = 0;
		int DamageBuff = 0;
		int DamageResistance = 0;
		int DamageResistanceBuff = 0;
		int Heal = 0;
		int ResistancePenetration = 0;
		foreach (GameObject cardObject in cards)
		{
			outputField.text = "Игрок:\n";
			CardInfo card = cardObject.GetComponent<CardInfo>();
			if (card.CardType == 3)
			{
				string combination = CheckCombination(cards.IndexOf(cardObject));
				if (combination == "-defence")
				{
					ResistancePenetration += card.DamagePenetration;
					continue;
				}
				Damage += card.Damage;
			}
			if (card.CardType == 2 || card.CardType == 4)
			{
				string combination = CheckCombination(cards.IndexOf(cardObject));
				if (combination == "+damage")
				{
					DamageBuff += card.Damage;
					PlayerProperties.Instance.honestReaction.PlayHappy();
				}
				else if (combination == "+defence")
				{
					DamageResistanceBuff += card.DamageResistance;
					PlayerProperties.Instance.honestReaction.PlayHappy();
				}
				else if (combination == "heal")
				{
					Heal += card.Heal;
					PlayerProperties.Instance.honestReaction.PlayHappy();
				}
				continue;
			}

			if (card.DamageResistance != 0 && card.CardType == 1)
			{
				DamageResistance += card.DamageResistance;
			}
			yield return new WaitForSeconds(compilationSpeed);
		}
		PlayerProperties.Instance.SetResistance(DamageResistance + DamageResistanceBuff);
		PlayerProperties.Instance.Heal(Heal);
		Debug.Log(Damage + DamageBuff);
		enemy.TakeDamage(Damage + DamageBuff, ResistancePenetration);
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
		int ResistancePenetration = 0;
		outputField.text = "Игрок:\n";
		foreach (GameObject cardObject in cards)
		{
			CardInfo card = cardObject.GetComponent<CardInfo>();
			if (card.CardType == 3)
			{
				string combination = CheckCombination(cards.IndexOf(cardObject));
				if (combination == "-defence")
				{
					Debug.Log("-defence!");
					ResistancePenetration += card.DamagePenetration;
					continue;
				}
				Damage += card.Damage;
			}
			if (card.CardType == 2 || card.CardType == 4)
			{
				string combination = CheckCombination(cards.IndexOf(cardObject));
				if (combination == "+damage")
				{
					DamageBuff += card.Damage;
				}
				else if (combination == "+defence")
				{
					DamageResistanceBuff += card.DamageResistance;
				}
				else if (combination == "heal")
				{
					Heal += card.Heal;
				}
				continue;
			}
			if (card.DamageResistance != 0 && card.CardType == 1)
			{
				DamageResistance += card.DamageResistance;
			}
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
		if (ResistancePenetration > 0)
		{
			outputField.text += "Снижение защиты: " + ResistancePenetration.ToString() + "\n";
		}
		if (DamageResistance != 1)
		{
			outputField.text += "Защита: " + DamageResistance.ToString();
			if (DamageResistanceBuff != 0)
			{
				outputField.text += " +" + DamageResistanceBuff;
			}
			outputField.text += "\n";
		}
		if (Heal > 0)
		{
			outputField.text += "Лечение: " + Heal.ToString();
			outputField.text += "\n";
		}
	}
}
