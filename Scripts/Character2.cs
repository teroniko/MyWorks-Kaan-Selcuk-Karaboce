using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character2 : MonoBehaviour {
	public GameObject key;
	public GameObject ground;
	public GameObject wall;
	public bool getkey;
	public bool b;
	public bool b2=true;
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (b) {
			ground.transform.Translate (Vector2.up * 3 * Time.deltaTime);

		} else {
			ground.transform.Translate (Vector2.down * 3*Time.deltaTime);
		}
		if(ground.transform.position.y>0.81f){b = false;
		}
		if(ground.transform.position.y<-2.62f){b = true;
		}
        
		/*if(Input.GetKeyDown (KeyCode.Q)&getkey){
			getkey = false;
			//key.transform.position = new Vector2(transform.position.x/*+ GetComponent<Renderer>().bounds.size.x,transform.position.y);
		}*/
		
	}/*
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.name == "key") {
			Debug.Log ("ali");getkey = true;
			Destroy (key);
		}
		if (coll.gameObject.name == "Door") {
			Destroy (key);
		}
	}*/
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.name == "key") {
			getkey = true;
			Destroy (key);
		}
		if (coll.gameObject.name == "Door"&getkey) {
			wall.transform.position = new Vector2 (wall.transform.position.x,-3.53f);
		}
		if (coll.gameObject.name == "Door2"&getkey) {
            SceneManager.LoadScene("Level1");
        }
	}
}
