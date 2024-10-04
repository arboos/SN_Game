using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CardManager.Instance.playerDeck.ReshuffleDeck();
        }
        
        else if (Input.GetKeyDown(KeyCode.E))
        {
            CardManager.Instance.playerHandDeck.cardsInDeck.Add(CardManager.Instance.playerDeck.TakeRandomCard());
        }
    }
}
