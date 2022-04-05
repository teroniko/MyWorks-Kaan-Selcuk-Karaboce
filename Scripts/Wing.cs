using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing : MonoBehaviour
{
    Vector3 p;
    float alan;
    Rigidbody r;
    Vector3 real_scale;
    Vector3 direction;
    Quaternion q;
    Vector3 a;
    int vx=60;
    Vector3 Angle;
    float asin;
    void Start()
    {
        p = transform.position;
        r = gameObject.GetComponent<Rigidbody>();
        real_scale = GetComponent<Renderer>().bounds.size;
        Angle = UnityEditor.TransformUtils.GetInspectorRotation(transform);
        direction= new Vector3(1,1,1);
    }

    // Update is called once per frame
    void Update()
    {
        p = transform.position;
        r = GetComponent<Rigidbody>();
        q = transform.localRotation;
        a = transform.eulerAngles;
        Angle = UnityEditor.TransformUtils.GetInspectorRotation(transform);
        SetRotation_0_180();
        Debug.Log(Angle.x);
        float pisagor = -Mathf.Sqrt(Mathf.Pow(r.velocity.x, 2) + Mathf.Pow(r.velocity.y, 2));
        /*if (r.velocity.x < 0)
        {
            pisagor = -Math.Sqrt(Math.Pow(r.velocity.x, 2) + Math.Pow(r.velocity.y, 2));
        }
        else
        {
            pisagor = Math.Sqrt(Math.Pow(r.velocity.x, 2) + Math.Pow(r.velocity.y, 2));
        }*/
        /*if (pisagor <= 1)
        {
            pisagor = 1;
        }
        else if (pisagor == 0)
        {
            pisagor = 0.00001f;
        }*/
        //a = new Vector3(a.x, a.y, a.z);
        asin = Mathf.Asin(r.velocity.x / pisagor) * Mathf.Rad2Deg;
        
        Debug.Log("###"+name+" : "+" "+ Angle.x);
        
        if (Angle.x < 0) {
            Angle.x += 90;
            direction.x = 1;
        }
        if (Angle.x > 90)
        {
            Angle.x -= 90;
            direction.x = -1;
        }
        float degree = -asin - Angle.x;


        GetComponent<Rigidbody>().velocity = new Vector3(r.velocity.x - air_friction_formula(r.velocity.y, friction_area(r.velocity.x, r.velocity.z, UnityEditor.TransformUtils.GetInspectorRotation(transform).z,-1) * real_scale.x),

            r.velocity.y - air_friction_formula(r.velocity.y, Mathf.Sin(degree * Mathf.Deg2Rad) * real_scale.z)
            //sürtünmenin kendisinin hızından fazla olmaması lazım
            , r.velocity.z - air_friction_formula(r.velocity.y, friction_area(r.velocity.z,r.velocity.x, UnityEditor.TransformUtils.GetInspectorRotation(transform).x,1) * real_scale.z));
        //Debug.Log("x : "+ GetComponent<Rigidbody>().velocity.x+" y : "+ GetComponent<Rigidbody>().velocity.y+" z : "+ GetComponent<Rigidbody>().velocity.z);
        //GetComponent<Rigidbody>().angularVelocity /= MainCamera.friction;

        /*
        hangi yüzey daha büyük ise o yüzeyi bulacak, o yüzeyin alanı=?(transform.localScale.x*transform.localScale.z)

        
        spring joint kullan patlamalarda parçalar ayrılabilsin
        */
    }
    float friction_area(float axis_velocity,float other_axis_velocity,float angle,int change_direction) {
        float asin;
        float degree;
        float pisagor;
        Angle = UnityEditor.TransformUtils.GetInspectorRotation(transform);
        pisagor = -Mathf.Sqrt(Mathf.Pow(other_axis_velocity, 2) + Mathf.Pow(r.velocity.y, 2));
        asin = Mathf.Asin(other_axis_velocity / pisagor) * Mathf.Rad2Deg;
        while (angle >= 180)
        {
            //angle=ChangeRotation(-180, 1);
            angle = angle + -180;
        }
        while (angle <= 0)
        {
            //angle = ChangeRotation(180, 1);
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
        degree = -asin - angle;
        return Mathf.Sin(degree * Mathf.Deg2Rad)* direction;
    }
    Vector3 Vector(int i,float value, float pisagor,Vector3 self) {
        switch (i)
        {
            case 0:
                return new Vector3(Mathf.Asin(r.velocity.x / pisagor) * Mathf.Rad2Deg, self.y, self.z);
                break;
            case 1:
                return new Vector3(self.x, Mathf.Asin(r.velocity.y / pisagor) * Mathf.Rad2Deg, self.z);
                break;
            default:
                return new Vector3(self.x, self.y, Mathf.Asin(r.velocity.z / pisagor) * Mathf.Rad2Deg);
                break;
        }

    }
    float air_friction_formula(float axis_velocity, float area)
    {
        float air_density = 0.45f, drag = 0.024f;
        
        Debug.Log(name+" area : " + area);
        return (MainCamera.friction*area * Mathf.Pow(axis_velocity, 2) * air_density * drag/100)/2;
        
    
    }
    void ChangeRotation(float add_value, int xyz) {
        if (xyz == 1) { Angle.x = Angle.x + add_value; }
        if (xyz == 2) { Angle.y = Angle.y + add_value; }
        if (xyz == 3) { Angle.z = Angle.z + add_value; }
        
    }
    void SetRotation_0_180() {
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
}
//joints are with collider(press enable collider)


/*air_friction(GetComponent<Rigidbody>().velocity.y, p.x * Mathf.Cos(transform.rotation.x)* 
GetComponent<Rigidbody>().velocity.z)*/
