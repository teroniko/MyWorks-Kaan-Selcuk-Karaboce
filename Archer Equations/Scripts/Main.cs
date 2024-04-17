using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
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
    public Color BlueAddition/* = new Color32(46,0,255,255)*//*new Color(0.3212531f, 0.3212531f, 0.8867924f)*/;
    public Color RedSubtraction/* = new Color(0.9056604f, 0.1742969f, 0.1742969f)*/;

    

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
    public TMP_Text LevelInfo;
    private sbyte StickedBlueCount;
    private sbyte ArrowBlueCount;
    private sbyte CurrentBlueCount = 0;
    private sbyte CurrentRedCount = 0;
    public RectTransform InformFinger;
    public RectTransform FingerPos1;
    public RectTransform FingerPos2;
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
    //public Image NextTrainingText;
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
    //public static bool TrainingAllLevelFinished=false;
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
    public float PopEffectDis = 0.15f;
    public float PopEffectBallSize = 1.2f;
    public bool DoPop = false;
    public Slider DifficultySlider;
    private ushort TrainingLastLevel = 7;
    public int TrainingLevelNo = 1;
    private bool TrainingFinished = false;
    public bool TrainingLevels = false;
    private List<Difficulty> Difficulties = new List<Difficulty>();
    public Difficulty CurrentDifficulty;
    public TMP_Text GameSpecsText;
    public Image GameSpecsImage;
    public bool TrainingCompleted = false;
    private float RefResolutionX = 0;
    public RectTransform GameGuideRect;
    public GameObject Terrain;
    public bool EnterRandomNums;//true
    public TMP_Text NextLevelBText;
    private ushort[] MathScores;
    private ushort ScoreCount=15;//(10du 2'ye tam bölünebilmeli idi), 3'e bölünebilmeli
    public TMP_Text ScoreText;
    public ushort CurrentMathScore;
    public TMP_Text MathLevelText;//Math Degree Text
    private ushort MathLevelPoint=0;
    private bool PlayedOnce = false;
    private Color TheYellow = new Color32(0xFF, 0xB3, 0x00, 0xFF);
    public Color MenuYellow;
    public Color MenuOrange;
    private IEnumerator CloseGameG;
    public Image CloseSettingBox;
    public Button ShareButton;
    public int PlayTicketCount;
    public TMP_Text PlayTicketText;
    public Image PlayTicketImage;

    public Image ShareBGift;
    public GameObject PlayTicketButtons;
    public GameObject ShareButtons;
    public GameObject LoadingPage;
    public Image FacebookGift;
    public Button FacebookButton;
    public GameObject ResetGameButton;
    public Material ArrowPathColor;


    public class Difficulty
    {
        public bool[] Sign;
        public short RangeUp;
        public short RangeDown;
        public ushort MaxTime;//second
        public bool RandomNumbers;
        public bool MustUseAllArrows;
        public ushort MaxScore;
        public byte MinThrow;
        public byte FBallCount;
        public sbyte ArrowBlueCount;
        public sbyte StickedBlueCount;
        public Difficulty(bool[] Sign, short RangeUp, short RangeDown, ushort MaxTime, bool RandomNumbers, bool MustUseAllArrows, ushort MaxScore, byte MinThrow, byte FBallCount, sbyte ArrowBlueCount, sbyte StickedBlueCount)
        {
            this.Sign = Sign;
            this.RangeUp = RangeUp;
            this.RangeDown = RangeDown;
            this.MaxTime = MaxTime;
            this.RandomNumbers = RandomNumbers;
            this.MustUseAllArrows = MustUseAllArrows;
            this.MaxScore = MaxScore;
            this.MinThrow = MinThrow;
            this.FBallCount = FBallCount;
            this.ArrowBlueCount = ArrowBlueCount;
            this.StickedBlueCount = StickedBlueCount;
        }
        public string DifficultySpecs(string MaxTime)
        {
            
            string RandomNumbersInfo = "";
            if (RandomNumbers)
            {
                RandomNumbersInfo = "\nRandom numbers at every restart";
            }
            string UseAll = "\nMust not use all the arrows";
            if (MustUseAllArrows)
            {
                UseAll = "\nMust use all the arrows";
            }

            return 
                //"Signs : + -\n" +
             "Numbers are between " + RangeDown + ", " + RangeUp +
            "\nMax time is : " + MaxTime +
            RandomNumbersInfo +
            UseAll
            //+"\nMay hit same ball multiple times"
            + "\nMax score : " + MaxScore
            + "\nArrows : " + ArrowBlueCount + " Blue + " + (MinThrow - ArrowBlueCount) + " Red or Vice Versa"
            + "\nBalls : " + StickedBlueCount + " Blue + " + (FBallCount - StickedBlueCount) + " Red or Vice Versa"
            ;
        }
    }
    
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
        public IEnumerator Timer;
        public GuideTarget(Image Target, bool Active)
        {
            this.Target = Target;
            this.Active = Active;
        }
    }
    
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
                Objects[i].GetComponent<Arrow>().PopCheck();
                Objects[i].GetComponent<Arrow>().DidHit = false;

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



    public void TrainingLevelChange(bool Up)
    {
        if (Up)
        {
            TrainingLevelNo++;
        }
        else
        {
            TrainingLevelNo--;
        }
        
        if (TrainingLevelNo > TrainingLastLevel)
        {
            
            TrainingLevelNo = TrainingLastLevel;
            PlayerPrefs.SetInt("Training Finished", 1);
            
            TrainingCompleted = true;
            TrainingFinished = true;
            
            
        }
        else if (TrainingLevelNo < 1)
        {
            TrainingLevelNo = 1;
        }



    }

    public void TrainingNextLevel()
    {
        TrainingLevelChange(true);
        NextLevel();

        
    }
    public void TrainingPreviousLevel()
    {
        TrainingLevelChange(false);
        SetLevel();
    }
    private IEnumerator RulesBlink()
    {
        bool blink = false;
        for(int i = 0; i < 8; i++)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            blink = !blink;
            if (blink)
            {
                GameSpecsImage.color = MenuYellow;
            }
            else
            {
                GameSpecsImage.color = MenuOrange;
            }
        }
        GameSpecsImage.color = MenuOrange;
        CloseGameG = CloseGameGuideLater(2.3f);
        StartCoroutine(CloseGameG);
    }
    private IEnumerator CloseGameGuideLater(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        CloseGameGuide();
    }
    public void CloseGameGuide()
    {
        GameGuideUI.SetActive(false);
        PlayTicketButtons.SetActive(false);
        ShareButtons.SetActive(false);

    }
    public void Play()
    {
        if (TrainingCompleted)
        {
            if (!PlayedOnce)
            {
                PlayedOnce = true;
                PlayerPrefs.SetInt("PlayedOnce", 1);
                
                StartCoroutine(RulesBlink());
                ShowGameGuide("Always read the rules before you play!", false, false);
            }
            else
            {
                TrainingLevels = false;
                LevelButtonT.SetActive(false);
                EnterRandomNums = true;//silinecek bug yoksa//TODO
                CloseGameGuide();
                SetLevel();
            }
        }
        else
        {
            OpenTraining();
        }
        ShowMathP = false;
        ShowMathPoints();
    }
    public void OpenMenu()
    {
        TE.enabled = false;
        HowToTArrowPosUpdate = false;
        Menu.SetActive(true);
        Game.SetActive(false);

        //CloseGameGuide();


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
            if (gt.Timer!=null)
            {

                StopCoroutine(gt.Timer);
            }
        }
    }
    
    public void OpenTraining()
    {
        TrainingLevels = true;
        if (TrainingCompleted)
        {
            LevelButtonT.SetActive(true);

        }
        if (TrainingFinished)
        {
            TrainingLevelNo = 1;
            TrainingFinished = false;
        }
        SetLevel();
    }
    public void Score0()
    {
        if (!TrainingLevels&& NextLevelBText.text != "Try Again")
        {
            CloseGameG = CloseGameGuideLater(1);
            StartCoroutine(CloseGameG);
            ScoreShower(0);
        }
        if (NextLevelBText.text == "Try Again")
        {
            CloseGameGuide();
        }
    }
    public void ComingSoonButton()
    {
        ShowGameGuide("<u>If enough support came : </u>\n× and ÷ signs\nNew difficulty level for faster cylinder\nFor app store", false,false);
        //\nWider range difficulty levels
        CloseSettingBox.enabled = true;
    }
    public void CloseSettingB()
    {
        if (!isProcessing)
        {
            CloseSettingBox.enabled = false;
            CloseGameGuide();

        }
    }
    public void Restart()
    {
        if (RestartButImage.color == RestartButColor)
        {

            CurrentMathScore = 0;
            
            IsItRandom();
            Score0();
            SetLevel();
            

        }
    }
    private void IsItRandom()
    {
        EnterRandomNums = CurrentDifficulty.RandomNumbers;
        

    }
    public void NextLevel()
    {
        if (TrainingLevels && TrainingFinished)
        {
            OpenMenu();
            CloseGameGuide();
        }
        else
        {
            CloseGameGuide();
            if (NextLevelBText.text == "Play Again")//Won
            {
                EnterRandomNums = true;
            }
            else
            {
                IsItRandom();
                
            }
            


            SetLevel();

        }
    }
    private void SaveMathPoint(int index, int point)
    {
        PlayerPrefs.SetInt("MathPoint " + index, point);
        MathLevelPoint += (ushort)point;
    }
    
    private void MathPoints(bool EnterZero)
    {
        if (EnterZero)
        {
            MathLevelPoint = 0;
            for (int i = MathScores.Length - 1; 0 < i; i--)
            {

                MathScores[i] = MathScores[i - 1];

                SaveMathPoint(i, MathScores[i]);

            }
            MathScores[0] = 0;
            SaveMathPoint(0, MathScores[0]);
        }
        else
        {
            MathScores[0] = CurrentMathScore;
            SaveMathPoint(0, MathScores[0]);
        }

        UpdateMainScoreText();

        //foreach (ushort u in MathScores)
        //{
        //    Debug.Log(u);
        //}
    }
    private void UpdateMainScoreText()
    {
        MathLevelText.text = "<size=160%>Math Level</size=160%>\n<size=370%>"+MathLevelPoint+"</size=370%>";
    }
    private bool ShowMathP = true;//ShowMathPoints
    public void ShowMathPoints()
    {
        ShowMathP = !ShowMathP;
        if (ShowMathP)
        {
            UpdateMainScoreText();
            MathLevelText.lineSpacing = -150;

        }
        else
        {
            MathLevelText.lineSpacing = 0;
            MathLevelText.text = "<size=170%><u>Last " + MathScores.Length + " Level</size=170%></u>";
            int DivideRange = 3;//2 idi
            for (int i = 0; i < MathScores.Length / DivideRange/*2idi*/; i++)
            {
                MathLevelText.text += "\nLevel " + (i + 1) + " Point : " + MathScores[i];
                MathLevelText.text += "  Level " + (i + 1+ MathScores.Length / DivideRange) + " Point : " + MathScores[i + MathScores.Length / DivideRange];
                MathLevelText.text += "  Level " + (i + 1 + MathScores.Length*2 / DivideRange) + " Point : " + MathScores[i + MathScores.Length / DivideRange];

            }

        }
    }
    
    public void SetLevel()
    {
        SliderDifficulty();
        OOAmmoText.enabled = false;

        CloseGuideArrows();
        CloseGuideTargets();

        ActiveNumberShowers(false);

        NextLevelButton.SetActive(false);


        NewLevel();
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
        //CloseGameGuide();
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
    
    public void ShowGameGuide(string s, bool Thin, bool Wide)
    {
        GameGuideUI.SetActive(true);
        GameGuideText.text = s;
        float SizeY = 7;
        float SizeX = 3;
        if (Thin)
        {
            SizeY = 12;
        }
        if (Wide)
        {
            SizeX = 1.7f;
        }
        GameGuideRect.sizeDelta = new Vector2(/*GameGuideRect.sizeDelta.x*/RefResolutionX / SizeX, RefResolutionX /SizeY);
    }
    public void TrainingNext()
    {
        int LevelNo = TrainingLevelNo;
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

                        
                        TrainingNextLevel();

                        break;
                    case 3:


                        Arrows.Objects[0].GetComponent<Arrow>().CurrentArrowUpdate();
                        TE.EquationUpdate(0, false, false);


                        IfWon();

                        ActiveNumberShowers(false);
                        NextLevelButton.SetActive(true);

                        break;
                    case 4:




                        IfWon();
                        CloseGameGuide();
                        NextLevelButton.SetActive(true);
                        
                        break;
                    case 5:

                        IfWon();


                        break;
                    case 6:

                        TrainingNextLevel();
                        break;
                    case 7:
                        CloseGameGuide();
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
                    case 6:
                        CloseGuideArrows();
                        NextGuideButton.SetActive(false);
                        GTs[0].Timer = GuideTargetUI(TE.Number, false, 0);
                        StartCoroutine(GTs[0].Timer);
                        ThrowAMouseArea.enabled = true;
                        break;
                    case 7:
                        StartCoroutine(GuideArrowUI(16, false));
                        NextGuideButton.SetActive(false);
                        
                       
                        ShowGameGuide("Equal to the target",false, false);
                        break;
                }
                break;
            case 2:

                

                switch (LevelNo)
                {
                    case 2:
                        CloseGuideArrows();
                        StartCoroutine(GuideArrowUI(12, false));
                        NextGuideButton.SetActive(false);

                        
                        break;
                    case 3:
                        GTs[0].Timer = GuideTargetUI(StickedArrows.Objects[0].GetComponent<Arrow>().Number, false, 0);
                        StartCoroutine(GTs[0].Timer);
                        NumberShower[1].enabled = false;
                        NextGuideButton.SetActive(false);
                        CloseGameGuide();
                        ThrowAMouseArea.enabled = true;
                        break;
                    case 4:
                        
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
                    
                    case 6:
                        StartCoroutine(GuideArrowUI(14, false));
                        NextGuideButton.SetActive(false);
                        CloseGameGuide();
                        break;
                }

                break;
            case 3:

                switch (LevelNo)
                {
                    case 2:

                        CloseGuideArrows();
                        StartCoroutine(GuideArrowUI(11, false));
                        CloseGameGuide();
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
                       
                        ShowGameGuide("Every ball sticked to middle cylinder have a number too. They are equation numbers", false, false);
                        break;
                    case 4:
                        CloseGuideArrows();
                        
                        PlusOrMinus.enabled = false;
                        NextGuideButton.SetActive(false);
                        ThrowAMouseArea.enabled = true;
                        SetCorrectFracIndex(0);
                        TE.ccs[0].RotSpeed = 15;
                        
                        break;
                    case 5:
                        CloseGuideArrows();
                        CloseGameGuide();
                        ThrowAMouseArea.enabled= true;
                        SetCorrectFracIndex(0);
                        NextGuideButton.SetActive(false);
                        break;
                }

                break;
            case 5:

                //level4:
                
                StartCoroutine(GuideArrowUI(11, true));
                CloseGameGuide();
                NextGuideButton.SetActive(false);

                break;
            case 6:
                //level4:
                CloseGuideArrows();
                //SetCorrectFracIndex(1);
                NextGuideButton.SetActive(true);
                
                ShowGameGuide("Be careful to the equation, red means minus number", false, false);
                PlusOrMinus.enabled = false;


                
                
                break;
            case 7:

                //level4:
                CloseGuideArrows();
                BlueShowed = false;
                CloseGameGuide();

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

        CloseGuideTargets();
        CloseGuideArrows();
        GTs[NextNumbersArrayIndex].Timer = GuideTargetUI(StickedArrows.Objects[CorrectFracIndex].GetComponent<Arrow>().Number, false, NextNumbersArrayIndex);
        StartCoroutine(GTs[NextNumbersArrayIndex].Timer);
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
        Color col = TheYellow;//FFB300


        float StartEndSpeed = 1f;//1
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


                StartEndSpeed = 0.7f;
                break;

        }


        if (ArrowType == 10)//mouse
        {
            //col = BlueAddition;
            col = ArrowPathColor.color;
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
            case 0:
                a = Arrows.Objects[0].GetComponent<Arrow>();

                ArrowHeadV = LetterPos(TE.EquationText, a.TextPlace, a.TextPlace + GetSpaceCountOfNo(a.ArrowNo));

                BackArrow = a.Number.GetComponent<RectTransform>();
                MovableAndStopped = 1;
                GoFromStartToEnd = true;
                Space = 0.95f;
                break;
            //case 1:

            //    ArrowHeadV = LetterPos(TE.EquationText, 0, GetSpaceCountOfNo(TE.No) - 1);
            //    BackArrow = TE.Number.GetComponent<RectTransform>();
            //    MovableAndStopped = 1;
            //    break;
            case 1:

                RectTransform ArrowHeadRect = BallsUI.Objects[0].GetComponent<RectTransform>();
                //ArrowHead = BallsUI.Objects[BallsUI.Next].GetComponent<RectTransform>();
                ArrowHeadV = ArrowHeadRect.localPosition + (Vector3)ArrowHeadRect.sizeDelta / 2;
                //ArrowHeadV = ArrowHead.localPosition;
                BackArrowV = GameGuideUI.GetComponent<RectTransform>().localPosition + (ArrowHeadV - GameGuideUI.GetComponent<RectTransform>().localPosition) * 0.2f;
                Space = 0.8f;
                MovableAndStopped = 2;
                GoFromStartToEnd = true;
                StartEndSpeed = 0.8f;
                break;
            case 2:

                ArrowHeadRect= BallsUI.Current.GetComponent<RectTransform>();
                //ArrowHead = BallsUI.Objects[BallsUI.Next].GetComponent<RectTransform>();
                ArrowHeadV = ArrowHeadRect.localPosition + (Vector3)ArrowHeadRect.sizeDelta / 2;
                //ArrowHeadV = ArrowHead.localPosition;
                BackArrowV = GameGuideUI.GetComponent<RectTransform>().localPosition+(ArrowHeadV- GameGuideUI.GetComponent<RectTransform>().localPosition)*0.2f;
                Space = 0.8f;
                MovableAndStopped = 2;
                GoFromStartToEnd = true;
                StartEndSpeed = 0.8f;
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
                SArrowNo = 0;
                ArrowHead = NumberShower[SArrowNo/*0*/].rectTransform;
                BackArrowV = LetterPos(TE.EquationText, TE.EquationText.text.Length - GetSpaceCountOfNo(TE.Equivalent), TE.EquationText.text.Length - 1);
                Space = 0.8f;
                MovableAndStopped = 2;
                GoFromStartToEnd = true;
                Vector3 WESpace = new Vector3((NumberShower[2].textInfo.characterInfo[0].topRight.x - NumberShower[2].textInfo.characterInfo[0].topLeft.x) / 2f + 20 + (NumberShower[0].textInfo.characterInfo[NumberShower[0].text.Length - 1].topRight.x - NumberShower[0].textInfo.characterInfo[0].topLeft.x) / 2f, 0);
                Vector2 EqualityPosMid = NumberShower[2].rectTransform.localPosition - WESpace;

                ArrowHeadV = EqualityPosMid;
                col = Color.white;
                break;
            case 5:
                SArrowNo = 1;

                ArrowHead = NumberShower[SArrowNo/*1*/].rectTransform;
                BackArrowV = LetterPos(TargetNoText, TargetNoText.text.Length - GetSpaceCountOfNo(TE.Equivalent), TargetNoText.text.Length - 1);
                Space = 0.8f;
                MovableAndStopped = 2;
                GoFromStartToEnd = true;
                WESpace = new Vector3((NumberShower[2].textInfo.characterInfo[0].topRight.x - NumberShower[2].textInfo.characterInfo[0].topLeft.x) / 2f + 20 + (NumberShower[0].textInfo.characterInfo[NumberShower[0].text.Length - 1].topRight.x - NumberShower[0].textInfo.characterInfo[0].topLeft.x) / 2f, 0);
                EqualityPosMid = NumberShower[2].rectTransform.localPosition + WESpace;

                ArrowHeadV = EqualityPosMid;
                col = Color.white;

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
            case 14:
                MinLengthArrow = 20;
                ArrowHeadV = LetterPos(TE.EquationText, 0, GetSpaceCountOfNo(TE.No));
                MovableAndStopped = 2;
                GoFromStartToEnd = true;
                BackArrowV = TE.NumberText.rectTransform.localPosition;
                Space = 0.95f;
                StartEndSpeed = 0.7f;
                break;
            case 15:
                ArrowHeadV = LetterPos(TE.EquationText, TE.EquationText.text.Length - GetSpaceCountOfNo(TE.Equivalent), TE.EquationText.text.Length - 1);
                BackArrowV = GameGuideUI.GetComponent<RectTransform>().localPosition + (ArrowHeadV - GameGuideUI.GetComponent<RectTransform>().localPosition) * 0.2f;
                Space = 0.8f;
                MovableAndStopped = 3;
                GoFromStartToEnd = true;
                StartEndSpeed = 0.85f;
                break;
            case 16:
                ArrowHeadV = LetterPos(TargetNoText, TargetNoText.text.Length - GetSpaceCountOfNo(TE.Equivalent), TargetNoText.text.Length - 1);
                BackArrowV = GameGuideUI.GetComponent<RectTransform>().localPosition + (ArrowHeadV - GameGuideUI.GetComponent<RectTransform>().localPosition) * 0.2f;
                Space = 0.8f;
                MovableAndStopped = 3;
                GoFromStartToEnd = true;
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
                        if (!GoFromStartToEnd)
                        {

                            Stable = true;
                        }
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
                                if (!(ArrowType == 4 || ArrowType == 5||ArrowType==0))
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
            if (ArrowType == 14)
            {
                NextGuideButton.SetActive(true);
            }
            if (ArrowType == 0)
            {
                TE.ccs[0].RotSpeed = 0;
                yield return new WaitForSecondsRealtime(1);

                NextGuideButton.SetActive(true);
                
                ShowGameGuide("If you hit the middle cylinder, new number will be added to the equation as you see", false, false);
                
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
        

        Game.SetActive(true);
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
        



        Won.SetActive(false);
        CalculateEquation();


        TargetAndEquivalentSame();
        
        
    }
    private bool TargetEquivalentSame = false;
    private void TargetAndEquivalentSame()
    {
        //loopa girmesin hep eşit çıkarsa girer:
        if (TE.Equivalent.UpNo == TE.TargetNo.UpNo)//<çarpma bölmede burası editlenecek
        {
            TargetEquivalentSame = true;
            SetLevel();
            TargetEquivalentSame = false;
        }
    }
    //public bool IsRandomNumbers = false;
    private void SetNextNumbers()
    {
        //Setting Incomplate Numbers: method yazılacak muhtemelen:

        BallsUI.SetReadyCount(MinThrow);
        BallsUI.RestartBallUIs();
        //IsRandomNumbers = CurrentDifficulty.RandomNumbers || NextNumbers == null;
        //if (IsRandomNumbers)
        if(EnterRandomNums)
        {
            NextNumbers = new IncompleteNumber[MinThrow];

        }
        List<ushort> ENumbersIndexes = new List<ushort>();
        for (ushort i = 0; i < FBallCount; i++)
        {
            ENumbersIndexes.Add(i);
        }
        for (ushort i = 0; i < NextNumbers.Length; i++)
        {
            BallsUI.GetObject(true);
            GameObject NextBall = BallsUI.Current;
            if (EnterRandomNums)
            {
                ushort RandomNo = (ushort)Random.Range(0, ENumbersIndexes.Count);

                NextNumbers[i] = new IncompleteNumber();
                IncompleteNumber icn = NextNumbers[i];
                icn.Index = ENumbersIndexes[RandomNo];
                icn.No = RandomFrac(2, NoRange1, NoRange2);
                //icn.No.Oparetion = 1;
                ENumbersIndexes.RemoveAt(RandomNo);



                Color color = Color.white;
                (NextNumbers[i].No.Oparetion, color) = SetColors(NextNumbers[i].No.Oparetion, ArrowOperation, ArrowBlueCount, MinThrow);





                NextBall.GetComponent<Image>().color = color;
                //BallUIParent şu an da fullscreen, fullscreen olmayıp bu pozisyon yeniden ayarlanacak:
                NextBall.GetComponent<RectTransform>().anchoredPosition = new Vector2(NextBall.GetComponent<RectTransform>().sizeDelta.x * (NextNumbers.Length - i - 1), 0);
                //NextBallsUI.Add(NextBall);

                NextBall.GetComponentInChildren<TMP_Text>().text = FracNoToString(NextNumbers[i].No, false, false);


            }
            //bu methodun aynısı setballda var optimize edilecek:

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
            if (!EnterRandomNums)
            {
                a.ArrowNo = a.OldArrowNo;
            }
            else
            {
                a.ArrowNo = RandomFrac(StickArrowOperation, NoRange1, NoRange2);
                a.OldArrowNo = a.ArrowNo;
            }



            SetBall(a, true);
            a.StickToCylinder(TE.transform, true);

            TE.EquationUpdate(0, false, false);

            StickedArrows.Current.transform.RotateAround(TE.transform.position, Vector3.forward, 360 * i / FBallCount);
            
        }
    }
    private void LevelValues(byte ArrowOperation, byte StickArrowOperation, byte MinThrow, byte FBallCount, sbyte StickedBlueCount, sbyte ArrowBlueCount, short NoRange1, short NoRange2)
    {
        this.ArrowOperation = ArrowOperation;
        this.StickArrowOperation = StickArrowOperation;
        this.MinThrow = MinThrow;//silinecek
        this.FBallCount = FBallCount;//silinecek
        this.StickedBlueCount = StickedBlueCount;
        this.ArrowBlueCount = ArrowBlueCount;
        this.NoRange1 = NoRange1;//silinecek
        this.NoRange2 = NoRange2;//silinecek




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

        if (TrainingLevels)
        {
            NextLevelButton.GetComponentInChildren<TMP_Text>().text = "Next";
            LevelInfo.text = "Training Level " + TrainingLevelNo;
            //sonradan çarpı bölü öğretilecek
            switch (TrainingLevelNo)
            {
                case 1:
                    CloseGuideArrows();
                    CloseGuideArrowUI679();
                    LevelValues(1, 1, 1, 1, 1, 1, 1, 4);

                    ThrowAMouseArea.enabled = true;
                    GuideCount = 0;
                    NextGuideButton.SetActive(false);
                    //AmmoShowerClose
                    StartCoroutine(InformTargeting());
                    StartCoroutine(HowToTArrow());

                    GTs[0].Timer = GuideTargetUI(StickedArrows.Objects[0].GetComponent<Arrow>().Number, false, 0);
                    StartCoroutine(GTs[0].Timer);
                    GTs[1].Timer = GuideTargetUI(TE.Number, false, 1);
                    StartCoroutine(GTs[1].Timer);
                    CloseGameGuide();
                    
                    ActiveNumberShowers(false);
                    PlusOrMinus.enabled = false;

                    TE.AddRT(4, 0);
                    TE.AddCC(0, 0, 25, 1, 0);
                    TE.AddRT(4, 0);
                    break;
                case 2:
                    HowToTArrowPosUpdate = false;
                    CloseGuideArrows();
                    CloseGuideArrowUI679();
                    LevelValues(1, 1, 1, 3, 3, 1, 1, 4);

                    
                    
                    ShowGameGuide("Every arrow have a number and next arrow's number is here", false, false);
                    ThrowAMouseArea.enabled = false;


                    NextGuideButton.SetActive(false);
                    GuideCount = 5;
                    ActiveNumberShowers(false);

                    StartCoroutine(GuideArrowUI(2, false));





                    TE.AddRT(4, 0);
                    TE.AddCC(0, 0, 5, 1, 0);
                    TE.AddRT(4, 0);

                    break;
                case 3:
                    LevelValues(1, 1, 1, 1, 1, 1, 1, 4);
                    CloseGuideArrowUI679();
                    CloseGuideArrows();


                    ThrowAMouseArea.enabled = false;
                    NextGuideButton.SetActive(true);
                    GuideCount = 3;


                    PlusOrMinus.enabled = false;
                    
                    ShowGameGuide("When you hit the ball, arrow will process with it", false, false);
                    TE.AddRT(4, 0);
                    TE.AddCC(0, 0, 10, 1, 0);
                    TE.AddRT(4, 0);



                    break;
                case 4:


                    LevelValues(1, 2, 2, 2, 1, 1, 1, 4);
                    ThrowAMouseArea.enabled = false;
                    PlusOrMinus.enabled = false;
                    NextGuideButton.SetActive(true);
                    
                    ShowGameGuide("Every arrow and ball have a color, every color has a sign", false, false);
                    TrainingOperationA = 0;
                    
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
                    ThrowAMouseArea.enabled = false;
                    
                    
                    ShowGameGuide("Look to your ammo there is minus arrows", false, false);
                    StartCoroutine(GuideArrowUI(1, false));
                    NextGuideButton.SetActive(false);
                    GuideCount = 5;



                    TE.AddRT(4, 0);
                    TE.AddCC(0, 0, 8, 1, 0);
                    TE.AddRT(4, 0);


                    break;
                case 6:
                    LevelValues(1, 2, 1, 2, 1, 1, 1, 4);
                    CloseGuideArrows();
                    CloseGuideTargets();
                    ThrowAMouseArea.enabled = false;
                    
                    
                    ShowGameGuide("Cylinder number is at the beginning of the equation", false, false);
                    NextGuideButton.SetActive(true);
                    GuideCount = 3;
                    TE.AddRT(4, 0);
                    TE.AddCC(0, 0, 15, 1, 0);
                    TE.AddRT(4, 0);
                    break;
                case 7:
                    LevelValues(2, 2, 2, 2, 1, 1, 1, 4);
                    
                    ShowGameGuide("To pass the level the equality must be...", false, false);
                    StartCoroutine(GuideArrowUI(15, false));
                    NextGuideButton.SetActive(false);
                    ThrowAMouseArea.enabled = false;
                    GuideCount = 2;
                    TE.AddRT(4, 0);
                    TE.AddCC(0, 0, 15, 1, 0);
                    TE.AddRT(4, 0);
                    break;

            }
            ScoreText.enabled = false;
            TE.TimeToEnd.enabled = false;
            EnterRandomNums = true;
            //aşağıda son satır olmazsa daha optimize olur:
            TE.FirstPositions();
        }
        else
        {
            if (PlayTicketCount > 0)
            {
                PlayTicketCount--;
                PlayTicketUpdate(PlayTicketCount);
                NextLevelButton.GetComponentInChildren<TMP_Text>().text = "Play Again";
                if (!TargetEquivalentSame)
                {
                    MathPoints(true);
                }
                
                CloseTrainingUIs();



                //profesör
                //doçent
                //doktor

                //difficultytext ve math level shower
                //profesörü seçip oynayan bitirince math level olarak profesör seviyesinin ortasında olmalı
                //Kid(7-12 age)
                //Teenager(12-18 age)
                //Young(18 above)
                //Adult(25 above)
                //Bachelor's degree (bu olmasın)
                //physicist
                //programmer
                //hacker
                //professor

                TE.AddRT(4, 1);
                TE.AddCC(1, 0, 25, 1, 0);
                TE.AddRT(4, 1);

                
                ArrowOperation = 2;
                StickArrowOperation = 2;
                NoRange1 = CurrentDifficulty.RangeDown;
                NoRange2 = CurrentDifficulty.RangeUp;
                MinThrow = CurrentDifficulty.MinThrow;
                FBallCount = CurrentDifficulty.FBallCount;
                ArrowBlueCount = CurrentDifficulty.ArrowBlueCount;
                StickedBlueCount = CurrentDifficulty.StickedBlueCount;
                if (EnterRandomNums)
                {
                    Debug.Log("MThrowFBallCountSwap");
                    MThrowFBallCountSwap();//TODO
                    //
                }
                TE.TimeToEnd.enabled = true;

                TE.CurrentTime = CurrentDifficulty.MaxTime;
                TE.TimeToEnd.text = "Time : " + ConvertTime(TE.CurrentTime);
                CurrentMathScore = CurrentDifficulty.MaxScore;
                ScoreText.text = "Score To Be Gained : " + CurrentMathScore;
                ScoreText.enabled = true;





                LevelInfo.text = "Difficulty : " + DifficultySlider.value;

                //aşağıda son satır olmazsa daha optimize olur:
                TE.FirstPositions();
            }
            else
            {
                StartCoroutine(PlayTicketBlink());
                OpenMenu();
            }
            
            
            
        }
        
        
        


    }
    public void PlayTicketButton()
    {
        CloseSettingBox.enabled = true;
        PlayTicketButtons.SetActive(true);
        ShowGameGuide("",false, true);
    }
    private bool PlayTicketBlinking = false;
    //blink baya var, sonradan optimizasyon için tek bir metodda yapılmalı şimdi gerek yok
    private IEnumerator PlayTicketBlink()
    {
        if (!PlayTicketBlinking)
        {
            PlayTicketBlinking = true;
            Color col = PlayTicketImage.color;
            bool blink = false;
            for (int i = 0; i < 6; i++)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                blink = !blink;
                if (blink)
                {
                    PlayTicketImage.color = new Color32(0xB0, 0x7F, 0x6A, 0xFF);
                }
                else
                {
                    PlayTicketImage.color = col;
                }
            }
            PlayTicketImage.color = col;
            PlayTicketBlinking = false;
        }
        

    }
    private void MThrowFBallCountSwap()//MinThrowFirstBallCountSwap
    {
        if (Random.Range(0, 2) == 0)
        {
            ArrowBlueCount = (sbyte)(MinThrow - ArrowBlueCount);
        }

        if (Random.Range(0, 2) == 0)
        {
            StickedBlueCount = (sbyte)(FBallCount - StickedBlueCount);
        }
    }
    public string ConvertTime(int TimeToConvert)
    {

        return System.TimeSpan.FromSeconds(TimeToConvert).ToString(@"mm\:ss");
    }
    public void SliderDifficulty()
    {
        CurrentDifficulty = Difficulties[(short)DifficultySlider.value - 1];
        GameSpecsText.text = CurrentDifficulty.DifficultySpecs(ConvertTime(CurrentDifficulty.MaxTime));
        
        
    }
    

    //private IEnumerator ss(Product p)
    //{
    //    yield return new WaitForSeconds(4);
    //    ShowGameGuide(p.metadata.localizedPrice.ToString(), true, false);
    //}
    private IEnumerator ShowLoadingPage()
    {
        LoadingPage.SetActive(true);
        yield return new WaitForSecondsRealtime(8);
        LoadingPage.SetActive(false);
    }
    void Awake()
    {
        //Bu fakestore açma:
        //StandardPurchasingModule.Instance().useFakeStoreAlways = true;

        Terrain.SetActive(true);
        if (Application.isEditor)
        {
            Time.timeScale = 1.5f;
            ResetGameButton.SetActive(true);
        }
        else
        {
            StartCoroutine(ShowLoadingPage());
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

        RefResolutionX = c.GetComponent<CanvasScaler>().referenceResolution.x;
        CanvasScalerRatio = RefResolutionX / Screen.width;


        OpenMenu();
        

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

        ArrowPath = new ObjectPool(45);//100
        ArrowPath.SpawnArrowPathParts(ArrowPathPartPrefab, ArrowPathParent);
        ArrowPath.Next = 0;

        //MinThrow = 2;//silinecek<
        //ArrowCount = MaxThrow + MaxFBallCount;
        //NextNumbers = new FracNo[MinThrow];
        PlatformTouchScreen = Application.platform== RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;

        TargetIK.transform.position = ShortenBowPos.position;






        //kaç kişide bir IAP satı alınıyor?
        //menüde rules'u bölmeye gerek var mı yok
        //ekstra 10,5    -5,-20 falan gibi bir kaç girilecek, işareti hiç eksi olmayan veya tam tersi


        //max score'lar girilecek
        //Random, every arrow girilecek

        Difficulties.Add(new Difficulty(null, 3, 0, 120, false, false, 5, 1, 1, 0, 0));
        Difficulties.Add(new Difficulty(null, 4, 0, 120, false, false, 5, 1, 2, 0, 0));
        Difficulties.Add(new Difficulty(null, 5, 0, 110, false, false, 5, 1, 2, 0, 1));
        Difficulties.Add(new Difficulty(null, 6, 0, 100, false, false, 5, 1, 2, 0, 1));
        Difficulties.Add(new Difficulty(null, 7, 0, 110, false, false, 5, 2, 2, 1, 0));
        Difficulties.Add(new Difficulty(null, 8, 0, 110, false, false, 5, 2, 2, 1, 1));
        Difficulties.Add(new Difficulty(null, 10, 0, 130, true, false, 5, 2, 2, 1, 1));
        Difficulties.Add(new Difficulty(null, 12, 0, 110, false, false, 5, 2, 2, 1, 1));
        Difficulties.Add(new Difficulty(null, 10, 0, 100, false, false, 5, 1, 3, 1, 0));
        Difficulties.Add(new Difficulty(null, 13, 0, 95, false, false, 5, 2, 2, 1, 1));
        Difficulties.Add(new Difficulty(null, 15, 0, 105, true, false, 5, 2, 2, 1, 1));
        Difficulties.Add(new Difficulty(null, 16, 0, 105, true, false, 5, 1, 3, 1, 1));
        Difficulties.Add(new Difficulty(null, 10, 0, 120, true, false, 5, 2, 3, 1, 0));
        Difficulties.Add(new Difficulty(null, 2, -2, 130, false, false, 5, 2, 2, 0, 1));
        Difficulties.Add(new Difficulty(null, 5, 0, 100, false, false, 5, 1, 4, 0, 2));
        Difficulties.Add(new Difficulty(null, 5, -5, 140, true, false, 5, 2, 2, 1, 1));
        Difficulties.Add(new Difficulty(null, 0, -3, 130, false, false, 5, 1, 2, 1, 1));
        Difficulties.Add(new Difficulty(null, 19, 0, 130, false, false, 5, 2, 2, 1, 0));
        Difficulties.Add(new Difficulty(null, 10, 0, 110, false, false, 5, 2, 4, 0, 2));
        Difficulties.Add(new Difficulty(null, 5, 0, 55, false, false, 5, 2, 2, 1, 1));
        Difficulties.Add(new Difficulty(null, 10, -10, 135, false, false, 5, 2, 2, 0, 1));
        Difficulties.Add(new Difficulty(null, 4, 0, 120, false, true, 5, 3, 5, 0, 1));
        Difficulties.Add(new Difficulty(null, 10, -10, 130, true, false, 5, 2, 2, 0, 1));
        Difficulties.Add(new Difficulty(null, 25, 0, 130, true, false, 5, 2, 2, 1, 1));
        Difficulties.Add(new Difficulty(null, 5, 0, 40, false, false, 5, 2, 2, 1, 0));
        Difficulties.Add(new Difficulty(null, 7, 0, 40, false, false, 5, 2, 2, 0, 1));
        Difficulties.Add(new Difficulty(null, 3, 0, 100, false, true, 5, 3, 3, 1, 1));
        Difficulties.Add(new Difficulty(null, 14, 0, 105, false, false, 5, 2, 3, 1, 1));
        Difficulties.Add(new Difficulty(null, 40, 0, 130, false, false, 5, 1, 3, 1, 0));
        Difficulties.Add(new Difficulty(null, 20, -4, 110, false, false, 5, 2, 2, 1, 0));
        Difficulties.Add(new Difficulty(null, 100, 0, 225, false, false, 5, 1, 2, 1, 1));
        Difficulties.Add(new Difficulty(null, 15, -15, 160, false, false, 5, 1, 2, 1, 1));
        Difficulties.Add(new Difficulty(null, 8, -8, 110, false, false, 5, 2, 5, 1, 2));
        Difficulties.Add(new Difficulty(null, 24, -5, 120, false, false, 5, 2, 3, 1, 0));
        Difficulties.Add(new Difficulty(null, 20, -20, 140, false, true, 5, 3, 3, 2, 2));



        DifficultySlider.maxValue = Difficulties.Count;

        //bir dahaki versiyonda rangenolar daha yavaş artabilir
        DifficultySlider.value = PlayerPrefs.GetInt("Difficulty Value");
        
        SliderDifficulty();

        MathScores = new ushort[ScoreCount];



        MathLevelPoint = 0;
        for (int i = 0; i < MathScores.Length; i++)
        {
            MathScores[i] = (ushort)PlayerPrefs.GetInt("MathPoint " + i);
            
            MathLevelPoint += MathScores[i];
        }
        UpdateMainScoreText();




        if (PlayerPrefs.GetInt("Training Finished") == 1)
        {
            TrainingCompleted = true;
        }
        if (PlayerPrefs.GetInt("PlayedOnce") == 1)
        {
            PlayedOnce = true;
        }
        //if (PlayerPrefs.GetInt("GameShared") == 1)
        //{
        //    ShareButton.enabled = false;
        //    ShareBGift.enabled = false;
        //}
        if (PlayerPrefs.HasKey("PlayTicket"))
        {
            PlayTicketUpdate(PlayerPrefs.GetInt("PlayTicket"));
        }
        else
        {
            PlayTicketUpdate(7);
        }
        if (PlayerPrefs.GetInt("FacebookShared") == 1)
        {
            FacebookGift.enabled = false;
            FacebookButton.interactable = false;
        }
        //if (PlayerPrefs.GetInt("OtherSocialNetworkShared") == 1)
        //{
        //    FacebookGift.enabled = false;
        //}
        //remove(PlayerPrefs.GetInt("GameShared") == 1)

    }
    private void Start()
    {
        //RequestConfiguration requestConfiguration =
        //    new RequestConfiguration.Builder().SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
        //    .SetMaxAdContentRating(MaxAdContentRating.G).build();


        //MobileAds.SetRequestConfiguration(requestConfiguration);


        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            LoadRewardedAd();
        });
        
    }
    
    public void PlayTicketUpdate(int count)
    {
        PlayTicketCount = count;
        PlayTicketText.text = "Play Ticket : " + PlayTicketCount;
        PlayerPrefs.SetInt("PlayTicket", PlayTicketCount);
    }
    public void GainPlayTicket(int PlayTCount)
    {
        PlayTicketUpdate(PlayTicketCount + PlayTCount);
        ShowGameGuide(PlayTCount + " Play Ticket Gained", true, false);
        PlayTicketButtons.SetActive(false);
    }
    public void OnPurchaseComplete(Product p)
    {
        GainPlayTicket(int.Parse(p.metadata.localizedTitle.ToString().Split(' ')[0]));
        
        Debug.Log(p.definition.id);

        PlayTicketButtons.SetActive(false);
        
        StartCoroutine(CloseIsProccessing(0.2f));
    }
    public void OnPurchaseFailed(Product p, PurchaseFailureDescription description)
    {
        ShowGameGuide(description.reason.ToString(), true, false);
        Debug.Log("PF");
        PlayTicketButtons.SetActive(false);
        StartCoroutine(CloseIsProccessing(0.2f));
    }
    public void OnPurchaseButton()
    {
        isProcessing = true;
        CloseSettingBox.enabled = true;
        CloseGameGuide();
    }
    public IEnumerator CloseIsProccessing(float time)
    {
        yield return new WaitForSeconds(time);//0.2f
        isProcessing = false;
    }
    
    //public void IAPurButton(string productId)
    //{
    //    IAPButton.productId = productId;
    //    CodelessIAPStoreListener.Instance.AddButton(this);
    //    IAPButton.onProductFetched.Invoke(CodelessIAPStoreListener.Instance.GetProduct(productId));

    //}
    public void Ad()
    {
        //VideoAd
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            ShowGameGuide("No Internet", true, false);
            PlayTicketButtons.SetActive(false);
        }
        else
        {
            if (rewardedAd != null && rewardedAd.CanShowAd())
            {
                ShowRewardedAd();
            }
            else
            {
                ShowGameGuide("Wait a moment and click again", true, false);
                PlayTicketButtons.SetActive(false);
                MobileAds.Initialize((InitializationStatus initStatus) =>
                {
                    // This callback is called once the MobileAds SDK is initialized.
                    LoadRewardedAd();

                });
            }

        }
    }
    
    private bool isFocus = false;
    public bool isProcessing = false;
    void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
        //Debug.Log("Focus : "+isFocus);
    }
    public void ShareGooglePlayStore()
    {
        if (!isProcessing)
        {
            StartCoroutine(ShareTextInAnroid());
            
        }
        else
        {
            Debug.Log("No sharing set up for this platform.");
        }
        
    }
    public void SharePage()
    {
        ShowGameGuide("", false, false);
        CloseSettingBox.enabled = true;
        ShareButtons.SetActive(true);
        
    }
    
    
    public IEnumerator ShareTextInAnroid()
    {
        var shareSubject = ""; //Subject text
        var shareMessage = "A fun, targeting and mathematical game : " + //Message text
                           "https://play.google.com/store/apps/details?id=com.KaanSelcukKaraboce.ArcherEquations"; //Your link
        //Previous message:
        //What is your level in mathematics?
        //Find out with this game : 
        isProcessing = true;

        if (!Application.isEditor)
        {
            CloseSettingBox.enabled = true;
            



            //Create intent for action send
            AndroidJavaClass intentClass =
                new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject =
                new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>
                ("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            //put text and subject extra
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");

            intentObject.Call<AndroidJavaObject>
                ("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);
            intentObject.Call<AndroidJavaObject>
                ("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);

            //call createChooser method of activity class
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            AndroidJavaObject currentActivity =
                unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser =
                intentClass.CallStatic<AndroidJavaObject>
                ("createChooser", intentObject, "Share your high score");
            currentActivity.Call("startActivity", chooser);

            yield return new WaitForSecondsRealtime(3);
            PlayerPrefs.SetInt("GameShared", 1);
            ShareButton.enabled = false;
            //PlayTicketUpdate(PlayTicketCount + 20);
            //ShowGameGuide("20 Play Ticket Gained", true, false);
            GainPlayTicket(20);
            ShareBGift.enabled = false;
        }

        yield return new WaitUntil(() => isFocus);
        isProcessing = false;


        
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


        if (TrainingLevels)
        {
            NumberShower[2].rectTransform.anchoredPosition = Vector3.zero;
        }
        else
        {
            NumberShower[2].rectTransform.position = OOAmmoText.rectTransform.position;
        }


        yield return new WaitForSecondsRealtime(0.001f);
        
        StartCoroutine(GuideArrowUI(4, false));
        StartCoroutine(GuideArrowUI(5, false));






    }









    private bool WonShowerReaching;
    

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
            //satır başka bir yere de koyulabilir her ok gerildiğinde yerine her yeni ok atıldığında ve bölüm başladığında çağırılabilir
            ArrowPathColor.color = a.MatPB.GetColor("_Color");

            //Color c=Color.RGBToHSV(a.MatPB.GetColor("_Color"));
            //Color.HSVToRGB(0, 0,1);
            //ArrowPathColor.color

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
    private void ScoreShower(ushort Score)
    {
        ShowGameGuide("+" + Score + " Point", true, false);
    }
    private void Win()
    {
        ThrowAMouseArea.enabled = false;
        Won.SetActive(true);
        TE.FastRotSpeed();
        PlayAudio(WinEffect);
        TE.CurrentTime = 0;

        

        if (TrainingLevels)
        {
            TrainingLevelChange(true);

        }
        else
        {
            if (CloseGameG != null)
            {
                StopCoroutine(CloseGameG);
            }
            CloseGameGuide();
            ScoreShower(CurrentMathScore);
            MathPoints(false);
        }
        NextLevelButton.SetActive(true);
        StartCoroutine(WonEqualityShower());

        RestartButImage.color = Color.gray;

        
    }
    //ifwon birkaç yerde kullanılıyor bazen check etmesine gerek olmuyor direk kazandıran method yazılacak
    public void IfWon()
    {
        //optimize edilecek TargetAndEquivalentSame() ile birleştirilebilir
        //yere saplanınca alttaki else if kısmına gerek kalmıyor, diğerlerinde gerekiyor, bu 2 ayrı method olarak yazılabilir belki:


        //kontrol etmeden won eklenecek bazen kontrole gerek yok
        if (!Won.activeSelf)
        {
            if (TE.TargetNo.UpNo == TE.Equivalent.UpNo/*||CurrentDifficulty.MustUseAllArrows*/)
            {
                if (!CurrentDifficulty.MustUseAllArrows)
                {

                    Win();

                }
                else if(Arrows.Next >= Arrows.ReadyCount)
                {
                    bool AllDidHit = true;
                    foreach (GameObject g in Arrows.Objects)
                    {
                        if (!g.GetComponent<Arrow>().DidHit && g.activeSelf)
                        {
                            AllDidHit = false;
                            
                        }
                    }
                    if (AllDidHit)
                    {
                        Win();
                    }
                }


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
                    TE.CurrentTime = 0;
                    CurrentMathScore = 0;
                    
                    OOAmmoText.enabled = true;
                    Tryagain();
                    EnterRandomNums = false;
                    

                }
            }
            //CurrentArrow.transform.eulerAngles = new Vector3(90, CurrentArrow.transform.eulerAngles.y, 0);

            //CurrentArrow.transform.rotation=Quaternion.identity;
        }

    }
    public void Tryagain()
    {
        if (!TrainingLevels)
        {
            ScoreShower(0);
        }
        NextLevelButton.GetComponentInChildren<TMP_Text>().text = "Try Again";
        NextLevelButton.SetActive(true);
        PlayAudio(LevelFailed);
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
    
    
    
    public void ShowLevelDurations()
    {
        TE.enabled = false;
        Menu.SetActive(false);
        Game.SetActive(false);
        Durations.enabled = true;

        //WriteDurations(Archery);
        //WriteDurations(Math);



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

    private string _adUnitIdAndroidRewarded = "ca-app-pub-7231413600508852/6299161912";
    //test Id:ca-app-pub-3940256099942544/5224354917
    //Correct Id:ca-app-pub-7231413600508852/6299161912


    //appId:ca-app-pub-7231413600508852~2565545437
    private RewardedAd rewardedAd;

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitIdAndroidRewarded, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
                RegisterEventHandlers();
            });
    }
    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        //if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(System.String.Format(rewardMsg, reward.Type, reward.Amount));
                //Debug.Log("Ödül");
                GainPlayTicket(8);

            });
        }
    }

    private void RegisterEventHandlers()
    {


        //// Raised when the ad is estimated to have earned money.
        //rewardedAd.OnAdPaid += (AdValue adValue) =>
        //{
        //    Debug.Log(System.String.Format("Rewarded ad paid {0} {1}.",
        //        adValue.Value,
        //        adValue.CurrencyCode));


        //};
        //// Raised when an impression is recorded for an ad.
        //rewardedAd.OnAdImpressionRecorded += () =>
        //{
        //    Debug.Log("Rewarded ad recorded an impression.");


        //};
        //// Raised when a click is recorded for an ad.
        //rewardedAd.OnAdClicked += () =>
        //{
        //    Debug.Log("Rewarded ad was clicked.");


        //};
        //// Raised when an ad opened full screen content.
        //rewardedAd.OnAdFullScreenContentOpened += () =>
        //{
        //    Debug.Log("Rewarded ad full screen content opened.");


        //};
        // Raised when the ad closed full screen content.
        rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            LoadRewardedAd();
        };
    }
}