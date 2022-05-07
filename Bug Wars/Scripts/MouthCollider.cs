using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthCollider : MonoBehaviour
{
    Spider SpiderScript;
    Collider c;
    LayerMask contactLayer;
    private void Awake()
    {
        SpiderScript = transform.parent.GetComponent<Spider>();
        c = GetComponent<Collider>();
        contactLayer = SpiderScript.contactLayer;
    }

    //IEnumerator LegActive(GameObject g)
    //{
    //    yield return new WaitForSeconds(0.01f);
    //    g.GetComponent<BugLeg>().enabled = false;
    //}
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("vvv"+other.gameObject.activeInHierarchy);
        if (other.transform.name == "Defence Collider")
        {
            Debug.Log("Blocked!");
            StartCoroutine(SpiderScript.Message("Blocked!"));
            Spider OtherSpiderScript = other.GetComponentInParent<Spider>();
            //other.transform.root.Find("Tarantula").GetComponent<Spider>()
            SpiderScript.RecoilJump(OtherSpiderScript.transform.position, 0.15f);
            OtherSpiderScript.Hit = true;
            SpiderScript.Hit = true;

            c.enabled = false;

        }
        else if (other.transform.tag == "Leg" && other.gameObject.activeInHierarchy)
        {
            Debug.Log("Leg life taken!");

            //Bunu yap:
            //walkCollider.radius = 0.5f;
            //walkCollider.transform.position = new Vector3(0, 0.1f, 0);


            StartCoroutine(SpiderScript.Message("Leg life taken!"));
            SpiderScript.Hit = true;
            Spider OtherSpiderScript = other.GetComponentInParent<Spider>();
            //bu tarz tanımlamalar optimize olmalı her spiderda olacak o ulaşacak
            //other.transform.tag = "Untagged";

            //OtherSpiderScript.subjectCollider.enabled = false;


            //OtherSpiderScript.LegsActive(false);







            //GameObject g = other.gameObject;
            //for (int i = 0; g.transform.childCount != 0; i++)
            //{
            //    g = g.transform.GetChild(0).gameObject;
            //}
            //BugLeg leg = g.GetComponent<BugLeg>();

            OtherSpiderScript.LegHealthDown(other.gameObject.GetComponentInChildren<BugLeg>(), 3);
            Death(OtherSpiderScript);


            c.enabled = false;



        }
        else if (other.transform.name == "tarantula_Abdomen_bone")
        {
            SpiderScript.Hit = true;
            Spider OtherSpiderScript = other.GetComponentInParent<Spider>();


            Debug.Log("Deadly hit!");
            StartCoroutine(SpiderScript.Message("Deadly hit!"));

            SpiderScript.BloodSplash(OtherSpiderScript);
            OtherSpiderScript.LegsHealthDown();
            Death(OtherSpiderScript);
            OtherSpiderScript.RecoilJump(SpiderScript.transform.position, 0.15f);
            c.enabled = false;
            //RecoilJump(OtherSpiderScript, SpiderScript.transform.position);

        }
        else if ((1 << other.gameObject.layer) == contactLayer && !SpiderScript.Hit)
        {
            SpiderScript.Hit = true;
            Debug.Log("Hit failed!");
            StartCoroutine(SpiderScript.Message("Hit failed!"));
            SpiderScript.RecoilJump(other.transform.position, 0.15f);
            c.enabled = false;
        }
    }
    private void Death(Spider OtherSpiderScript)
    {
        if (!OtherSpiderScript.AI && OtherSpiderScript.Powers[0].slider.value <= 0)
        {
            OtherSpiderScript.gameObject.SetActive(false);
            StartCoroutine(OtherSpiderScript.Death(/*SpiderScript*/));
        }
    }

    //IEnumerator Death(Spider OtherSpiderScript)
    //{
    //    StartCoroutine(SpiderScript.Message("He is dead!"));
        
    //    OtherSpiderScript.transform.Find("Tarantula_mesh").GetComponent<SkinnedMeshRenderer>().material = OtherSpiderScript.Black;
    //    OtherSpiderScript.WalkingModifier = 0;
    //    OtherSpiderScript.turningModifier = 0;
    //    yield return new WaitForSeconds(4);
    //    OtherSpiderScript.gameObject.SetActive(false);
    //}
    //private void BloodSplash(Spider spider)
    //{
    //    float angle = Vector3.Angle(spider.transform.position, SpiderScript.transform.position)/*Mathf.Atan2(hit.normal.x, hit.normal.z) * Mathf.Rad2Deg + 180*/;

    //    if (effectIdx == BloodFX.Length) effectIdx = 0;

    //    var instance = Instantiate(BloodFX[effectIdx], spider.transform.position, Quaternion.Euler(0, angle + 90, 0));
    //    effectIdx++;

    //    var settings = instance.GetComponent<BFX_BloodSettings>();

    //    settings.LightIntensityMultiplier = DirLight.intensity;


    //    var nearestBone = GetNearestObject(spider.transform, spider.transform.position);
    //    if (nearestBone != null)
    //    {
    //        var attachBloodInstance = Instantiate(BloodAttach);
    //        var bloodT = attachBloodInstance.transform;
    //        bloodT.position = spider.transform.position;
    //        bloodT.localRotation = Quaternion.identity;
    //        bloodT.localScale = Vector3.one * Random.Range(0.75f, 1.2f);
    //        bloodT.LookAt(spider.transform.position + transform.forward, direction);
    //        bloodT.Rotate(90, 0, 0);
    //        bloodT.transform.parent = spider.transform;//nearestBone
    //    }

    //}
    //private void RecoilJump(Spider spider, Vector3 MainLegPos)
    //{

    //    SpiderScript.EnergyConsumer += 0.1f;
    //    spider.EnergyConsumer -= 0.1f;
    //    if (spider.EnergyConsumer <= 0)
    //    {
    //        spider.EnergyConsumer = 0;
    //    }

    //    spider.transform.position += spider.transform.up * 0.9f;

    //    spider.Speed = (spider.transform.position - MainLegPos).normalized * 0.2f + spider.transform.up * 0.007f;


    //    //OtherSpiderScript.walkCollider.enabled = true;



    //    //for (int i = 0; i < 50; i++)
    //    //{
    //    //    yield return new WaitForSeconds(0.005f);


    //    //    //OtherSpiderScript.transform.position += (MainLegPos - SpiderScript.transform.position) / 10f + OtherSpiderScript.transform.up / 100f/*+ Vector3.down * OtherSpiderScript.FallSpeed*/;
    //    //    //OtherSpiderScript.transform.Translate();
    //    //    //OtherSpiderScript.FallSpeed += OtherSpiderScript.gravityModifier;
    //    //}
    //    spider.LegsActive(true, true);
    //}
    //IEnumerator RecoilJump0(Spider spider, Vector3 MainLegPos)
    //{
        



    //    SpiderScript.EnergyConsumer += 0.1f;
    //    spider.EnergyConsumer -= 0.1f;
    //    if (spider.EnergyConsumer <= 0)
    //    {
    //        spider.EnergyConsumer = 0;
    //    }
    //    //OtherSpiderScript.walkCollider.enabled = false;

    //    yield return new WaitForSeconds(0.01f);

    //    spider.transform.position += spider.transform.up * 0.9f;

    //    spider.Speed = (spider.transform.position - MainLegPos).normalized * 0.2f + spider.transform.up * 0.007f;


    //    //OtherSpiderScript.walkCollider.enabled = true;



    //    //for (int i = 0; i < 50; i++)
    //    //{
    //    //    yield return new WaitForSeconds(0.005f);


    //    //    //OtherSpiderScript.transform.position += (MainLegPos - SpiderScript.transform.position) / 10f + OtherSpiderScript.transform.up / 100f/*+ Vector3.down * OtherSpiderScript.FallSpeed*/;
    //    //    //OtherSpiderScript.transform.Translate();
    //    //    //OtherSpiderScript.FallSpeed += OtherSpiderScript.gravityModifier;
    //    //}
    //    spider.LegsActive(true, true);
    //}





    //public bool InfiniteDecal;
    //public Light DirLight;
    //public bool isVR = true;
    //public GameObject BloodAttach;
    //public GameObject[] BloodFX;


    //Transform GetNearestObject(Transform hit, Vector3 hitPos)
    //{
    //    var closestPos = 100f;
    //    Transform closestBone = null;
    //    var childs = hit.GetComponentsInChildren<Transform>();

    //    foreach (var child in childs)
    //    {
    //        var dist = Vector3.Distance(child.position, hitPos);
    //        if (dist < closestPos)
    //        {
    //            closestPos = dist;
    //            closestBone = child;
    //        }
    //    }

    //    var distRoot = Vector3.Distance(hit.position, hitPos);
    //    if (distRoot < closestPos)
    //    {
    //        closestPos = distRoot;
    //        closestBone = hit;
    //    }
    //    return closestBone;
    //}

    //public Vector3 direction;
    //int effectIdx;
    


    //public float CalculateAngle(Vector3 from, Vector3 to)
    //{

    //    return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;

    //}
}
