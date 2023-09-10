using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CylinderCondition
{

    //public float Speed;
    //public float SpeedAcceleration;
    public CylinderSpeed CSpeed;
    public float RotSpeed;
    public sbyte RotDir;
    public float RotAcceleration;
    public void SetValues(/*bool SpeedActive<sil,*/ float Speed, float SpeedAcceleration, float RotSpeed, sbyte RotDir, float RotAcceleration)
    {

        CSpeed = new CylinderSpeed();
        CSpeed.Speed = Speed;
        CSpeed.SpeedAcceleration = SpeedAcceleration;


        this.RotSpeed = RotSpeed;
        this.RotDir = RotDir;
        this.RotAcceleration = RotAcceleration;
        
    }
}
//buna gerek yok silinecek: 
public class CylinderSpeed
{
    public float Speed;
    public float SpeedAcceleration;
}
public struct RoutesAndTime
{
    public byte Position;
    public float Time;
    public void SetValues(byte Position, float Time)
    {
        this.Position = Position;
        this.Time = Time;
        
    }
}

public class TargetEquation : MonoBehaviour
{
    public SphereCollider Collider;
    public Main M;
    public FracNo Equivalent;
    public FracNo TargetNo;
    public FracNo No;
    public GameObject Number;
    public TMP_Text NumberText;
    public TMP_Text EquationText;
    public Transform[] CylinderRoutes;
    public List<RoutesAndTime> SelectedRoutes;
    public List<CylinderCondition> ccs = new List<CylinderCondition>();
    private byte CurrentConIndex = 0;
    private byte CurrentRouteIndex = 0;
    private byte CurrentCCIndex = 0;
    private CylinderCondition cc;
    private RoutesAndTime rt;
    private bool JustRot = false;

    private void Awake()
    {
        SelectedRoutes = new List<RoutesAndTime>();
        
        NumberText = Number.GetComponent<TMP_Text>();


        


    }
    public void FastRotSpeed()
    {
        if (cc.CSpeed != null)
        {

            cc.CSpeed.Speed = 0;
            cc.CSpeed.SpeedAcceleration = 0;
        }
        cc.RotSpeed = 200;
        cc.RotAcceleration = 0;
    }
    public void LevelStart()
    {
        SelectedRoutes.Clear();
        ccs.Clear();
        CurrentConIndex = 0;
        CurrentRouteIndex = 0;

    }
    public void FirstPositions()
    {
        transform.position = CylinderRoutes[SelectedRoutes[0].Position].position;
        CurrentRoute = CylinderRoutes[SelectedRoutes[0].Position].position;
        if (SelectedRoutes.Count >= 2)
        {

            NextRoute = CylinderRoutes[SelectedRoutes[1].Position].position;
        }
        cc = ccs[0];
        rt = SelectedRoutes[0];
        //ccs.RemoveAt(ccs.Count - 1);

        //JustRot = cc.CSpeed == null;
        JustRot= cc.CSpeed.Speed==0&&cc.CSpeed.SpeedAcceleration==0;
    }
    public void SetNo()
    {
        FracNo TargetFrac = M.RandomFrac(0, -10, 11);
        Equivalent = TargetFrac;
        No = TargetFrac;

        TargetNo = TargetFrac;
       
        NumberText.text = M.FracNoToString(Equivalent, false, false);

        //EquationBlink(0, 0, false);
        EquationUpdate(0,false,false);
    }
    public void AddRT(byte Position, float Time)
    {
        RoutesAndTime rt = new RoutesAndTime();
        rt.SetValues(Position, Time);
        SelectedRoutes.Add(rt);
    }
    public void AddCC(float Speed, float SpeedAcceleration, float RotSpeed, sbyte RotDir, float RotAcceleration)
    {
        CylinderCondition cc = new CylinderCondition();
        cc.SetValues(Speed,  SpeedAcceleration,  RotSpeed,  RotDir,  RotAcceleration);
        ccs.Add(cc);
    }
    private void UpperRoute()
    {
        SetCurrent();
        NextRoute = CylinderRoutes[SelectedRoutes[CurrentRouteIndex + 1].Position].position;
        CurrentRoute = CylinderRoutes[rt.Position].position;

        CurrentConIndex++;
        
        if (CurrentConIndex == ccs.Count)
        {
            
            ccs.Reverse();
            CurrentConIndex = 0;
            
        }
        CurrentRouteIndex++;

        if (CurrentRouteIndex == SelectedRoutes.Count - 1)
        {
            CurrentRouteIndex = 0;
            SelectedRoutes.Reverse();
        }
    }
    private void SetCurrent()
    {
        cc = ccs[CurrentConIndex];
        rt = SelectedRoutes[CurrentRouteIndex];
    }
    private Vector3 NextRoute;
    private Vector3 CurrentRoute;
    private float CChangeTime = 0;
    private bool Timing = false;
    private float speed0;
    private float speedA0;
    private void CheckChange()
    {
        switch (JustRot)
        {
            case false:
                if (Timing)
                {
                    CChangeTime += Time.deltaTime;
                    //Debug.Log("Counting");
                    if (CChangeTime >= rt.Time)
                    {
                        CChangeTime = 0;

                        Timing = false;

                        cc.CSpeed.Speed = speed0;
                        cc.CSpeed.SpeedAcceleration = speedA0;

                        speed0 = 0;
                        speedA0 = 0;
                    }
                }
                else
                {
                    if (Vector3.Distance(transform.position, NextRoute) < 0.15f/*0.1*/)
                    {
                        //Debug.Log("Dis");



                        UpperRoute();

                        if (rt.Time != 0)
                        {
                            Timing = true;
                            speed0 = cc.CSpeed.Speed;
                            speedA0 = cc.CSpeed.SpeedAcceleration;
                            cc.CSpeed.Speed = 0;
                            cc.CSpeed.SpeedAcceleration = 0;
                            //Debug.Log("timing true");
                        }



                    }
                }

                break;
            case true:
                if (!M.Won.activeSelf)
                {
                    //Debug.Log("rt.time : "+rt.Time);
                    
                    CChangeTime += Time.deltaTime;
                    if (CChangeTime >= rt.Time)
                    {
                        //Debug.Log("||||||||||||||||||||||||||||");
                        SetCurrent();

                        CChangeTime = 0;
                        //optimize edilebilir belki:
                        CurrentConIndex++;
                        if (CurrentConIndex == ccs.Count)
                        {
                            CurrentConIndex = 0;
                        }
                        CurrentRouteIndex++;

                        if (CurrentRouteIndex == SelectedRoutes.Count - 1)
                        {
                            CurrentRouteIndex = 0;
                        }
                        //optimize^
                    }

                }


                break;
        }
        




    }
    private void Update()//was fixed, (delta time)
    {

        float Delta = Time.deltaTime;
        CheckChange();
        
        
        transform.Rotate(0,cc.RotDir* Delta * cc.RotSpeed, 0);
        transform.Translate((NextRoute - CurrentRoute).normalized * cc.CSpeed.Speed * Delta, Space.World);

        
        
        
    }
    
    public void EquationBlink(byte Attention, int NoOrder, bool Brackets)
    {
        //StartCoroutine(EquationBlink0(Attention, NoOrder, Brackets));

        EquationUpdate(NoOrder, Brackets, false);
    }
    public IEnumerator EquationBlink0(byte Attention, int NoOrder, bool Brackets)
    {

        bool Attention0 = false;
        float time = 0;
        ushort blinkCount = 6;
        //if (M.CurrentLevelType.Type == "Training")
        //{
        //    blinkCount = 24;
        //}

        for (int i = 0; i < blinkCount;)
        {


            if (Attention != 0)
            {
                time += Time.deltaTime * 50;
                yield return new WaitForSeconds(0.25f);
            }
            //Debug.Log("time : " + time);
            if (time > 0.1f || Attention == 0)
            {
                i++;
                time = 0;
                Attention0 = !Attention0;
                //EquationText.text = TargetNo;


                EquationUpdate(NoOrder, Brackets, Attention0);
                if (Attention == 0)
                {
                    break;
                }
            }

        }



    }
    //optimize:
    public IEnumerator WaitAndModify(bool firstWait, Arrow a)
    {
        EquationUpdate(0, false, false);
        
        
        yield return new WaitForSecondsRealtime(2);
        M.GameGuideText.text = "New Number Added To Equation";
        M.GameGuideUI.SetActive(true);
        M.NextGuideButton.SetActive(true);
        //yield return new WaitForSecondsRealtime(3);
        //M.CloseGuideArrows();
        //M.IfWon();
        //M.NextLevelButton.SetActive(true);
        //M.NextTrainingText.enabled = false;
        //M.GameGuideUI.SetActive(false);
    }
    public void EquationUpdate(int NoOrder, bool Brackets, bool Attention0)
    {
        EquationText.text = M.FracNoToString(No, false,/*true*/false);
        for (int i2 = 0; i2 < M.EquationNumbers.Count; i2++)
        {
            string s;
            if (Brackets)
            {
                s = "(" + EquationText.text + ")";
            }
            else
            {
                s = EquationText.text;
            }

            string BlinkNo = M.FracNoToString(M.EquationNumbers[i2], true, true);
            if (i2 == NoOrder && Attention0)
            {

                s += "<color=red>" + BlinkNo + "</color>";

            }
            else
            {
                s += BlinkNo;
            }

            M.TE.EquationText.text = s;

            //m.TE.EquationText.text += m.EquationNumbers[i2];
        }
        M.TE.EquationText.text += " = " + /*"<color=red>" +*/ M.FracNoToString(Equivalent, false, false) /*+ "</color>"*/;

    }
}
