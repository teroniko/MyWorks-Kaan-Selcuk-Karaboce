using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour {
    public GameObject g;
    public float f, f2;
    public bool b = true;
    public void yaratma(int i, int i2, float height, byte empty, byte red1, byte red2)
    {
        byte colorpick = 0;
        if (red1 == i | red2 == i) { colorpick = 1; }
        if (empty == i) { colorpick = 2; }
        g = new GameObject();
        g.name = i2 + "block";
        g.gameObject.AddComponent<SpriteRenderer>();
        g.transform.position = new Vector2(i * 5.4f / 5 - 5.4f / 10 - 2.7f, height);

        if (colorpick < 2)
        {

            color_pick(colorpick);
            g.AddComponent<BoxCollider2D>();
            g.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        /*g.AddComponent<Rigidbody2D>();
		g.GetComponent<Rigidbody2D> ().gravityScale = 0;
		g.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePosition|RigidbodyConstraints2D.FreezeRotation;
        */
    }
    void color_pick(byte colorpick)
    {
        switch (colorpick)
        {
            case 0:
                g.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("lavafloor2");
                g.transform.localScale = new Vector2(0.03f, 0.03f);
                break;
            case 1:
                g.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ice");
                g.transform.localScale = new Vector2(0.5f, 0.8f);
                break;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
