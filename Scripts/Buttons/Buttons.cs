using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Buttons : EventTrigger
{
    ButtonScript bs;
    public Vector2 OldPos;
    public List<GameObject> Childs;
    Vector2 canvasSize;
    bool just_do_once;
    bool dragging;
    public int button_count;
    public GameObject Exit;
    private void Awake()
    {
        button_count = 2;
        Exit = transform.Find("Exit").gameObject;
    }
    void Start()
    {
        bs = GetComponent<ButtonScript>();
        canvasSize = bs.canvas.GetComponent<RectTransform>().sizeDelta;

        name = "Buttons" + MainCamera.buttonsCount;
        transform.parent = bs.canvas.transform;
    }
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (bs.dragging && just_do_once)
        {
            
            if (collision.tag == "Button")
            {
                MainCamera.buttonsCount++;
                collision.transform.parent = gameObject.transform;
                OldPos = gameObject.GetComponent<RectTransform>().position;
                bs.button_count++;
                collision.GetComponent<ButtonScript>().button_number = bs.button_count;
                Childs.Add(collision.gameObject);
                Vector2 gameObjectSize = gameObject.GetComponent<RectTransform>().sizeDelta;
                Vector2 buttonsSize = GetComponent<RectTransform>().sizeDelta;
                gameObject.transform.Find("Exit").gameObject.SetActive(true);
                int y = 0;
                for (int x = 0; x < bs.button_count; x++)
                {
                    if (x > 1)
                    {
                        y++;
                        x = 0;
                    }
                    if (y > 1) { break; }
                    GameObject ButtonsButton = null;
                    if (x == 0 && y == 0)
                    {
                        ButtonsButton = Childs[0];
                    }
                    else if (x == 1 && y == 0)
                    {
                        ButtonsButton = Childs[1];
                    }
                    else if (x == 1 && y == 0)
                    {
                        ButtonsButton = collision.gameObject;
                    }
                    ButtonsButton.transform.position = new Vector2(gameObjectSize.x * 1.5f * x + canvasSize.x / 2 - buttonsSize.x / 4
                            , gameObjectSize.y * 1.5f * y + canvasSize.y / 2 + buttonsSize.y / 4);

                }
                

                /*
                int y = 0;
                for (int x = 0; x < bs.button_count; x++)
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
            else if (collision.tag == "Button")
            {



                collision.transform.parent = gameObject.transform;
                /*OldPos = gameObject.GetComponent<RectTransform>().position;

                Childs.Add(collision.gameObject);


                bs.button_count++;
                collision.GetComponent<ButtonScript>().button_number = bs.button_count;
                buttons(gameObject, 2);
                buttons(collision.gameObject, 2);
                */
                /*

            }
            just_do_once = false;
        }
    }*/
    void ButtonsSize(GameObject g,float multiplier)
    {
        g.GetComponent<RectTransform>().sizeDelta *= multiplier;
        //g.GetComponent<BoxCollider2D>().size *= new Vector2(multiplier, multiplier);
        //g.GetComponent<BoxCollider2D>().enabled = false;
        //make unclickable
        g.transform.position = new Vector2(bs.canvas.GetComponent<RectTransform>().sizeDelta.x/2, bs.canvas.GetComponent<RectTransform>().sizeDelta.y/2);

    }
    
    void Update()
    {
        bs = gameObject.GetComponent<ButtonScript>();
        if (bs.little_dragging && bs.just_do_once && !bs.dragging) {
            ButtonsSize(gameObject,6);
            OrderChilds(6);
            Exit.SetActive(true);
            /*int number = 0;
            foreach (GameObject Child in Childs)
            {

                ButtonsSize(Childs[number],6);
                number++;
            }
            
            Vector2 gameObjectSize = gameObject.GetComponent<RectTransform>().sizeDelta;
            int y = 0;
            for (int x = 0; x <= button_count / 2; x++)
            {
                if (x > button_count / 2)
                {
                    y++;
                    x = 0;
                }
                Debug.Log("x" + x);
                Vector2 child_size = Childs[x].GetComponent<RectTransform>().sizeDelta;
                Childs[x].transform.position = new Vector2(child_size.x * 1.5f * x + gameObject.transform.position.x - gameObjectSize.x / 4
                , child_size.y * 1.5f * y + gameObject.transform.position.y + gameObjectSize.y / 4);
                if (y > button_count / 2) { break; }


            }*/

            bs.just_do_once = false;
        }
        /*if (dragging)
        {

            transform.position = new Vector2(Input.mousePosition.x + oldpos.x - firstMousePos.x, Input.mousePosition.y + oldpos.y - firstMousePos.y);

            positionChange = transform.position.x != oldpos.x && transform.position.y != oldpos.y;
            if (firstMousePos.x - Input.mousePosition.x < 8 && firstMousePos.x - Input.mousePosition.x > -8
                && firstMousePos.y - Input.mousePosition.y < 8 && firstMousePos.y - Input.mousePosition.y > -8)
            {
                transform.position = oldpos;
            }
        }*/
    }

    public void OrderChilds(float multiplier)
    {
        foreach (GameObject Child in Childs)
        {
            ButtonsSize(Child, multiplier);
        }
        
        Vector2 gameObjectSize = GetComponent<RectTransform>().sizeDelta;
        int squareEdge=0;
        if (button_count <= 4) { squareEdge = 2; }
        else if (button_count <= 9) { squareEdge = 3; }
        else if (button_count <= 16) { squareEdge = 4; }
        else if (button_count <= 25) { squareEdge = 5; }
        int y = 0;
        int x = 0;
        for (int i = 0; i < button_count; i++)
        {
            
            //Debug.Log("x" + x+" y"+y);
            Vector2 child_size = Childs[i].GetComponent<RectTransform>().sizeDelta;
            Childs[i].transform.position = new Vector2(child_size.x * 1.5f * x + gameObject.transform.position.x - gameObjectSize.x / 4
            , child_size.y * 1.5f * -y + gameObject.transform.position.y + gameObjectSize.y / 4);
            x++;
            if (x >= squareEdge)
            {
                y++;
                x = 0;
            }


        }
    }
    public void ExitFromButtons()
    {
        gameObject.GetComponent<RectTransform>().position = OldPos;
        Exit.SetActive(false);
        
        gameObject.GetComponent<RectTransform>().sizeDelta *= 1/6f;
        OrderChilds(1 / 6f);
        //Vector2 gameObjectSize = GetComponent<RectTransform>().sizeDelta;
        /*gameObject.GetComponent<BoxCollider2D>().size *= 1 / 6f;
        foreach (GameObject Child in Childs)
        {

            buttons(Child, 1f / 6);
        }
        int y = 0;
        for (int x = 0; x <= button_count / 2; x++)
        {
            if (x > button_count / 2)
            {
                y++;
                x = 0;
            }
            Debug.Log("x" + x);
            Vector2 child_size = Childs[x].GetComponent<RectTransform>().sizeDelta;
            Childs[x].transform.position = new Vector2(child_size.x * 1.5f * x + gameObject.transform.position.x - gameObjectSize.x / 4
            , child_size.y * 1.5f * y + gameObject.transform.position.y + gameObjectSize.y / 4);
            if (y > button_count / 2) { break; }


        }*/

        //buttonlar buttonsun childı olunca algılamayacak transform.find ile de tarattır
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (ChangeGameMode.GameMode)
        {
            foreach (GameObject child in Childs)
            {
                if (child.GetComponent<MotorButton>() != null)
                {
                    child.GetComponent<MotorButton>().OnPointerDown(eventData);
                    /*Motor motor = child.GetComponent<MotorButton>().ConnectedMotor.GetComponent<Motor>();
                    switch (child.name.Substring(7))
                    {
                        case "1":
                            motor.Button1Down();
                            break;
                        case "2":
                            motor.Button2();
                            break;
                        case "3":
                            motor.Button3();
                            break;
                        case "4":
                            motor.Button4();
                            break;
                    }*/
                }
            }
        }

    }
    public override void OnPointerUp(PointerEventData eventData)
    {

        if (ChangeGameMode.GameMode)
        {
            foreach (GameObject child in Childs)
            {
                if (child.GetComponent<MotorButton>() != null)
                {
                    child.GetComponent<MotorButton>().OnPointerUp(eventData);
                    /*Motor motor = child.GetComponent<MotorButton>().ConnectedMotor.GetComponent<Motor>();
                    switch (child.name.Substring(7))
                    {
                        case "1":
                            motor.Button1Down();
                            break;
                        case "2":
                            motor.Button2();
                            break;
                        case "3":
                            motor.Button3();
                            break;
                        case "4":
                            motor.Button4();
                            break;
                    }*/
                }
            }
        }
    }
}
