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
            var itemSpawned = Instantiate(item, cardsPool);
            itemSpawned.AddComponent<Button>().onClick.AddListener(delegate
            {
                BuyCard(gameObject);
            });
        }
    }


    public void BuyCard(GameObject card)
    {
        CardManager.Instance.allCards.Add(card);
        itemsInShop.Remove(card);
        // Receive fame!!!!!!!!
    }
    
    private void UpdateShop()
    {
        for (int i = 0; i < cardsPool.childCount; i++)
        {
            Destroy(cardsPool.GetChild(i).gameObject);
        }

        // Пока не нужно
        //
        // RelativeJoint2D joint2D = new RelativeJoint2D();
        // joint2D.GetComponent<GameObject>().SetActive(false);
        
        items = new List<GameObject>();
        foreach (var item in itemsInShop)
        {
            items.Add(item);
        }
        
        //Добавляем новые карты в магазин и рендерим их

        for (int i = 0; i < itemsCount; i++)
        {
            var item = items[Random.Range(0, items.Count)]; 
            Destroy(item.gameObject.GetComponent<Button>());
            itemsInShop.Add(item);
            items.Remove(item);
        }

        foreach (var item in itemsInShop)
        {
            var itemSpawned = Instantiate(item, cardsPool);
            itemSpawned.AddComponent<Button>().onClick.AddListener(delegate
            {
                BuyCard(gameObject);
            });
        }
    }
    
    
}
