using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rock : MonoBehaviour {

    bool right,birkere=true;
	void Start () {
        

    }
	
	void Update () {
        if (birkere) {
            if (right) { GetComponent<Rigidbody2D>().AddTorque(600); }
            else { GetComponent<Rigidbody2D>().AddTorque(-600); }
            birkere = false;
        }
        
        //GetComponent<Rigidbody2D>().velocity = new Vector2(100, GetComponent<Rigidbody2D>().velocity.y);
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name == "stairs")
        {
            birkere = true;
            right = !right;
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Character")
        {
            SceneManager.LoadScene("Level3");
        }
    }
}
