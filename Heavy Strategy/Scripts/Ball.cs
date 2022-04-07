using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Thrower thw;
    float firsty;
    
    void Start()
    {
        firsty = transform.position.y;
    }
    private void Update()
    {
        if (firsty - 0.1f > transform.position.y)
        {
            GameObject[] Soldiers = GameObject.FindGameObjectsWithTag("Soldier");
            //RaycastHit hit;
            //if (other.tag == "Soldier" &&
            //    Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, 2))
            //{
            //    float distance = Vector3.Distance(transform.position, other.transform.position - transform.position);
            //    if (distance >= 2.0f)
            //    {
            //        thw.GetComponent<Target>().Hit(1);
            //        //zaten collider var raycast ekleme
            //        Debug.Log("hit.distance : " + hit.distance);
            //    }
            //}
            foreach (GameObject Soldier in Soldiers)
            {
                RaycastHit hit;
                float explodeDistance = 1.5f;
                //LayerMask mask = LayerMask.GetMask("Soldier");
                if (Physics.Raycast(transform.position, Soldier.transform.position-transform.position, out hit, explodeDistance)
                   && Soldier.transform.parent.gameObject == hit.transform.parent.gameObject)
                //    int layerMask = 1 << 10;
                //if (Physics.Raycast(transform.position,Soldier.transform.position - transform.position,out hit, 3, layerMask))
                {
                    //geri itme yap
                    //atış hızı * soldier hızı bölgesine at
                    //collider engelliyor değil
                    Debug.Log("Ball");
                    Soldier.GetComponent<Target>().HitThis(hit.distance * 1.0f/*3.0f*/, thw.GetComponent<Target>().self.attackPower);
                }
            }
            gameObject.SetActive(false);



        }
    }
    //private void OnTriggerStay(Collider other)
    //{
        //RaycastHit hit;
        //if(other.tag=="Soldier"&&
        //    Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, 2))
        //{
        //    float distance = Vector3.Distance(transform.position, other.transform.position - transform.position);
        //    if (distance >= 2.0f)
        //    {
        //        thw.GetComponent<Target>().Hit(1);
        //        //zaten collider var raycast ekleme
        //        Debug.Log("hit.distance : "+hit.distance);
        //    }
        //}
        //gameObject.SetActive(false);
    //}
}
