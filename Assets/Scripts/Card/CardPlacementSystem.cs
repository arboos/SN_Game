using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public CharacterDialog playerDialog;
    [SerializeField] private float timeToWaitPlayer;
    
    public CharacterDialog enemyDialog;
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

        playerDialog.gameObject.SetActive(false);
        enemyDialog.gameObject.SetActive(false);

        for (int i = 0; i < cardTakeStart; i++)
        {
            await TakeCard();
        }

        turnBlocker.SetActive(false);
    }
    
    
    public async UniTask TakeCard()
    {
        GameObject cardPrefab = deck.TakeUpperCard();
        if(cardPrefab == null) return;
		GameObject card = Instantiate(cardPrefab,canvas.transform);
        card.transform.position = deckTransform.position;

        float xPos = 90f;
        Vector3 movePos = new Vector3();

        
        movePos = hand.transform.position + new Vector3((handDeck.cardsInDeck.Count) * 100f, 0, 0);
        
        print(movePos);
        
        await MoveCard(card, movePos);
        
        card.transform.SetParent(hand.transform,false);
        handDeck.AddCardToDeck(card);
        card.GetComponent<CardLogic>().currentContainer = handDeck;
    }

    public async UniTask MoveCard(GameObject card,  Vector3 destination)
    {
        Tween move = card.transform.DOMove(destination, 0.5f);
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
