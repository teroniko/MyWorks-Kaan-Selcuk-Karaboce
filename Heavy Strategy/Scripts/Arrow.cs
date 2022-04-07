using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Thrower thw;
    float firsty;
    void Start()
    {
        firsty = transform.position.y;

        //Debug.Log("ArrowStart");
    }
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
        if (firsty-0.5f > transform.position.y)
        {
            thw.GetComponent<Target>().Hit(1f);
            gameObject.SetActive(false);

        }
    }
    private void OnTriggerStay(Collider other)
    {
        //if(other.gameObject.tag == thw.GetComponent<Target>().target.gameObject.tag)
        //if(other.gameObject!=null&&other.gameObject.tag != thw.gameObject.tag)
        //if (firsty<=transform.position.y)
        //{
        //    thw.GetComponent<Target>().Hit();
        //    gameObject.SetActive(false);

        //}
    }
}
