using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Character : MonoBehaviour {
	public Animator a;public bool b=true,b2/*,lbLighterbool*/,b4=false;
	public GameObject lighter;public byte dhti;
    public Sprite jump;
	// Use this for initialization
	void Start () {
		a = GetComponent<Animator>();
    }
	
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        if (Input.GetKeyDown (KeyCode.UpArrow)&b& !b2) {
			
			GetComponent<Rigidbody2D> ().AddForce (Vector2.up * 300);
            transform.GetComponent<SpriteRenderer>().sprite = jump;
            //GetComponent<Rigidbody2D> ().velocity = Vector2.up * 300 * Time.deltaTime;



        }
        if (b2) {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector2.up * 3 * Time.deltaTime);
                //GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("climb", typeof(RuntimeAnimatorController));
                a.SetBool("Character", false);
                a.SetBool("climb", true);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector2.down * 3 * Time.deltaTime);
                //GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("climb", typeof(RuntimeAnimatorController));
                a.SetBool("Character", false);
                a.SetBool("climb", true);
            }
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                a.SetBool("Character", false);
                a.SetBool("climb", false);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                a.SetBool("Character", false);
                a.SetBool("climb", false);
            }
        }
		
		/*if(b3&b2){
			transform.Translate (Vector2.up * 3*Time.deltaTime);
		}*/
		if (Input.GetKey (KeyCode.RightArrow)) {
			GetComponent<SpriteRenderer> ().flipX = false;
			transform.Translate (Vector2.right * 3*Time.deltaTime);
            a.SetBool("climb", false);
            a.SetBool("Character", true);
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			GetComponent<SpriteRenderer> ().flipX = true;
			transform.Translate (Vector2.left * 3*Time.deltaTime);
            a.SetBool("climb", false);

            a.SetBool("Character", true);
		}
		if(Input.GetKeyDown (KeyCode.Q)&dhti==1){
			dhti=2;
			lighter.transform.position = new Vector2(transform.position.x/*+ GetComponent<Renderer>().bounds.size.x*/,transform.position.y);
		}
		if (Input.GetKeyUp (KeyCode.RightArrow)) {

            a.SetBool("climb", false);
            a.SetBool("Character", false);
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow)) {

            a.SetBool("climb", false);
            a.SetBool("Character", false);
		}
		transform.rotation = Quaternion.identity;
	}
	/*void OnCollisionEnter2D(Collider2D coll){

		if (coll.gameObject.name == "Explosion"&b3) {
			Application.LoadLevel ("level1");
		}
	}*/
	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.name == "stairs") {
			b2 = true;
			GetComponent<Rigidbody2D> ().velocity =Vector2.zero;
			GetComponent<Rigidbody2D> ().gravityScale = 0;
			//GetComponent<Rigidbody2D> ().AddForce (Vector2.up * 0);
		}
		if (coll.gameObject.name == "Lighter"&dhti!=2&dhti!=1) {
			lighter.transform.position = new Vector2(9.91f,-5.76f);
			dhti=1;
		}
		if (coll.gameObject.name == "Explosion"&b4) {
			SceneManager.LoadScene ("Level1");
		}
		if (coll.gameObject.name == "Door") {
			SceneManager.LoadScene("Level3");
		}
	}
	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.name == "stairs") {
			b2 = false;
			GetComponent<Rigidbody2D> ().gravityScale = 1;b = true;
		}
		if (coll.gameObject.name == "Lighter"&dhti!=1) {
			dhti=0;
		}
	}
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.name == "Zombie") {
			/*dhti = 0;
			b = true;
			b2 = false;
			//lb = false;
			transform.position = new Vector2 (7.49f, -2.49f);
			lighter.transform.position = new Vector2(-2.8f,-4.02f);*/
			//Application.LoadLevel ("level2");
            SceneManager.LoadScene("Level1");
        }

	}

}
