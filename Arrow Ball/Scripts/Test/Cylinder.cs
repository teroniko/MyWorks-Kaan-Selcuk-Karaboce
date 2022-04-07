using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) {
            Vector3 MousePos;

            Vector3 mouseInputPos = Input.mousePosition;
            mouseInputPos.z = Camera.main.nearClipPlane+8.7f;
            MousePos = Camera.main.ScreenToWorldPoint(mouseInputPos);
            transform.position = new Vector3(MousePos.x,MousePos.y, 0);
        }
    }
}
