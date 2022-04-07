using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCamera : MonoBehaviour
{
    public static int active_motor_number;
    public static int active_battery_field_number;
    public int time;
    public static GameObject[] motor;
    GameObject[] battery_field;
    public static float friction;
    public static Canvas ButtonDesigner;
    public Canvas ButtonDesigner0;
    
    public static int buttonsCount;
    public GameObject Canvas;
    private void Start()
    {
        active_motor_number = 1;
        active_battery_field_number = 1;
        friction = 1f;
        ButtonDesigner = ButtonDesigner0;
        motor = GameObject.FindGameObjectsWithTag("Motor");
        battery_field = GameObject.FindGameObjectsWithTag("Battery Field");
    }
    void Update()
    {
        time++;
        if (time >= 5) 
        {
            time = 0;
            motor = GameObject.FindGameObjectsWithTag("Motor");
            battery_field = GameObject.FindGameObjectsWithTag("Battery Field");

            for (int i = 1; i < motor.Length+1; i++) 
            {
                motor[i-1].name = "Motor" + i;
            }
            for (int i = 1; i < battery_field.Length + 1; i++)
            {
                battery_field[i - 1].transform.parent.name = "Battery" + i;
            }
            active_motor_number++;
            for (int i = 0; i < battery_field.Length; i++)
            {
                BatteryFieldScript(i).collided_with_motor = true;
            }
            if (active_motor_number > motor.Length)
            {
                active_battery_field_number++;
                for (int i = 0; i < motor.Length; i++) 
                {
                    MotorScript(i).collided_with_battery_field = true;
                    
                }
                
                active_motor_number = 1;
                

                if (active_battery_field_number > battery_field.Length)
                {
                    
                    for (int i = 0; i < motor.Length; i++) 
                    {
                        MotorScript(i).calculated_collided_battery_field_number =
                            MotorScript(i).collided_battery_field_number;
                        MotorScript(i).collided_battery_field_number = 0;
                        
                    }
                    for (int i2 = 0; i2 < battery_field.Length; i2++)
                    {


                        
                        BatteryFieldScript(i2).calculated_collided_motor_number =
                            BatteryFieldScript(i2).collided_motor_number/ battery_field.Length;

                        
                        BatteryFieldScript(i2).collided_motor_number = 0;
                        //Debug.Log(battery_field[i2].transform.parent.name + " : " + BatteryFieldScript(i2).calculated_collided_motor_number);
                    }
                    
                    active_battery_field_number = 1;
                }
                //Debug.Log(motor[active_battery_field_number].transform.name + " : " + MotorScript(active_battery_field_number).calculated_collided_battery_field_number);
                    //Debug.Log(battery_field[active_motor_number].transform.name + " : " + BatteryFieldScript(active_motor_number).calculated_collided_motor_number);

            }
        }
        
    }
    Motor MotorScript(int i) {
        return motor[i].GetComponent<Motor>();
    }
    BatteryField BatteryFieldScript(int i2)
    {
        return battery_field[i2].GetComponent<BatteryField>();
    }
    
    
}

//çoğu hesaplama main camera üzerinden motorların ve bayaryaların hepsi tarandıktan sonra yapılmalı yani TODO'da