using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public Main_Camera MC;
    bool dragging = false;
    Vector3 firstMousePos;
    Vector3 direction;
    GameObject canvas;
    private void Awake()
    {
        MC = GameObject.Find("Main Camera").GetComponent<Main_Camera>();
        canvas = GameObject.Find("Canvas");
    }
    public void Soldier()
    {
        //GetCamera();
        MC.Selected = MC.Soldier;
        MC.unitNumber = 0;
        
    }
    public void Archer()
    {
        //GetCamera();
        MC.Selected = MC.Archer;
        MC.unitNumber = 1;
    }

    public void Gaint()
    {
        //GetCamera();
        MC.Selected = MC.Gaint;
        MC.unitNumber = 2;
    }
    //void GetCamera()
    //{
    //    MC = gameObject.GetComponent<Main_Camera>();
    //}
    private void Update()
    {
        if (dragging)
        {
            //açma:
            //transform.position = Vector3.Lerp(transform.position, Vector3.forward, 0.001f);


            //direction = Camera.main.ScreenToViewportPoint(Input.mousePosition+new Vector3(0, Camera.main.transform.position.y, 0)) - firstMousePos;

            
            if (direction.magnitude <= 0.15f)
            {
                direction = (Input.mousePosition - firstMousePos) / 400;
            }
            //transform.position = Vector3.Lerp(transform.position, new Vector3(direction.x, transform.position.y, direction.y), 0.0005f * direction.magnitude);//evet z'ye y yazdım
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            transform.position += new Vector3(direction.x, 0, direction.y);

            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(direction.x, transform.position.y, direction.y), 0.05f*direction.magnitude);
        }
    }
    public void TravelMouseDrag()
    {

        

    }
    public void TravelMouseDown()
    {
        //Debug.Log("Down : "+Input.mousePosition);
        dragging = true;
        //firstMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition + new Vector3(0, Camera.main.transform.position.y, 0));
        firstMousePos = Input.mousePosition;
    }
    //mouseover
    public void TravelMouseUp()
    {
        //Debug.Log("Up : "+Input.mousePosition);
        direction = Vector3.zero;
        dragging = false;
    }
    public void Play()
    {
        SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
        
    }
    public void Quit()
    {
        Application.Quit();
    }
    void UnloadScene()
    {
        SceneManager.UnloadSceneAsync("GamePlay");
    }

    
}
