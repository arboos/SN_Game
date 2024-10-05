using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextAnim : MonoBehaviour
{
    private RectTransform _transform;
    private Button _button;

    private void Start()
    {
        
        _transform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
        
        EventTrigger.Entry smallButt = new EventTrigger.Entry();
        smallButt.eventID = EventTriggerType.PointerExit;
        smallButt.callback.AddListener((eventData) => { _transform.localScale = new Vector3(1f, 1f, 1f); });
        
        EventTrigger.Entry bigButt = new EventTrigger.Entry();
        bigButt.eventID = EventTriggerType.PointerEnter;
        bigButt.callback.AddListener((eventData) => { _transform.localScale = new Vector3(1.1f, 1.1f, 1.1f); });
        
        _button.AddComponent<EventTrigger>();
        _button.GetComponent<EventTrigger>().triggers.Add(bigButt);
        _button.GetComponent<EventTrigger>().triggers.Add(smallButt);
    }

    private void OnDisable()
    {
        _transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
