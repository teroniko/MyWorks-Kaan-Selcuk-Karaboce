using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Soldier")
        {
            //Debug.Log("ignore " + transform.parent.name);

            transform.parent.GetComponent<TargetField>().TargetSoldiers.Remove(other.gameObject);
        }
    }
}
