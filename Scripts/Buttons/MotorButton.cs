using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MotorButton : EventTrigger
{
    public GameObject ConnectedMotor;
    int time = 0;
    public bool onpointerdown;
    void ConMot() {
        string motorname = "";
        for (int i = 0; i < name.Length; i++)
        {
            if (name[i] == '.') { break; }
            motorname = motorname + name[i];
        }
        ConnectedMotor = GameObject.Find(motorname);
    }
    void Start()
    {
        ConMot();
    }
    private void Update()
    {
        time++; if (time >= 4) 
        {
            ConMot();
            time = 0;
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (ChangeGameMode.GameMode)
        {
            switch (name.Substring(7)) {
                case "1":
                    ConnectedMotor.GetComponent<Motor>().Button1Down();
                    break;
                case "2":
                    ConnectedMotor.GetComponent<Motor>().Button2();
                    break;
                case "3":
                    ConnectedMotor.GetComponent<Motor>().Button3();
                    break;
                case "4":
                    ConnectedMotor.GetComponent<Motor>().Button4();
                    break;
            }
        }

    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (ChangeGameMode.GameMode)
        {
            if (name.Substring(7)=="1") 
            {
                ConnectedMotor.GetComponent<Motor>().Button1Up();
            }
        }
    }
}
