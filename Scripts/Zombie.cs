using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {
	bool b;public Animator a;Character c;GameObject wall;
	public GameObject bomb;bool bb;
	// Use this for initialization
	void Start () {
		c = GameObject.Find("Character").GetComponent<Character>();

		a = GetComponent<Animator>();
		a.SetBool("zombiewalk",true);
		wall = GameObject.Find ("Floor4");
	}
	
	// Update is called once per frame
	void Update () {
		bb = GameObject.FindGameObjectWithTag ("Player").GetComponent<Character>().b4;
		if (b) {
			transform.Translate (Vector2.right * 3 * Time.deltaTime);

		} else {
			transform.Translate (Vector2.left * 3*Time.deltaTime);
		}
		if(transform.position.x>8.53){b = false;
			GetComponent<SpriteRenderer> ().flipX = true;
		}
		if(transform.position.x<-1.36){b = true;
			GetComponent<SpriteRenderer> ().flipX = false;
		}
	}
	void OnTriggerEnter2D(Collider2D coll){

		if (coll.gameObject.tag=="Explosion") {
			Destroy (gameObject);
			wall.transform.position = new Vector2 (wall.transform.position.x,-5.21f);
		}

	}
}
