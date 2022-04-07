using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rat : MonoBehaviour {
    public bool ratmovement;
    public float y;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (ratmovement) { transform.Translate(Vector2.right / 10); }
        else { transform.Translate(Vector2.left / 10); }
        if (transform.position.x < y) { ratmovement = true; GetComponent<SpriteRenderer>().flipX = true; }
        else if (transform.position.x > 17.18f) { ratmovement = false; GetComponent<SpriteRenderer>().flipX = false; }
    }
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.name == "Character") { SceneManager.LoadScene("Level3"); }
    }
}
