using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
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
    [Header("��������� ����")]
    [SerializeField] private int maxHandCapacity;//�������� ���� � ����
    [SerializeField] private int maxPlayboardCapacity; //�������� ���� �� ������� ����
    [SerializeField] private int cardTakeAmount;//���������� ���� ���������� � ������ ������� ����
    [SerializeField] private int cardTakeStart;//���������� ���� ���������� � ������ ������� ����
    [SerializeField] private TurnManager turnManager;

    [SerializeField] private List<string> playerPhrases;
    [SerializeField] private List<string> enemyPhrases;

    [SerializeField] private CharacterDialog playerDialog;
    [SerializeField] private float timeToWaitPlayer;
    
    [SerializeField] private CharacterDialog enemyDialog;
    [SerializeField] private float timeToWaitEnemy;

    [SerializeField] private Transform deckTransform;
    
    
    public TextMeshProUGUI textHP_Player;
    public TextMeshProUGUI textHP_Enemy;

    
    //Decks
    public CardDeck deck;
    public CardDeck playboardDeck;
    public CardDeck handDeck;

    public EnemyPreset enemyPreset;

    public Shop shop;

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

        StartGame();
    }

    public async void StartGame()
    {
        playerDialog.StartText(playerPhrases);
        await UniTask.Delay(TimeSpan.FromSeconds(timeToWaitPlayer));
        
        enemyDialog.StartText(enemyPhrases);
        await UniTask.Delay(TimeSpan.FromSeconds(timeToWaitEnemy));
        
        turnBlocker.SetActive(false);

        for (int i = 0; i < cardTakeStart; i++)
        {
            await TakeCard();
        }

        textHP_Player.text = PlayerProperties.Instance.fame.ToString();
    }
    
    
    public async UniTask TakeCard()
    {
        GameObject cardPrefab = deck.TakeUpperCard();
        if(cardPrefab == null) return;
		GameObject card = Instantiate(cardPrefab,canvas.transform);
        card.transform.position = deckTransform.position;

        await MoveCard(card, hand.transform);
        
        card.transform.SetParent(hand.transform,false);
        handDeck.AddCardToDeck(card);
        card.GetComponent<CardLogic>().currentContainer = handDeck;
    }

    public async UniTask MoveCard(GameObject card, Transform destination)
    {
        Tween move = card.transform.DOMove(destination.position, 0.5f);
        await move.ToUniTask();
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
