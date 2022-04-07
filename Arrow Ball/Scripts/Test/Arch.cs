using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arch : MonoBehaviour
{
    GameObject Arrow;
    float old_hipotenus = 0;
    Vector3 oldArrow;
    Vector3 previousVelocity;
    float spring;
    private void Start()
    {
        spring = transform.Find("Spring").GetComponent<SpringJoint>().spring;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Arrow = Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"), transform.position, Quaternion.Euler(0, 0,0));
            Arrow.name = "Arrow";
            transform.Find("Spring").GetComponent<SpringJoint>().connectedBody = Arrow.GetComponent<Rigidbody>();

            transform.Find("Spring").GetComponent<SpringJoint>().autoConfigureConnectedAnchor = false;
            transform.Find("Spring").GetComponent<SpringJoint>().connectedAnchor = new Vector3(-2, 0, 0);
            transform.Find("Spring").GetComponent<SpringJoint>().spring = spring;

        }
        else if (Input.GetMouseButton(0))
        {
            Arrow.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z -90);
            Vector3 MousePos;

            Vector3 mouseInputPos = Input.mousePosition;
            mouseInputPos.z = Camera.main.nearClipPlane + 8.7f;
            MousePos = Camera.main.ScreenToWorldPoint(mouseInputPos);
            Vector3 distance = (MousePos - transform.position)*20000;
            distance.z = 0;
            float hipotenus = Mathf.Sqrt(Mathf.Pow(distance.x, 2) + Mathf.Pow(distance.y, 2));
            
            if (hipotenus < old_hipotenus)
            {
                Arrow.transform.Translate(Vector3.left * (old_hipotenus - hipotenus) / 100000f);
                //Arrow.transform.position = new Vector3(Arrow.transform.position.x+(old_hipotenus - hipotenus)/10000f, Arrow.transform.position.y, Arrow.transform.position.z);
            }
            old_hipotenus = hipotenus;
            float angle = Mathf.Atan2(distance.y, distance.x) * (180 / Mathf.PI);
            
            transform.localEulerAngles = new Vector3(0,0,angle+90);

            //oldArrow = Arrow.transform.position;

            previousVelocity = Arrow.GetComponent<Rigidbody>().velocity;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //Arrow.GetComponent<Arrow>().archOut = true;
            /*Debug.Log(previousVelocity.sqrMagnitude);
            if (Arrow.transform.Find("default").GetComponent<Rigidbody>().velocity.sqrMagnitude < previousVelocity.sqrMagnitude) 
            {
                Arrow.transform.Find("default").GetComponent<Rigidbody>().useGravity = true;
                Arrow.transform.Find("default").GetComponent<Rigidbody>().isKinematic = false;
                //transform.Find("Spring").GetComponent<SpringJoint>().breakForce = 1f;
                Destroy(transform.Find("Spring").GetComponent<SpringJoint>());
                transform.Find("Spring").gameObject.AddComponent<SpringJoint>();
            transform.Find("Spring").GetComponent<SpringJoint>().autoConfigureConnectedAnchor = false;
            transform.Find("Spring").GetComponent<SpringJoint>().connectedAnchor = new Vector3(-2, 0, 0);
            }*/
            //Arrow.transform.position = oldArrow;
            Arrow.GetComponent<Rigidbody>().isKinematic = false;
            IEnumerator waitandrelease = WaitAndRelease();
            StartCoroutine(waitandrelease);
        }
    }
    IEnumerator WaitAndRelease()
    {
        yield return new WaitForSeconds(1);
        Arrow.GetComponent<Rigidbody>().useGravity = true;
        //transform.Find("Spring").GetComponent<SpringJoint>().breakForce = 1f;
        Destroy(transform.Find("Spring").GetComponent<SpringJoint>());
        transform.Find("Spring").gameObject.AddComponent<SpringJoint>();
    }

}

//Arrow.transform.Find("default").GetComponent<SpringJoint>().connectedBody = gameObject.transform.Find("default").GetComponent<Rigidbody>();
/*transform.Find("Arch Top").GetComponent<SpringJoint>().connectedBody = Arrow.transform.Find("default").GetComponent<Rigidbody>();
transform.Find("Arch Top").GetComponent<SpringJoint>().connectedAnchor = Arrow.transform.position;

transform.Find("Arch Bottom").GetComponent<SpringJoint>().connectedBody = Arrow.transform.Find("default").GetComponent<Rigidbody>();
transform.Find("Arch Bottom").GetComponent<SpringJoint>().connectedAnchor = Arrow.transform.position;*/
/*
Component[] SpringJoints = Arrow.transform.Find("default").GetComponents<SpringJoint>();

SpringJoints[0].GetComponent<SpringJoint>().connectedBody = gameObject.transform.Find("default").GetComponent<Rigidbody>();
SpringJoints[0].GetComponent<SpringJoint>().connectedAnchor = gameObject.transform.Find("Arch Top").gameObject.transform.position;

SpringJoints[1].GetComponent<SpringJoint>().connectedBody = gameObject.transform.Find("default").GetComponent<Rigidbody>();
SpringJoints[1].GetComponent<SpringJoint>().connectedAnchor = gameObject.transform.Find("Arch Bottom").gameObject.transform.position;
foreach (Component SpringJoint in Arrow.transform.Find("default").GetComponents(typeof(SpringJoint))) 
{
    SpringJoint.GetComponent<SpringJoint>().connectedBody = gameObject.transform.Find("default").GetComponent<Rigidbody>();
}
*/

//Debug.Log(MousePos);
//transform.rotation = Quaternion.LookRotation(Input.mousePosition - transform.position, Vector3.back);
//Vector3 direction = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x,
//    Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y, 0);
//transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
//Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

//transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);


//Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
/*
float x = Input.mousePosition.x - transform.position.x - Screen.width / 2,
    y = Input.mousePosition.y - transform.position.y - Screen.height / 2;
float pisagor = Mathf.Sqrt(Mathf.Pow(x,2) + Mathf.Pow(y,2));

if (x > 0 && y > 0)
{
    transform.eulerAngles = new Vector3(0, 180, 90 + Mathf.Asin((Input.mousePosition.x - transform.position.x) / pisagor
       ) * Mathf.Rad2Deg);
}
else {
    transform.eulerAngles = new Vector3(0, 180, 90);
}
Debug.Log("x : " + x + "y : " + y);
Debug.Log("Camera.main.ScreenToWorldPoint(Input.mousePosition) : " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
*/


/*



look at komutu
tek ekseni döndürme için:
 https://www.youtube.com/watch?v=dp3lZUDij6Y



 */
