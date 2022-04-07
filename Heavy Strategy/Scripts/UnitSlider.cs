using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSlider : MonoBehaviour, IPointerUpHandler
{
    GameObject Value;
    GameObject MC;
    GameObject ManaSlider;
    float value;
    private void Awake()
    {
        Value = transform.Find("Value").gameObject;
        MC = GameObject.Find("Main Camera");
        ManaSlider = MC.GetComponent<Strategy0>().ManaSlider.gameObject;
    }
    public void OnValueChanged(float value)
    {
        this.value = value;
        Value.GetComponent<Text>().text = "" + value;
        
    }
    void DecreaseMana(float decreasement)
    {
        Strategy0.mana -= decreasement;
        ManaSlider.GetComponent<Slider>().value = Strategy0.mana;
        ManaSlider.transform.Find("Value").GetComponent<Text>().text = Strategy0.mana + "";
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (transform.Find("Text").GetComponent<Text>().text)
        {
            case "Attack Power":
                DecreaseMana(1f * -(Strategy0.attackables[Strategy0.unitNumber].attackPower - value));
                Strategy0.attackables[Strategy0.unitNumber].attackPower = value;
                Debug.Log("Attack Power Changed");

                break;
            case "Attack Speed":
                DecreaseMana(1f * -(Strategy0.attackables[Strategy0.unitNumber].attackSpeed - value));
                Strategy0.attackables[Strategy0.unitNumber].attackSpeed = value;
                Debug.Log("Attack Speed Changed");


                break;
            case "Life":
                DecreaseMana(1f * -(Strategy0.attackables[Strategy0.unitNumber].life - value));
                Strategy0.attackables[Strategy0.unitNumber].life = value;
                Debug.Log("Life Changed");


                break;
            case "Count":
                DecreaseMana(1f * -(Strategy0.counts[Strategy0.unitNumber] - value));
                Strategy0.counts[Strategy0.unitNumber] = (int)value;

                Debug.Log("Count Changed");

                break;
            case "Mana":
                Strategy0.mana = value;
                Debug.Log("Mana Changed");
                break;
        }
    }
}
