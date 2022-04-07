using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour {
    public List<ground> gl = new List<ground>();
    public Rigidbody2D r;

    void Start () {
        for (byte i = 0; i < 5; i++)
        {
            gl.Add(new ground(i, 5 - 2.5f * (i - 1) - 1));
        }
    }
	
	
	void Update () {
		
	}
}
