using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    
    public Transform Camera;
    int time = 0;
    Attackable attackable;
    bool First = false;
    public Slider slider;
    GameObject Life;
    Target Target=null;
    bool ItsSoldier=false;
    
    IEnumerator Start0()
    {
        Life = transform.parent.parent.gameObject;
        //ItsSoldier = LifeCanvas.GetComponent<PathFind>() != null;
        yield return new WaitForSeconds(0.5f);
        
        if (transform.parent.parent.parent.Find("Soldier"))
        {
            //Soldier
            Target = transform.parent.parent.parent.Find("Soldier").gameObject.GetComponent<Target>();
            getAttackable(Life.transform.parent.Find("Soldier").gameObject);
            slider.value = attackable.life;
            First = true;
        }
        else if(!transform.parent.parent.parent.Find("Tower").GetComponent<PathFind>()&&transform.parent.parent.parent.Find("Tower").gameObject.GetComponent<Target>()
            )
        {
            if (!First)
            {
                Target = transform.parent.parent.parent.Find("Tower").gameObject.GetComponent<Target>();
                
                getAttackable(Life.transform.parent.Find("Tower").gameObject);
                Debug.Log("Tower Health Bar Active");
                First = true;
            }
            
        }
    }
    void Start()
    {
        
        //if (gameObject.activeSelf)
        //{
            
        //}
        StartCoroutine(Start0());

    }
    void getAttackable(GameObject g)
    {
        attackable = Target.GetComponent<Target>().self;
        slider.value = attackable.life;
        slider.maxValue = attackable.firstLife;
    }
    private void Update()
    {
        //if (gameObject.activeSelf)
        {
            time++;
            if (time >= 10 && First)
            {
                time = 0;

                slider.value = attackable.life;
                Life.transform.LookAt(Life.transform.position + Camera.forward);

                //Vector3 relativePos = Camera.position - transform.position;

                //Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                //transform.rotation = rotation;
                //Quaternion targetRot = Quaternion.LookRotation(Camera.transform.position - transform.position);
                //transform.rotation = targetRot;
            }
        }
        

    }
}
