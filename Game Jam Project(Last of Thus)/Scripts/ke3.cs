using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ke3 : MonoBehaviour {
    Character3 c;
    // Use this for initialization
    void Start()
    {
        GameObject go = GameObject.Find("Character");
        c = go.GetComponent<Character3>();
    }
    
    void OnTriggerStay2D(Collider2D coll)
    {
        for (int i = 1; i < 8; i++)
        {
            if (coll.gameObject.name == "Floor" + i)
            {
                c.b = true;
            }
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        for (int i = 1; i < 8; i++)
        {
            if (coll.gameObject.name == "Floor" + i)
            {
                c.b = false;
            }
        }
    }
}
