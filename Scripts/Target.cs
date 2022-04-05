using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    int time = 20, time2;
    public Attackable self;
    public GameObject target;
    public float AttackTime = 15;
    bool FirstHit = false;
    public int TargetStage = 0;
    public bool TowerColliderDid = false;
    public bool SoldierColliderDid = false;
    public bool LockTarget = false;
    //bool TargetFound = false;
    public bool OwnPath = false;
    public GameObject MainCamera0;
    public bool TargetDestroied;
    bool tower = false;
    void Start()
    {
        TargetStage = 0;
        MainCamera0 = GameObject.Find("Main Camera");//Towerları pool edince, maincamarayı referansla ve bu satırı sil
        //if (gameObject.tag == "Soldier")
        //{
        //    Self.attackPower = 1;
        //    Self.attackSpeed = 1000;
        //    Self.life = 10;
        //    Debug.Log("Soldier 1");
        //}
        //else
        if (gameObject.tag == "Tower")
        {

            //transform.Find("Life").transform.Find("Canvas").GetComponentsInChildren<Transform>(true);
            
            tower = true;

            //TargetFound = true;

            //Debug.Log("Tower 1");
        }
        //else if(gameObject.tag == "TowerSoldier")
        //{

        //}
    }
    
    void Update()
    {
        time++;
        if (time >= 10)
        {
            time = 0;
            time2++;
            
            if (gameObject.transform.parent&& self.life <= 0.01f)
            {
                transform.parent.gameObject.SetActive(false);
                if (!GetComponent<PathFind>())
                {
                    transform.parent.Find("Life").gameObject.SetActive(false);
                    if (transform.parent.tag != "King")
                    {
                        transform.parent.Find("Target Field").gameObject.SetActive(false);
                    }
                    if (MainCamera0.GetComponent<Main_Camera>().TargetTower == target)
                    {
                        MainCamera0.GetComponent<Main_Camera>().TargetTower = null;
                    }
                }
            }
            if (target && target.GetComponent<Target>().self.life <= 0.01f || transform.parent.name == "Gaint(Clone)" && GetComponent<PathFind>().TargetTower && GetComponent<PathFind>().TargetTower.GetComponent<Target>().self.life <= 0.01f)
            {
                //ballun colliderını büyüt, büyütme
                //lifeı ve targetfieldı false yap****
                //target.transform.parent.Find("Target Field").gameObject.SetActive(false);
                
                target = null;
                FirstHit = false;
                //TargetFound = false;
                if (GetComponent<PathFind>())
                {
                    Debug.Log("öldü");
                    GetComponent<PathFind>().TargetTower = null;
                    GetComponent<PathFind>().TargetEdgeFound = false;
                    if (transform.parent.name != "Gaint(Clone)")
                    {
                        StartCoroutine(WaitandFind());
                    }
                    else
                    {
                        GetComponent<PathFind>().anim.SetBool("Attacking", false);
                        GetComponent<PathFind>().anim.SetBool("Running", true);
                    }
                    
                    
                }
            }
            
            if (time2 >= AttackTime)
            {
                time2 = 0;

                //if (tag == "Tower" && transform.parent.tag != "King")
                //{
                //    GameObject targetField = transform.parent.transform.Find("Target Field").gameObject;
                //    List<GameObject> Soldiers = targetField.GetComponent<TargetField>().TargetSoldiers;
                //    GameObject closest = null;
                //    float beforeDistance = 1000;
                //    foreach (GameObject Soldier in Soldiers)
                //    {

                //        float distance = Vector3.Distance(transform.position, Soldier.transform.position);
                //        Debug.Log(distance);
                //        if (distance < beforeDistance)
                //        {
                //            beforeDistance = distance;
                //            closest = Soldier;
                //        }
                //    }
                //    if (closest != null)
                //    {
                //        TowerField();
                //    }
                //    target = closest;




                //}



                if (target != null/* && FirstHit*/)
                {
                    if (!FirstHit)
                    {
                        FirstHit = true;
                        if (GetComponent<PathFind>())
                        {
                            GetComponent<PathFind>().TargetEdgeFound = false;
                        }
                    }
                    
                    
                    if (self.life>0.01)
                    {
                        if (GetComponent<Thrower>())
                        {
                            GetComponent<Thrower>().ThrowArrow();
                            
                        }
                        else
                        {
                            //Debug.Log(gameObject.tag + "Other.life" + self.life);
                            //Debug.Log("Self.attackPower" + self.attackPower);
                            Hit(1);

                        }
                        //Debug.Log(name + " - Timer");
                    }


                }
                if (target == null/* && !TargetFound*/)
                {
                    if (tag == "Soldier")
                    {
                        //if (transform.parent.name == "Gaint(Clone)")
                        //{
                        //    Debug.Log("Gaint");
                        //    GetComponent<PathFind>().b = false;
                        //    GetComponent<PathFind>().ThereisMainTarget = false;
                        //}
                        //TargetFound = true;
                        FirstHit = false;
                        if (GetComponent<PathFind>())
                        {
                            //Debug.Log("Hedef seçildi");
                            if(transform.parent.name != "Gaint(Clone)")
                            {
                                GetComponent<PathFind>().FindClosestEdge();
                            }
                            GetComponent<PathFind>().TargetEdgeFound = false;

                        }
                    }
                    else if (tag == "Tower" && transform.parent.tag != "King")
                    {
                        //GameObject targetField = transform.parent.transform.Find("Target Field").gameObject;
                        //List<GameObject> Soldiers= targetField.GetComponent<TargetField>().TargetSoldiers;
                        //GameObject closest = null;
                        //float beforeDistance = 1000;
                        //foreach (GameObject Soldier in Soldiers)
                        //{

                        //    float distance = Vector3.Distance(transform.position, Soldier.transform.position);
                        //    Debug.Log(distance);
                        //    if (distance < beforeDistance)
                        //    {
                        //        beforeDistance = distance;
                        //        closest = Soldier;
                        //    }
                        //}
                        //if (closest != null)
                        //{
                        //    TowerField();
                        //}
                        //target = closest;




                        //GameObject targetField = transform.parent.transform.Find("Target Field").gameObject;
                        //targetField.SetActive(false);
                        //targetField.SetActive(true);

                        //Physics.IgnoreCollision(targetField.GetComponent<Collider>(), transform.parent.Find("Ignore Field"). GetComponent<Collider>(), true);

                    }
                    
                    
                    
                }
                
            }
            


        }
    }
    
    IEnumerator WaitandFind()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<PathFind>().FindClosestEdge();
        //self.attackPower = MainCamera0.GetComponent<Main_Camera>().TowerAttackable.attackPower;
        //self.attackSpeed = MainCamera0.GetComponent<Main_Camera>().TowerAttackable.attackSpeed;
        //self.life = MainCamera0.GetComponent<Main_Camera>().TowerAttackable.life;
    }
    public void Hit(float product)
    {
        if (target)
        {
            target.GetComponent<Target>().self.life = target.GetComponent<Target>().self.life - self.attackPower * product;


        }
    }
    public void HitThis(float product, float opponentAtPower)
    {
        self.life = self.life - opponentAtPower * product;
        

        
    }
    public void TowerField()
    {
        FirstHit = true;
    }
    public void DetectTarget(GameObject other)
    {
        //Debug.Log(tag + " 00000000000000000000000000");
        bool Gaint = gameObject.transform.parent.name == "Gaint(Clone)"/*&& Main_Camera.GetComponent<Main_Camera>().TargetTower*/;
        //Debug.Log("Gaint : " + Gaint);
        
        if (!Gaint ||
            Gaint && GetComponent<PathFind>().TargetTower != null && GetComponent<PathFind>().TargetTower.transform.parent.gameObject.name == other.gameObject.transform.parent.name)
        {
            //Debug.Log("TargetTower " + GetComponent<PathFind>().TargetTower.transform.parent.gameObject.name);
            //Debug.Log("other " + other.gameObject.transform.parent.name);
            target = other.gameObject;
            AttackTime = 100f / self.attackSpeed;

            FirstHit = true;
            Animator anim = GetComponent<PathFind>().anim;
            anim.SetBool("Attacking", true);
            anim.SetBool("Running", false);
            //Debug.Log("SoldierDetectedTarget");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (target == null)
        {

            if (other.tag == "Tower" && tag == "Soldier")
            {
                DetectTarget(other.gameObject);

                
            }
            if(tag == "Tower"&&other.tag=="Soldier")
            {
                
            }
        }
        else
        {
            if (tag == "Tower"/* && transform.parent.tag == "Cannon Ball" && Vector3.Distance(transform.position, other.transform.position) <= 3f*/)
            {
                //TargetSoldiers.Add(other.gameObject);
                //target = null;
            }
        }



    }
    
    IEnumerator WaitTargetStage()
    {
        yield return new WaitForSeconds(0.15f);
        TargetStage = 2;
    }
    //Other = null;
}
