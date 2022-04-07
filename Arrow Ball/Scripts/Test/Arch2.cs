using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arch2 : MonoBehaviour
{

    GameObject Arrow;
    float old_hipotenus = 0;
    Vector3 ThrustPower;
    float speedRange;
    Vector3 distance;
    float hipotenus;
    float decreasement = 100000;
    private void Start()
    {
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Arrow = Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"), transform.position, Quaternion.Euler(0, 0, 0));
            Arrow.name = "Arrow";

        }
        else if (Input.GetMouseButton(0))
        {
            Arrow.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z - 90);
            Vector3 MousePos;

            Vector3 mouseInputPos = Input.mousePosition;
            mouseInputPos.z = Camera.main.nearClipPlane + 8.7f;
            MousePos = Camera.main.ScreenToWorldPoint(mouseInputPos);
            distance = (MousePos - transform.position) * 20000;
            distance.z = 0;
            hipotenus = Mathf.Sqrt(Mathf.Pow(distance.x, 2) + Mathf.Pow(distance.y, 2));

            if (hipotenus < old_hipotenus)
            {
                Arrow.transform.Translate(Vector3.left * (old_hipotenus - hipotenus) / decreasement);
                speedRange += 0.5f;
                decreasement+=10000;
                //Arrow.transform.position = new Vector3(Arrow.transform.position.x+(old_hipotenus - hipotenus)/10000f, Arrow.transform.position.y, Arrow.transform.position.z);
            }
            old_hipotenus = hipotenus;
            float angle = Mathf.Atan2(distance.y, distance.x) * (180 / Mathf.PI);

            transform.localEulerAngles = new Vector3(0, 0, angle + 90);


        }
        else if (Input.GetMouseButtonUp(0))
        {
            ThrustPower = new Vector3(speedRange * distance.x / hipotenus, speedRange * distance.y / hipotenus, 0);
            Arrow.GetComponent<Rigidbody>().velocity = ThrustPower;
            //Arrow.GetComponent<Arrow>().archOut = true;
            
            Arrow.GetComponent<Rigidbody>().isKinematic = false;
            speedRange = 0;
            decreasement = 100000;
        }
    }
}
