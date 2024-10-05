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
    
    void Start()
    {
        dialogText = dialogText.GetComponent<TextMeshProUGUI>();
        dialogPanelRectTransform = dialogPanel.GetComponent<RectTransform>();
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
                if(alignmentLeft) dialogPanelRectTransform.position = new Vector3(dialogPanelRectTransform.rect.width/2f, 
                    dialogPanelRectTransform.position.y, dialogPanelRectTransform.position.z);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
        }

        dialogText.text = "";
        dialogPanel.SetActive(false);
    }
}