using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{
    public List<GameObject> itemsInShop;
    public int itemsCount;
    
    public Transform cardsPool;
    public TextMeshProUGUI fameValue;
    public GameObject buttonPrefab;
    


    public void Open()
    {
        fameValue.text = "Ваша слава: "+PlayerProperties.Instance.fame;
        for (int i = 0; i < itemsCount; i++)
        {
            var item = CardManager.Instance.lockedCards[Random.Range(0, CardManager.Instance.lockedCards.Count)];
            Destroy(item.gameObject.GetComponent<Button>());
            itemsInShop.Add(item);
            CardManager.Instance.lockedCards.Remove(item);
        }

        foreach (var item in itemsInShop)
        {
            var itemSpawned = Instantiate(buttonPrefab, cardsPool);
            itemSpawned.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                item.GetComponent<CardInfo>().Price.ToString();
			itemSpawned.transform.GetChild(1).GetComponent<Image>().sprite =
				item.transform.GetChild(1).GetComponent<Image>().sprite;
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
            PlayerProperties.Instance.fame -= price;

            button.interactable = false;

            Destroy(button.GetComponent<TextAnim>());

            button.transform.localScale = Vector3.one;

            itemsInShop.Remove(card);

            card.GetComponent<CardLogic>().currentContainer = CardPlacementSystem.Instance.deck;

            CardManager.Instance.allCards.Add(card);
			PlayerProperties.Instance.UpdateViewModels();
			fameValue.text = "Ваша слава: " + PlayerProperties.Instance.fame;
		}
    }
    
    private void UpdateShop()
    {
        // Удаляем старые карты 
        for (int i = 0; i < cardsPool.childCount; i++)
        {
            Destroy(cardsPool.GetChild(i).gameObject);
        }
        
        foreach (var item in itemsInShop)
        {
            CardManager.Instance.lockedCards.Add(item);
        }
        
        // Добавляем новые карты в магазин и рендерим их

        Open();
    }
    
    
}
