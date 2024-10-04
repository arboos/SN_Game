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


    private IEnumerator CountValues()
    {
        var cards = cardsField.cardsInDeck;

		int Damage = 0;
        float DamageResistance = 0;
        int Heal = 0;
        foreach (GameObject cardObject in cards)
        {
            outputField.text = "Игрок:\n";
            CardInfo card = cardObject.GetComponent<CardInfo>();
            Damage += card.Damage;
            if (card.DamageResistance != 0)
            {
				DamageResistance += (card.DamageResistance /100f);
            }
            Heal += card.Heal;
            /*if (Damage > 0)
            {
                outputField.text += "Óðîí ïðîòèâíèêó: " + Damage.ToString() + "\n";
            }
            if (DamageResistance != 1)
            {
                outputField.text += "Ñíèæåíèå ïîëó÷àåìîãî óðîíà: " + ((1-DamageResistance) * 100).ToString() + "%\n";
            }
            if (Heal > 0)
            {
                outputField.text += "Ëå÷åíèå: " + Heal.ToString();
            }*/
            yield return new WaitForSeconds(compilationSpeed);
        }
        player.SetResistance(DamageResistance);
        player.Heal(Heal);
        enemy.TakeDamage(Damage);
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
		float DamageResistance = 0;
		int Heal = 0;
		outputField.text = "Игрок:\n";
		foreach (GameObject cardObject in cards)
		{
			CardInfo card = cardObject.GetComponent<CardInfo>();
			Damage += card.Damage;
			if (card.DamageResistance != 0)
			{
				DamageResistance += (card.DamageResistance / 100f);
			}
			Heal += card.Heal;
			
		}
		if (Damage > 0)
		{
			outputField.text += "Урон: " + Damage.ToString() + "\n";
		}
		if (DamageResistance != 1)
		{
			outputField.text += "Защита: " + ((DamageResistance) * 100).ToString() + "%\n";
		}
		if (Heal > 0)
		{
			outputField.text += "Лечение: " + Heal.ToString() + "\n";
		}
	}
}
