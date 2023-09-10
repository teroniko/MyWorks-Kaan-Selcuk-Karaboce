using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationNumber : MonoBehaviour
{
    public Transform Ball;
    private void Start()
    {
        if(Ball.gameObject.name== "Cylinder")
        {
            //optimize edilecek(canvas.width ile yapýlacak):
            //transform.localScale = Vector3.one * 3;
            GetComponent<RectTransform>().sizeDelta = Vector2.one*40;//65
        }
    }
    private void Update()
    {
        transform.position = (Vector2)Camera.main.WorldToScreenPoint(Ball.position);
    }
}
