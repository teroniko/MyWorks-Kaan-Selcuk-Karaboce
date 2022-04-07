using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] Bugs;
    private void Start()
    {
        Bugs = GameObject.FindGameObjectsWithTag("Player");
        //Time.timeScale = 0.5f;
    }
    
}
