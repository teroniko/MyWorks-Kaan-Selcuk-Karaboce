using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    public bool IsSticked = false;
    public Vector2 Speed;
    public GameObject Ball;
    public Transform Arrow0;
    public Transform[] balls0;
    public Main m;
    public Collider[] Colliders;
    private LayerMask LMaskTarget;
    public GameObject Number;
    public TMP_Text NoText;
    public FracNo ArrowNo;
    public Material Mat;
    public MaterialPropertyBlock MatPB;
    public MeshRenderer BallMeshRenderer;
    private float ArrowTurnSpeed;

    public int NoOrder = 0;
    public int TextPlace = 0;
    public float ArrowAngle;
    public float ArrowPower;
    private void Awake()
    {
        m = Camera.main.GetComponent<Main>();
        MatPB = new MaterialPropertyBlock();

        ArrowTurnSpeed = m.ArrowTurnSpeed;
        




        //optimize:
        LMaskTarget = LayerMask.NameToLayer("Target");


        Colliders = new Collider[1];

    }
    private void Start()
    {

        NoText = Number.GetComponent<TMP_Text>();
        
    }
    public void SetColor(Color color)
    {
        BallMeshRenderer.GetPropertyBlock(MatPB);
        MatPB.Clear();
        MatPB.SetColor("_Color", color);
        BallMeshRenderer.SetPropertyBlock(MatPB);
    }
    private Arrow a;
    private RaycastHit hit;
    public IEnumerator Go()
    {
        float deltaTime;
        m.NArrowIndex++;
        //m.NextArrowNumbers();
        IsSticked = false;
        while (!IsSticked)
        {
            yield return new WaitForSeconds(0.01f);//0.005
            //lerp ile gittikçe yavaşlayabililir miydi dönme hızı, lerp ile yapınca ok hedef alırken ok yol noktaları biraz aşağı çekerken bir yerde direk ışınlanıyor düzgün bir şekilde yavaş geçmiyor
            deltaTime = Time.deltaTime * 50f;
            Speed = new Vector2(Mathf.Cos(ArrowAngle), Mathf.Sin(ArrowAngle)) * ArrowPower* deltaTime;
            //if (ArrowAngle * Mathf.Rad2Deg >= -90)
            {
                ArrowAngle -= ArrowTurnSpeed * deltaTime;
            }
            //if (ArrowAngle * Mathf.Rad2Deg > -90)
            //{
            //    ArrowAngle = Mathf.Lerp(ArrowAngle, -90 * Mathf.Deg2Rad, ArrowTurnSpeed * deltaTime);
            //}
            //else
            //{
            //    ArrowAngle = -90 * Mathf.Deg2Rad;
            //}


            //optimize edilecek:
            transform.LookAt(transform.position + (Vector3)Speed);
            transform.Rotate(0, -90, 0);
            transform.Translate(Speed, Space.World);




            if (Physics.Raycast(transform.position /*+ transform.right * Arrow0Height*/, transform.right, out /*RaycastHit*/ hit, 0.8f, m.LMaskTarget))
            {
                a = hit.transform.GetComponentInParent<Arrow>();
                
                if (hit.transform.tag == "Ball")//Another arrow
                {
                    m.PlayAudio(m.BubbleHit);
                    m.StartCoroutine(Pop(false));

                    IsSticked = true;


                    CheckIfWon(false, 2);
                }
                else if (hit.transform.tag == "Block")
                {
                    m.PlayAudio(m.Leaf);
                    IsSticked = true;
                    if(m.CurrentLevelType.Type == "Training"&&m.CurrentLevelType.LevelNo == 2)
                    {
                        m.SetLevel();
                    }
                    else
                    {
                        m.IfWon();

                    }
                    
                }
                else/* if(hit.transform.tag=="Object")*///Cylinder
                {
                    //transform.rotation = new Quaternion(1, transform.rotation.y, 0, 0);
                    
                    
                    StickToCylinder(hit.transform, false);

                    CheckIfWon(true, 4);
                    
                }

            }


        }
        IsSticked = true;
    }
    
    private float PopEffectDis = 0.15f;
    private float PopEffectBallSize = 1.2f;
    private void CylinderEffect(float PopEffectDis)
    {
        for (int i = 0; i < m.TE.transform.childCount; i++)
        {
            Arrow a = m.TE.transform.GetChild(i).GetComponentInChildren<Arrow>();
            a.transform.position += a.transform.right * PopEffectDis;
            //a.transform.localScale *= PopEffectBallSize;
            //a.Number.transform.localScale *= PopEffectBallSize;
        }
    }
    private void ArrowBallSizeEffect(float PopEffectBallSize)
    {
        a.transform.localScale *= PopEffectBallSize;
        a.Number.transform.localScale *= PopEffectBallSize;
        //a.Ball.transform.localScale /= PopEffectBallSize;
        //a.gameObject.transform.position -= a.transform.right * PopEffectDis;
    }
    private IEnumerator Pop(bool Cylinder)
    {
        if (Cylinder)
        {
            CylinderEffect(-PopEffectDis);
        }
        else
        {
            ArrowBallSizeEffect(PopEffectBallSize);
        }


        float time = 0;

       

        while (time <= 0.17f)//bu sayıya dokunulmayacak
        {
            yield return new WaitForSeconds(0.005f);
            time += Time.deltaTime;



        }

        if (Cylinder)
        {
            CylinderEffect(PopEffectDis);
        }
        else
        {
            ArrowBallSizeEffect(1/PopEffectBallSize);

        }

    }

    private void TrainingOperation()
    {
        m.TrainingOperationSA = a.NoOrder;
        StartCoroutine(m.SetTrainingOperation());
        m.ThrowAMouseArea.enabled = false;


        //trainingde level2 olunca nexte basmalardan birinde çağrılacak:
        //m.TE.EquationUpdate(0, false, false);



        a.NoText.text = m.FracNoToString(a.ArrowNo, false, false);

        //optimize:
        m.CloseGuideArrows();
        m.CloseGuideTargets();
        m.TE.ccs[0].RotSpeed = 0;

        m.NextTrainingText.enabled = false;
        m.GameGuideUI.SetActive(false);


        //hit ile yap
        //bu koddan 2 tane var:(stick to cylinderda)
        Vector3 ArrowBall = a.transform.GetChild(1).position;
        transform.position = hit.point;
        transform.position = ArrowBall + (transform.position - ArrowBall).normalized * 1.1f;



        //optimize(cross ile olabilir)
        transform.LookAt(a.transform.position);
        transform.Rotate(0, -90, 0);



        transform.parent = m.TE.transform;
    }
    private bool tooClose = false;
    private void CheckIfWon(bool CylinderStick, int Level)
    {

        if (m.CurrentLevelType.Type == "Training"&& m.CurrentLevelType.LevelNo != 1  && m.CurrentLevelType.LevelNo != 6)
        {
            //bakılacak!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!:
            if (CylinderStick && m.CurrentLevelType.LevelNo != 4)
            {
                m.SetLevel();
            }
            else
            {
                switch (m.CurrentLevelType.LevelNo)
                {
                    case 5:
                        TrainingOperation();
                        CurrentArrowUpdate();
                        break;
                    case 4:
                        
                        TrainingOperation();
                        CurrentArrowUpdate();

                        break;
                    case 3:
                        TrainingOperation();



                        break;
                    //case 3:
                        
                    //    CurrentArrowUpdate();
                        
                    //    if (m.StickedArrows.Objects[m.CorrectFracIndex].GetComponent<Arrow>() == a)
                    //        //a.NoOrder ile yapılabilir gerçi class ile classı eşit mi deyince içindeki numaraları karşılaştıruyo olabilir
                    //    {

                    //        m.TE.EquationUpdate(0, false, false);
                    //        a.NoText.text = m.FracNoToString(a.ArrowNo, false, false);
                    //        //optimize:
                    //        m.CloseGuideArrows();
                    //        m.CloseGuideTargets();
                    //        //m.NextLevelButton.SetActive(true);
                    //        m.NextTrainingText.enabled = false;
                    //        m.GameGuideUI.SetActive(false);
                            
                    //        m.NextTrainingText.enabled = false;
                    //        //m.StartCoroutine(m.GuideArrowUI(0, false, 8, false));
                    //        //m.StartCoroutine(m.BowToAmmo());

                    //        m.IfWon();
                    //    }
                    //    else
                    //    {
                    //        m.SetLevel();
                    //    }
                        
                    //    m.SetCorrectFracIndex(1);
                    //    break;
                    //case 4:

                    //    if (!CylinderStick)
                    //    {

                    //        m.SetLevel();
                            
                    //    }
                    //    else if(!tooClose)
                    //    {
                    //        //optimize:
                    //        m.StartCoroutine(m.GuideArrowUI(0, true, 0, false));
                    //        m.TE.StartCoroutine(m.TE.WaitAndModify(false, null));
                    //        m.CloseGuideArrows();
                    //        m.CloseGuideTargets();
                    //        m.NextTrainingText.enabled = false;
                    //    }
                        
                    //    break;
                }


            }
        }
        else
        {
            if (!CylinderStick)
            {
                CurrentArrowUpdate();
            }
            
            m.CloseGuideArrows();
            m.CloseGuideTargets();
            
            m.TE.EquationUpdate(0, false, false);

            m.IfWon();
        }
    }
    
    public void CurrentArrowUpdate()
    {
        a.ArrowNo = m.FractionalOperation(a.ArrowNo, ArrowNo);


        m.EquationNumbers[a.NoOrder] = a.ArrowNo;
        FracNo Equation = m.TE.No;

        foreach (FracNo f in m.EquationNumbers)
        {
            Equation = m.FractionalOperation(Equation, f);
        }

        m.TE.Equivalent = Equation;
        
        m.TE.NumberText.text = m.FracNoToString(m.TE.No, false, false);




        a.NoText.text = m.FracNoToString(a.ArrowNo, false, false);

        //Number.SetActive(false);
        //gameObject.SetActive(false);
    }
    private IEnumerator TooClose(bool SetLevel)
    {
        tooClose = true;
        bool blink = false;
        float time = 0;
        for (int i = 0; i < 6;)
        {
            yield return new WaitForSeconds(0.005f);
            time += Time.deltaTime * 10;
            if (time >= 1)
            {
                time = 0;
                i++;
                blink = !blink;
                if (blink)
                {
                    SetColor(Color.black);
                }
                else
                {
                    SetColor(Color.white);
                }
            }


        }
        
        if (SetLevel)
        {
            m.SetLevel();
        }
        gameObject.SetActive(false);
        tooClose = false;
    }
    
    public void StickToCylinder(Transform Cylinder, bool StickInStart)
    {
        
        IsSticked = true;


        transform.parent = Cylinder;

        transform.localScale *= 2.5f;//4


        Number.transform.localScale *= 2.4f;//3

        Ball.transform.localScale *= 1.1f;
        if (!StickInStart)
        {
            transform.position = hit.point;
        }
        transform.position = Cylinder.position + (transform.position - Cylinder.position).normalized * 2;


        //transform.rotation = new Quaternion(-1, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        ////optimize edilecek:

        transform.LookAt(Cylinder.position);
        transform.Rotate(0, -90, 0);




        if (!StickInStart && Physics.CheckSphere(Ball.transform.position, Ball.GetComponent<SphereCollider>().radius*0.6f/*0.7*/, m.LMaskTarget))
        {
            //gameObject.SetActive(false);
            
            Number.SetActive(false);
            Debug.Log("TooClose");
            if (m.CurrentLevelType.Type == "Training" && m.CurrentLevelType.LevelNo == 4)
            {
                StartCoroutine(TooClose(true));
            }
            else
            {
                StartCoroutine(TooClose(false));

            }
            m.PlayAudio(m.TooClose);
        }
        else
        {
            if (!StickInStart)
            {
                m.PlayAudio(m.CylinderHit);
                m.StartCoroutine(Pop(true));
            }

            //sadece bir ballun layerı değişecek:
            gameObject.layer = LMaskTarget;
            transform.GetChild(0).gameObject.layer = LMaskTarget;
            transform.GetChild(1).gameObject.layer = LMaskTarget;
            

            TargetEquation t = m.TE;

            TextPlace = t.EquationText.text.Length - 3 - m.GetSpaceCountOfNo(t.Equivalent);

            t.Equivalent = m.FractionalOperation(t.Equivalent, ArrowNo);


            NoOrder = m.EquationNumbers.Count;
            m.EquationNumbers.Add(ArrowNo);


            t.NumberText.text = m.FracNoToString(t.No, false, false);



        }


        //üstüste top gelince silindirin içinden geçiyor önemli bir problem değil zaten üstüste gelince yapışmaması gerekiyor, işlem yapması gerekiyor
    }

}
