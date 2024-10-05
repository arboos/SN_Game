using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{
    public List<GameObject> items;
    public List<GameObject> itemsInShop;
    public int itemsCount;
    
    public Transform cardsPool;

    public GameObject buttonPrefab;


    public void Open()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            var item = items[Random.Range(0, items.Count)];
            Destroy(item.gameObject.GetComponent<Button>());
            itemsInShop.Add(item);
            items.Remove(item);
        }

        foreach (var item in itemsInShop)
        {
            var itemSpawned = Instantiate(buttonPrefab, cardsPool);
            itemSpawned.AddComponent<TextAnim>();
            itemSpawned.GetComponent<Image>().sprite = item.GetComponent<Image>().sprite;
            itemSpawned.GetComponent<Button>().onClick.AddListener(delegate
            {
                print("BUY ON CLICK");
                BuyCard(item, itemSpawned.GetComponent<Button>(), item.GetComponent<CardInfo>().Price);
            });
        }
    }


    public void BuyCard(GameObject card, Button button, int price)
    {
        if (PlayerProperties.Instance.fame >= price)
        {
            // Receive fame!!!!!!!!

            button.interactable = false;

            Destroy(button.GetComponent<TextAnim>());

            button.transform.localScale = Vector3.one;

            itemsInShop.Remove(card);

            card.GetComponent<CardLogic>().currentContainer = CardPlacementSystem.Instance.deck;

            CardManager.Instance.allCards.Add(card);
            
        }
    }
    
    private void UpdateShop()
    {
        // Удаляем старые карты 
        for (int i = 0; i < cardsPool.childCount; i++)
        {
            Destroy(cardsPool.GetChild(i).gameObject);
        }
        items = new List<GameObject>();
        foreach (var item in itemsInShop)
        {
            items.Add(item);
        }
        
        // Добавляем новые карты в магазин и рендерим их

        Open();
    }
    
    
}
