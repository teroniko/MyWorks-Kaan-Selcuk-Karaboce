using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour {
	int i=0;
	public bool c;
	Character cc;
	public GameObject z;
	public Animator a;
	public GameObject explosion;
	void Start(){
		c = GameObject.FindGameObjectWithTag ("Player").GetComponent<Character>().b4;
		//cc = GameObject.Find ("Character");
		cc = GameObject.FindGameObjectWithTag ("Player").GetComponent<Character>() as Character;
		//Character character = thePlayer.GetComponent<Character>();
		z = GameObject.Find ("Zombie");
		a = explosion.GetComponent<Animator>();
		explosion.gameObject.SetActive (false);
		explosion.GetComponent<Renderer> ().enabled = false;
	}
	IEnumerator Example()
	{
		yield return new WaitForSeconds(3f);

		explosion.gameObject.SetActive (true);
		explosion.GetComponent<Renderer> ().enabled = true;

		//myObject.GetComponent<MyScript>().MyFunction();

		cc.lighter.transform.position = new Vector2(-5.38f,-3.66f);
		cc.b2 = false;c = true;cc.b4 = true;//TODO

		cc.dhti = 0;
		a.SetBool("explosion",true);
		yield return new WaitForSeconds(1.1f);

		a.SetBool("explosion",false);
		explosion.GetComponent<Renderer> ().enabled = false;
        Destroy(gameObject); Destroy(GameObject.Find("Lighter"));
        cc.b4 = false;c = false;
		explosion.gameObject.SetActive (false);
	}
	void d2(){
		z.transform.position = new Vector2(-2.8f,-4.02f);
	}
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.name == "Lighter") {
			StartCoroutine(Example());
		}
	}
}
