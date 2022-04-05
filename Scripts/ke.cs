using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ke : MonoBehaviour {
	Character c;
	// Use this for initialization
	void Start () {
		c = GameObject.Find("Character").GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerStay2D(Collider2D coll){
		for(int i=0;i<7;i++){
			if (coll.gameObject.name == "Floor"+i) {
			c.b = true;
			}
		}
	}
	void OnTriggerExit2D(Collider2D coll){
		for(int i=0;i<7;i++){
			if (coll.gameObject.name == "Floor"+i) {
			c.b = false;
			}
		}
	}
}
