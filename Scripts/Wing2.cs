using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing2 : MonoBehaviour
{
    Vector3 p;
    float alan;
    Rigidbody r;
    Vector3 real_scale;
    Vector3 direction;
    Quaternion q;
    Vector3 a;
    int vx = 60;
    Vector3 Angle;
    void Start()
    {
        p = transform.position;
        r = gameObject.GetComponent<Rigidbody>();
        real_scale = GetComponent<Renderer>().bounds.size;
        Angle = UnityEditor.TransformUtils.GetInspectorRotation(transform);
        direction = new Vector3(1, 1, 1);
        //r.velocity = new Vector3(r.velocity.x, r.velocity.y, r.velocity.x + 40);
    }

    void Update()
    {
        p = transform.position;
        r = GetComponent<Rigidbody>();
        a = UnityEditor.TransformUtils.GetInspectorRotation(transform);

        SetRotation_0_180();
        float z = air_friction_formula(r.velocity.y, friction_area_z());
        float y = air_friction_formula(r.velocity.y, friction_area_y());
        GetComponent<Rigidbody>().velocity = new Vector3(r.velocity.x,
             r.velocity.y-y/* - Mathf.Cos(air_friction_formula(r.velocity.y, friction_area(r.velocity.z, a.x, 1) * real_scale.z))*/
             , r.velocity.z - z);
        /*
         Mathf.Sin(direction.x * air_friction_formula(r.velocity.y, Mathf.Sin(degree * Mathf.Deg2Rad) * real_scale.z))
         direction.x * air_friction_formula(r.velocity.y, Mathf.Sin(degree * Mathf.Deg2Rad) * real_scale.z))
         */
    }
    float friction_area_z()
    {
        float j;
        float degree;
        float pisagor;
        pisagor = -Mathf.Sqrt(Mathf.Pow(r.velocity.z, 2) + Mathf.Pow(r.velocity.y, 2));
        j = asin(r.velocity.z, pisagor);
        Debug.Log(r.velocity.z + " " + pisagor);
        /*while (a.x >= 180)
        {
            a.x = a.x + -180;
        }
        while (a.x <= 0)
        {
            a.x = a.x + 180;
        }*/
        int direction = 1;
        float angle = a.x;
        if (angle < 0)
        {
            angle += 90;
            direction = 1;
        }
        else if (angle > 90)
        {
            angle -= 90;
            direction = -1;
        }
        int change_direction = 1;
        direction *= change_direction;
        degree = -j - angle;
        float k = degree;
        /*int directionn = 1;
        if (k < 0)
        {
            k += 90;
            directionn = 1;
        }
        else if (k > 90)
        {
            k -= 90;
            directionn = -1;
        }*/
        float m = sin(k)/**directionn*/ * real_scale.z;
        Debug.Log("fkgthjserıkghp "+ sin(j));
        return /*Mathf.Sin(degree * Mathf.Deg2Rad) * direction*/cos(j) * m * direction;//sin'i de dene
    }

    float friction_area_y()
    {
        float j;
        float degree;
        float pisagor;
        pisagor = -Mathf.Sqrt(Mathf.Pow(r.velocity.z, 2) + Mathf.Pow(r.velocity.y, 2));
        j = asin(r.velocity.z , pisagor);
        /*while (a.x >= 180)
        {
            a.x = a.x + -180;
        }
        while (a.x <= 0)
        {
            a.x = a.x + 180;
        }*/
        int direction = 1;
        float angle = a.x;
        if (angle < 0)
        {
            angle += 90;
            direction = 1;
        }
        else if (angle > 90)
        {
            angle -= 90;
            direction = -1;
        }
        int change_direction = 1;
        direction *= change_direction;
        degree = -j - angle;
        float k = degree;
        float m = sin(k) * real_scale.z;
        return /*Mathf.Sin(degree * Mathf.Deg2Rad) * direction*/sin(j) * m * direction;//cos'u da dene
    }

    /*
    float friction_area_y(float angle, int change_direction)
    {
        float j;
        float degree;
        float pisagor;
        pisagor = -Mathf.Sqrt(Mathf.Pow(r.velocity.z, 2) + Mathf.Pow(r.velocity.y, 2));
        j = Mathf.Asin(r.velocity.z / pisagor) * Mathf.Rad2Deg;
        while (angle >= 180)
        {
            angle = angle + -180;
        }
        while (angle <= 0)
        {
            angle = angle + 180;
        }
        int direction = 1;
        if (angle < 0)
        {
            angle += 90;
            direction = 1;
        }
        else if (angle > 90)
        {
            angle -= 90;
            direction = -1;
        }
        direction *= change_direction;
        degree = -j - a.x;
        float k = degree;
        float m = Mathf.Sin(k * Mathf.Deg2Rad) * real_scale.z;
        return /*Mathf.Sin(degree * Mathf.Deg2Rad) * direction(not kapama)      cos(j) * m * direction;
    }*/
    float air_friction_formula(float axis_velocity, float area)
    {
        float air_density = 0.45f, drag = 0.024f;

        Debug.Log(name + " area : " + area);
        return (MainCamera.friction * area * /*Mathf.Pow(axis_velocity, 2) * */air_density * drag / 100) / 2;


    }
    void ChangeRotation(float add_value, int xyz)
    {
        if (xyz == 1) { Angle.x = Angle.x + add_value; }
        if (xyz == 2) { Angle.y = Angle.y + add_value; }
        if (xyz == 3) { Angle.z = Angle.z + add_value; }

    }
    void SetRotation_0_180()
    {
        while (Angle.x >= 180)
        {
            ChangeRotation(-180, 1);

        }
        while (Angle.x <= 0)
        {
            ChangeRotation(180, 1);
        }
        while (Angle.y >= 180)
        {
            ChangeRotation(-180, 2);
        }
        while (Angle.y <= 0)
        {
            ChangeRotation(180, 2);
        }
        while (Angle.z >= 180)
        {
            ChangeRotation(-180, 3);
        }
        while (Angle.z <= 0)
        {
            ChangeRotation(180, 3);
        }
    }
    float sin(float sin) {
        return Mathf.Sin(sin*Mathf.Deg2Rad);
    }
    float cos(float cos)
    {
        return Mathf.Cos(cos * Mathf.Deg2Rad);
    }
    float asin(float x,float pisagor)
    {
        return Mathf.Asin(x / pisagor) * Mathf.Rad2Deg;
    }
    
}
//joints are with collider(press enable collider)


/*air_friction(GetComponent<Rigidbody>().velocity.y, p.x * Mathf.Cos(transform.rotation.x)* 
GetComponent<Rigidbody>().velocity.z)*/
/*
 
     
     Angle = UnityEditor.TransformUtils.GetInspectorRotation(transform);
        SetRotation_0_180();
        float pisagor = -Mathf.Sqrt(Mathf.Pow(r.velocity.z, 2) + Mathf.Pow(r.velocity.y, 2));
       
        asin = Mathf.Asin(r.velocity.z / pisagor) * Mathf.Rad2Deg;
        
        if (Angle.x < 0)
        {
            Angle.x += 90;
            direction.x = 1;
        }
        if (Angle.x > 90)
        {
            Angle.x -= 90;
            direction.x = -1;
        }
     float degree = -asin - Angle.x;
     */
