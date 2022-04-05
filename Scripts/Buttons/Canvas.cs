using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Canvas : EventTrigger
{
    // Start is called before the first frame update
    void Start()
    {
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        transform.parent.gameObject.GetComponent<Buttons>().OnPointerDown(eventData);
        Debug.Log("mjfgkhld");
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        transform.parent.gameObject.GetComponent<Buttons>().OnPointerUp(eventData);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
