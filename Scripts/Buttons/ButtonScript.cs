using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : EventTrigger
{
    //subway surf gibi olcak ama giden araç biz olacağız ve belirli bir mesafede bir pil biticek ve pil alacaksın yeni bir pil kazanacağız
    //Hatta pil bitince en büyük bir pil olacak o çalışacak yani o pil çok büyük olacak ve onun sayesinde pilin bitmişken bile biraz gidebileceksin
    //canvasın kenarlarına, daha kenarlara gitmesin diye collider koyulabilir
    public Canvas ButtonDesigner;
    public GameObject canvas;
    public GameObject Main_Camera;
    public bool dragging;
    Vector2 oldpos;
    Vector2 firstMousePos;
    public bool just_do_once;
    public bool little_dragging;
    public int button_number;
    //button üzeri button nasıl olur fırata sor
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        
    }
    
    private void Update()
    {
        if (!ChangeGameMode.GameMode)
        {
            if (dragging)
            {

                transform.position = new Vector2(Input.mousePosition.x + oldpos.x - firstMousePos.x, Input.mousePosition.y + oldpos.y - firstMousePos.y);
                little_dragging = firstMousePos.x - Input.mousePosition.x < 8 && firstMousePos.x - Input.mousePosition.x > -8
                    && firstMousePos.y - Input.mousePosition.y < 8 && firstMousePos.y - Input.mousePosition.y > -8;
                if (little_dragging)
                {
                    transform.position = oldpos;
                }
            }
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!dragging && just_do_once)
        {

            if (collision.tag == "Button" && tag != "Buttons")
            {

                GameObject Buttons = Instantiate(Resources.Load("Buttons") as GameObject, collision.transform.position, Quaternion.identity);
                MainCamera.buttonsCount++;
                Buttons.name = "Buttons" + MainCamera.buttonsCount;
                Buttons.transform.parent = canvas.transform;
                gameObject.transform.parent = Buttons.transform;
                collision.transform.parent = Buttons.transform;
                Buttons.GetComponent<Buttons>().OldPos = gameObject.GetComponent<RectTransform>().position;





                Buttons.GetComponent<Buttons>().Childs.Add(collision.gameObject);
                Buttons.GetComponent<Buttons>().Childs.Add(gameObject);

                ButtonsSize(gameObject,2);
                ButtonsSize(collision.gameObject,2);

                Vector2 gameObjectSize = gameObject.GetComponent<RectTransform>().sizeDelta;
                Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
                Buttons.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
                Vector2 buttonsSize = Buttons.GetComponent<RectTransform>().sizeDelta;

                Transform Exit = Buttons.transform.Find("Exit");
                Exit.GetComponent<RectTransform>().sizeDelta = canvasSize;
                Exit.GetComponent<RectTransform>().position = new Vector2(canvasSize.x / 2, canvasSize.y / 2);

                int y = 0;
                for (int x = 0; x < Buttons.GetComponent<Buttons>().button_count; x++)
                {
                    if (x > 1)
                    {
                        y++;
                        x = 0;
                    }
                    if (y > 1) { break; }
                    if (x == 0)
                    {
                        collision.transform.position = new Vector2(gameObjectSize.x * 1.5f * x + canvasSize.x / 2 - buttonsSize.x / 4
                            , gameObjectSize.y * 1.5f * y + canvasSize.y / 2 + buttonsSize.y / 4);
                    }
                    else
                    {
                        gameObject.transform.position = new Vector2(gameObjectSize.x * 1.5f * x + canvasSize.x / 2 - buttonsSize.x / 4
                            , gameObjectSize.y * 1.5f * y + canvasSize.y / 2 + buttonsSize.y / 4);
                    }

                }
                
                
            }
            else if (collision.tag == "Buttons" && gameObject.tag!="Buttons") {
                transform.parent = collision.transform;
                collision.GetComponent<Buttons>().button_count++;
                collision.GetComponent<Buttons>().Childs.Add(gameObject);
                ButtonsSize(gameObject, 1 / 3f);
                //collision.GetComponent<Buttons>().OldPos = gameObject.GetComponent<RectTransform>().position;



                /*
                ButtonScript bs = Buttons.GetComponent<ButtonScript>();

                Buttons.GetComponent<Buttons>().Childs.Add(collision.gameObject);
                Buttons.GetComponent<Buttons>().Childs.Add(gameObject);
                */
                /*gameObject.GetComponent<ButtonScript>().button_number = bs.button_count;
                bs.button_count++;
                collision.GetComponent<ButtonScript>().button_number = bs.button_count;*/


                Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
                collision.transform.position = new Vector2(canvasSize.x / 2, canvasSize.y / 2);

                collision.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
                GameObject Exit = collision.GetComponent<Buttons>().Exit;
                Exit.gameObject.SetActive(true);
                Exit.GetComponent<RectTransform>().sizeDelta = canvasSize;
                Exit.GetComponent<RectTransform>().position = new Vector2(canvasSize.x / 2, canvasSize.y / 2);
                
                collision.GetComponent<Buttons>().OrderChilds(6);
                //GetComponent<ButtonScript>().enabled = false;
                //gameObject.SetActive(false);

            }

            
            just_do_once = false;
        }
    }

    public void ButtonsSize(GameObject g,float multiplier) {
        g.GetComponent<RectTransform>().sizeDelta *= multiplier;
        //g.GetComponent<BoxCollider2D>().size *= new Vector2(2, 2);
        g.GetComponent<BoxCollider2D>().enabled = false;
        //g.SetActive(false);
        //g.GetComponent<ButtonScript>().enabled = false;
        //make unclickable
        //g.GetComponent<BoxCollider2D>().enabled = false;

    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!ChangeGameMode.GameMode)
        {
            oldpos = transform.position;
            firstMousePos = Input.mousePosition;
            just_do_once = true;
            dragging = true;
        }
        
    }
    
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!ChangeGameMode.GameMode)
        {
            dragging = false;
        }
    }
}


/*public void OnBeginDrag(PointerEventData eventData)
{
    Vector2 pos;
    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, Camera.main, out pos))
    {
        dragOffset = (Vector2)transform.position - (Vector2)canvasRectTransform.TransformPoint(pos);
        transform.SetParent(canvasRectTransform, true);
    }
}

public void OnDrag(PointerEventData eventData)
{
    Vector2 pos;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, Camera.main, out pos);

    transform.position = canvasRectTransform.TransformPoint(pos) + (Vector3)dragOffset;
}*/

/*if (Input.GetMouseButton(0)) 
        {/*
            Mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(Mouse.x, Mouse.y);
            Debug.Log("h"+Camera.main.WorldToScreenPoint(Input.mousePosition));
            Debug.Log("j"+transform.position);
            Debug.Log("a" + RectTransformToScreenSpace(gameObject.GetComponent<RectTransform>()));
            //Debug.Log(ScreenPointToLocalPointInRectangle(transform.localPosition, Input.mousePosition, Main_Camera, out Vector2 localPoint));
            
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 diff = currentMousePosition - lastMousePosition;
            RectTransform rect = GetComponent<RectTransform>();

            Vector3 oldPos = rect.position;
            rect.position = rect.position + new Vector3(diff.x, diff.y, transform.position.z);

            Rect rect2 = new Rect(0, 0, Screen.width, Screen.height);
            

            rect2.position = oldPos;
            
            lastMousePosition = currentMousePosition;
        }
    */


/*private Vector2 lastMousePosition;
public static Rect RectTransformToScreenSpace(RectTransform transform)
{
    Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
    Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
    rect.x -= (transform.pivot.x * size.x);
    rect.y -= ((1.0f - transform.pivot.y) * size.y);
    return rect;
}*/
