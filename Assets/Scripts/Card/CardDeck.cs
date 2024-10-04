using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDeck : MonoBehaviour
{
    public int deckCapacity;
    public List<GameObject> cardsInDeck;

    private void Start()
    {
        ReshuffleDeck();
    }


    /// <summary>
    /// Возвращает СЛУЧАЙНУЮ карту и удаляет ее из короды
    /// </summary>
    public GameObject TakeRandomCard()
    {
        if (cardsInDeck.Count == 0) print("Not more cards to take from this deck : " + gameObject.name);
        GameObject cardToReturn = cardsInDeck[Random.Range(0, cardsInDeck.Count)];

        cardsInDeck.Remove(cardToReturn);
        return cardToReturn;
    }

    public void RemoveCard(GameObject card)
    {
        if (card == null) return;
        cardsInDeck.Remove(card);
    }

    /// <summary>
    /// Возвращает ВЕРХНЮЮ карту из колоды
    /// </summary>
    /// <returns></returns>
    public GameObject TakeUpperCard()
    {
        if (cardsInDeck.Count == 0)
        {
            print("Not more cards to take from this deck : " + gameObject.name);
            return null;
        }
        GameObject cardToReturn = cardsInDeck[0];

        cardsInDeck.Remove(cardToReturn);

        return cardToReturn;
    }
    
    /// <summary>
    /// Кладет карту ВНИЗ колоды
    /// </summary>
    /// <returns></returns>
    public void AddCardToDeck(GameObject card)
    {
        if (card == null)
        {
            print("GameObject card is null");
            return;
        }

        if (deckCapacity > 0 && cardsInDeck.Count >= deckCapacity)
        {
            print("Max deck capacity in : " + gameObject.name);
            return;
        }
        cardsInDeck.Add(card);
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
