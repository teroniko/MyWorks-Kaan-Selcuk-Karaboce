using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackCollider : MonoBehaviour
{
    Spider SpiderScript;
    private void Awake()
    {
        SpiderScript = transform.parent.GetComponent<Spider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.name == "tarantula_Abdomen_bone")
        {
            StartCoroutine(SpiderScript.Attack(other.gameObject));
        }
    }
}
