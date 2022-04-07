using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wingman : MonoBehaviour {
	public bool b;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (b) {
			transform.Translate (Vector2.right * 2 * Time.deltaTime);

		} else {
			transform.Translate (Vector2.left * 2*Time.deltaTime);
		}
		if(transform.position.x>7){b = false;
			GetComponent<SpriteRenderer> ().flipX = false;
		}
		if(transform.position.x<-7){b = true;
			GetComponent<SpriteRenderer> ().flipX = true;
		}

	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.name == "Character") {
			//Application.LoadLevel ("Level2");
            SceneManager.LoadScene("Level2");
        }

	}
}
