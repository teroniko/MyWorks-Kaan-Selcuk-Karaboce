using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tube : MonoBehaviour {

    public Animator a;
    public GameObject floor7;
    void Start () {
        a = GameObject.Find("Floor7").GetComponent<Animator>();
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name=="Floor7") {
            a.SetBool("melting",true);
            Invoke("destroy",1);
        }
    }
    void destroy()
    {
        Destroy(gameObject);
        Destroy(floor7);
    }
}
