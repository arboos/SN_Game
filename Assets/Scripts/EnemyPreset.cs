using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPreset : MonoBehaviour
{
    public int currentTurnIndex;
    public List<Turn> presets;

    public Transform playboardDeck;

    public List<GameObject> playedCards;
    
    [Serializable]
    public struct Turn
    {
        public GameObject[] cards;

        public int damage;
        public int heal;
    }
    

    
    public IEnumerator TakeTurn()
    {
        Turn turn = presets[currentTurnIndex];

        yield return new WaitForSeconds(1f);
        foreach (var card in turn.cards)
        {
            playedCards.Add(Instantiate(card.gameObject, playboardDeck.transform));
            yield return new WaitForSeconds(1f);
        }
        
        print("Take Damage: " + turn.damage);
        print("Take Heal: " + turn.heal);
    }
    
}
