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

	[SerializeField] private float followMouseSpeed;

	private void Awake()
	{
		placementSystem = GameObject.Find("Canvas").GetComponent<CardPlacementSystem>();
		currentParent = transform.parent;
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
				currentParent = placementSystem.playdeck.transform;
			}
			else if (eventData.pointerCurrentRaycast.gameObject.name == "Hand")
			{
				currentParent = placementSystem.hand.transform;
			}
		}
		transform.SetParent(currentParent, false);
		isPickedUp = false;
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
		if (Input.GetMouseButtonUp(0) && isPickedUp)
		{
			//Place();
		}
	}

	private void Place()
	{
		Vector2 placeCords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Debug.Log(placeCords);
		if (placementSystem.playdeck.GetComponent<RectTransform>().rect.Contains(placeCords))
		{
			currentParent = placementSystem.playdeck.transform;
		}
		else if (placementSystem.hand.GetComponent<RectTransform>().rect.Contains(placeCords))
		{
			currentParent = placementSystem.hand.transform;
		}
		transform.SetParent(currentParent, false);
		isPickedUp = false;

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
