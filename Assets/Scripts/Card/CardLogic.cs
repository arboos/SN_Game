using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private Transform currentParent;
	[SerializeField] private TextMeshProUGUI namefield;
	[SerializeField] private CardPlacementSystem placementSystem;
	[SerializeField] private bool isPickedUp = false;
	private TurnManager turnManager;
	public CardDeck currentContainer;
	[Header("Drag settings")]
	[SerializeField] private float followMouseSpeed;
	[SerializeField] private float turnCoef = 1;
	[SerializeField] private float pickedUpScale;

	private void Awake()
	{
		placementSystem = CardPlacementSystem.Instance;
		turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
		currentParent = placementSystem.hand.transform;
		namefield.text = GetComponent<CardInfo>().Name;
		namefield.gameObject.SetActive(false);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!isPickedUp)
		{
			namefield.gameObject.SetActive(false);
			Pick();
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.isValid)
		{
			if (eventData.pointerCurrentRaycast.gameObject.name == "Playboard")
			{
				if (placementSystem.playboard.GetComponent<CardDeck>().deckCapacity >
					placementSystem.playboard.GetComponent<CardDeck>().cardsInDeck.Count)
				{
					currentParent = placementSystem.playboard.transform;
					namefield.gameObject.SetActive(true);
				}
			}
			else if (eventData.pointerCurrentRaycast.gameObject.name == "Hand")
			{
				currentParent = placementSystem.hand.transform;
				namefield.gameObject.SetActive(false);
			}
			else
			{
				currentParent = placementSystem.hand.transform;
				namefield.gameObject.SetActive(false);
			}
		}
		transform.localScale = new Vector3(1, 1, 1);
		transform.SetParent(currentParent, false);
		transform.rotation = Quaternion.Euler(0, 0, 0);
		currentContainer.RemoveCard(gameObject);
		currentParent.GetComponent<CardDeck>().AddCardToDeck(gameObject);
		currentContainer = currentParent.GetComponent<CardDeck>();
		isPickedUp = false;
		turnManager.PreCompilate();
		PlayerProperties.Instance.audioSource.PlayOneShot(PlayerProperties.Instance.cardPlaced);
		GetComponent<Image>().raycastTarget = true;
	}

	private void Update()
	{
		HandleInput();
	}

	private void HandleInput()
	{
		if (Input.GetMouseButton(0) && isPickedUp)
		{
			Follow();
		}
	}
	
	private void Follow()
	{
		transform.position = Vector2.Lerp(transform.position, Input.mousePosition, Time.deltaTime * followMouseSpeed);
		transform.rotation = Quaternion.Euler(0,0,  -(Input.mousePosition.x - transform.position.x)*turnCoef);
	}

	public void Pick()
	{
		if (!isPickedUp)
		{
			transform.localScale = new Vector3(pickedUpScale,pickedUpScale,pickedUpScale);
			PlayerProperties.Instance.audioSource.PlayOneShot(PlayerProperties.Instance.cardTaken);
			transform.SetParent(placementSystem.canvas.transform, true);
			isPickedUp = true;
			GetComponent<Image>().raycastTarget = false;
		}
	}

}
