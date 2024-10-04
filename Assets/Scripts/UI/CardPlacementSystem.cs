using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlacementSystem : MonoBehaviour
{
    [Header("ѕол€ раскладки карт")]
    [SerializeField] public GameObject playboard;
    [SerializeField] public GameObject hand;
    //[SerializeField] public GameObject deck;
    [SerializeField] public GameObject canvas;
    [Header(" олода")]
    [SerializeField] private CardDeck deck;
    [Header("Ќастройки игры")]
    [SerializeField] private int maxHandCapacity;//максимум карт в руке
    [SerializeField] private int maxPlayboardCapacity; //максимум карт на игровом поле
    [SerializeField] private int cardTakeAmount;//количество карт получаемых в начале каждого хода

    public void TakeCard()
    {
        GameObject cardPrefab = deck.TakeUpperCard();
        hand.GetComponent<CardDeck>().AddCardToDeck(cardPrefab);
		GameObject card = Instantiate(cardPrefab,canvas.transform);
        card.transform.SetParent(hand.transform,false);
    }
}
