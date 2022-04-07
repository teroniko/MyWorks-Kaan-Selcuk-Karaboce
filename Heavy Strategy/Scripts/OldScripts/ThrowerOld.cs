using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerOld : MonoBehaviour
{
    public GameObject Thrown;
    public GameObject Main_Camera;
    float ALP = 0;//ArrowLastPosition
    public float fallTime;//t
    int time = 0;
    float XorZspeed;
    public int arrownumber, arrowcount = 6;
    public List<GameObject> Throwns = new List<GameObject>();
    float h/*ArrowMaxHeight*/;
    float firstHeight = 1;
    //public Animator anim;
    
    void Start()
    {
        if (GetComponent<PathFind>())
        {
            GetComponent<PathFind>().anim.SetFloat("ShootMultiplier", GetComponent<Target>().self.attackSpeed / 8);
        }
        
    }
    private void Awake()
    {
        Main_Camera = GameObject.Find("Main Camera");
        
        if (transform.parent!=null&&transform.parent.tag == "Cannon Ball")
        {
            Thrown = Main_Camera.GetComponent<Main_Camera>().Ball;

        }
        else
        {
            Thrown = Main_Camera.GetComponent<Main_Camera>().Arrow;
        }
        for (int i = 0; i < arrowcount; i++)
        {
            GameObject g = Instantiate(Thrown);
            g.SetActive(false);
            if (g.GetComponent<Arrow>())
            {
                g.GetComponent<Arrow>().thw = GetComponent<Thrower>();
            }
            else
            {
                g.GetComponent<Ball>().thw = GetComponent<Thrower>();
            }
            
            //g.GetComponent<Arrow>().ar = GetComponent<Archer>();
            //g.name = "Arrow";
            //ballu yada arrowu child yap
            Throwns.Add(g);

        }
        firstHeight = 1;
        if (GetComponent<PathFind>())
        {
            firstHeight = 0.1f;
        }
        h/*ArrowMaxHeight*/ = 2.00f + fallTime * (fallTime + 2.00f) - 1.00f / (fallTime * (fallTime / 2.00f + 1.00f));
    }
    // Update is called once per frame
    void Update()
    {
        time++;
        if (time > 25)//500
        {
            if (GetComponent<PathFind>() && GetComponent<PathFind>().navMeshAgent.velocity.magnitude == 0 &&
                transform.parent.Find("Target") != null &&
                !transform.parent.Find("Target").GetComponent<CapsuleCollider>().enabled)
            {
                Debug.Log("Thrower Target Detected");
                transform.parent.Find("Target").GetComponent<CapsuleCollider>().enabled = true;
            }
            time = 0;
        }
    }
    public void ThrowArrow()
    {
        GameObject thrown = Throwns[arrownumber];
        arrownumber++;
        if (arrownumber >= arrowcount) { arrownumber = 0; }
        
        
        ALP = (Vector3.Distance(transform.position, new Vector3(GetComponent<Target>().target.transform.position.x, GetComponent<Target>().target.transform.position.y+firstHeight, GetComponent<Target>().target.transform.position.z)));
        
        thrown.transform.position = new Vector3(transform.position.x, transform.position.y+firstHeight/* +
            GetComponent<Collider>().bounds.size.y / 2.00f*/, transform.position.z);
        //arrow.transform.rotation = Quaternion.identity;
        //Debug.Log("alp : " + ALP);
        if(transform.parent.tag=="Cannon Ball")
        {
            if (ALP >= 7)
            {
                ALP *= 1.45f;
            }
            else if (ALP >= 13)
            {
                ALP *= 1.9f;
            }
        }
        
        XorZspeed = (ALP / fallTime);


        
        float Yspeed = h - fallTime * (fallTime / 2.00f + 1.00f);
        float x= (float.Parse(GetComponent<Target>().target.transform.position.x.ToString("f6")) - float.Parse(transform.position.x.ToString("f6"))) /20 * XorZspeed
            , y=Yspeed, z= (float.Parse(GetComponent<Target>().target.transform.position.z.ToString("f6")) - float.Parse(transform.position.z.ToString("f6")))/20 * XorZspeed;
        Vector3 Velocity = (GetComponent<Target>().target.transform.position - transform.position).normalized * XorZspeed;
        Velocity.y = Yspeed;
        thrown.GetComponent<Rigidbody>().velocity = Velocity;
        thrown.SetActive(true);

    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (target.Other == null && other.gameObject.tag == "Soldier")
    //    {
    //        //anim.SetBool("Attacking", true);
    //        //anim.SetBool("Running", false);
    //        target.Other = other.transform.gameObject;
    //        GetComponent<Warrior>().navMeshAgent.isStopped = true;
    //        
    //    }
    //    if (GetComponent<CapsuleCollider>())
    //    {

    //        GetComponent<CapsuleCollider>().enabled = false;
    //    }
    //}

}
