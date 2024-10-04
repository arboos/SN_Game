using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlacementSystem : MonoBehaviour
{
    public static CardPlacementSystem Instance { get; private set; }
    
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
    [SerializeField] private TurnManager turnManager;
    public CardDeck playboardDeck;
    public CardDeck handDeck;

    public EnemyPreset enemyPreset;

    public GameObject turnBlocker;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playboardDeck = playboard.GetComponent<CardDeck>();
        handDeck = hand.GetComponent<CardDeck>();
    }

    public void TakeCard()
    {
        GameObject cardPrefab = deck.TakeUpperCard();
        if(cardPrefab == null) return;
		GameObject card = Instantiate(cardPrefab,canvas.transform);
        card.transform.SetParent(hand.transform,false);
        handDeck.AddCardToDeck(card);
        card.GetComponent<CardLogic>().currentContainer = handDeck;
    }

    public void EndTurn()
    {
		turnBlocker.SetActive(true);
        int count = playboardDeck.cardsInDeck.Count;
        for(int i = 0; i < count; i++)
        {
            var card = playboardDeck.TakeUpperCard();
            deck.AddCardToDeck(card);
            transform.SetParent(deck.transform, false);
            handDeck.RemoveCard(gameObject);
        }
        
        for (int j = 0; j < count; j++)
        {
            print("Remove this");
            playboard.transform.GetChild(0).transform.SetParent(deck.transform);
        }
		StartCoroutine(enemyPreset.TakeTurn());
	}

    public void StartTurn()
    {
        enemyPreset.outputField.text = "";
        GiveCardsToPlayer(cardTakeAmount);
        turnBlocker.SetActive(false);
    }
    
    public void GiveCardsToPlayer(int count)
    {
        for (int i = 0; i < count; i++)
        {
            TakeCard();
        }
    }
}
