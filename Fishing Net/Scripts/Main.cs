using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
public class Net_and_Ring :MonoBehaviour
{
    public GameObject net, ring;
    public Net_and_Ring(GameObject Net, GameObject Ring)
    {
        net = Net;
        ring = Ring;
    }
    private void Update()
    {
        
    }
}
*/
public class Main : MonoBehaviour {
    //public List<Net_and_Ring> Net_and_Ring_List=new List<Net_and_Ring>();
    public GameObject Shape, Net_Size, Circle_Net, Score, Net;
    public byte shape;
    public float time;
    public float high_score,score;
    public static float score0,fishSpeed=5;
    public float net_size = 8;
    float speedUp;
    
    public int big_fish;
    //static olarak değiştir
    void Start () {
        for (byte i = 0; i < 7; i++) {
            GameObject fish = new GameObject("Fish");
            fish.AddComponent<Fish>();
            
            //fish.GetComponent<Animator>().updateMode= AnimatorUpdateMode.AnimatePhysics;
            //fish.GetComponent<Animator>().Play("fish_FINAL");
        }
        /*for (byte b = 0; b < 3; b++) {
            Net_and_Ring_List.Add(new Net_and_Ring(Net, Ring));
        }
        */
    }
    private void FixedUpdate()
    {
        score += 0.1f;
        Score.GetComponent<Text>().text = "Score : " + (int)score;
        high_score = PlayerPrefs.GetInt("highscore", (int)high_score);
        if (score > high_score) {
            high_score = score;
            PlayerPrefs.SetInt("highscore", (int)high_score);
        }
        big_fish++;
    }
    void Update () {
        net_size -= 0.005f;
        Net_Size.transform.localScale = new Vector2(net_size, Net_Size.transform.localScale.y);
        if (net_size<=0)
        {
            score0 = score;
            SceneManager.LoadScene("Menu");
            score = 0;
        }
        speedUp++;
        if (speedUp>400) {
            speedUp = 0;
            if (fishSpeed < 10) {
                fishSpeed++;
            }
        }
        /*time += Time.deltaTime * 70;
        if (time>40) {

            GameObject fish = GameObject.Find("Fish");
            fish.GetComponent<Fish>().Start();
            time = 0;
            //GameObject fish = new GameObject("Fish");
            //fish.AddComponent<Fish>();
        }*/
        /*if (Input.GetMouseButtonDown(0))
        {
            switch (shape)
            {
                case 0: Circle_Net.GetComponent<CircleCollider2D>().enabled = true;
                    //Circle_Net.GetComponent<CircleCollider2D>().radius = 0;
                    break;
                case 1: Circle_Net.GetComponent<BoxCollider2D>().enabled = true; break;
            }
            
            Circle_Net.GetComponent<SpriteRenderer>().enabled = true;
            Circle_Net.GetComponent<Net>().shape = shape;
            
        }*/
    }
    public void ChangeShape()
    {
        shape++;
        if (shape > 1) { shape = 0; }
        switch (shape)
        {
            case 0: Shape.GetComponent<Text>().text = "Circle Net"; break;
            case 1: Shape.GetComponent<Text>().text = "Square Net"; break;
        }
        
    }
}
