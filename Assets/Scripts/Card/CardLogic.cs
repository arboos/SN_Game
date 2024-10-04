using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private Transform currentParent;
	[SerializeField] private CardPlacementSystem placementSystem;
	[SerializeField] private bool isPickedUp = false;
	private TurnManager turnManager;
	public CardDeck currentContainer;
	
	[SerializeField] private float followMouseSpeed;

	private void Awake()
	{
		placementSystem = CardPlacementSystem.Instance;
		turnManager = GameObject.Find("CardManager").GetComponent<TurnManager>();
		currentParent = placementSystem.hand.transform;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!isPickedUp)
		{
			Pick();
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.isValid)
		{
			if (eventData.pointerCurrentRaycast.gameObject.name == "Playboard")
			{
				currentParent = placementSystem.playboard.transform;
			}
			else if (eventData.pointerCurrentRaycast.gameObject.name == "Hand")
			{
				currentParent = placementSystem.hand.transform;
			}
			else
			{
				currentParent = placementSystem.hand.transform;
			}
		}
		transform.SetParent(currentParent, false);
		
		currentParent.GetComponent<CardDeck>().AddCardToDeck(gameObject);
		currentContainer.RemoveCard(gameObject);
		currentContainer = currentParent.GetComponent<CardDeck>();
		isPickedUp = false;
		turnManager.PreCompilate();
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
	}

	public void Pick()
	{
		if (!isPickedUp)
		{
			transform.SetParent(placementSystem.canvas.transform, true);
			isPickedUp = true;
			GetComponent<Image>().raycastTarget = false;
		}
	}

}
