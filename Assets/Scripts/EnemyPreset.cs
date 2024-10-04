using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPreset : MonoBehaviour
{
    public int currentTurnIndex;
    public List<Turn> presets;
    public List<GameObject> playedCards;
    [SerializeField] private PlayerProperties player;

    public int HP;
    public int MaxHP;
    public float DamageResistance;

    [SerializeField] private Image HPBar;

    
    [Serializable]
    public struct Turn
    {
        public GameObject[] cards;

        public int damage;
        public int heal;
        public float damageResistance;
    }
    
    public void TakeDamage(int damage)
    {
        if (HP - (int)(damage * DamageResistance) <= 0)
        {
            HP = 0;
            UpdateViewModels();
            Die();
            return;
        }
		if (DamageResistance != 0)
		{
			HP -= (int)(damage * DamageResistance);
		}
		else
		{
			HP -= damage;
		}
		UpdateViewModels();
    }

    private void UpdateViewModels()
    {
        HPBar.fillAmount = HP / (float)MaxHP;
    }

    private void Die()
    {
        //handle death
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
		print("DamageResistance: " + turn.heal);
        player.TakeDamage(turn.damage);
        DamageResistance = turn.damageResistance;
		currentTurnIndex++;

        int count = CardPlacementSystem.Instance.playboard.transform.childCount;
        for (int j = 0; j < count; j++)
        {
            Destroy(CardPlacementSystem.Instance.playboard.transform.GetChild(j).gameObject);
        }
        
        CardPlacementSystem.Instance.StartTurn();
    }
    
}
