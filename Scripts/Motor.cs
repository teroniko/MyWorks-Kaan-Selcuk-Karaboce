using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Motor : MonoBehaviour
{
    //akım ile mototr çalışsın ama voltu yüksek olunca pervane büyüyünce işe yarasın
    Vector3 Size;
    public double required_volt;//power
    public double required_amper;//speed
    public float CurrentVersusVolt;
    public int collided_battery_field_number;
    public int calculated_collided_battery_field_number;
    public bool active;
    public GameObject Main_Camera;
    //double decrease_propeller_speed_number = 1/30f;
    GameObject CollidedBattery = null;
    public BatteryField bf = null;
    public Battery battery = null;
    public bool collided_with_battery_field;
    public bool exit_from_battery_field;
    public Transform Propeller;
    public Transform Wheel;
    public float turning_speed = 0;
    int calculate_total_turning_speed;
    float calculated_total_turning_speed;
    float calculated_total_turning_speed_decreasement;
    float total_turning_speed;
    public bool all_zero;
    int bf_calculated_collided_motor_number;
    public Canvas canvas;
    public int direction;
    public bool Turning;

    void Start()
    {
        Size = transform.localScale;
        required_volt = Size.x * Size.z / Size.y;
        required_amper = Size.y / (Size.x * Size.z);
        CurrentVersusVolt = 3;
        collided_with_battery_field = true;
        Propeller = transform.Find("Propeller1");
        Wheel = transform.Find("Wheel").transform.Find("default");
        //Debug.Log(Propeller.Find("default").GetComponent<Renderer>().bounds.size.x);


        direction = 1;
        Turning = false;
}
    
    void Update()
    {
        if (CollidedBattery != null)
        {
            
            turning_speed = 0;
            if (bf.calculated_collided_motor_number == 0)
            {
                bf_calculated_collided_motor_number = 1;
            }
            else {
                bf_calculated_collided_motor_number = bf.calculated_collided_motor_number;
            }
            /*if (bf.calculated_collided_motor_number == 0)
            {
                calculated_total_turning_speed = 0;
                calculated_total_turning_speed_decreasement = 0;
            }*/
            turning_speed = (float)((battery.amper * Mathf.Pow(2, CurrentVersusVolt)) * battery.volt *
                battery.calculate_cylinder_volume(transform.localScale.x, transform.localScale.y, transform.localScale.z) *
                GetComponent<Rigidbody>().mass * calculated_collided_battery_field_number *30/
                (required_volt * required_amper/*  *air friction*/* battery.GetComponent<Rigidbody>().mass *
                bf_calculated_collided_motor_number));
                calculate_total_turning_speed++;
                total_turning_speed += turning_speed;
                if (calculate_total_turning_speed >= MainCamera.motor.Length)
                {
                    
                    calculated_total_turning_speed = 0;
                    calculated_total_turning_speed = total_turning_speed / (MainCamera.motor.Length)/5;// bölü 50 ydi
                    calculate_total_turning_speed = 0;
                    total_turning_speed = 0;
                }
                //Debug.Log(transform.name + " Speed : " + calculated_total_turning_speed);
                calculated_total_turning_speed_decreasement = calculated_total_turning_speed;
                
            if (calculated_total_turning_speed >= 50) { calculated_total_turning_speed_decreasement = 50; }

            if (Turning) 
            {
                //Wheel.GetComponent<WheelCollider>().motorTorque = 400* Input.GetAxis("Vertical");
                //Wheel.GetComponent<WheelCollider>().steerAngle = 30 * Input.GetAxis("Horizontal");
                Wheel.GetComponent<Rigidbody>().angularVelocity += new Vector3(0, calculated_total_turning_speed/*_decreasement*/ * direction*10000, 0);
                //Wheel.GetComponent<Rigidbody>().AddTorque(transform.up * calculated_total_turning_speed/*_decreasement*/ * direction * 10000);
                //Wheel.GetComponent<Rigidbody>().AddTorque(transform.right*calculated_total_turning_speed/*_decreasement*/ * direction);
                //Propeller.Rotate(0, calculated_total_turning_speed/*_decreasement*/ * direction, 0);
                if (battery.capasity > 0) { battery.capasity -= battery.actual_mass / 1000f; } else { battery.capasity = 0; }
                
            }

            //Wheel.transform.Find("Sphere").transform.position
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.transform.parent.name+" "+ MainCamera.active_battery_field_number);
        if (other.transform.parent.name=="Battery" + MainCamera.active_battery_field_number && collided_with_battery_field)
        {
            collided_with_battery_field = false;
            collided_battery_field_number++;
            CollidedBattery = other.gameObject;
            battery = CollidedBattery.transform.parent.GetComponent<Battery>();
            bf = CollidedBattery.GetComponent<BatteryField>();
            if (battery.capasity <= 0)
            {
                battery.volt /= 2;
                battery.amper /= 10;
            }
        }
    }
    
    public void Button1Down()
    {
        if (ChangeGameMode.GameMode)
        {
            Turning = true;
        }
        else {  }
    }
    public void Button1Up()
    {
        if (ChangeGameMode.GameMode)
        {
            Turning = false;
        }
    }
    public void Button2() {
            if (ChangeGameMode.GameMode)
            {
                direction = 1;
        }
        else { }
    }
    public void Button3()
    {
        if (ChangeGameMode.GameMode)
        {
            direction = -1;
        }
        else { }
    }
    public void Button4()
    {
        if (ChangeGameMode.GameMode)
        {
            direction *= -1;
        }
        else { }
    }


    
}
