using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ground : MonoBehaviour {
    public List<block> bl = new List<block>();
    public byte empty, red1, red2;
    // Use this for initialization
    void Start () {
        for (byte i = 1; i < 7; i++)
        {
            block b = new block();
            b.yaratma(i, i2, height, empty, red1, red2);
            bl.Add(b);
        }
    }
}
