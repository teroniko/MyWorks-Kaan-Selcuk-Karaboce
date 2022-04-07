using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class walkjumpclimb : MonoBehaviour {


    public Animator a;
    public bool b = true, b4 = false, stairs,elinde,b2=true,gb,rb,bb;
    public byte dhti;
    public Sprite jump;
    public GameObject tube;
    void Start()
    {
        a = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) & b & !stairs)
        {

            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 300);



        }
        if (Input.GetKey(KeyCode.UpArrow) & stairs)
        {
            transform.Translate(Vector2.up * 3 * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.DownArrow) & stairs)
        {
            transform.Translate(Vector2.down * 3 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<SpriteRenderer>().flipX = false;
            transform.Translate(Vector2.right * 3 * Time.deltaTime);
            a.SetBool("Character", true);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            transform.Translate(Vector2.left * 3 * Time.deltaTime);
            a.SetBool("Character", true);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            a.SetBool("Character", false);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            a.SetBool("Character", false);
        }
        transform.rotation = Quaternion.identity;
        if (Input.GetKeyDown(KeyCode.Q) & dhti == 1) {
            dhti = 2;
            tube.transform.position = transform.position;
            tube.GetComponent<BoxCollider2D>().size = new Vector2(tube.GetComponent<BoxCollider2D>().size.x, tube.GetComponent<BoxCollider2D>().size.y+2);
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name == "tube" & dhti != 2 & dhti != 1)
        {
            coll.gameObject.transform.position = new Vector2(14.87f, -11.16f);
            elinde = true;
            dhti = 1;
        }
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.name == "stairs")
        {
            stairs = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            if (coll.gameObject.name == "greenbutton")
            {
                gb = true; Destroy(coll.gameObject);
            }
            if (coll.gameObject.name == "redbutton" & gb)
            {
                rb = true; Destroy(coll.gameObject);
            }
            if (coll.gameObject.name == "bluebutton" & rb)
            {
                Invoke("end", 3); Destroy(coll.gameObject);
            }
        }
        
    }
    public void end()
    {
        SceneManager.LoadScene("End");
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.name == "stairs")
        {
            stairs = false;
            GetComponent<Rigidbody2D>().gravityScale = 1; b = true;
        }
        if (coll.gameObject.name == "tube" & dhti != 1)
        {
            dhti = 0;
        }
    }
}
