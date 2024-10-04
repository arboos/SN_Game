using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    public List<GameObject> allCards;
    public List<GameObject> lockedCards;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("One more CardManager by name" + gameObject.name);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerDeck.cardsInDeck = allCards;
    }

    public CardDeck playerDeck;
    public CardDeck playerHandDeck;
    
    
}
