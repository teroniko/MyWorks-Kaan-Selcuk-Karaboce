using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDetection : MonoBehaviour
{
    /*private void OnMouseDown()
    {
        Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }*/
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            /*Vector3 mouseposition=
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
            */
            Vector3 mouseposition=Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit))
            { mouseposition = hit.point; }
            Debug.Log(mouseposition);

        }
    }
}
