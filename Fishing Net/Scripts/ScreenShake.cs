using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    public Animator Camera;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //if (Net.caughtsmallfish)
        //{
            /* if ()
             {
                 //less shake
                 Camera.SetFloat("low_health", 1f);
             }
             else if ()
             {
                 //medium shake
                 Camera.SetFloat("low_health", 2f);
             }
             else if ()*/
            {
                //high shake
                Camera.SetFloat("low_health", 3f);
            }

            //Net.caughtsmallfish = false;
        //}
	}
    /*E. Sena==>> screenshake to warn you when you catch a small fish
    caughtsmallfish = true;*/
}
