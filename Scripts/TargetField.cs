using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetField : MonoBehaviour
{

    public List<GameObject> TargetSoldiers;
    private void Awake()
    {
        TargetSoldiers = new List<GameObject>();
    }
    private void Update()
    {
        List<GameObject> Soldiers = TargetSoldiers;
        GameObject closest = null;
        float beforeDistance = 1000;
        foreach (GameObject Soldier in Soldiers)
        {
            if (!Soldier.transform.parent)
            {
                Soldiers.Remove(Soldier);
            }
            float distance = Vector3.Distance(transform.position, Soldier.transform.position);
            
            if (distance < beforeDistance)
            {
                beforeDistance = distance;
                closest = Soldier;
            }
        }
        Target tar = transform.parent.Find("Tower").GetComponent<Target>();
        if (closest != null)
        {
            tar.TowerField();
        }
        tar.target = closest;
        tar.AttackTime = 100f / tar.self.attackSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Physics.IgnoreCollision(GetComponent<Collider>(), transform.parent.Find("Ignore Field").GetComponent<Collider>());
        if (other.tag == "Soldier"/* && transform.parent.transform.Find("Tower") != null &&
            transform.parent.transform.Find("Tower").gameObject.GetComponent<Target>().target == null*/)
        {

            TargetSoldiers.Add(other.gameObject);
            //Debug.Log(Vector3.Distance(transform.position, other.transform.position)+"k");

            //Target tar = transform.parent.transform.Find("Tower").gameObject.GetComponent<Target>();
            //tar.target = other.gameObject;
            //tar.TowerField();


            //Debug.Log("TowerDetectedTarget");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Soldier"/* && transform.parent.transform.Find("Tower") != null &&
            transform.parent.transform.Find("Tower").gameObject.GetComponent<Target>().target == null*/)
        {
            TargetSoldiers.Remove(other.gameObject);
            
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Soldier"&& other.gameObject.GetComponent<Target>().target!=null && other.gameObject.GetComponent<Target>().target.transform.parent.name ==
    //        transform.parent.gameObject.name)
    //    {
    //        //other.gameObject.GetComponent<Target>().target = null;
    //        Debug.Log("target = null");
    //    }
    //}
}
