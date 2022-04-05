using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public int objectnumber;
    public int objectcount;
    public List<GameObject> Objects;
    
    public ObjectPool(GameObject Object, Attackable ObjectAttackable, int count)
    {
        Objects = new List<GameObject>();
        objectcount = count;
        
        {
            for (int i = 0; i < objectcount; i++)
            {
                if (Main_Camera.ObjectType==1)
                {
                    GameObject g = UnityEngine.Object.Instantiate(Object);
                    g.transform.Find("Soldier").GetComponent<Target>().self = new Attackable();


                    g.transform.Find("Soldier").GetComponent<Target>().self.attackPower = ObjectAttackable.attackPower;
                    g.transform.Find("Soldier").GetComponent<Target>().self.attackSpeed = ObjectAttackable.attackSpeed;
                    g.transform.Find("Soldier").GetComponent<Target>().self.life = ObjectAttackable.life;
                    g.transform.Find("Soldier").GetComponent<Target>().self.firstLife = ObjectAttackable.firstLife;
                    g.SetActive(false);
                    Objects.Add(g);
                }
                else if (Main_Camera.ObjectType == 2)
                {
                    GameObject g = UnityEngine.Object.Instantiate(Object);
                    g.transform.Find("Tower").GetComponent<Target>().self = new Attackable();


                    g.transform.Find("Tower").GetComponent<Target>().self.attackPower = ObjectAttackable.attackPower;
                    g.transform.Find("Tower").GetComponent<Target>().self.attackSpeed = ObjectAttackable.attackSpeed;
                    g.transform.Find("Tower").GetComponent<Target>().self.life = ObjectAttackable.life;
                    g.transform.Find("Tower").GetComponent<Target>().self.firstLife = ObjectAttackable.firstLife;

                    g.SetActive(false);
                    Objects.Add(g);
                    
                }


                    
            }
        }


        //for (int i = 0; i < objectcount; i++)
        //{
        //    GameObject g = UnityEngine.Object.Instantiate(Object);
            
            

        //    g.GetComponent<Target>().self = new Attackable();


        //    g.GetComponent<Target>().self.attackPower = ObjectAttackable.attackPower;
        //    g.GetComponent<Target>().self.attackSpeed = ObjectAttackable.attackSpeed;
        //    g.GetComponent<Target>().self.life = ObjectAttackable.life;

        //    g.SetActive(false);
        //    Objects.Add(g);
        //}
    }
    
    public GameObject ActivateObject(Vector3 position)
    {
        GameObject g = Objects[objectnumber];
        objectnumber++;
        //if (objectnumber >= objectcount) { objectnumber = 0; }
        g.SetActive(true);
        g.transform.position = position;
        g.transform.rotation = Quaternion.identity;
        return g;
    }
    
}
