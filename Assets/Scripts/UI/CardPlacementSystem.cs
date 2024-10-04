using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlacementSystem : MonoBehaviour
{
    [Header("���� ��������� ����")]
    [SerializeField] public GameObject playboard;
    [SerializeField] public GameObject hand;
    //[SerializeField] public GameObject deck;
    [SerializeField] public GameObject canvas;
    [Header("������")]
    [SerializeField] private CardDeck deck;
    [Header("��������� ����")]
    [SerializeField] private int maxHandCapacity;//�������� ���� � ����
    [SerializeField] private int maxPlayboardCapacity; //�������� ���� �� ������� ����
    [SerializeField] private int cardTakeAmount;//���������� ���� ���������� � ������ ������� ����

    public void TakeCard()
    {
        GameObject cardPrefab = deck.TakeUpperCard();
        hand.GetComponent<CardDeck>().AddCardToDeck(cardPrefab);
		GameObject card = Instantiate(cardPrefab,canvas.transform);
        card.transform.SetParent(hand.transform,false);
    }
}
