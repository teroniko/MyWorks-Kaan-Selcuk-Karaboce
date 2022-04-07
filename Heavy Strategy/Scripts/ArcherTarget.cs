using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTarget : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tower")
        {
            //if (Vector3.Angle(other.transform.position - transform.position, transform.forward) < 1)
            {
                transform.parent.Find("Soldier").GetComponent<Target>().DetectTarget(other.gameObject);
            }
        }
    }
}
