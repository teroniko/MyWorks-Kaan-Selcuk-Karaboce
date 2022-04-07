using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    //alttan başlax,y,üste geç,y,z
    Rigidbody r;
    public bool bowOut = false;
    bool CollisionEn = false;
    public float turningSpeed = 1.5f;
    void Start()
    {
        r = GetComponent<Rigidbody>();
        //r.centerOfMass = new Vector3(1000, 0, 0);
    }

    void Update()
    {
        if (bowOut && !CollisionEn)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, 8.7f);
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 179.9f, transform.eulerAngles.z);
            //r.velocity = new Vector3(0, 0, r.velocity.sqrMagnitude);
            //transform.rotation = Quaternion.LookRotation(r.velocity);
            transform.Rotate(0, 0, -turningSpeed);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Arrow")
        {
            CollisionEn = true;
            r.constraints = RigidbodyConstraints.FreezeAll;
        }
        else if (collision.contacts[0].otherCollider.transform.gameObject.name == "Sphere")
        {
            Main.RestartButton();
        }
        if (collision.gameObject.tag == "Object") 
        {
            Vector3 distance = (collision.transform.position - transform.position);
            float angle = Mathf.Atan2(distance.y, distance.x) * (180 / Mathf.PI);
            transform.localEulerAngles = new Vector3(0, 0, angle);
            transform.parent = collision.gameObject.transform;
            transform.Find("Sphere").gameObject.SetActive(true);
            Main.saplanmaNumber--;
            Camera.main.GetComponent<Main>().SaplanmaNumber.GetComponent<Text>().text = Main.saplanmaNumber + "";
            if (Main.saplanmaNumber <= 0) 
            {
                Main.accessedLevel++;
                SceneManager.LoadScene(Main.accessedLevel - 1);
                //SceneManager.LoadScene(SceneManager.GetSceneAt().path);
                switch (Main.accessedLevel) {
                    case 1:
                        Main.ArrowCount = 100;
                        Main.saplanmaNumber = 5;
                        break;
                    case 2:
                        Main.ArrowCount = 30;
                        Main.saplanmaNumber = 10;
                        break;
                    case 3:
                        Main.ArrowCount = 30;
                        Main.saplanmaNumber = 10;
                        break;
                    case 4:
                        Main.ArrowCount = 20;
                        Main.saplanmaNumber = 10;
                        break;
                    case 5:
                        Main.ArrowCount = 5;
                        Main.saplanmaNumber = 5;
                        break;
                }
                GameObject.Find("Bow").GetComponent<Bow>().ArrowCountText.GetComponent<Text>().text = Main.ArrowCount + "";
                

                if (Main.accessedLevel >= 10) { }
            }
        }
        
    }
}
