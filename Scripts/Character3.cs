

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Character3 : MonoBehaviour
{

    public AudioClip bombsound;
    public Animator a1,a2;
    public bool b = true, b4 = false, stairs, elinde, b2 = true, gb, rb, bb,bombactive=true,gamefinished;
    public byte dhti,red,green,blue,general;
    public Sprite jump,bombsprite,bombred,bombblue,bombgreen;
    public GameObject tube,fall,floor8,rat1,rat2,spike1,spike2,spike3,bomb;
    int seconds=0,minutes=3;
    public Text time;
    void Start()
    {
        //GetComponent<Animator>().runtimeAnimatorController = a2 as RuntimeAnimatorController;
        rat1.SetActive(false); rat2.SetActive(false);
        rat1.GetComponent<Renderer>().enabled = false;
        rat2.GetComponent<Renderer>().enabled = false;
        
        InvokeRepeating("timer", 0, 1);

    }
    void timer() {
        if (!gamefinished) {
            seconds--;
            if (seconds < 1)
            {
                if (minutes < 1)
                {
                    CancelInvoke();
                    a2.enabled = true;
                    a2.SetBool("bomb", true);
                    GameObject.Find("Main Camera").GetComponent<AudioSource>().clip = bombsound;
                    GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
                    Invoke("restart", 1.2f);
                }
                else
                {
                    seconds = 60; minutes--;
                }
            }
            if (seconds < 10) { time.text = "0" + minutes + ":"+"0" + seconds; } else {
                time.text = "0" + minutes + ":" + seconds;
            }
           
        }
        
        
        
    }
    void restart() {
        SceneManager.LoadScene("Level3");
    }
    void Update()
    {
        if (transform.position.y<-14) {
            SceneManager.LoadScene("Level3");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) & b & !stairs)
        {

            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 300);



        }
        if (stairs) {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector2.up * 3 * Time.deltaTime);
                //GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("adventurer_walk2", typeof(RuntimeAnimatorController));
                a1.SetBool("Character", false);
                a1.SetBool("climb", true);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector2.down * 3 * Time.deltaTime);
                a1.SetBool("Character", false);
                a1.SetBool("climb", true);
            }
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                a1.SetBool("Character", false);
                a1.SetBool("climb", false);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                a1.SetBool("Character", false);
                a1.SetBool("climb", false);
            }
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<SpriteRenderer>().flipX = false;
            transform.Translate(Vector2.right * 4.5f * Time.deltaTime);
            a1.SetBool("climb", false);
            a1.SetBool("Character", true);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            transform.Translate(Vector2.left * 4.5f * Time.deltaTime);
            a1.SetBool("climb", false);
            a1.SetBool("Character", true);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            a1.SetBool("climb", false);
            a1.SetBool("Character", false);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            a1.SetBool("climb", false);
            a1.SetBool("Character", false);
        }
        transform.rotation = Quaternion.identity;
        if (Input.GetKeyDown(KeyCode.Q) & dhti == 1)
        {
            dhti = 2;
            tube.transform.position = transform.position;
            tube.GetComponent<BoxCollider2D>().size = new Vector2(tube.GetComponent<BoxCollider2D>().size.x, tube.GetComponent<BoxCollider2D>().size.y + 2);
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
        if (coll.gameObject.name=="fall") {
            Destroy(fall);
            floor8.SetActive(false);
            floor8.GetComponent<Renderer>().enabled = false;
            rat1.SetActive(true); rat2.SetActive(true);
            rat1.GetComponent<Renderer>().enabled = true;
            rat2.GetComponent<Renderer>().enabled = true;
            Invoke("reopen",2);
        }
    }
    void reopen() {
        floor8.SetActive(true);
        floor8.GetComponent<Renderer>().enabled = true;
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        
        if (coll.gameObject.name == "stairs")
        {
            stairs = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (coll.gameObject.name == "greenbutton")
            {
                //if (GameObject.Find("greenbutton").gameObject.GetComponent<Animator>().GetBool("greenbutton"))
                //{
                coll.gameObject.GetComponent<Animator>().SetBool("greenbutton", false);
                Invoke("closegreen", 0.3f);
                //}

                if (green == general)
                {
                    general++;
                }

                if (general == 3) {
                    gamefinished = true;
                    Invoke("end", 2);
                }
            }
            if (coll.gameObject.name == "redbutton")
            {
                //if (GameObject.Find("redbutton").gameObject.GetComponent<Animator>().GetBool("redbutton"))
                //{
                coll.gameObject.GetComponent<Animator>().SetBool("redbutton", false);
                Invoke("closered", 0.3f);
                //}
                if (red == general)
                {
                    
                    general++;
                }
                

                if (general == 3) {
                    gamefinished = true;
                    Invoke("end", 2);
                }
            }
            if (coll.gameObject.name == "bluebutton")
            {
                //if (GameObject.Find("bluebutton").gameObject.GetComponent<Animator>().GetBool("bluebutton")){

                coll.gameObject.GetComponent<Animator>().SetBool("bluebutton", false);
                Invoke("closeblue", 0.3f);
                //}
                if (general == blue)
                {
                    general++;
                }
                
                if (general==3) {
                    gamefinished = true;
                    Invoke("end", 2);
                }
            }
            if (coll.gameObject.name == "bomb"&bombactive)
            {
                bombactive = false;
                blue = (byte)Random.Range(0,3);
                while (blue == green) { green = (byte)Random.Range(0, 3); }
                if (blue == 0 & green == 1 | blue == 1 & green == 0) { red = 2; }
                else if (blue == 2 & green == 1 | blue == 1 & green == 2) { red = 0; }
                else if (blue == 2 & green == 0 | blue == 0 & green == 2) { red = 1; }
                /*
                 while (true)
                {
                    if (blue == green) { green = (byte)Random.Range(0, 3); }
                    else{ break; }
                }
                 
                 */
                /*if (blue == 0 & green == 1| blue == 1 & green == 0) { red = 2; }
                else if (blue == 2 & green == 1 | blue == 1 & green == 2) { red = 0; }
                else if (blue == 2 & green == 0 | blue == 0 & green == 2) { red = 1; }*/
                Debug.Log(blue+""+green+""+red);
                
                bomb.GetComponent<SpriteRenderer>().sprite = bombred;
                Invoke("greensprite", green);
                Invoke("bluesprite", blue);
                Invoke("redsprite", red);
                Invoke("normalsprite", 4);
            }
        }

    }
    void normalsprite()
    {
        bomb.GetComponent<SpriteRenderer>().sprite = bombsprite;
        bombactive = true;
        //iptal = false;
        general = 0;
    }
    void greensprite()
    {
        bomb.GetComponent<SpriteRenderer>().sprite = bombgreen;
    }
    void bluesprite()
    {
        bomb.GetComponent<SpriteRenderer>().sprite = bombblue;
    }
    void redsprite()
    {
        bomb.GetComponent<SpriteRenderer>().sprite = bombred;
    }
    void closegreen() {
        GameObject.Find("greenbutton").gameObject.GetComponent<Animator>().SetBool("greenbutton", true);
        
    }
    void closered()
    {
        GameObject.Find("redbutton").gameObject.GetComponent<Animator>().SetBool("redbutton", true);

    }
    void closeblue()
    {
        GameObject.Find("bluebutton").gameObject.GetComponent<Animator>().SetBool("bluebutton", true);

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



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character3 : MonoBehaviour {

    public Animator a; 
    public byte dhti;
    public Sprite jump;
    public bool b = true, b4 = false, stairs;
    
    void Start()
    {
        a = GetComponent<Animator>();
    }
    void Update()
    {
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
            a.SetBool("walk", true);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            transform.Translate(Vector2.left * 3 * Time.deltaTime);
            a.SetBool("walk", true);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            a.SetBool("walk", false);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            a.SetBool("walk", false);
        }
        transform.rotation = Quaternion.identity;
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.name == "stairs")
        {
            stairs = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.name == "stairs")
        {
            stairs = false;
            GetComponent<Rigidbody2D>().gravityScale = 1; b = true;
        }
    }
}
*/
