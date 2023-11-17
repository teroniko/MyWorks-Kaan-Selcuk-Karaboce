using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    //optimization:(gameobject to transform?)
    private bool Sliding = false;
    private Vector2 mouseStartedPos;
    public GameObject ArcherSpine;
    public GameObject TargetSphere;
    public GameObject ArmPos;
    public Transform StretchBowPos;
    public Transform ShortenBowPos;
    public GameObject TargetIK;
    public Transform ArrowFinger;
    public Transform ArrowPoint;
    public Transform[] RopePoints;
    public LineRenderer Rope;
    public GameObject ArrowPrefab;
    public float ArrowTurnSpeed;//Componente bak//0.006f
    //public List<GameObject> Arrows;//array out of list?
    public GameObject ArrowsParent;
    public Transform NumbersParent;
    public int NextArrow;
    public int ArrowCount;
    public LayerMask LMaskTarget;
    public Canvas c;
    public GameObject number;
    public TMP_Text cylindertext;
    public Transform testsphere;
    //private FracNo[] NextNumbers;
    public List<FracNo> EquationNumbers = new List<FracNo>();
    private IncompleteNumber[] NextNumbers;
    //private List<int> IncompleteNoIndexes;
    public TargetEquation TE;
    private bool PlatformTouchScreen;
    private ushort FBallCount = 3;//first ball count
    private ushort MaxFBallCount = 7;//first ball count
    private ushort MinThrow = 3;
    private ushort MinThrowForArrows = 0;
    private ushort MaxThrow = 7;
    private ushort MaxThrowForArrows = 0;
    private short NoRange1 = 1;//-5
    private short NoRange2 = 5;
    public byte NArrowIndex = 0;//NextArrowIndex
    public TMP_Text NextANumbersText;
    public GameObject Won;
    public TMP_Text TargetNoText;
    public bool breakIfWon = false;
    public GameObject NextBallPrefab;
    public GameObject NextLevelButton;

    private TMP_Text LastBallUI;
    private Color BlueAddition = new Color(0.3212531f, 0.3212531f, 0.8867924f);
    private Color RedSubtraction = new Color(0.9056604f, 0.1742969f, 0.1742969f);

    

    public ObjectPool StickedArrows;
    public ObjectPool Arrows;
    private ObjectPool BallsUI;
    private ObjectPool ArrowPath;
    public Transform BallUIParent;
    public GameObject ArrowPathPartPrefab;
    public Transform ArrowPathParent;
    private float spaceBetweenAPD = 0.3f;//spaceBetweenArrowPathDot
    private byte ArrowOperation = 0;//+,-,*,/
    private byte StickArrowOperation = 0;
    public TMP_Text LevelText;
    private sbyte StickedBlueCount;
    private sbyte ArrowBlueCount;
    private sbyte CurrentBlueCount = 0;
    private sbyte CurrentRedCount = 0;
    public RectTransform InformFinger;
    public RectTransform FingerPos1;
    public RectTransform FingerPos2;
    public LevelType CurrentLevelType;
    public GameObject Menu;
    public GameObject Game;
    public Image ThrowAMouseArea;
    public TMP_Text GameGuideText;
    public GameObject GameGuideUI;
    public Image[] TargetUIGuide;
    public Image GuideUIArrow;
    public Image GuideUIArrow2;
    public Image[] GuideUIArrows;
    public Image NextGuideDetectClick;
    public TMP_Text ArcheryTextInfo;
    public TMP_Text MathTextInfo;
    public TMP_Text CLevelTypeText;
    public Image NextTrainingText;
    public TMP_Text[] NumberShower;
    public Image[] GuideUIArrow0;
    public Image[] GuideUIArrowTop;
    public Image PlusMinusShower;
    public Image HTTAAim;
    public Image HTTAPower;
    public GameObject HTTA;
    private bool HowToTArrowPosUpdate = false;
    public TMP_Text OOAmmoText;
    private bool OOABlinking;
    public GameObject NextGuideButton;
    private byte GuideCount = 0;
    public Image PlusOrMinus;
    public Sprite[] PMShower;
    private GuideArrow[] GAs = new GuideArrow[3];
    private GuideArrow AimingGA;
    public Image AimingGAStick;
    public Image AimingGATop;
    //public TMP_Text TrainingOperation;
    public byte CorrectFracIndex = 0;
    public TMP_Text Durations;
    public GameObject LevelDurationButton;
    //private static bool TrainingNewlyFinished = false;
    public GameObject LevelButtonT;//LevelButtonTraining
    private Color RestartButColor;
    public Image RestartButImage;
    public TMP_Text ArrowNumberShower;
    public TMP_Text StickedANumberShower;
    private GuideTarget[] GTs = new GuideTarget[2];
    public Image CurrentBallShower;
    public static bool TrainingAllLevelFinished=false;
    private float CanvasScalerRatio;
    private float MaxFingerSlideDis = 75;
    private float WidthRatio;
    public AudioSource AudioS;
    
    public AudioClip ArrowThrowing0;
    public AudioClip BubbleHit;
    public AudioClip CylinderHit;
    public AudioClip Leaf;
    public AudioClip LevelFailed;
    public AudioClip TooClose;
    public AudioClip WinEffect;//bunun sesini açınca cızırtı oluyor, belki başka bir ses ile değiştirilebilir ama çok kötü değil
    //public TimelineClipExtensions timeline;
    //public ushort BallsUINextAmmo;
    private FracNo OldFracNo;
    
    public class GuideArrow
    {
        public Image Arrow;
        public Image ArrowTop;
        public bool Active;
        public GuideArrow(Image Arrow, Image ArrowTop, bool Active)
        {
            this.Arrow = Arrow;
            this.ArrowTop = ArrowTop;
            this.Active = Active;
        }
    }
    public class GuideTarget
    {
        public Image Target;
        public bool Active;
        public GuideTarget(Image Target, bool Active)
        {
            this.Target = Target;
            this.Active = Active;
        }
    }
    public class LevelType
    {
        public int LastLevel;
        public int LevelNo = 1;
        
        public string Type = "";//enum ile yapılacak
        public TMP_Text TextInfo;
        public bool Finished = false;
        public int[] LevelDurations;
        public LevelType(string Type, int LastLevel, TMP_Text TextInfo)
        {
            this.Type = Type;
            this.LastLevel = LastLevel;
            this.TextInfo = TextInfo;
            LevelNo = 1;
            LevelNo = PlayerPrefs.GetInt(Type + "LevelNo");
            if (LevelNo == 0)
            {
                LevelNo = 1;
            }
            //else if(AtLastLevel())
            //{


            //    if (PlayerPrefs.GetInt(Type + " Finished") == 0)
            //    {
            //        Finished = false;
            //    }
            //    else
            //    {
            //        Finished = true;
            //        InfoTextOn();
            //    }
            //}

            if (PlayerPrefs.GetInt(Type + " Finished") == 0)
            {
                Finished = false;
            }
            else
            {
                Finished = true;
                InfoTextOn();
            }




            LevelDurations = new int[LastLevel];
            for(int i = 0; i < LevelDurations.Length; i++)
            {
                LevelDurations[i] = 0;
            }
        }
        public void SaveLevelDuration()
        {
            //Archery Level3 Duration
            PlayerPrefs.SetInt(Type + " Level" + LevelNo + " Duration", LevelDurations[LevelNo - 1]);
        }
        public void LevelChange(bool Up)
        {
            SaveLevelDuration();
            if (Up)
            {
                LevelNo++;
            }
            else
            {
                LevelNo--;
            }

            if (LevelNo > LastLevel)
            {
                LevelNo = LastLevel;
                InfoTextOn();
                PlayerPrefs.SetInt(Type+" Finished", 1);
                //if (Type != "Training")
                {
                    Finished = true;

                }
                if (Type == "Training")
                {
                    TrainingAllLevelFinished = true;
                }
            }
            else if (LevelNo < 1)
            {
                LevelNo = 1;
            }

            PlayerPrefs.SetInt(Type + "LevelNo", LevelNo);


        }
        private void InfoTextOn()
        {
            if (TextInfo)
            {

                TextInfo.enabled = true;
            }
        }
        public bool AtLastLevel()
        {
            return LevelNo == LastLevel;
        }
    }
    private LevelType Archery;
    private LevelType Math;
    private LevelType Training;
    private class IncompleteNumber
    {
        public FracNo No;
        public int Index;
    }
    public class ObjectPool
    {
        public List<GameObject> Objects;
        public ushort Count;
        public ushort Next;
        public ushort ReadyCount;//activeObjectCount
        public GameObject Current;
        
        //public List<ushort> AvailableObjectsIndex;
        public ObjectPool(ushort Count)
        {
            Objects = new List<GameObject>();
            this.Count = Count;
            Next = 0;


        }
        public void SetReadyCount(ushort ReadyCount)
        {
            Next = 0;
            this.ReadyCount = ReadyCount;
            
        }
        public void RestartArrows(Transform ArrowsParent)
        {
            for (int i = 0; i < Count; i++)
            {
                Objects[i].SetActive(false);
                Objects[i].GetComponent<Arrow>().Number.transform.localScale = Vector3.one;

                Objects[i].transform.parent = ArrowsParent;
                Objects[i].transform.localScale = Vector3.one;
                Objects[i].transform.GetChild(1).localScale = Vector3.one * 0.2665091f;

                //Objects[i].GetComponent<Arrow>().ArrowNo = new FracNo(0, 0, 1);
                Objects[i].GetComponent<Arrow>().Number.SetActive(false);
                Objects[i].GetComponent<Arrow>().NoOrder = 0;

                //sadece bir ballun layerı değişecek:
                Objects[i].gameObject.layer = LayerMask.NameToLayer("Default");
                Objects[i].transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");
                Objects[i].transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Default");



            }
        }
        public void SpawnArrows(GameObject Prefab, Transform Parent, GameObject NumberPrefab, Transform NumbersParent)
        {
            for (int i = 0; i < Count; i++)//buna bütün oklar eklenecek her bölüm için(object pooling)
            {
                GameObject g = Instantiate(Prefab, Parent);
                g.SetActive(false);
                Objects.Add(g);
                GameObject text = Instantiate(NumberPrefab, NumbersParent);
                text.GetComponent<EquationNumber>().Ball = g.transform;
                text.SetActive(false);
                g.GetComponent<Arrow>().Number = text;

            }
        }
        public void SpawnBallUIs(GameObject Prefab, Transform parent)
        {
            //optimize edilecek alttaki methoddan  birden fazla var
            for (int i = 0; i < Count; i++)
            {
                GameObject g = Instantiate(Prefab, parent);
                g.SetActive(false);
                Objects.Add(g);



            }
        }
        public void SpawnArrowPathParts(GameObject Prefab, Transform parent)
        {
            for (int i = 0; i < Count; i++)
            {
                GameObject g = Instantiate(Prefab,parent);
                g.SetActive(false);
                Objects.Add(g);



            }
        }
        public void RestartBallUIs()
        {
            for (int i = 0; i < Count; i++)
            {
                Objects[i].SetActive(false);
                Objects[i].GetComponentInChildren<TMP_Text>().color = Color.black;
            }
        }

        public void GetObject(bool active)
        {
            Current = Objects[Next];
            
            Current.SetActive(active);
            
            Next++;

        }
    }

    public void NextLevel()
    {
        CurrentLevelType.LevelChange(true);
        SetLevel();
        
    }
    public void PreviousLevel()
    {
        CurrentLevelType.LevelChange(false);
        SetLevel();
    }
    private void IfTrained()
    {
        if (!Training.Finished)
        {
            CurrentLevelType = Training;
        }
    }
    public void ArcheryFocus()
    {
        CurrentLevelType = Archery;

        LevelButtonT.SetActive(false);
        IfTrained();
        CLevelTText();
        SetLevel();
    }
    public void MathFocus()
    {
        CurrentLevelType = Math;
        LevelButtonT.SetActive(false);
        IfTrained();
        CLevelTText();
        SetLevel();
    }
    private void CLevelTText()
    {//leveltype içine koyulup ordan değiştirilebilir belki:
        CLevelTypeText.text = CurrentLevelType.Type;
    }
    public void OpenMenu()
    {
        if (CurrentLevelType != null)
        {
            CurrentLevelType.SaveLevelDuration();
        }
        TE.enabled = false;

        Menu.SetActive(true);
        Game.SetActive(false);
    }
    public void CloseGuideArrows()
    {
        
        foreach (GuideArrow ga in GAs)
        {
            ga.Arrow.enabled = false;
            ga.ArrowTop.enabled = false;
            ga.Active = false;
        }
        
    }
    public void CloseGuideTargets()
    {

        foreach (GuideTarget gt in GTs)
        {
            gt.Target.enabled = false;
            gt.Active = false;
        }
    }
    
    public void OpenTraining()
    {
        CurrentLevelType = Training;
        
        if (Training.Finished)
        {
            LevelButtonT.SetActive(true);
            CurrentLevelType.LevelNo = 1;
        }


        CLevelTText();
        SetLevel();
    }
    public void Restart()
    {
        if (RestartButImage.color == RestartButColor)
        {
            SetLevel();
        }
    }
    public void SetLevel()
    {
        
        
        
        OOAmmoText.enabled = false;
        
        CloseGuideArrows();
        CloseGuideTargets();

        ActiveNumberShowers(false);

        NextLevelButton.SetActive(false);
        if (CurrentLevelType.Finished && CurrentLevelType.Type != "Training" || TrainingAllLevelFinished)
        {
            TrainingAllLevelFinished = false;
            OpenMenu();
        }
        else
        {
            NewLevel();
        }
        if (Arrows.Current)
        {

            Arrows.Current.GetComponent<Arrow>().IsSticked = true;
        }


        //CurrentLevelType.TextInfo < bu iki kez kullanılıyor
        
        UpdateNextAShower();
        StartCoroutine(NextArrowShower());
        //her level türünde son bölümü geçince menü açılabilir olması gerekiyor
        //training finished olsa bile açılıp kapanabilmesi gerekiyor
        //

        if (CurrentLevelType.Type == "Training" && CurrentLevelType.LevelNo == 5)
        {
            SetCorrectFracIndex(0);
        }
    }
    public IEnumerator BowToAmmo()
    {
        byte SArrowNo = 2;
        bool AimingBegan = false;
        GuideArrow ga = GAs[SArrowNo];
        while (true)
        {
            
            yield return new WaitForSecondsRealtime(0.1f);
            if (Sliding)
            {
                AimingBegan = true;
                
                if (!ga.Active)
                {
                    StartCoroutine(GuideArrowUI(3, false));

                }
            }
            else
            {
                if (AimingBegan)
                {
                    break;

                }
            }
        }
        ga.Arrow.enabled = false;
        ga.ArrowTop.enabled = false;
        ga.Active = false;
    }
    private void CloseTrainingUIs()
    {
        ActiveNumberShowers(false);
        GameGuideUI.SetActive(false);
        NextGuideButton.SetActive(false);
        PlusOrMinus.enabled = false;
    }
    string ballsNoTrainingL2;
    string stickedNoTrainingL2;
    public int TrainingOperationSA = 0;//TrainingOperationStickedArrow
    private int TrainingOperationA = 0;//TrainingOperationArrow
    public IEnumerator SetTrainingOperation()
    {
        Arrow StickedCurrentArrow = StickedArrows.Objects[TrainingOperationSA].GetComponent<Arrow>();
        OldFracNo = StickedCurrentArrow.ArrowNo;
        yield return new WaitForSecondsRealtime(0.01f);
        ActiveNumberShowers(true);

        Arrow CurrentArrow= Arrows.Objects[TrainingOperationA].GetComponent<Arrow>();


        StickedCurrentArrow.NoText.text = FracNoToString(OldFracNo,false,false);


        //optimize edilecek(belki) textten almak yerine asıl sayı olarak alınacak:
        ballsNoTrainingL2 = FracNoToString(CurrentArrow.ArrowNo, false, false);
        stickedNoTrainingL2 = FracNoToString(OldFracNo, false, false);
        float NS2x = 0;
        string operation = "+";
        
        if (CurrentArrow.ArrowNo.Oparetion == 1)
        {
            ballsNoTrainingL2 = "-" + ballsNoTrainingL2;
            NS2x = NumberShower[1].preferredWidth / 2;
        }
        
        NumberShower[0].text = ballsNoTrainingL2;
        NumberShower[1].text = stickedNoTrainingL2;


        NumberShower[0].fontSize = CurrentArrow.Number.GetComponent<TMP_Text>().fontSize;
        NumberShower[1].fontSize = StickedCurrentArrow.Number.GetComponent<TMP_Text>().fontSize * StickedCurrentArrow.Number.transform.localScale.x;

        NumberShower[2].text = /*ballsNo + */operation/* + stickedNo*/;
        NumberShower[2].rectTransform.anchoredPosition = new Vector2(NS2x, -c.GetComponent<RectTransform>().rect.height / 4f);
        NumberShower[2].fontSize = TargetNoText.fontSize * 1.6f;

        GuideArrowUI679[0] = GuideArrowUI(6, false);
        GuideArrowUI679[1] = GuideArrowUI(7, false);



        StartCoroutine(GuideArrowUI679[0]);
        StartCoroutine(GuideArrowUI679[1]);
        

    }
    
    public void TrainingNext()
    {
        int LevelNo = CurrentLevelType.LevelNo;
        GuideCount--;
        switch (GuideCount)
        {
            case 0:
                CloseGuideArrows();
                CloseGuideTargets();
                ThrowAMouseArea.enabled = true;
                NextGuideButton.SetActive(false);

                switch (LevelNo)
                {
                    case 1:

                        break;
                    case 2:

                        
                        NextLevel();

                        break;
                    case 3:


                        //ThrowAMouseArea.enabled = true;
                        //NextGuideButton.SetActive(false);
                        //GameGuideUI.SetActive(false);
                        //StartCoroutine(GuideTargetUI(StickedArrows.Current.GetComponent<Arrow>().Number, false, 0));
                        
                        Arrows.Objects[0].GetComponent<Arrow>().CurrentArrowUpdate();
                        TE.EquationUpdate(0, false, false);


                        IfWon();

                        ActiveNumberShowers(false);
                        NextLevelButton.SetActive(true);

                        break;
                    case 4:




                        IfWon();
                        GameGuideUI.SetActive(false);
                        NextLevelButton.SetActive(true);
                        
                        break;
                    case 5:

                        IfWon();


                        break;
                }
                break;
            case 1:
                switch (LevelNo)
                {
                    case 2:


                        CloseGuideArrows();
                        StartCoroutine(GuideArrowUI(13, false));
                        NextGuideButton.SetActive(false);

                        break;
                    case 3:
                        CloseGuideArrows();
                        NextGuideButton.SetActive(false);
                        int result = int.Parse(ballsNoTrainingL2) + int.Parse(stickedNoTrainingL2);
                        NumberShower[2].text = ballsNoTrainingL2 + " + " + stickedNoTrainingL2 + " = " + result;
                        NumberShower[0].enabled = false;
                        NumberShower[1].enabled = false;
                        StartCoroutine(WaitAndBringResult(result));

                        break;
                    case 4:
                        TrainingOpResult(true);
                        
                        break;
                    case 5:
                        TrainingOpResult(false);






                        break;
                }
                break;
            case 2:

                //level3:
                //CloseGuideArrows();
                //CloseGuideTargets();
                //PlusOrMinus.enabled = true;
                //StartCoroutine(GuideArrowUI(1, false, 0, true));

                switch (LevelNo)
                {
                    case 2:
                        CloseGuideArrows();
                        StartCoroutine(GuideArrowUI(12, false));
                        NextGuideButton.SetActive(false);

                        
                        break;
                    case 3:

                        StartCoroutine(GuideTargetUI(StickedArrows.Objects[0].GetComponent<Arrow>().Number, false, 0));
                        NumberShower[1].enabled = false;
                        NextGuideButton.SetActive(false);
                        GameGuideUI.SetActive(false);
                        ThrowAMouseArea.enabled = true;
                        break;
                    case 4:
                        //CloseGuideArrows();
                        CloseGuideTargets();
                        ActiveNumberShowers(false);
                        NextGuideButton.SetActive(false);
                        ThrowAMouseArea.enabled = true;
                        //StartCoroutine(GuideTargetUI(TE.GetComponent<TargetEquation>().Number, true, 0));
                        SetCorrectFracIndex(1);
                        Arrows.Objects[0].SetActive(false);
                        Arrows.Objects[0].GetComponent<Arrow>().Number.SetActive(false);
                        TE.ccs[0].RotSpeed = 15;


                        

                        break;
                    case 5:



                        CloseGuideTargets();
                        ActiveNumberShowers(false);
                        NextGuideButton.SetActive(false);
                        ThrowAMouseArea.enabled = true;
                        SetCorrectFracIndex(1);
                        Arrows.Objects[0].SetActive(false);
                        Arrows.Objects[0].GetComponent<Arrow>().Number.SetActive(false);
                        TE.ccs[0].RotSpeed = 15;



                        break;
                }

                break;
            case 3:

                switch (LevelNo)
                {
                    case 2:

                        CloseGuideArrows();
                        StartCoroutine(GuideArrowUI(11, false));
                        GameGuideUI.SetActive(false);
                        NextGuideButton.SetActive(false);

                        break;
                    case 4:
                        TrainingOpResult(true);
                        TrainingOperationA++;
                        


                        break;
                    case 5:
                        TrainingOpResult(false);
                        TrainingOperationA++;
                        break;
                }
                break;
            case 4:
                switch (LevelNo)
                {
                    case 2:
                        NextGuideButton.SetActive(true);
                        CloseGuideArrows();
                        CloseGuideTargets();
                        GameGuideText.text = "Every ball sticked to middle cylinder have a number too. They are equations numbers.";
                        GameGuideUI.SetActive(true);
                        break;
                    case 4:
                        CloseGuideArrows();
                        
                        PlusOrMinus.enabled = false;
                        NextGuideButton.SetActive(false);
                        ThrowAMouseArea.enabled = true;
                        SetCorrectFracIndex(0);
                        TE.ccs[0].RotSpeed = 15;
                        




                        break;
                }

                break;
            case 5:

                //level4:
                




                StartCoroutine(GuideArrowUI(11, true));
                GameGuideUI.SetActive(false);
                NextGuideButton.SetActive(false);

                break;
            case 6:
                //level4:
                CloseGuideArrows();
                //SetCorrectFracIndex(1);


                NextGuideButton.SetActive(true);
                GameGuideText.text = "Be careful to the equation, red means minus number.";
                GameGuideUI.SetActive(true);
                PlusOrMinus.enabled = false;


                
                
                break;
            case 7:

                //level4:
                CloseGuideArrows();
                BlueShowed = false;
                GameGuideUI.SetActive(false);

                StartCoroutine(GuideArrowUI(11, true));
                NextGuideButton.SetActive(false);

                PlusOrMinus.enabled = false;
                break;
        }
        
    }
    private void TrainingOpResult(bool UpdateMinus)
    {
        CloseGuideArrows();
        NextGuideButton.SetActive(false);
        
        //operation = " + ";
        //opSign = 1;
        if (UpdateMinus)
        {
            Arrow CurrentArrow = Arrows.Objects[TrainingOperationA].GetComponent<Arrow>();
            if(CurrentArrow.ArrowNo.Oparetion == 1)
            {
                ballsNoTrainingL2 = "-" + ballsNoTrainingL2;

            }
        }
        int StickedCurrentArrowNo = OldFracNo.UpNo;

        int result = int.Parse(ballsNoTrainingL2) + StickedCurrentArrowNo;
        NumberShower[2].text = ballsNoTrainingL2 + " + " + stickedNoTrainingL2 + " = " + result;
        NumberShower[0].enabled = false;
        NumberShower[1].enabled = false;
        StartCoroutine(WaitAndBringResult(result));



    }
    public void SetCorrectFracIndex(byte NextNumbersArrayIndex)
    {

        CorrectFracIndex = 0;
        //if (NextNumbersArrayIndex == 0)
        {
            
            for (int i = 0; i < EquationNumbers.Count; i++)
            {
                if (EquationNumbers[NextNumbers[NextNumbersArrayIndex].Index] == StickedArrows.Objects[i].GetComponent<Arrow>().ArrowNo)
                {
                    CorrectFracIndex = (byte)StickedArrows.Objects[i].GetComponent<Arrow>().NoOrder;
                }

            }
        }
        //else
        //{
        //    if (CorrectFracIndex == 0)
        //    {
        //        CorrectFracIndex = 1;
        //    }
        //    else
        //    {
        //        CorrectFracIndex = 0;
        //    }
        //}

        CloseGuideTargets();
        CloseGuideArrows();
        StartCoroutine(GuideTargetUI(StickedArrows.Objects[CorrectFracIndex].GetComponent<Arrow>().Number, false, NextNumbersArrayIndex));
    }

    private IEnumerator WaitAndBringResult(int result)
    {
        //bekletmek gerçekten gerekli mi?
        yield return new WaitForSecondsRealtime(0.75f);
        Arrows.Objects[TrainingOperationA].GetComponent<Arrow>().Number.SetActive(false);
        Arrows.Objects[TrainingOperationA].SetActive(false);
        //Debug.Log(Arrows.Objects[TrainingOperationA].GetComponent<Arrow>().Number.GetComponent<TMP_Text>().text);
        GameObject Number = StickedArrows.Objects[TrainingOperationSA].GetComponent<Arrow>().Number;

        Number.GetComponent<TMP_Text>().text = result.ToString();
        Number.SetActive(false);

        NumberShower[0].enabled = true;

        NumberShower[0].text = result.ToString();



        NumberShower[0].rectTransform.localPosition = LetterPos(NumberShower[2], NumberShower[2].text.Length - 1, NumberShower[2].text.Length - 1);


        //StartCoroutine(MovableArrowedText(NumberShower[0], Number.GetComponent<RectTransform>().localPosition, Number.GetComponent<TMP_Text>().fontSize * Number.transform.localScale.x));
        GuideArrowUI679[0] = GuideArrowUI(9, false);
        StartCoroutine(GuideArrowUI679[0]);



    }

    private IEnumerator OutOfAmmoBlink()
    {
        if (!OOABlinking)
        {
            bool Blink = false;
            OOABlinking = true;
            ushort finish = 0;
            while (OOABlinking)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                Blink = !Blink;
                if (Blink)
                {
                    OOAmmoText.color = Color.red;
                }
                else
                {
                    OOAmmoText.color = Color.black;

                }
                finish++;
                if (finish > 6)
                {
                    OOABlinking = false;
                    OOAmmoText.color = Color.black;
                }
            }
        }
        
    }
    private IEnumerator[] GuideArrowUI679=new IEnumerator[3];
    private void CloseGuideArrowUI679()
    {
        foreach(IEnumerator i in GuideArrowUI679)
        {
            if (i != null)
            {

                StopCoroutine(i);
            }
        }
    }
    private Vector3 GetMousePos()
    {
        if (PlatformTouchScreen)
        {
            return Touchscreen.current.position.ReadValue();
        }
        else
        {
            return Mouse.current.position.ReadValue();
        }
        
    }
    public struct GuideArrowParams
    {
        public byte i;
        public bool b;
        public GuideArrowParams(byte i,bool b)
        {
            this.i = i;
            this.b = b;
        }
    }


    byte firstBlue = 0;
    bool BlueShowed = false;
    //şu methodun değer kısmına yazılacak:
    //RectTransform ArrowHead,RectTransform BackArrow,Vector3 BackArrowV,Vector3 ArrowHeadV,float Space,float MinLengthArrow,float OtherTextFontSize,float fontSizeDif, byte MovableAndStopped, Color col, bool Aiming, byte SArrowNo, bool GoFromStartToEnd
    //arrowtype yerine de 3 4 tip farklı değer alabilen değer yapılacak int ya da enum olabilir
    public IEnumerator GuideArrowUI(byte ArrowType, bool OperationColor)
    {

        yield return new WaitForSecondsRealtime(0.01f);
        int SArrowNo = 0;
        Color col = Color.black;
        byte Operation = 0;

        switch (OperationColor)
        {
            case true:


                if (!BlueShowed)
                {
                    col = BlueAddition;
                    Operation = StickedArrows.Objects[0].GetComponent<Arrow>().ArrowNo.Oparetion;
                    firstBlue = Operation;
                    BlueShowed = true;
                }
                else
                {
                    col = RedSubtraction;
                }



                break;

        }


        if (ArrowType == 10)//mouse
        {
            col = BlueAddition;
        }

        bool GoFromStartToEnd = false;
        bool Stable = false;
        RectTransform ArrowHead=null;
        RectTransform GuideArrowUI;
        RectTransform BackArrow = null;
        Vector3 BackArrowV = Vector3.zero;
        Vector3 ArrowHeadV = Vector3.zero;
        float Space = 0.88f;
        float MinLengthArrow = 50;
        
        float OtherTextFontSize=0;
        float fontSizeDif = 0;


        byte MovableAndStopped = 0;
        //0 both movable
        //1 just Backarrow movable
        //2 just arrowhead movable
        //3 both stopped
        
        switch (ArrowType)
        {
            case 13:
            case 12:
            case 11:
                SArrowNo = ArrowType - 11;
                Arrow a;
                if (OperationColor)
                {
                    a = StickedArrows.Objects[firstBlue].GetComponent<Arrow>();
                    if (firstBlue == 0)
                    {
                        firstBlue = 1;
                    }
                    else
                    {
                        firstBlue = 0;
                    }
                }
                else
                {
                    a = StickedArrows.Objects[SArrowNo].GetComponent<Arrow>();
                    if (firstBlue == 0)
                    {
                        firstBlue = 1;
                    }
                    else
                    {
                        firstBlue = 0;
                    }
                }
                
                Space = 0.94f;

                ArrowHeadV = LetterPos(TE.EquationText, a.TextPlace, a.TextPlace + GetSpaceCountOfNo(a.ArrowNo));
                
                BackArrow = a.Number.GetComponent<RectTransform>();
                BackArrowV = BackArrow.localPosition;
                MovableAndStopped = 0;
                GoFromStartToEnd = true;

                break;
            //case 0:
                
            //    a = StickedArrows.Objects[0].GetComponent<Arrow>();
                
            //    ArrowHeadV = LetterPos(TE.EquationText, a.TextPlace, a.TextPlace + GetSpaceCountOfNo(a.ArrowNo));
                
            //    BackArrow = a.Number.GetComponent<RectTransform>();
            //    MovableAndStopped = 0;
            //    GoFromStartToEnd = true;
            //    Space = 0.95f;
            //    break;
            //case 1:

            //    ArrowHeadV = LetterPos(TE.EquationText, 0, GetSpaceCountOfNo(TE.No) - 1);
            //    BackArrow = TE.Number.GetComponent<RectTransform>();
            //    MovableAndStopped = 1;
            //    break;
            case 2:
                //bişey yok

                RectTransform ArrowHeadRect= BallsUI.Current.GetComponent<RectTransform>();
                //ArrowHead = BallsUI.Objects[BallsUI.Next].GetComponent<RectTransform>();
                ArrowHeadV = ArrowHeadRect.localPosition + (Vector3)ArrowHeadRect.sizeDelta / 2;
                //ArrowHeadV = ArrowHead.localPosition;
                BackArrowV = GameGuideUI.GetComponent<RectTransform>().localPosition+(ArrowHeadV- GameGuideUI.GetComponent<RectTransform>().localPosition)*0.2f;
                Space = 0.8f;
                MovableAndStopped = 2;
                GoFromStartToEnd = true;
                break;
            //case 3:

            //    SArrowNo = 2;
            //    ArrowHead = Arrows.Current.GetComponent<Arrow>().Number.GetComponent<RectTransform>();
                
            //    BackArrow = BallsUI.Objects[BallsUI.Next].GetComponent<RectTransform>();
            //    BackArrowV = BackArrow.localPosition + (Vector3)BackArrow.sizeDelta / 2;
            //    Space = 0.7f;
            //    MovableAndStopped = 2;
            //    break;
            case 4:
                //bişey yok
                SArrowNo = 0;
                ArrowHead = NumberShower[SArrowNo/*0*/].rectTransform;
                BackArrowV = LetterPos(TE.EquationText, TE.EquationText.text.Length - GetSpaceCountOfNo(TE.Equivalent), TE.EquationText.text.Length - 1);
                Space = 0.8f;
                MovableAndStopped = 2;
                GoFromStartToEnd = true;
                Vector3 WESpace = new Vector3((NumberShower[2].textInfo.characterInfo[0].topRight.x - NumberShower[2].textInfo.characterInfo[0].topLeft.x) / 2f + 20 + (NumberShower[0].textInfo.characterInfo[NumberShower[0].text.Length - 1].topRight.x - NumberShower[0].textInfo.characterInfo[0].topLeft.x) / 2f, 0);
                Vector2 EqualityPosMid = NumberShower[2].rectTransform.localPosition - WESpace;

                ArrowHeadV = EqualityPosMid;
                break;
            case 5:
                //bişey yok
                SArrowNo = 1;

                ArrowHead = NumberShower[SArrowNo/*1*/].rectTransform;
                BackArrowV = LetterPos(TargetNoText, TargetNoText.text.Length - GetSpaceCountOfNo(TE.Equivalent), TargetNoText.text.Length - 1);
                Space = 0.8f;
                MovableAndStopped = 2;
                GoFromStartToEnd = true;
                WESpace = new Vector3((NumberShower[2].textInfo.characterInfo[0].topRight.x - NumberShower[2].textInfo.characterInfo[0].topLeft.x) / 2f + 20 + (NumberShower[0].textInfo.characterInfo[NumberShower[0].text.Length - 1].topRight.x - NumberShower[0].textInfo.characterInfo[0].topLeft.x) / 2f, 0);
                EqualityPosMid = NumberShower[2].rectTransform.localPosition + WESpace;

                ArrowHeadV = EqualityPosMid;

                break;
                
            case 6:
                SArrowNo= 0;
                BackArrow = Arrows.Objects[TrainingOperationA].GetComponent<Arrow>().NoText.GetComponent<RectTransform>();

                ArrowHead = NumberShower[SArrowNo].rectTransform;

                BackArrowV = BackArrow.localPosition;
                Space = 0.8f;
                MovableAndStopped = 2;
                MinLengthArrow = 20;
                GoFromStartToEnd = true;
                
                OtherTextFontSize = NumberShower[2].fontSize;
                fontSizeDif = OtherTextFontSize - NumberShower[SArrowNo].fontSize;
                float EquationFontSizeDif = OtherTextFontSize / NumberShower[SArrowNo].fontSize;
                WESpace = new Vector3(NumberShower[2].preferredWidth / 2 + /*NumberShower[1]=tek karakter değilse sorun çıkararır*/NumberShower[1].preferredWidth + NumberShower[SArrowNo].preferredWidth / 2 * EquationFontSizeDif, 0);

                EqualityPosMid = NumberShower[2].rectTransform.localPosition - WESpace/*WESpacePreferredWidth*/;
                ArrowHeadV = EqualityPosMid;
                break;
            case 7:
                SArrowNo = 1;
                BackArrow = StickedArrows.Objects[TrainingOperationSA].GetComponent<Arrow>().Number.GetComponent<RectTransform>();
                //ArrowHeadV = LetterPos(TrainingOperation, TrainingOperation.text.Length - 5, TrainingOperation.text.Length - 5);//eğer StickedArrows'da 1 den fazla karakter olursa tam ortaya denk gelmez
                ArrowHead = NumberShower[SArrowNo].rectTransform;
                BackArrowV = BackArrow.localPosition;
                Space = 0.8f;
                MinLengthArrow = 20;
                MovableAndStopped = 2;
                GoFromStartToEnd = true;

                OtherTextFontSize = NumberShower[2].fontSize;
                fontSizeDif = OtherTextFontSize - NumberShower[SArrowNo].fontSize;
                EquationFontSizeDif = OtherTextFontSize / NumberShower[SArrowNo].fontSize;
                WESpace = new Vector3(NumberShower[2].preferredWidth / 2 + /*NumberShower[1]=tek karakter değilse sorun çıkararır*/NumberShower[1].preferredWidth + NumberShower[SArrowNo].preferredWidth / 2* EquationFontSizeDif, 0);

                EqualityPosMid = NumberShower[2].rectTransform.localPosition + WESpace/*WESpacePreferredWidth*/;
                ArrowHeadV = EqualityPosMid;
                
                
                break;
            case 8:

                a = StickedArrows.Objects[CorrectFracIndex].GetComponent<Arrow>();
                BackArrow = StickedArrows.Objects[CorrectFracIndex].GetComponent<Arrow>().Number.GetComponent<RectTransform>();
                BackArrowV = BackArrow.localPosition + (Vector3)BackArrow.sizeDelta / 2;
                Space = 0.92f;
                ArrowHeadV = LetterPos(TE.EquationText, a.TextPlace, a.TextPlace + GetSpaceCountOfNo(a.ArrowNo));
                MovableAndStopped = 0;
                GoFromStartToEnd = true;
                break;
            case 9:
                SArrowNo = 0;
                ArrowHead = NumberShower[SArrowNo].rectTransform;
                BackArrowV = NumberShower[SArrowNo].rectTransform.localPosition;
                Space = 0.8f;

                MovableAndStopped = 2;
                GoFromStartToEnd = true;
                GameObject n = StickedArrows.Objects[TrainingOperationSA].GetComponent<Arrow>().Number;
                OtherTextFontSize = n.GetComponent<TMP_Text>().fontSize*n.transform.localScale.x;
                ArrowHeadV = n.GetComponent<RectTransform>().localPosition;
                fontSizeDif = OtherTextFontSize -NumberShower[SArrowNo].fontSize;
                break;
            case 10:




                ArrowHeadV = mouseStartedPos;
                BackArrowV = mouseStartedPos;
                MovableAndStopped = 1;

                Space = 1;
                

                break;

        }
        GuideArrow GA;

        if (ArrowType == 10)
        {
            GA = AimingGA;

        }
        else
        {
            GA = GAs[SArrowNo];
        }
        GA.Arrow.color = col;
        GA.ArrowTop.color = col;
        RectTransform TopArrow = GA.ArrowTop.rectTransform;


        GuideArrowUI = GA.Arrow.rectTransform;



        
        Vector2 EndPos=Vector2.zero;
        if (GoFromStartToEnd)
        {
            EndPos = ArrowHeadV;
            ArrowHeadV = BackArrowV;
            
        }

        if (MovableAndStopped != 2)
        {
            GA.Arrow.enabled = true;
            GA.ArrowTop.enabled = true;
            
        }

        float StartEndSpeed = 1;
        float StartToEndPos = 0;


        GA.Active = true;
        GuideArrowUI.sizeDelta = new Vector2(0, GA.Arrow.rectTransform.sizeDelta.y);
        float time = 0;
        while (GA.Active)
        {
            yield return new WaitForSeconds(0.005f);
            time += Time.deltaTime;
            if (time > 0.01f)
            {
                time = 0;

                switch (MovableAndStopped)
                {
                    case 0:
                        if (!GoFromStartToEnd)
                        {
                            ArrowHeadV = ArrowHead.localPosition;

                        }
                        BackArrowV = BackArrow.localPosition;

                        break;
                    case 1:
                        if (BackArrow != null)
                        {

                            BackArrowV = BackArrow.localPosition;
                        }
                        else
                        {
                            
                            BackArrowV = GetMousePos();
                            Vector2 Between = ((Vector2)BackArrowV - (Vector2)ArrowHeadV) * (1 - Space) / 2;

                            GuideArrowUI.position = (Vector2)ArrowHeadV + Between;
                            TopArrow.position = GuideArrowUI.position;
                            float Dis = Vector2.Distance(BackArrowV, ArrowHeadV) * Space * CanvasScalerRatio;
                            if (Dis > MaxFingerSlideDis)
                            {
                                Dis = MaxFingerSlideDis;
                            }
                            GuideArrowUI.sizeDelta = new Vector2(Dis, GuideArrowUI.sizeDelta.y);


                        }
                        break;
                    case 2:
                        
                        if(!GoFromStartToEnd)
                        {
                            ArrowHeadV = ArrowHead.localPosition;
                            
                        }
                        
                        if (GuideArrowUI.sizeDelta.x > MinLengthArrow && GA.Active)
                        {

                            GA.Arrow.enabled = true;
                            GA.ArrowTop.enabled = true;
                        }
                        break;
                    case 3:
                        Stable = true;
                        break;
                }


                

                if (ArrowType != 10)
                {
                    
                    if (GoFromStartToEnd)
                    {
                        float FixedSpeed = StartEndSpeed * Time.deltaTime;
                        if (StartEndSpeed != 0)
                        {
                            ArrowHeadV = BackArrowV + ( (Vector3)EndPos- BackArrowV) * StartToEndPos;
                            StartToEndPos += FixedSpeed;
                        }
                        

                        if (ArrowType == 4 || ArrowType == 5 || ArrowType == 6 || ArrowType == 7||ArrowType==9)
                        {
                            ArrowHead.anchoredPosition = ArrowHeadV;
                            if (OtherTextFontSize != 0)
                            {
                                
                                NumberShower[SArrowNo].fontSize += fontSizeDif * FixedSpeed;
                                
                            }

                        }
                        if (StartToEndPos>=1)
                        {
                            if(StartEndSpeed != 0)
                            {
                                if (ArrowType == 6 || ArrowType == 7)
                                {
                                    ArrowHead.anchoredPosition = EndPos;
                                    NumberShower[SArrowNo].fontSize = OtherTextFontSize;
                                }
                                if (!(ArrowType == 4 || ArrowType == 5))
                                {

                                    NextGuideButton.SetActive(true);
                                }
                            }
                            
                            if (MovableAndStopped == 0)
                            {

                                if (StartEndSpeed != 0)
                                {
                                    if (col == BlueAddition)
                                    {
                                        PlusOrMinus.sprite = PMShower[0];
                                        PlusOrMinus.enabled = true;
                                    }
                                    else if (col == RedSubtraction && ArrowType == 11)
                                    {
                                        PlusOrMinus.sprite = PMShower[1];
                                        PlusOrMinus.enabled = true;
                                    }
                                }

                                StartEndSpeed = 0;
                            }
                            else
                            {
                                GA.Active = false;

                            }


                            if (ArrowType == 11 || ArrowType == 12 || ArrowType == 13)
                            {
                                

                                StartEndSpeed = 0;
                            }
                            
                            
                            
                        }
                        
                    }
                    Vector2 Between = ((Vector2)BackArrowV - (Vector2)ArrowHeadV) * (1 - Space) / 2;
                    
                    GuideArrowUI.localPosition = (Vector2)ArrowHeadV + Between;
                    TopArrow.localPosition = GuideArrowUI.localPosition;
                    GuideArrowUI.sizeDelta = new Vector2(Vector2.Distance(BackArrowV, ArrowHeadV)* Space, GuideArrowUI.sizeDelta.y);
                
                }

                


                Vector3 relative = ArrowHeadV - BackArrowV;

                float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg + 180;
                GuideArrowUI.rotation = Quaternion.Euler(0, 0, angle);
                TopArrow.rotation = Quaternion.Euler(0, 0, angle + 90);
                if (Stable)
                {
                    break;
                }
                



                //hedefe ulaşınca durdurulabilecek var mı

            }

        }
        //Debug.Log("GuideArrowUI Finished");
        yield return new WaitForSecondsRealtime(0.005f);
        
        if (!WonShowerReaching)
        {
            
            if (ArrowType == 4 || ArrowType == 5)
            {
                NumberShower[SArrowNo].rectTransform.localPosition = EndPos;
            }
            if (ArrowType == 6 || ArrowType == 7)
            {
                
                NextGuideButton.SetActive(true);
            }
            if (ArrowType == 9)
            {
                NumberShower[0].enabled = false;
                GAs[0].Active = false;
                StickedArrows.Objects[TrainingOperationSA].GetComponent<Arrow>().Number.SetActive(true);
                GAs[0].Arrow.enabled = false;
                GAs[0].ArrowTop.enabled = false;
                NextGuideButton.SetActive(true);
                //StickedArrows.Objects[TrainingOperationSA].GetComponent<Arrow>().ArrowNo = CurrentFracNo;

                

                TE.EquationUpdate(0, false, false);
                
            }
            if (ArrowType == 8)
            {
                NextGuideButton.SetActive(true);
            }
        }
       
    }
    
    private void NewLevel()
    {
        
        RestartButImage.color =RestartButColor;
        TE.LevelStart();
        ThrowAMouseArea.enabled = true;
        TE.enabled = true;
        Menu.SetActive(false);
        if (!Game.activeSelf)
        {
            Game.SetActive(true);
            if (LevelDurationButton.activeSelf)
            {
                StartCoroutine(LevelDurationCounter());
            }
            

        }
        CurrentBlueCount = 0;
        CurrentRedCount = 0;
        EquationNumbers.Clear();
        Levels();
        TE.SetNo();
        MinThrowForArrows = (ushort)(MinThrow + 1);

        Arrows.RestartArrows(ArrowsParent.transform);
        Arrows.SetReadyCount(MinThrowForArrows);
        Arrows.GetObject(false);
        
        StickedArrows.RestartArrows(ArrowsParent.transform);
        StickedArrows.SetReadyCount(FBallCount);
        ArrowPath.Next = 0;

        StickArrows();

        CurrentBlueCount = 0;
        CurrentRedCount = 0;
        SetNextNumbers();
        NextLevelButton.GetComponentInChildren<TMP_Text>().text = "Next";


        
        Won.SetActive(false);
        CalculateEquation();


        TargetAndEquivalentSame();
        
    }
    private void TargetAndEquivalentSame()
    {
        //loopa girmesin hep eşit çıkarsa girer:
        if (TE.Equivalent.UpNo == TE.TargetNo.UpNo)//<çarpma bölmede burası editlenecek
        {
            SetLevel();
        }
    }
    private void SetNextNumbers()
    {
        //Setting Incomplate Numbers: method yazılacak muhtemelen:

        BallsUI.SetReadyCount(MinThrow);
        BallsUI.RestartBallUIs();
        NextNumbers = new IncompleteNumber[MinThrow];
        List<ushort> ENumbersIndexes = new List<ushort>();
        for (ushort i = 0; i < FBallCount; i++)
        {
            ENumbersIndexes.Add(i);
        }
        for (ushort i = 0; i < NextNumbers.Length; i++)
        {
            ushort RandomNo = (ushort)Random.Range(0, ENumbersIndexes.Count);

            NextNumbers[i] = new IncompleteNumber();
            IncompleteNumber icn = NextNumbers[i];
            icn.Index = ENumbersIndexes[RandomNo];
            icn.No = RandomFrac(2, NoRange1, NoRange2);
            //icn.No.Oparetion = 1;
            ENumbersIndexes.RemoveAt(RandomNo);


            BallsUI.GetObject(true);
            GameObject NextBall = BallsUI.Current;


            Color color = Color.white;
            //bu methodun aynısı setballda var optimize edilecek:







            //silinecek:
            //if (ArrowOperation > 1)
            //{

            //    switch (NextNumbers[i].No.Oparetion)
            //    {
            //        case 0:
            //            if (ArrowBlueCount <= CurrentBlueCount)
            //            {
            //                color = RedSubtraction;
            //                NextNumbers[i].No.Oparetion = 1;

            //            }
            //            else
            //            {

            //                color = BlueAddition;

            //                NextNumbers[i].No.Oparetion = 0;
            //                CurrentBlueCount++;
            //            }
            //            break;

            //        case 1:

            //            if (MinThrow - ArrowBlueCount <= CurrentRedCount)
            //            {
            //                color = BlueAddition;

            //                NextNumbers[i].No.Oparetion = 0;
            //            }
            //            else
            //            {
            //                color = RedSubtraction;


            //                NextNumbers[i].No.Oparetion = 1;
            //                CurrentRedCount++;
            //            }
            //            break;
            //    }

            //}
            //else
            //{
            //    color = BlueAddition;


            //    NextNumbers[i].No.Oparetion = 0;
            //}





            (NextNumbers[i].No.Oparetion, color) = SetColors(NextNumbers[i].No.Oparetion, ArrowOperation, ArrowBlueCount, MinThrow);







            //switch (NextNumbers[i].No.Oparetion)
            //{
            //    case 0: color = BlueAddition; break;
            //    case 1: color = RedSubtraction; break;
            //}
            NextBall.GetComponent<Image>().color = color;
            //BallUIParent şu an da fullscreen, fullscreen olmayıp bu pozisyon yeniden ayarlanacak:
            NextBall.GetComponent<RectTransform>().anchoredPosition = new Vector2(NextBall.GetComponent<RectTransform>().sizeDelta.x * (NextNumbers.Length - i - 1), 0);
            //NextBallsUI.Add(NextBall);

            NextBall.GetComponentInChildren<TMP_Text>().text = FracNoToString(NextNumbers[i].No, false, false);

        }


        BallsUI.SetReadyCount(MinThrow);

        //foreach (FracNo fn in EquationNumbers)
        //{
        //    Debug.Log(fn.UpNo);
        //}
        //Debug.Log("");
        //foreach (IncompleteNumber In in NextNumbers)
        //{
        //    Debug.Log("Index : "+In.Index);
        //    Debug.Log("FracNo : " + In.No.UpNo);
            
        //}
    }
    private (byte,Color) SetColors(byte operation, byte OperationType,sbyte BlueCount, ushort Count)
    {
        Color c = Color.white;
        if (OperationType > 1)
        {

            switch (operation)
            {
                case 0:
                    if (BlueCount <= CurrentBlueCount)
                    {
                        c = RedSubtraction;
                        operation = 1;

                    }
                    else
                    {

                        c = BlueAddition;

                        operation = 0;
                        CurrentBlueCount++;
                    }
                    break;

                case 1:

                    if (Count - BlueCount <= CurrentRedCount)
                    {
                        c = BlueAddition;

                        operation = 0;
                    }
                    else
                    {
                        c = RedSubtraction;


                        operation = 1;
                        CurrentRedCount++;
                    }
                    break;
            }

        }
        else
        {
            c = BlueAddition;


            operation = 0;
        }
        return (operation,c);
    }
    private void StickArrows()
    {
        for (int i = 0; i < FBallCount; i++)
        {
            StickedArrows.GetObject(true);
            ArrowLoading(StickedArrows.Current);
            Arrow a = StickedArrows.Current.GetComponent<Arrow>();


            a.ArrowNo = RandomFrac(StickArrowOperation, NoRange1, NoRange2);
            SetBall(a, true);
            a.StickToCylinder(TE.transform, true);

            TE.EquationUpdate(0, false, false);

            StickedArrows.Current.transform.RotateAround(TE.transform.position, Vector3.forward, 360 * i / FBallCount);
            
        }
    }
    private void LevelValues(byte ArrowOperation, byte StickArrowOperation, ushort MinThrow, ushort FBallCount, sbyte StickedBlueCount, sbyte ArrowBlueCount, short NoRange1, short NoRange2)
    {
        this.ArrowOperation = ArrowOperation;
        this.StickArrowOperation = StickArrowOperation;
        this.MinThrow = MinThrow;
        this.FBallCount = FBallCount;
        this.StickedBlueCount = StickedBlueCount;
        this.ArrowBlueCount = ArrowBlueCount;
        this.NoRange1 = NoRange1;
        this.NoRange2 = NoRange2;


        

    }
    
    private IEnumerator HowToTArrow()
    {
        HowToTArrowPosUpdate = true;
        HTTA.SetActive(false);
        HTTAPower.rectTransform.position = (Vector2)Camera.main.WorldToScreenPoint(ArcherSpine.transform.position);
        float OldRot = -ArcherSpine.transform.rotation.x;
        int CloseHTTA = 0;
        while (HowToTArrowPosUpdate)
        {
            yield return new WaitForSecondsRealtime(0.02f);
            Quaternion HTTARot = HTTAPower.transform.rotation;
            
            if(OldRot== -ArcherSpine.transform.rotation.x)
            {
                CloseHTTA++;
                if (CloseHTTA >= 50)
                {
                    HTTA.SetActive(false);
                    CloseHTTA = 0;
                }
                
            }
            else
            {
                CloseHTTA = 0;
                HTTA.SetActive(true);
                HTTAPower.rectTransform.rotation = new Quaternion(HTTARot.x, HTTARot.y, -ArcherSpine.transform.rotation.x, HTTARot.w);
            }
            OldRot = -ArcherSpine.transform.rotation.x;
        }
        HTTA.SetActive(false);
    }
    private IEnumerator InformTargeting()
    {
        float time = 0;

        InformFinger.gameObject.SetActive(true);
        InformFinger.GetComponent<RectTransform>().position = FingerPos1.position;
        bool Close = false;
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            float delta = Time.deltaTime;
            time += 15 * delta;
            if (time >= 0.1f)
            {
                InformFinger.position += (FingerPos2.position - InformFinger.position).normalized * 300* delta;//10

                if (!Close)
                {
                    time = 0;
                    if (Vector3.Distance(InformFinger.position, FingerPos2.position) < 20)
                    {

                        Close = true;
                    }

                }
                else
                {
                    InformFinger.gameObject.SetActive(false);
                    if (time > 2)
                    {
                        Close = false;
                        InformFinger.GetComponent<RectTransform>().position = FingerPos1.position;
                        InformFinger.gameObject.SetActive(true);
                        time = 0;
                    }

                }
                if (PlatformTouchScreen&& Touchscreen.current.press.isPressed || !PlatformTouchScreen&& Mouse.current.leftButton.isPressed)
                {
                    break;
                }
            }
        }
        InformFinger.gameObject.SetActive(false);
    }
    
    private void Levels()
    {
        LevelText.text = "Level " + CurrentLevelType.LevelNo;
        switch (CurrentLevelType.Type)
        {
            //bekleme %100 doğru olabilir gerçek saniyeye göre
            case "Archery":
                CloseTrainingUIs();
                switch (CurrentLevelType.LevelNo)
                {
                    
                    case 1:
                        LevelValues(1, 2, 1, 2, 1,0, 1, 5);
                        TE.AddRT(4, 1);
                        TE.AddCC(1, 0, 20, 1, 0);
                        TE.AddRT(4, 1);
                        TE.AddCC(1, 0, 40, 1, 0);
                        TE.AddRT(4, 1);
                        break;
                    case 2:
                        LevelValues(1, 2, 1, 3, 1, 1, 1, 5);
                        TE.AddRT(4, 0);
                        TE.AddCC(1, 0, 30, 1, 0);
                        TE.AddRT(4, 4);
                        TE.AddCC(1, 0, 70, 1, 0);
                        TE.AddRT(4, 0);
                        break;
                    case 3:

                        LevelValues(1, 2, 2, 2, 1, 1, 1, 5);
                        //justRot(RT ile CC sayısı aynı):
                        TE.AddRT(4, 2);
                        TE.AddCC(0, 0, 40, 1, 0);
                        TE.AddRT(4, 3);
                        TE.AddCC(0, 0, 90, 1, 0);
                        TE.AddRT(4, 1);
                        break;
                    case 4:

                        LevelValues(1, 2, 2, 2, 1, 1, 1, 5);
                        TE.AddRT(3, 1.25f);
                        TE.AddCC(1.5f, 0, 25, 1, 0);
                        TE.AddRT(4, 1.25f);
                        break;
                    case 5:
                        LevelValues(1, 2, 1, 1, 0, 1, 1, 5);
                        TE.AddRT(4, 0);
                        TE.AddCC(1, 0, 55, 1, 0);
                        TE.AddRT(5, 0);
                        break;
                    case 6:

                        LevelValues(1, 2, 2, 2, 1, 1, 1, 5);
                        TE.AddRT(1, 0);
                        TE.AddCC(0.7f, 0, 60, -1, 0);
                        TE.AddRT(8, 0);
                        break;
                    case 7:
                        LevelValues(1, 2, 3, 3, 2, 1, 1, 6);
                        TE.AddRT(0, 0);
                        TE.AddCC(1, 0, 32, 1, 0);

                        TE.AddRT(1, 0);
                        TE.AddCC(1, 0, 32, -1, 0);

                        TE.AddRT(8, 0);
                        break;
                    case 8:
                        LevelValues(1, 2, 3, 3, 2, 1, 1, 5);
                        
                        TE.AddRT(3, 1.2f);
                        TE.AddCC(1.5f, 0, 50, 1, 0);
                        TE.AddRT(5, 1.2f);
                        
                        break;
                    case 9:
                        LevelValues(1, 2, 3, 3, 2, 1, 1, 5);

                        TE.AddRT(0, 1.5f);
                        TE.AddCC(0.5f, 0, 60, 1, 0);
                        TE.AddRT(1, 1.5f);
                        break;
                    case 10:

                        LevelValues(1, 2, 3, 3, 2, 1, 1, 7);

                        TE.AddRT(0, 0.5f);
                        TE.AddCC(3, 0, 60, 1, 0);
                        TE.AddRT(4, 6);
                        TE.AddCC(3, 0, 60, 1, 0);
                        TE.AddRT(8, 0.5f);

                        break;
                        //üzerinden geçilecek:
                    case 11:

                        LevelValues(1, 2, 2, 5, 3, 1, 1, 7);

                        TE.AddRT(0, 1.5f);
                        TE.AddCC(1.5f, 0, 45, 1, 0);
                        TE.AddRT(3, 0);
                        TE.AddCC(1.5f, 0, 45, 1, 0);
                        TE.AddRT(4, 0);
                        TE.AddCC(1.5f, 0, 45, 1, 0);
                        TE.AddRT(7, 1.5f);

                        break;
                    case 12:

                        LevelValues(1, 2, 2, 2, 1, 1, 1, 7);
                        TE.AddRT(8, 2);
                        TE.AddCC(2/*1.4f*/, 0, 60, 1, 0);
                        TE.AddRT(2, 0);

                        break;
                    case 13:

                        LevelValues(1, 2, 2, 3, 1, 1, 1, 7);
                        TE.AddRT(2, 0);
                        TE.AddCC(4, 0, /*80*/70, 1, 0);
                        TE.AddRT(6, 0);
                        TE.AddCC(1, 0, /*40*/30, -1, 0);
                        TE.AddRT(8, 0);

                        break;
                    case 14:

                        LevelValues(1, 2, 2, 5, 4, 1, 1, 7);
                        TE.AddRT(5, 0);
                        TE.AddCC(0, 0, 75/*65*/, 1, 0);
                        TE.AddRT(5, 0);

                        break;
                    case 15:

                        LevelValues(1, 2, 2, 3, 2, 1, 1, 7);
                        TE.AddRT(6, 0);
                        TE.AddCC(2.5f, 0, 60, 1, 0);
                        TE.AddRT(1, 0);
                        TE.AddCC(2.5f, 0, 20, -1, 0);
                        TE.AddRT(8, 0);

                        break;
                    case 16:

                        LevelValues(1, 2, 3, 3, 1, 1, 0, 12);
                        TE.AddRT(2, 0);
                        TE.AddCC(2, 0, /*50*/70, -1, 0);
                        TE.AddRT(5, 0);

                        break;
                    case 17:

                        LevelValues(1, 2, 3, 4, 2, 1, 0, 12);
                        TE.AddRT(4, 0);
                        TE.AddCC(0, 0, 85, 1, 0);
                        TE.AddRT(4, 0);

                        break;
                    case 18:

                        LevelValues(1, 2, 2, 3, 2, 1, 0, 15);
                        TE.AddRT(2, 0);
                        TE.AddCC(0.6f/*0.4f*/, 0, 110/*90*/, 1, 0);
                        TE.AddRT(3, 0);

                        break;
                    case 19:

                        LevelValues(1, 2, 3, 5, 2, 1, 0, 15);
                        TE.AddRT(0, 0);
                        TE.AddCC(0, 0, /*110*/130, 1, 0);
                        TE.AddRT(0, 0);

                        break;
                    case 20:

                        LevelValues(1, 2, 3, 3, 1, 1, 0, 15);
                        TE.AddRT(5, 0);
                        TE.AddCC(/*3.5f*/4, 0, 35, 1, 0);
                        TE.AddRT(6, 0);

                        break;








                    //case 21:

                    //    LevelValues(1, 2, 2, 4, 3, 1, 0, 15);
                    //    TE.AddRT(5, 0);
                    //    TE.AddCC(1.5f, 0, 70, 1, 0);
                    //    TE.AddRT(4, 0);
                    //    TE.AddCC(1.5f, 0, 70, 1, 0);
                    //    TE.AddRT(7, 0);
                    //    TE.AddCC(1.5f, 0, 70, 1, 0);
                    //    TE.AddRT(8, 0);

                    //    break;
                    //case 22:

                    //    LevelValues(1, 2, 3, 4, 2, 1, 0, 13);
                    //    TE.AddRT(5, 0);
                    //    TE.AddCC(1, 0, 50, 1, 0);
                    //    TE.AddRT(3, 0);

                    //    break;
                    //case 23:

                    //    LevelValues(1, 2, 2, 5, 4, 1, 0, 15);
                    //    TE.AddRT(2, 0);
                    //    TE.AddCC(2, 0, 65, 1, 0);
                    //    TE.AddRT(4, 0);

                    //    break;
                    //case 24:

                    //    LevelValues(1, 2, 2, 3, 2, 1, 0, 15);
                    //    TE.AddRT(0, 0);
                    //    TE.AddCC(1.7f, 0, 90, -1, 0);
                    //    TE.AddRT(2, 0);
                    //    TE.AddCC(1.7f, 0, 45, 1, 0);
                    //    TE.AddRT(8, 0);

                    //    break;
                    //case 25:

                    //    LevelValues(1, 2, 2, 3, 2, 1, 0, 15);
                    //    TE.AddRT(0, 3);
                    //    TE.AddCC(2.5f, 0, 70, -1, 0);
                    //    TE.AddRT(1, 0);
                    //    TE.AddCC(2.5f, 0, 70, -1, 0);
                    //    TE.AddRT(4, 0);
                    //    TE.AddCC(2.5f, 0, 70, -1, 0);
                    //    TE.AddRT(3, 3);

                    //    break;
                        
                }

                break;
            case "Math":
                CloseTrainingUIs();
                switch (CurrentLevelType.LevelNo)
                {
                    case 1:
                        LevelValues(1, 2, 1, 1, 0, 1, 1, 10);
                        TE.AddRT(4, 0);
                        TE.AddCC(0, 0, 15, 1, 0);
                        TE.AddRT(4, 0);
                        break;
                    case 2:
                        LevelValues(1, 2, 1, 4, 1, 1, 1, 10);

                        TE.AddRT(4, 0);
                        TE.AddCC(0, 0, 30, 1, 0);
                        TE.AddRT(4, 0);
                        break;
                    case 3:
                        LevelValues(1, 2, 2, 3, 2, 1, 1, 10);
                        TE.AddRT(4, 0);
                        TE.AddCC(0, 0, 30, 1, 0);
                        TE.AddRT(4, 0);
                        break;
                    case 4:
                        LevelValues(1, 2, 2, 3, 1, 1, 1, 10);
                        TE.AddRT(4, 3);
                        TE.AddCC(0, 0, 30, 1, 0);
                        TE.AddRT(4, 2);
                        TE.AddCC(0, 0, 45, 1, 0);
                        TE.AddRT(4, 2);
                        break;
                    case 5:
                        LevelValues(1, 2, 3, 3, 2, 1, 1, 9);
                        TE.AddRT(3, 5);
                        TE.AddCC(1, 0, 35, -1, 0);
                        TE.AddRT(5, 3);
                        break;
                    case 6:
                        LevelValues(1, 2, 2, 4, 2, 1, 1, 13);
                        TE.AddRT(2, 0.5f);
                        TE.AddCC(0, 0, 110, -1, 0);
                        TE.AddRT(2, 3);
                        TE.AddCC(0, 0, 25, -1, 0);
                        TE.AddRT(2, 3);
                        break;
                    case 7:
                        LevelValues(1, 2, 3, /*4*/3, 2, 1, 1, 10);
                        TE.AddRT(6, 1);
                        TE.AddCC(0, 0, /*100*/90, 1, 0);
                        TE.AddRT(6, 4);
                        TE.AddCC(0, 0, 30, 1, 0);
                        TE.AddRT(6, 4);
                        break;
                    case 8:
                        LevelValues(1, 2, 2, 4, 1, 1, 0, 12/*18*/);
                        TE.AddRT(7, 0);
                        TE.AddCC(0, 0, 40, 1, 0);
                        TE.AddRT(7, 0);
                        break;
                    case 9:
                        LevelValues(1, 2, 3, 3, 2, 1, 0, 15);
                        TE.AddRT(2, 0);
                        TE.AddCC(3, 0, 40, 1, 0);
                        TE.AddRT(4, 5);
                        TE.AddCC(3, 0, 40, 1, 0);
                        TE.AddRT(6, 0);
                        break;
                    case 10:
                        LevelValues(1, 2, 2, 5, 3, 1, 0, 22);
                        TE.AddRT(4, 2.5f);
                        TE.AddCC(0.25f, 0, 30, -1, 0);
                        TE.AddRT(5, 2.5f);
                        break;
                        //üzerinden geçilecek:(hepsi iyi gibi math için)
                    case 11:
                        LevelValues(1, 2, 2/*1*/, 6, 4, 2/*1*/, 0, 22);
                        TE.AddRT(1, 3);
                        TE.AddCC(0.7f, 0, 30, 1, 0);
                        TE.AddRT(7, 3);
                        break;
                    case 12:
                        LevelValues(1, 2, 3, 4, 1, 1, 1, 14);
                        TE.AddRT(4, 0);
                        TE.AddCC(0, 0, 35, 1, 0);
                        TE.AddRT(4, 0);
                        break;
                    case 13:
                        LevelValues(1, 2, 2, 5, 4, 1, 5, 20);
                        TE.AddRT(7, 0);
                        TE.AddCC(0.7f, 0, 35, -1, 0);
                        TE.AddRT(8, 0);
                        break;
                    case 14:
                        LevelValues(1, 2, 3, /*3*/4, 1/*2*/, 1, 1, 20);
                        TE.AddRT(0, 0);
                        TE.AddCC(1.5f, 0, 25, 1, 0);
                        TE.AddRT(5, 0);
                        break;
                    case 15:
                        LevelValues(2, 2, 2, 3, 2, 1, 1, 10);
                        TE.AddRT(3, 0);
                        TE.AddCC(0, 0, 35, 1, 0);
                        TE.AddRT(3, 0);
                        break;


                    case 16:
                        LevelValues(2, 2, 2, 3, 2, 1, 1, 25/*20*/);
                        TE.AddRT(0, 0);
                        TE.AddCC(/*1*/1.5f, 0, 45, 1, 0);
                        TE.AddRT(3, 0);
                        break;
                    case 17:
                        LevelValues(2, 2, 3, 3, 1, 1, 0, /*30*/20);
                        TE.AddRT(4, 3.5f);
                        TE.AddCC(5, 0, 40, -1, 0);
                        TE.AddRT(6, 3.5f);
                        break;
                    case 18:
                        LevelValues(1, 2, 3, 3, 1, 1, 0, 30);
                        TE.AddRT(4, 0);
                        TE.AddCC(0, 0, 30, -1, 0);
                        TE.AddRT(4, 0);
                        break;
                    case 19:
                        LevelValues(/*2*/1, 2, 2, 3, 1, 1, 0, 25);
                        TE.AddRT(8, 0);
                        TE.AddCC(0, 0, /*50*/60, 1, 0);
                        TE.AddRT(8, 0);
                        break;
                    case 20:
                        //LevelValues(2, 2, 3, 3, 1, 1/*3*/, 0, 20);
                        //TE.AddRT(3, 0);
                        //TE.AddCC(/*1*/1.5f, 0, 40, 1, 0);
                        //TE.AddRT(4, 0);



                        LevelValues(2, 2, 7, 7, 1, 1/*3*/, 0, 20);
                        TE.AddRT(3, 0);
                        TE.AddCC(/*1*/1.5f, 0, 40, 1, 0);
                        TE.AddRT(4, 0);
                        break;
                        //16-20 değiştirildi





                    //case 21:
                    //    LevelValues(2, 2, 3, 4, 3, 1, 0, 17);
                    //    TE.AddRT(5, 0);
                    //    TE.AddCC(0, 0, 30, -1, 0);
                    //    TE.AddRT(5, 0);
                    //    break;
                    //case 22:
                    //    LevelValues(1, 2, 4, 4, 3, 1, 0, 15);
                    //    TE.AddRT(4, 3.5f);
                    //    TE.AddCC(2, 0, 40, -1, 0);
                    //    TE.AddRT(2, 3.5f);
                    //    break;
                    //case 23:
                    //    LevelValues(2, 2, 2, 4, 3, 1, 1, 30);
                    //    TE.AddRT(3, 2);
                    //    TE.AddCC(0.7f, 0, 40, 1, 0);
                    //    TE.AddRT(5, 2);
                    //    break;
                    //case 24:
                    //    LevelValues(2, 2, 3, 3, 2, 1, 0, 20);
                    //    TE.AddRT(3, 0);
                    //    TE.AddCC(0.5f, 0, 40, 1, 0);
                    //    TE.AddRT(7, 0);
                    //    break;
                    //case 25:
                    //    LevelValues(2, 2, 2, 3, 2, 1, -10, 10);
                    //    TE.AddRT(1, 0);
                    //    TE.AddCC(0, 0, 40, 1, 0);
                    //    TE.AddRT(1, 0);
                    //    break;
                        
                }

                break;

            case "Training":
                //cc0.SetValues(0, 0, 40, -1, 0, 0);
                //TE.AddCCsAndPos(cc0, 4);


                /*
                 Nasıl oynanır>ok atma, saplanınca denkleme girme, denkleme saplı top sayısını değiştirme, Targete atılacağını gösterme, kırmızı eksi, mavi artı, ve her biri için yazı
                 your number , target number
                 */
                //sonradan çarpı bölü öğretilecek
                switch (CurrentLevelType.LevelNo)
                {
                    case 1:
                        //StopAllCoroutines();
                        CloseGuideArrows();
                        CloseGuideArrowUI679();
                        LevelValues(1, 1, 1, 1, 1, 1, 1,4);

                        ThrowAMouseArea.enabled = true;
                        GuideCount = 0;
                        NextGuideButton.SetActive(false);
                        //AmmoShowerClose
                        StartCoroutine(InformTargeting());
                        StartCoroutine(HowToTArrow());

                        StartCoroutine(GuideTargetUI(StickedArrows.Objects[0].GetComponent<Arrow>().Number, false, 0));
                        StartCoroutine(GuideTargetUI(TE.Number, false, 1));
                        GameGuideUI.SetActive(false);
                        //StartCoroutine(BowToAmmo());

                        //AmmoShower:
                        //GameGuideText.text = "Ammo";
                        //GameGuideUI.SetActive(true);
                        //StartCoroutine(GuideArrowUI(0,false,2, false));

                        ActiveNumberShowers(false);
                        PlusOrMinus.enabled = false;

                        TE.AddRT(4, 0);
                        TE.AddCC(0, 0, 25, 1, 0);
                        TE.AddRT(4, 0);
                        break;
                    case 2:
                        HowToTArrowPosUpdate = false;
                        //StopAllCoroutines();
                        CloseGuideArrows();
                        CloseGuideArrowUI679();
                        LevelValues(1, 1, 1, 3, 3, 1, 1, 4);

                        GameGuideText.text = "Every arrow have a number and next arrow's number is here";
                        GameGuideUI.SetActive(true);
                        ThrowAMouseArea.enabled = false;


                        NextGuideButton.SetActive(false);
                        GuideCount = 5;
                        ActiveNumberShowers(false);

                        StartCoroutine(GuideArrowUI(2, false));

                        //StartCoroutine(MovableArrow());


                        //StartCoroutine(GuideTargetUI(null, false, 0));




                        //4.bölümde renklerin işaretini göstersin, 5 tede rehbersiz trainingin bitimi olarak oynasın



                        //PlusOrMinus.enabled = true;

                        //StartCoroutine(GuideArrowUI(0, false, 0, true));

                        TE.AddRT(4, 0);
                        TE.AddCC(0, 0, 5, 1, 0);
                        TE.AddRT(4, 0);

                        //NextTrainingText.enabled = true;
                        //burada targete ulaşmak zorunlu olmasın
                        break;
                    case 3:
                        LevelValues(1, 1, 1, 1, 1, 1, 1, 4);
                        CloseGuideArrowUI679();
                        CloseGuideArrows();
                        

                        ThrowAMouseArea.enabled = false;
                        NextGuideButton.SetActive(true);
                        GuideCount = 3;


                        PlusOrMinus.enabled = false;
                        GameGuideText.text = "When you hit the ball, arrow will process with it.";
                        GameGuideUI.SetActive(true);

                        TE.AddRT(4, 0);
                        TE.AddCC(0, 0, 10, 1, 0);
                        TE.AddRT(4, 0);

                        //StopAllCoroutines();
                        //LevelValues(1, 2, 2, 2, 1, 1, 1, 4);
                        //CloseGuideArrowUI679();
                        //CloseGuideArrows();
                        //ThrowAMouseArea.enabled = false;
                        //NextGuideButton.SetActive(true);
                        //GuideCount = 3;
                        //GameGuideUI.SetActive(false);

                        //PlusOrMinus.enabled = true;

                        //StartCoroutine(GuideArrowUI(0, false, 0, true));

                        //TE.AddRT(4, 0);
                        //TE.AddCC(0, 0, 10, 1, 0);
                        //TE.AddRT(4, 0);
                        break;
                    case 4:
                        //ActiveNumberShowers(false);
                        //PlusOrMinus.enabled = false;
                        //GameGuideUI.SetActive(false);
                        //LevelValues(1, 1, 1, 3, 1, 1, 1, 4);
                        //GuideCount = 2;

                        //NextGuideButton.SetActive(true);
                        //StartCoroutine(GuideArrowUI(0, false, 1, false));
                        //ThrowAMouseArea.enabled = false;

                        //TE.AddRT(4, 0);
                        //TE.AddCC(0, 0, 10, 1, 0);
                        //TE.AddRT(4, 0);


                        LevelValues(1, 2, 2, 2, 1, 1, 1, 4);
                        ThrowAMouseArea.enabled = false;
                        PlusOrMinus.enabled = false;
                        NextGuideButton.SetActive(true);
                        GameGuideUI.SetActive(true);
                        TrainingOperationA = 0;
                        GameGuideText.text = "Every arrow and ball have a color, every color has a sign.";
                        GuideCount = 8;
                        TE.AddRT(4, 0);
                        TE.AddCC(0, 0, 5, 1, 0);
                        TE.AddRT(4, 0);
                        break;
                    case 5:
                        LevelValues(2, 2, 2, 2, 1, 0, 1, 4);
                        CloseGuideArrowUI679();
                        CloseGuideArrows();
                        CloseGuideTargets();
                        TrainingOperationA = 0;
                        ThrowAMouseArea.enabled = true;
                        GameGuideUI.SetActive(false);
                        NextGuideButton.SetActive(false);
                        GuideCount = 4;

                        

                        TE.AddRT(4, 0);
                        TE.AddCC(0, 0, 15, 1, 0);
                        TE.AddRT(4, 0);
                       

                        //NextGuideButton.SetActive(true);
                        //GameGuideUI.SetActive(true);
                        ////StopAllCoroutines();
                        //LevelValues(1, 2, 2, 2, 1, 1, 1, 6);
                        //GuideCount = 1;
                        //GameGuideText.text = "Do it yourself!";




                        ////aşağısı optimize (normalde de 4. levelın çağırılmaması gerekiyor if (!CurrentLevelType.Finished) saçma)?
                        ////if (!CurrentLevelType.Finished)
                        ////{
                        ////    NextGuideButton.SetActive(true);
                        ////    //PlusMinusShower.enabled = true;
                        ////    ThrowAMouseArea.enabled = false;
                        ////    ////NextTrainingText.enabled = true;
                        ////    //TrainingTable.enabled = true;
                        ////}
                        //TE.AddRT(4, 0);
                        //TE.AddCC(0, 0, 20, 1, 0);
                        //TE.AddRT(4, 0);
                        break;
                    
                }
                //aşağıda son satır olmazsa daha optimize olur:
                
                break;
        }

        TE.FirstPositions();

    }
    private bool ShowerArrowB = false;
    private IEnumerator ShowerArrowBlink(Image ShowerArrow)
    {
        ShowerArrowB = false;
        yield return new WaitForSeconds(0.1f);
        ShowerArrowB = true;
        float time = 0;
        bool Blink = false;
        while (ShowerArrowB)
        {
            yield return new WaitForSeconds(0.005f);
            time += Time.deltaTime * 10;
            if (time >= 2)
            {
                time = 0;
                Blink = !Blink;
                if (Blink)
                {
                    ShowerArrow.enabled = true;
                }
                else
                {
                    ShowerArrow.enabled = false;
                }
            }
        }
        ShowerArrow.enabled = false;
    }
    public void GuideTextShow(string Message, Image ShowerArrow)
    {
        GameGuideUI.SetActive(true);
        GameGuideText.text = Message;
        if (ShowerArrow)
        {
            StartCoroutine(ShowerArrowBlink(ShowerArrow));
            //ShowerArrow.enabled = true;
        }
        //GuideUIArrow.enabled = true;
        //GuideUIArrow.rectTransform.localScale = new Vector2(1,1);
        //GuideUIArrow.rectTransform.position = GuideEquationShowerPos.position;
        //GuideUIArrow.rectTransform.sizeDelta= GuideEquationShowerPos.sizeDelta;
        
        //NextLevelButton.SetActive(true);
        //TargetGuideVisible = false;
        CloseGuideArrows();
        CloseGuideTargets();
    }
    
    void Awake()
    {
        if (Application.isEditor)
        {
            Time.timeScale = 1.5f;
        }
        WidthRatio = c.pixelRect.width / 100;
        AimingGA = new GuideArrow(AimingGAStick, AimingGATop, false);
        for (int i = 0; i < 3; i++)
        {
            GAs[i] = new GuideArrow(GuideUIArrow0[i], GuideUIArrowTop[i], false);
        }

        for (int i = 0; i < 2; i++)
        {
            GTs[i] = new GuideTarget(TargetUIGuide[i], false);
        }

        
        CanvasScalerRatio = c.GetComponent<CanvasScaler>().referenceResolution.x / Screen.width;


        OpenMenu();
        //Levels();

        LMaskTarget = LayerMask.GetMask("Target");
        //LevelFirstArrowCounts();

        RestartButColor = RestartButImage.color;




        StickedArrows = new ObjectPool(MaxFBallCount);
        StickedArrows.SpawnArrows(ArrowPrefab, ArrowsParent.transform, number, NumbersParent);
        StickedArrows.SetReadyCount(FBallCount);
        MaxThrowForArrows = (ushort)(MaxThrow + 1);
        Arrows = new ObjectPool(MaxThrowForArrows);
        Arrows.SpawnArrows(ArrowPrefab, ArrowsParent.transform, number, NumbersParent);
        Arrows.SetReadyCount(MinThrow);
        
        BallsUI = new ObjectPool(MaxThrowForArrows);
        BallsUI.SpawnBallUIs(NextBallPrefab, BallUIParent);
        BallsUI.SetReadyCount(MinThrow);

        ArrowPath = new ObjectPool(100);//30
        ArrowPath.SpawnArrowPathParts(ArrowPathPartPrefab, ArrowPathParent);
        ArrowPath.Next = 0;

        //MinThrow = 2;//silinecek<
        //ArrowCount = MaxThrow + MaxFBallCount;
        //NextNumbers = new FracNo[MinThrow];
        PlatformTouchScreen = Application.platform== RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;

        TargetIK.transform.position = ShortenBowPos.position;


        Archery = new LevelType("Archery",20, ArcheryTextInfo);
        Math = new LevelType("Math",20, MathTextInfo);
        Training = new LevelType("Training",6, null);

        if (Training.LevelNo <= 4)
        {
            Training.LevelNo = 1;
        }

        GetLevelDurations(Archery);
        GetLevelDurations(Math);
        
    }
    private void GetLevelDurations(LevelType lt)
    {
        for (int i = 0; i < lt.LastLevel; i++)
        {

            lt.LevelDurations[i] = PlayerPrefs.GetInt(lt.Type + " Level" + (i + 1) + " Duration");
        }
    }
    public void ResetLevels()
    {
        
        PlayerPrefs.DeleteAll();



    }
    private void ActiveNumberShowers(bool Active)
    {
        foreach(TMP_Text ns in NumberShower)
        {
            ns.enabled = Active;
        }
    }
    //karakter sayısı ölçer:
    public int GetSpaceCountOfNo(FracNo fn)
    {
        int SpaceNo = (int)Mathf.Floor(Mathf.Log10(Mathf.Abs(fn.UpNo)) + 1);
        if (fn.UpNo == 0)
        {
            SpaceNo = 1;
        }

        if (Mathf.Sign(fn.UpNo) == -1)
        {
            SpaceNo++;
        }
        return SpaceNo;
    }
    private IEnumerator WonEqualityShower()
    {
        yield return new WaitForSecondsRealtime(0.001f);
        ActiveNumberShowers(true);
        string Equivalent = FracNoToString(TE.Equivalent, false, false);
        NumberShower[0].text = Equivalent;
        NumberShower[1].text = Equivalent;
        NumberShower[2].text = "=";
        

        NumberShower[0].fontSize = TargetNoText.fontSize;
        NumberShower[1].fontSize = TargetNoText.fontSize;
        NumberShower[2].fontSize = TargetNoText.fontSize;

        NumberShower[2].rectTransform.anchoredPosition = Vector3.zero;

        yield return new WaitForSecondsRealtime(0.001f);
        
        StartCoroutine(GuideArrowUI(4, false));
        StartCoroutine(GuideArrowUI(5, false));






    }









    private bool WonShowerReaching;
    private IEnumerator MovableArrowedText(TMP_Text text, Vector2 TextEndPos, float OtherTextFontSize)
    {
        WonShowerReaching = false;
        yield return new WaitForSecondsRealtime(0.2f);
        WonShowerReaching = true;
        Vector2 firstPos = text.rectTransform.anchoredPosition;
        float time = 0;
        bool FontSizeChange = OtherTextFontSize != text.fontSize;
        float s = (OtherTextFontSize - text.fontSize) / 25f;
        while (WonShowerReaching)
        {
            //yield return new WaitForSecondsRealtime(0.02f);
            yield return new WaitForSeconds(0.005f);
            time += Time.deltaTime;
            if (time >= 0.01f)
            {
                time = 0;
                //WonShower.rectTransform.anchoredPosition = Vector2.Lerp(WonShower.rectTransform.anchoredPosition, WonShowerEnd, 0.09f);
                text.rectTransform.anchoredPosition += (TextEndPos - firstPos) / 25f;

                if (FontSizeChange)
                {
                    text.fontSize += s;
                }
                //if (Vector2.Distance(text.rectTransform.anchoredPosition, TextEndPos) < 5)
                //{
                //    Debug.Log("Movable Arrowed Text Break");
                //    text.rectTransform.anchoredPosition = TextEndPos;
                //    WonShowerReaching = false;
                //    break;
                //}
            }




        }
        GAs[0].Active = false;
        GAs[1].Active = false;
        GAs[2].Active = false;
        WonShowerReaching = false;
    }

    private Vector2 LetterPos(TMP_Text Text, int Index, int Index2)
    {
        //characterInfo her karakterin pozisyonunu veriyor ama asıl fixed pozisyonunu vermiyor yani bir charın orta noktasını, o yüzden - yazınca hata veriyordu, onu aşağıda düzelttim eğer fixed pozisyonunu verebiliyorsa minus bool'üne gerek yok



        //NumberShower[2].preferredWidth ile yapılacak width hesaplanırken!!!
        TMP_CharacterInfo CI = Text.textInfo.characterInfo[Index];
        TMP_CharacterInfo CI2 = Text.textInfo.characterInfo[Index2];
        
        return Text.rectTransform.localPosition + CI2.topRight+(CI2.bottomRight-CI2.topRight)/2+new Vector3(CI.topLeft.x - CI2.topRight.x, 0)/2;
    }

    private void CalculateEquation()
    {
        for (int i = 0; i < EquationNumbers.Count; i++)
        {
            FracNo AddedFracNo = null;
            for (int i2 = 0; i2 < NextNumbers.Length; i2++)
            {
                if (i == NextNumbers[i2].Index)
                {
                    //nextnumbers list olacak ve NextNumbers[i2] remove edilecek
                    AddedFracNo = FractionalOperation(EquationNumbers[i], NextNumbers[i2].No);
                }
            }
            if (AddedFracNo == null)
            {
                TE.TargetNo = FractionalOperation(TE.TargetNo, EquationNumbers[i]);
            }
            else
            {
                TE.TargetNo = FractionalOperation(TE.TargetNo, AddedFracNo);
            }
            //TE.TargetNo = FractionalOperation(TE.TargetNo, EquationNumbers[i]);
        }
        //Debug.Log("Target No : " + TE.TargetNo.UpNo);





        //foreach (FracNo fn in EquationNumbers)
        //{
        //    Debug.Log(FracNoToString(fn, true, false));
        //}


        NArrowIndex = 0;
        //NextArrowNumbers();
        TargetNoText.text = "Target : " + TE.TargetNo.UpNo.ToString();

        //LastBallUI = NextBallsUI[0].GetComponentInChildren<TMP_Text>();
        LastBallUI = BallsUI.Objects[BallsUI.Next].GetComponentInChildren<TMP_Text>();
        //if (BallsUI.Next >= 0)
        //{

        //    //StartCoroutine(NextArrowBlink());
        //    Debug.Log("BallsUI.Next >= 0");
        //    CurrentBallShower.enabled = false;

        //}

    }
    private IEnumerator UpdateTargetPos(GameObject Number, byte TargetNo)
    {
        while (GTs[TargetNo].Active)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            TargetUIGuide[TargetNo].rectTransform.position = Number.GetComponent<RectTransform>().position;
        }
    }
    private IEnumerator GuideTargetUI(GameObject Number, bool SamePos, byte TargetNo)
    {
        
        yield return new WaitForSecondsRealtime(0.25f);
        if (Number==null)//TrainingLevel2
        {
            Number = StickedArrows.Current.GetComponent<Arrow>().Number;
        }
        bool blink = false;
        TargetUIGuide[TargetNo].rectTransform.position = Number.GetComponent<RectTransform>().position;
        //TargetGuideVisible = true;
        GTs[TargetNo].Active = true;
        TargetUIGuide[TargetNo].enabled = true;

        if (!SamePos)
        {

            StartCoroutine(UpdateTargetPos(Number, TargetNo));
        }


        while (GTs[TargetNo].Active)
        {
            yield return new WaitForSecondsRealtime(0.36f);
            
            if (blink)
            {
                TargetUIGuide[TargetNo].enabled = false;
            }
            else
            {
                TargetUIGuide[TargetNo].enabled = true;
                
            }

            blink = !blink;
        }

        TargetUIGuide[TargetNo].enabled = false;


    }
    private bool NextABlink = true;
    
    private IEnumerator NextArrowBlink()
    {
        NextABlink = false;
        yield return new WaitForSeconds(0.1f);
        NextABlink = true;

        float time = 0;
        bool Blink = false;
        Color c = LastBallUI.color;
        while (NextABlink)
        {
            yield return new WaitForSeconds(0.005f);
            if (BallsUI.ReadyCount <= BallsUI.Next/*BallsUI.Count <= 0*//*NextBallsUI.Count <= 0*/)
            {

                break;
            }
            //TMP_Text text = NextBallsUI[0].GetComponentInChildren<TMP_Text>();


            time += Time.deltaTime * 10;
            if (time >= 3.5)
            {
                time = 0;
                Blink = !Blink;
                if (Blink)
                {
                    LastBallUI.color = c;
                }
                else
                {
                    LastBallUI.color = Color.red;

                }
            }
        }




    }
    private void UpdateNextAShower()
    {

        CurrentBallShower.rectTransform.localPosition = BallsUI.Objects[BallsUI.Next].transform.localPosition + (Vector3)BallsUI.Objects[BallsUI.Next].GetComponent<RectTransform>().sizeDelta / 2;
    }
    private IEnumerator NextArrowShower()
    {
        
        if (!CurrentBallShower.enabled)
        {
            //Debug.Log("CurrentBallShower");
            CurrentBallShower.enabled = true;

            while (CurrentBallShower.enabled)
            {
                yield return new WaitForSeconds(0.005f);
                //optimize edilecek her seferinde kontrol edilmektense her top harcandığın da kontrol etse daha güzel
                //if (BallsUI.ReadyCount <= BallsUI.Next/*BallsUI.Count <= 0*//*NextBallsUI.Count <= 0*/)
                //{
                //    Debug.Log("NextArrowShower Break");
                //    CurrentBallShower.enabled = false;
                //    break;
                //}
                //TMP_Text text = NextBallsUI[0].GetComponentInChildren<TMP_Text>();



                CurrentBallShower.rectTransform.Rotate(0, 0, -Time.deltaTime * 60);

            }
        }
        



    }

    public void Throw()
    {
        if (Arrows.Next < Arrows.ReadyCount)
        {
            StartCoroutine(AimTarget());

        }
        else
        {
            StartCoroutine(OutOfAmmoBlink());
        }
    }
    public void ReleaseArrow()
    {
        Sliding = false;
    }
    private void SetBall(Arrow a, bool WillStick)
    {
        a.Number.SetActive(true);
        ////ok eklenecek mi? bu method ok harcadığı için?
        //a.ArrowNo = RandomFrac(2, NoRange1, NoRange2);//random değil next

        //a.ArrowNo = new FracNo(0,0,1);//random değil next
        a.Number.GetComponent<TMP_Text>().text = FracNoToString(a.ArrowNo, false, false);
        if (WillStick)
        {
            Color c;
            (a.ArrowNo.Oparetion, c) = SetColors(a.ArrowNo.Oparetion, StickArrowOperation, StickedBlueCount, FBallCount);

            a.SetColor(c);



            //silinecek:
            //if (StickArrowOperation > 1)
            //{

            //    switch (a.ArrowNo.Oparetion)
            //    {
            //        case 0:
            //            if (StickedBlueCount <= CurrentBlueCount)
            //            {
            //                a.SetColor(RedSubtraction);
            //                a.ArrowNo.Oparetion = 1;

            //            }
            //            else
            //            {

            //                a.SetColor(BlueAddition);

            //                a.ArrowNo.Oparetion = 0;
            //                CurrentBlueCount++;
            //            }
            //            break;

            //        case 1:

            //            if (FBallCount - StickedBlueCount <= CurrentRedCount)
            //            {
            //                a.SetColor(BlueAddition);

            //                a.ArrowNo.Oparetion = 0;
            //            }
            //            else
            //            {
            //                a.SetColor(RedSubtraction);


            //                a.ArrowNo.Oparetion = 1;
            //                CurrentRedCount++;
            //            }
            //            break;
            //    }

            //}
            //else
            //{
            //    a.SetColor(BlueAddition);


            //    a.ArrowNo.Oparetion = 0;
            //}
        }
        else
        {

            switch (a.ArrowNo.Oparetion)
            {
                case 0: a.SetColor(BlueAddition);
                    break;
                case 1: a.SetColor(RedSubtraction);
                    break;
            }
        }


    }
    
    public FracNo RandomFrac(byte Operation, short NoRange1, short NoRange2)
    {
        return new FracNo((byte)Random.Range(0, Operation)/*0,4*/, Random.Range(NoRange1, NoRange2), 1);
    }
    Vector3 CurrentArrowPathPartPos;
    float Power;
    float ArrowAngle;
    private float MinStretch = 10;
    private IEnumerator AimTarget()
    {
        Sliding = true;

        float StretchPow;
        bool Aiming = false;
        
        mouseStartedPos = GetMousePos();
        Arrow a=null;
        while (Sliding)
        {
            yield return new WaitForSeconds(0.005f);//0.001
            //Time.deltatime koyulacak pcde ve androidde bakılacak fark var mı bu aiming dis
            //sürüklemeye başladığında eğer bir noktaya gelirse germeye başlıyor ama başlangıcı o nokta olarak alıyor, sürüklemeye başladığı nokta alarak başlamalı

            Vector3 MousePos;
            
            MousePos = GetMousePos();

            MousePos.z = 9.5f;//Camera.main.nearClipPlane
            Vector3 MousePosWorld = Camera.main.ScreenToWorldPoint(MousePos);

            
            StretchPow = Vector2.Dot(mouseStartedPos - (Vector2)MousePos, Vector2.right)* CanvasScalerRatio;


            Vector3 mouseStartedPosW = mouseStartedPos;
            mouseStartedPosW.z = 9.5f;
            Vector3 mouseStartedPosWorld = Camera.main.ScreenToWorldPoint(mouseStartedPosW);


            if (/*StretchPow > 30 * WidthRatio && */Vector2.Angle(mouseStartedPosWorld - MousePosWorld, Vector2.left) > 120)
            {

                //ArcherSpine.transform.LookAt(TargetSphere.transform.position);
                ArcherSpine.transform.LookAt(ArcherSpine.transform.position + mouseStartedPosWorld - MousePosWorld);
                ArcherSpine.transform.Rotate(0, 90, 0);
            }


            Rope.SetPosition(0, RopePoints[0].position);
            Rope.SetPosition(1, ArrowFinger.position);
            Rope.SetPosition(2, RopePoints[1].position);

            if (!Aiming)
            {
                if(StretchPow > MinStretch)
                {
                    StartCoroutine(GuideArrowUI(10, false));
                    UpdateNextAShower();





                    

                    a = Arrows.Current.GetComponent<Arrow>();

                    Arrows.Current.SetActive(true);

                    a.ArrowNo = NextNumbers[Arrows.Next - 1].No;
                    //ArcherArrow++;
                    SetBall(a, false);

                    //silinidire atmayı da sayacak

                    Aiming = true;
                }
                


            }
            else
            {
                
                
                ArrowLoading(Arrows.Current);


                //check if touchscreen connected(maybe helps)

                TargetSphere.transform.position = MousePosWorld;
                //Debug.Log(StretchPow);



                

                if (StretchPow < 0)
                {
                    StretchPow = 0;
                }
                else if (StretchPow > MaxFingerSlideDis /** WidthRatio*/)
                {
                    StretchPow = MaxFingerSlideDis/* * WidthRatio*/;
                }

                TargetIK.transform.position = ShortenBowPos.position + StretchPow / (MaxFingerSlideDis/*/CanvasScalerRatio*//* * WidthRatio*/) * (StretchBowPos.position - ShortenBowPos.position);



                /*((Screen.width/100*Screen.width/ Screen.height)) * (c.scaleFactor / 2.4f)* */
                Power = (Mathf.Abs(StretchPow) / 300/*450*/ + 0.025f/*0.015f*/) /*/ WidthRatio*/;//Hipotenus
                ArrowAngle = Arrows.Current.transform.eulerAngles.z * Mathf.Deg2Rad;

                a.ArrowAngle = ArrowAngle;
                a.ArrowPower = Power;
                CurrentArrowPathPartPos = ArrowFinger.position;




                float ArrowsWidth = 0.7f;
                ArrowStep(ArrowsWidth);

                bool NoMoreDots = false;
                for (int i = 0; i < ArrowPath.Count; i++)
                {
                    
                    if (NoMoreDots)
                    {
                        ArrowPath.GetObject(false);
                    }
                    else
                    {

                        ArrowPath.GetObject(true);





                        ArrowStep(spaceBetweenAPD);

                        //CurrentArrowPathPartPos += new Vector3(Mathf.Cos(ArrowAngle), Mathf.Sin(ArrowAngle)) * spaceBetweenAPD;
                        ////if (ArrowAngle * Mathf.Rad2Deg >= -90)
                        ////{
                        //ArrowAngle -= ArrowTurnSpeed * spaceBetweenAPD / Power;

                        //}
                        //else
                        //{
                        //    ArrowAngle = -90 * Mathf.Deg2Rad;
                        //}

                        //if (ArrowAngle * Mathf.Rad2Deg > -90)
                        //{
                        //    ArrowAngle = Mathf.Lerp(ArrowAngle, -90 * Mathf.Deg2Rad, ArrowTurnSpeed * spaceBetweenAPD / Power);

                        //}
                        //else
                        //{
                        //    ArrowAngle = -90 * Mathf.Deg2Rad;
                        //}

                        ArrowPath.Current.transform.position = CurrentArrowPathPartPos;
                        if (Physics.CheckSphere(ArrowPath.Current.transform.position, ArrowPath.Current.GetComponent<MeshRenderer>().bounds.size.x, LMaskTarget))
                        {
                            NoMoreDots = true;
                        }
                    }
                    //if (i < 3)
                    //{
                    //    //optimize edilebilir
                    //    ArrowPath.Current.SetActive(false);
                    //}
                }
                
                ArrowPath.Next = 0;



                if (StretchPow <= MinStretch)
                {
                    Aiming = false;
                    Arrows.Current.SetActive(false);
                    a.Number.SetActive(false);
                    //CloseAiming();
                }
            }





        }





        CloseAiming();
        if (Aiming)
        {
            PlayAudio(ArrowThrowing0);
            Arrows.GetObject(false);

            //yield return new WaitForSecondsRealtime(2);
            BallsUI.GetObject(false);


            if (BallsUI.Next < BallsUI.ReadyCount)//5 yapınca hata veriyor
            {

                LastBallUI = BallsUI.Objects[BallsUI.Next].GetComponentInChildren<TMP_Text>();
            }

            
            HowToTArrowPosUpdate = false;
            
            StartCoroutine(a.Go());
            UpdateNextAShower();

            if (BallsUI.ReadyCount <= BallsUI.Next)
            {
                CurrentBallShower.enabled = false;
            }

            
        }


    }
    
    public void PlayAudio(AudioClip clip)
    {
       
        AudioS.clip = clip;
        AudioS.PlayOneShot(AudioS.clip);
    }
    private void CloseAiming()
    {
        AimingGA.Active = false;
        AimingGA.Arrow.enabled = false;
        AimingGA.ArrowTop.enabled = false;
        foreach (GameObject g in ArrowPath.Objects)
        {
            g.SetActive(false);
        }
    }
    private void ArrowStep(float StepLength)
    {
        CurrentArrowPathPartPos += new Vector3(Mathf.Cos(ArrowAngle), Mathf.Sin(ArrowAngle)) * StepLength;

        ArrowAngle -= ArrowTurnSpeed * StepLength / Power;
        
    }
    //ifwon birkaç yerde kullanılıyor bazen check etmesine gerek olmuyor direk kazandıran method yazılacak
    public void IfWon()
    {
        //optimize edilecek TargetAndEquivalentSame() ile birleştirilebilir
        //yere saplanınca alttaki else if kısmına gerek kalmıyor, diğerlerinde gerekiyor, bu 2 ayrı method olarak yazılabilir belki:


        //kontrol etmeden won eklenecek bazen kontrole gerek yok
        if (!Won.activeSelf)
        {
            if (TE.TargetNo.UpNo == TE.Equivalent.UpNo)
            {
                //ThrowAMouseArea.enabled = false;
                Won.SetActive(true);
                TE.FastRotSpeed();
                PlayAudio(WinEffect);

                //AnotherLevel ve BlinkTargetNoText 'i buraya yazabilirim ayrı method olmasındansa:
                //StartCoroutine(AnotherLevel());

                //StartCoroutine(BlinkTargetNoText());
                if (!(CurrentLevelType.Type == "Training" && CurrentLevelType.LevelNo == 2))
                {
                    NextLevelButton.SetActive(true);
                }

                CurrentLevelType.LevelChange(true);
                StartCoroutine(WonEqualityShower());

                RestartButImage.color = Color.gray;



                //screen=notclikable
                //StartCoroutine(CylinderShake());

            }
            else if (Arrows.Next >= Arrows.ReadyCount)
            {
                bool AllSticked = true;
                foreach (GameObject g in Arrows.Objects)
                {
                    if (!g.GetComponent<Arrow>().IsSticked && g.activeSelf)
                    {
                        AllSticked = false;
                    }
                }
                //Debug.Log("AllSticked : " + AllSticked);
                if (AllSticked && !Sliding)
                {
                    OOAmmoText.enabled = true;
                    NextLevelButton.GetComponentInChildren<TMP_Text>().text = "Try Again";

                    NextLevelButton.SetActive(true);
                    PlayAudio(LevelFailed);
                    
                }
            }
            //CurrentArrow.transform.eulerAngles = new Vector3(90, CurrentArrow.transform.eulerAngles.y, 0);

            //CurrentArrow.transform.rotation=Quaternion.identity;
        }

    }
    private IEnumerator Failed()
    {
        //PlayAudio(FailedSound);
        AudioS.Play();
        while (true)
        {
            yield return new WaitForSeconds(0.005f);
            if (AudioS.time>0.35f)
            {
                
                
                break;
            }
        }
        
    }

    private IEnumerator CylinderShake()
    {
        bool Blink = false;
        float time = 0;

        TE.NumberText.enableAutoSizing = false;
        for (int i = 0; i < 16;)
        {
            yield return new WaitForSeconds(0.04f);
            time += Time.deltaTime * 50;
            if (time >= 0.2)
            {
                i++;
                time = 0;
                Blink = !Blink;
                if (Blink)
                {
                    //TE.transform.position += new Vector3(-0.08f, 0, 0);
                    //TE.NumberText.transform.eulerAngles = new Vector3(0, 0, -10);
                    TE.NumberText.fontSize += 40;
                }
                else
                {
                    //TE.transform.position += new Vector3(0.08f, 0, 0);
                    //TE.NumberText.transform.eulerAngles = new Vector3(0, 0, 10);
                    TE.NumberText.fontSize -= 40;
                }

            }
        }
        TE.NumberText.enableAutoSizing = true;
        //TE.NumberText.transform.eulerAngles = Vector3.zero;
    }


    private IEnumerator BlinkTargetNoText()
    {
        bool Blink = false;
        float time = 0;
        Color OldColor = TargetNoText.color;
        for (int i = 0; i < 16;)
        {
            yield return new WaitForSeconds(0.04f);
            time += Time.deltaTime * 50;
            //time 1 olunca tam 1 saniye olmuyor optimize edilirken tam 1 saniye yapılabilir
            if (time >= 1.9)//2.7
            {
                i++;
                time = 0;
                Blink = !Blink;
                if (Blink)
                {
                    TargetNoText.color = Color.black;
                    //TE.NumberText.color = OldColor;
                }
                else
                {
                    TargetNoText.color = OldColor;
                    //TE.NumberText.color = Color.black;
                }
            }

        }
        TargetNoText.color = OldColor;
        TE.NumberText.color = Color.black;
    }
    //private void SaveLevelDuration()
    //{
    //    //Archery Level3 Duration
    //    PlayerPrefs.SetInt(CurrentLevelType.Type+" Level"+CurrentLevelType.LevelNo+" Duration", CurrentLevelType.LevelDurations[CurrentLevelType.LevelNo-1]);
    //}
    private IEnumerator LevelDurationCounter()
    {
        ushort second = 0;
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            if (Game.activeSelf)
            {
                second++;
                if (second >= 10)
                {
                    CurrentLevelType.SaveLevelDuration();
                    second = 0;
                }
                
                //Debug.Log("Counted");
                CurrentLevelType.LevelDurations[CurrentLevelType.LevelNo - 1]++;
            }
            else
            {
                break;
            }
            
        }

    }
    
    private void WriteDurations(LevelType lt)
    {
        string[] Lines = new string[5];
        int LineIndex = 0;
        for (int i0 = 0; i0 < Lines.Length; i0++)
        {
            Lines[i0] = "";
        }

        int LevelNo = 1;
        while (LevelNo <= lt.LastLevel)
        {
            int AllSecond = lt.LevelDurations[LevelNo - 1];
            int minute = AllSecond / 60;
            int second= AllSecond % 60;
            Lines[LineIndex] += "  Level " + (LevelNo) + " : " + minute+":"+second;
            LevelNo++;
            LineIndex++;
            if (LineIndex >= Lines.Length)
            {
                LineIndex = 0;
            }
        }
        Durations.text += "\n";
        Durations.text += lt.Type;
        Durations.text += "\n";
        foreach (string s in Lines)
        {
            Durations.text += s + "\n";
        }
    }
    public void ShowLevelDurations()
    {
        TE.enabled = false;
        Menu.SetActive(false);
        Game.SetActive(false);
        Durations.enabled = true;

        WriteDurations(Archery);
        WriteDurations(Math);



    }
    private void FixedUpdate()
    {
        Debug.DrawLine(ArmPos.transform.position, ArmPos.transform.position + ArmPos.transform.up * 100, Color.black, 0.02f);

        


        //TestSphere2.transform.position = TestSphere.transform.position + Vector3.Cross(TestCube.transform.position - TestSphere.transform.position, Vector3.forward).normalized;


        //Debug.DrawLine(ArcherSpine.transform.position,ArcherSpine.transform.position+ ArcherSpine.transform.forward*100, Color.black, 0.02f);
    }
    private void ArrowLoading(GameObject CurrentArrow)
    {
        CurrentArrow.transform.position = ArrowFinger.position;
        //optimize edilecek:
        CurrentArrow.transform.LookAt(ArrowPoint.transform.position);
        CurrentArrow.transform.Rotate(0, -90, 0);

    }
    public FracNo FractionalOperation(FracNo fn1, FracNo fn2)
    {
        int UpNo = 0;
        int DownNo = 1;
        switch (fn2.Oparetion)
        {
            case 0://toplama
                if (fn1.DownNo != fn2.DownNo)
                {
                    UpNo = fn1.UpNo * fn2.DownNo + fn2.UpNo * fn1.DownNo;
                    DownNo = fn1.DownNo * fn2.DownNo;
                }
                else
                {
                    UpNo = fn1.UpNo + fn2.UpNo;
                    DownNo = fn1.DownNo;
                }
                break;
            case 1://çıkarma
                if (fn1.DownNo != fn2.DownNo)
                {
                    UpNo = fn1.UpNo * fn2.DownNo - fn2.UpNo * fn1.DownNo;
                    DownNo = fn1.DownNo * fn2.DownNo;
                }
                else
                {
                    UpNo = fn1.UpNo - fn2.UpNo;
                    DownNo = fn1.DownNo;
                }
                break;
            case 2://çarpma
                UpNo = fn1.UpNo * fn2.UpNo;
                DownNo = fn1.DownNo * fn2.DownNo;
                break;
            case 3://bölme

                UpNo = fn1.UpNo * fn2.DownNo;
                DownNo = fn1.DownNo * fn2.UpNo;
                break;

        }
        return new FracNo(fn1.Oparetion, UpNo, DownNo);
    }
    public string FracNoToString(FracNo fn, bool ShowSign, bool ShowBrackets)
    {
        string s = "";
        if (ShowSign)
        {
            switch (fn.Oparetion)
            {
                case 0:
                    s += "+";
                    break;
                case 1:
                    s += "-";
                    break;
                case 2:
                    s += "×";
                    break;
                case 3:
                    s += "/";
                    break;
            }
        }

        if (fn.DownNo == 1)
        {
            if (fn.UpNo < 0)
            {
                if (ShowBrackets)
                {
                    s += "(" + fn.UpNo + ")";

                }
                else
                {
                    s += fn.UpNo;
                }
            }
            else
            {
                s += fn.UpNo;
            }


        }
        else
        {
            s += fn.UpNo + "/" + fn.DownNo;
        }
        return s;
    }
}