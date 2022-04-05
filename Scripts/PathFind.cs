using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFind : MonoBehaviour
{
    int time = 0;
    public NavMeshAgent navMeshAgent;
    public Animator anim;
    public GameObject MainCamera0;
    public GameObject TargetTower;
    public bool TargetEdgeFound = false;
    public bool TargetDetected = false;
    void Awake()
    {
        TargetEdgeFound = false;
        MainCamera0 = GameObject.Find("Main Camera");//sil yada yeşile dönüştür
    }
    private void Update()
    {
        time++;
        if (time > 15)
        {
            time = 0;
            if(transform.parent.name == "Gaint(Clone)")
            {
                if (TargetTower == null) { OwnPath(); }

                if (/*MainCamera0.GetComponent<Main_Camera>().TargetTower != null &&*/!TargetEdgeFound && TargetTower != null
                   && !navMeshAgent.isStopped && navMeshAgent.path.corners.Length <= 3 /*&& GetComponent<Target>().OwnPath*/)
                {
                    //int layerMask = 1 << 11;

                    //layerMask = ~layerMask;
                    //int layer = 11;
                    Vector3 distance = TargetTower.transform.position - transform.position;
                    RaycastHit[] hits = Physics.RaycastAll(transform.position, distance, 500/*Vector3.Distance(transform.position,distance)*/);

                    for (int i = hits.Length - 1; i >= 0; i--)
                    {
                        if (hits[i].transform.tag=="Tower"
                       )
                        {
                            //Debug.Log(hit.transform.parent.name + " " + TargetTower.transform.parent.name);
                            
                            {
                                navMeshAgent.destination = hits[i].point - new Vector3(0.3f, 0.3f, 0.3f);
                                GetComponent<Target>().OwnPath = false;
                                TargetEdgeFound = true;
                                Debug.Log("hit.point : " + hits[i].point);
                                Debug.Log("TargetEdgeFound");
                                break;
                            }

                        }
                    }

                    //RaycastHit hit;//GetComponent<CapsuleCollider>().radius
                    //    if (Physics.Raycast(transform.position+ 0.1f * (TargetTower.transform.position - transform.position), TargetTower.transform.position - transform.position, out hit, 10000, layerMask)
                    //   )
                    //{
                    //    //Debug.Log(hit.transform.parent.name + " " + TargetTower.transform.parent.name);
                    //    if (hit.transform.parent.tag == "Tower"&&hit.transform.parent.name == TargetTower.transform.parent.name
                    //        )
                    //    {
                    //        navMeshAgent.destination = hit.point - new Vector3(0.3f, 0.3f, 0.3f);
                    //        GetComponent<Target>().OwnPath = false;
                    //        TargetEdgeFound = true;
                    //        Debug.Log("hit.point : " + hit.point);
                    //        Debug.Log("TargetEdgeFound");
                    //    }

                    //}

                }

            }
            

        }
        
    }
    public void OwnPath()
    {
        if (MainCamera0.GetComponent<Main_Camera>().TargetTower)
        {
            TargetTower = MainCamera0.GetComponent<Main_Camera>().TargetTower;
            navMeshAgent.destination = TargetTower.transform.position;
            Debug.Log("TargetTower.transform.position : "+ TargetTower.transform.position);
        }
        


        //if (Vector3.Distance(TargetTower.transform.position/*pathEndPosition*/, transform.position) < distance && Physics.Raycast(transform.position, TargetTower.transform.position - transform.position, out hit, 100000)/* && hit.transform.gameObject.tag == "Tower"*/&& hit.transform.gameObject.name == TargetTower.name)
        //{
        //    Debug.Log("Right Path");
        //    navMeshAgent.destination = hit.point + (transform.localScale - Vector3.one/*/5*/) / 2;

        //}
    }
    public void FindClosestEdge()
    {
        NavMeshHit hit;
        if (navMeshAgent.isActiveAndEnabled&&navMeshAgent.FindClosestEdge(out hit))
        {

            navMeshAgent.destination = hit.position;
            anim.SetBool("Attacking", false);
            anim.SetBool("Running", true);
        }



    }
}
