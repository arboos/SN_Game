using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public PlayerProperties player;
    [SerializeField] private CardDeck cardsField;
    [SerializeField] private TextMeshProUGUI outputField;
    [Header("Visual Settings")]
    [SerializeField] private float compilationSpeed;

    private IEnumerator CountValues()
    {
        int Damage = 0;
        float DamageResistance = 1;
        int Heal = 0;
        foreach (GameObject cardObject in cardsField.cardsInDeck)
        {
            outputField.text = "Игрок:\n";
            CardInfo card = cardObject.GetComponent<CardInfo>();
            Damage += card.Damage;
            if (card.DamageResistance != 0)
            {
				DamageResistance *= (card.DamageResistance /100f);
            }
            Heal += card.Heal;
            if (Damage > 0)
            {
                outputField.text += "Урон противнику: " + Damage.ToString() + "\n";
            }
            if (DamageResistance != 1)
            {
                outputField.text += "Снижение получаемого урона: " + ((1-DamageResistance) * 100).ToString() + "%\n";
            }
            if (Heal > 0)
            {
                outputField.text += "Лечение: " + Heal.ToString();
            }
            yield return new WaitForSeconds(compilationSpeed);
        }
    }

    public void Compilate()
    {
        StartCoroutine(CountValues());
    }
}
