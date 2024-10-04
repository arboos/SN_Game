using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPreset : MonoBehaviour
{
    public int currentTurnIndex;
    public List<Turn> presets;

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
        if (currentTurnIndex >= presets.Count) currentTurnIndex = 0;
        
        Turn turn = presets[currentTurnIndex];

        yield return new WaitForSeconds(1f);
        foreach (var card in turn.cards)
        {
            playedCards.Add(Instantiate(card.gameObject, CardPlacementSystem.Instance.playboard.transform));
            yield return new WaitForSeconds(1f);
        }
        
        print("Take Damage: " + turn.damage);
        print("Take Heal: " + turn.heal);

        currentTurnIndex++;

        int count = CardPlacementSystem.Instance.playboard.transform.childCount;
        for (int j = 0; j < count; j++)
        {
            Destroy(CardPlacementSystem.Instance.playboard.transform.GetChild(j).gameObject);
        }
        
        CardPlacementSystem.Instance.StartTurn();
    }
    
}
