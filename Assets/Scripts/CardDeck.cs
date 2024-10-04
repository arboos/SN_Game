using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDeck : MonoBehaviour
{
    public List<GameObject> cardsInDeck;

    private void Start()
    {
        ReshuffleDeck();
    }


    /// <summary>
    /// Возвращает карту и удаляет ее из короды
    /// </summary>
    public GameObject TakeRandomCard()
    {
        if (cardsInDeck.Count == 0) print("Not more cards to take from this deck : " + gameObject.name);
        GameObject cardToReturn = cardsInDeck[Random.Range(0, cardsInDeck.Count)];

        cardsInDeck.Remove(cardToReturn);

        return cardToReturn;
    }
    
    /// <summary>
    /// Перетасовывает колоду
    /// </summary>
    public void ReshuffleDeck()
    {
        List<GameObject> shuffledDeck = new List<GameObject>();

        int cardsCount = cardsInDeck.Count;
        
        for (int i = 0; i < cardsCount; i++)
        {
            GameObject card = cardsInDeck[Random.Range(0, cardsInDeck.Count)];
            cardsInDeck.Remove(card);
            shuffledDeck.Add(card);
        }

        cardsInDeck = shuffledDeck;
    }
    
}
