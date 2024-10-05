using System;
using System.Collections;
using System.Collections .Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private GameObject dialogPanel;

    private RectTransform dialogPanelRectTransform;
    
    [SerializeField] private bool alignmentLeft;

    private void Start()
    {
        dialogText = dialogText.GetComponent<TextMeshProUGUI>();
        dialogPanelRectTransform = dialogPanel.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(alignmentLeft) dialogPanelRectTransform.anchoredPosition = new Vector2(dialogPanelRectTransform.rect.width/2f, 
            dialogPanelRectTransform.anchoredPosition.y);
    }

    public void StartText(List<string> phrases)
    {
        StartCoroutine(GoingText(phrases));
    }

    private IEnumerator GoingText(List<string> phrases)
    {
        dialogPanel.SetActive(true);
        foreach (var phrase in phrases)
        {
            dialogText.text = "";
            foreach (var letter in phrase)
            {
                dialogText.text += letter.ToString(); 
                yield return new WaitForSeconds(0.1f);
            }
            
            yield return new WaitForSeconds(0.5f);
        }

        dialogText.text = "";
        dialogPanel.SetActive(false);
    }
}