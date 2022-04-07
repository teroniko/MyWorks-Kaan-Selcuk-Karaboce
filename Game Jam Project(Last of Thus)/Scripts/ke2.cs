using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ke2 : MonoBehaviour {
	walkjumpclimb c;
	// Use this for initialization
	void Start () {
		c = GameObject.Find("Character").GetComponent<walkjumpclimb>();

	}

	// Update is called once per frame
	void Update () {

	}
	void OnTriggerStay2D(Collider2D coll){
		for(int i=1;i<5;i++){
			if (coll.gameObject.name == "Floor"+i) {
				c.b = true;
			}
		}
	}
	void OnTriggerExit2D(Collider2D coll){
		for(int i=1;i<5;i++){
			if (coll.gameObject.name == "Floor"+i) {
				c.b = false;
			}
		}
	}
}
