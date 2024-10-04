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
        int DamageResistance = 1;
        int Heal = 0;
        foreach (GameObject cardObject in cardsField.cardsInDeck)
        {
            outputField.text = "�����:\n";
            CardInfo card = cardObject.GetComponent<CardInfo>();
            Damage += card.Damage;
            DamageResistance *= card.DamageResistance;
            Heal += card.Heal;
            if (Damage > 0)
            {
                outputField.text += "���� ����������: " + Damage.ToString() + "\n";
            }
            if (DamageResistance != 1)
            {
                outputField.text += "�������� ����������� �����: " + DamageResistance.ToString() + "%\n";
            }
            if (Heal > 0)
            {
                outputField.text += "�������: " + Heal.ToString();
            }
            yield return new WaitForSeconds(compilationSpeed);
        }
    }

    public void Compilate()
    {
        StartCoroutine(CountValues());
    }
}
