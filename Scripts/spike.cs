using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class spike : MonoBehaviour {
    bool spikemovement;
    void Update() {
        if (spikemovement) { transform.Translate(Vector2.up/27); }
        else { transform.Translate(Vector2.down/27); }
        if (transform.position.y < -2.2f) { spikemovement = true; }
        else if (transform.position.y>2.03) { spikemovement = false; }
	}
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.name=="Character") { SceneManager.LoadScene("Level3"); }
    }
}
