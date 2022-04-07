using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class noinfinitejump : MonoBehaviour {
    public Character c;
    void Start()
    {
        c = GameObject.Find("Character").GetComponent<Character>();
    }
    
    void OnTriggerStay2D(Collider2D coll)
    {
        for (int i = 0; i < 7; i++)
        {
            if (coll.gameObject.name == "Floor" + i)
            {
                c.b = true;
            }
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        for (int i = 0; i < 7; i++)
        {
            if (coll.gameObject.name == "Floor" + i)
            {
                c.b = false;
            }
        }
    }
}
