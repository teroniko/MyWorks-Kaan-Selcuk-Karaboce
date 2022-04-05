using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryField : MonoBehaviour
{
    public int collided_motor_number;
    public int calculated_collided_motor_number;
    public bool collided_with_motor;
    public static int name_change;
    public bool active;
    //public bool dont_do_again = false;
    //batarya field kaç motor ile collide ediyor?
    /*motorun kendisi kaç tane ile collide ediyor ve batarya kaç tane ile collide ediyor?*/
    void Start()
    {
        Material material = gameObject.GetComponent<Renderer>().material;
        gameObject.GetComponent<Renderer>().material.color = new Color(material.color.r, material.color.g, material.color.b, 0.05f);
        collided_with_motor = true;
    }
    private void Update()
    {
        

    }
    private void OnTriggerStay(Collider other)
    {
        
        //tag ile sıra ile tarama yapılabilir mi düşün
        if (other.name == "Motor" + MainCamera.active_motor_number && collided_with_motor)
        {
            collided_with_motor = false;
            collided_motor_number++;
        }

    }
    
    //motorun uzunluğuna göre hızlanacak
    //pervanenin büyüklüğü hızı düşürecek
}
