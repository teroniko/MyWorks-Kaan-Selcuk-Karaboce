using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Motor : MonoBehaviour
{
    public GameObject ConnectionPos;
    public GameObject Cylinder;
    public GameObject Platform;
    public float MaxLenght;
    public float MinLenght;
    public bool Success = true;
    public TMP_Text Text;
    public Transform CubeForRot;
    public Transform PlatformJoint;
    public GameObject Motor0;

    void Awake()
    {


        CylinderLook();
        Cylinder.transform.localScale = new Vector3(Cylinder.transform.localScale.x, Vector3.Distance(ConnectionPos.transform.position, Cylinder.transform.position), Cylinder.transform.localScale.z);


        GetComponent<ConfigurableJoint>().connectedBody = Cylinder.GetComponent<Rigidbody>();
        GetComponent<ConfigurableJoint>().anchor = Cylinder.transform.localPosition;


    }


    private void CylinderLook()
    {
        Cylinder.transform.LookAt(ConnectionPos.transform.position, transform.up);
        Cylinder.transform.Rotate(Vector3.right, 90);
        //if (PlatformJoint != null)
        {
            float offset=PlatformJoint.eulerAngles.y;
            PlatformJoint.LookAt(Cylinder.transform.position, transform.up);
            PlatformJoint.Rotate(Vector3.left,90);
            PlatformJoint.rotation = Quaternion.Euler(PlatformJoint.eulerAngles.x, offset, PlatformJoint.eulerAngles.z);
        }
        //if (Motor0 != null)
        {
            Motor0.transform.LookAt(ConnectionPos.transform.position, transform.forward);
            Motor0.transform.Rotate(Vector3.forward, 28);
        }


    }
   
    public void StickStrect()
    {
        float StickLenght = Vector3.Distance(ConnectionPos.transform.position, Cylinder.transform.position);
        if (StickLenght < MinLenght || StickLenght > MaxLenght)
        {
            Success = false;
            return;
        }
        else
        {

            Success = true;
        }
        Cylinder.transform.localScale = new Vector3(Cylinder.transform.localScale.x, StickLenght, Cylinder.transform.localScale.z);
        Text.text = StickLenght * 50f + "mm";



        GetComponent<ConfigurableJoint>().anchor = Cylinder.transform.localPosition;


        CylinderLook();
        
    }
}
