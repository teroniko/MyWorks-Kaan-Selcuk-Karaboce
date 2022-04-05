using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Net : MonoBehaviour {
    public byte shape = 0;
    public float net_scale_x, net_scale_y;
    public bool mouseup, catched;
    public GameObject Ring, other_net;
    public byte net_number;
    private void Update()
    {
        /*if (!catched)
            Mouse_Down();*/
        switch (net_number)
        {
            case 0:
                if (!catched)
                    Mouse_Down();
                break;
            case 1:
                if (other_net.GetComponent<Net>().catched & !catched)
                    Mouse_Down();

                break;
            case 2:
                if (other_net.GetComponent<Net>().catched & !catched)
                    Mouse_Down();

                break;
        }
        
    }
    void Mouse_Down() {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            net_scale_x = mouse_position.x;
            net_scale_y = mouse_position.y;
            transform.position = mouse_position;
            GetComponent<SpriteRenderer>().enabled = true;
            Ring.GetComponent<EdgeCollider2D>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
            transform.localScale = new Vector2(0, 0);
            InvokeRepeating("DefineScale", 0, 0.01f);
        }

        Ring.transform.position = transform.position;
        Ring.transform.localScale = new Vector2(transform.localScale.x * 1.2f, transform.localScale.y * 1.2f);

    }
    void DefineScale() {
        if (Input.GetMouseButton(0))
        {
            Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float hipotenus = (float)Math.Sqrt(Math.Pow((net_scale_x - mouse_position.x) / 5, 2) +
                Math.Pow((net_scale_y - mouse_position.y) / 5, 2));
            float max_hipotenus=0.16f * GameObject.Find("Main Camera").GetComponent<Main>().net_size+0.3f;
            if (hipotenus > max_hipotenus) { hipotenus = max_hipotenus; }
            transform.localScale = new Vector2(hipotenus, hipotenus);
        }
        else if (Input.GetMouseButtonUp(0))
        {


            
            CancelInvoke();
            /*
            switch (shape)
            {
                case 0:
                    GetComponent<CircleCollider2D>().enabled = true;
                    break;
                case 1:
                    GetComponent<BoxCollider2D>().enabled = true;
                    break;
            }*/
            Invoke("Destroy", 0.1f);
            mouseup = true;
            if (transform.localScale.x == 0 && transform.localScale.y == 0/*||!trigger*/)
            {
                finish();
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name=="Fish"&&!collision.GetComponent<Fish>().in_the_line) { 
        if (mouseup) {
            switch (shape)
            {
                case 0: GetComponent<CircleCollider2D>().enabled = false; break;
                case 1: GetComponent<BoxCollider2D>().enabled = false; break;
            }
            collision.GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
            collision.GetComponent<Animator>().enabled = false;
            Main m = GameObject.Find("Main Camera").GetComponent<Main>();
            catched = true;
            if (collision.transform.localScale.x < 0.34f)//0.6f(eski değer)
            {
                m.net_size -=4;
            }
            else {
                m.net_size += 1.5f;
            }
        }


        }
        //trigger = true;
    }
    void finish() {
        switch (shape)
        {
            case 0: GetComponent<CircleCollider2D>().enabled = false; break;
            case 1: GetComponent<BoxCollider2D>().enabled = false; break;
        }

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.position = new Vector2(16,0);
        Ring.GetComponent<EdgeCollider2D>().enabled = true;
        //trigger = false;
        mouseup = false;
        catched = false;
    }
    private void OnBecameInvisible()
    {
        finish();
    }
    void Destroy() {
        if (!catched) { finish(); }
        
    }
}