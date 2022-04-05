using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "WalkCollider")
        {
            Spider spider = other.transform.parent.GetComponent<Spider>();
            spider.HealUp();

            //for(int i = 0; i < spider.TakenLegLimbs.Count; i++)
            //{
            //    GameObject limb = spider.TakenLegLimbs[i];
            //    limb.SetActive(true);
            //    limb.transform.position -= limb.transform.right * 0.025f;
                
            //    //if (limb.GetComponent<BugLeg>())
            //    //{
            //    //    limb.GetComponent<BugLeg>().enabled = true;
            //    //}
            //}
            //spider.TakenLegLimbs.Clear();



            //Transform Limbs = other.transform.parent;
            //Debug.Log(Limbs.name);
            //for(int i = 0; i < Limbs.childCount; i++)
            //{
            //    //if (Limbs.GetChild(i).name.Substring(12, 5) == "CoxaI")
            //    Transform child = Limbs.GetChild(i);
            //    if (child.name.Contains("CoxaI"))
            //    {
            //        if (child.)
            //        {
            //            Debug.Log("disabled");

            //        }
            //        //if (child.GetChild(0))
            //        //{
            //        //    for(int i2=0;i2<6)
            //        //}
            //    }
            //}
        }
    }
}
