using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {
    bool ilk = true;
    public bool in_the_line = false;
    public bool checked0 = false;
    public float size_limit = 4.5f, speed;

    private IEnumerator WaitAndStart()
    {
        
        yield return new WaitForSeconds(Random.Range(0, 2));
        if (ilk)
        {
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.AddComponent<Rigidbody2D>();
            gameObject.AddComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("1");
            GetComponent<Animator>().Play("Fish");
            
        }
        /*switch (Random.Range(0, 2))
        {
            case 0: GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("palamut"); break;
            case 1: GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("fish2"); break;
        }*/
        GetComponent<Animator>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("1");
        switch (Random.Range(0, 5))
        {
            case 0: GetComponent<SpriteRenderer>().color = Color.red; break;
            case 1: GetComponent<SpriteRenderer>().color = Color.blue; break;
            case 2: GetComponent<SpriteRenderer>().color = Color.white; break;
            case 3: GetComponent<SpriteRenderer>().color = Color.yellow; break;
            case 4: GetComponent<SpriteRenderer>().color = Color.green; break;
        }
        switch (Random.Range(0, 2))
        {
            case 0:
                transform.position = new Vector2(14, Random.Range(-3.5f, 3.5f));
                GetComponent<Rigidbody2D>().velocity = Vector2.left * Random.Range(3, Main.fishSpeed);//3,10

                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 1:
                transform.position = new Vector2(-14, Random.Range(-3.5f, 3.5f));
                GetComponent<Rigidbody2D>().velocity = Vector2.right * Random.Range(3, Main.fishSpeed);//3,10

                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
        }
        GetComponent<Animator>().speed = Mathf.Pow(GetComponent<Rigidbody2D>().velocity.x/4,2);
        /*float scale = 0;
        switch (Random.Range(0, 2))
        {
            case 0: scale = Random.Range(1, 3) * 0.3f; break;
            case 1: scale = Random.Range(5, 8) * 0.3f; break;
        }*/
        if (size_limit > 1.5f)
        {
            size_limit -= 0.2f;
        }
        float scale = Random.Range(0.5f, size_limit) * 0.15f;//0.3f(eski değer)
        Main main = GameObject.Find("Main Camera").GetComponent<Main>();

        if (main.big_fish > 165) { main.big_fish = 0; scale = 1f; }

        transform.localScale = new Vector2(scale, scale);
        if (ilk) { gameObject.AddComponent<PolygonCollider2D>(); }
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<PolygonCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().freezeRotation = true;
        ilk = false;

    }
    private void Start()
    {
        StartCoroutine("WaitAndStart");
        
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name=="Fish")
        {
            
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            Rigidbody2D rb2 = collision.gameObject.GetComponent<Rigidbody2D>();
            rb2.velocity = new Vector2(-rb2.velocity.x, rb2.velocity.y);
        }
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name=="Ring"|| collision.name == "Ring2" || collision.name == "Ring3") {
            in_the_line = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Ring" || collision.name == "Ring2" || collision.name == "Ring3")
        {
            in_the_line = false;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<PolygonCollider2D>().enabled = true;
        }
        else if (Input.GetMouseButtonUp(0)) {
            Invoke("enabled_false", 0.1f);
            
        }
        //rigidbodyye vector.left eklenebiliyor eski projede kullan
        //rigidbody change rotation
    }
    public void enabled_false() {
        GetComponent<PolygonCollider2D>().enabled = false;
    }
    private void OnBecameInvisible()
    {
        Start();
    }
}
