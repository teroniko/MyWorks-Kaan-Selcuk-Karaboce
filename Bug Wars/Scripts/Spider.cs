using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Spider : MonoBehaviour
{
    
    [Space, Header("Settings")]
    public float gravityModifier = 0.0098f;
    public float WalkingModifier = 0.4f;
    private float WalkingHighSpeed = 0.4f;
    public float turningModifier = 0.5f;
    //private float JumpEnergy = 0.1f;

    //public float EnergyConsumer = 1;
    public CapsuleCollider walkCollider;
    Vector3 WalkColFront;

    public Transform CameraLate;

    [Space, Header("Debugger")]
    public int contactCount = 0;
    public float contactSpace = 0;
    public LayerMask contactLayer = 0;
    //public LayerMask attackLayer = 0;

    public Collider[] contactColliders = new Collider[10];
    public BugLeg[] Legs;

    public Vector3 contactPosition { get; set; }
    public Vector3 contactRotation { get; set; }
    Vector3 gravityRotation;

    public float radiusMultip = 0.95f;
    public float walkRadiusMultip = 2 / 5f;
    public float contactRadius => walkCollider.radius * radiusMultip;
    public GameObject Cam;
    public CinemachineFreeLook FreeLook;


    public SphereCollider AttackCollider;
    public Collider MouthCollider;

    public BoxCollider DefenceCollider;

    public bool AttackTime;
    //public bool TurnForAttack;
    public bool Hit;
    public Text MessageText;
    public Image MessageBox;
    public bool AI = false;
    public Material Black;
    public bool ZeroSpeed;

    public List<GameObject> TakenLegLimbs;
    public Transform Body;
    public Transform ParentBody;
    public Power[] Powers;

    float FreelookCameraYPos = 2;
    float freelookYvalue = 0.7f;

    bool cliff = false;
    public Vector3 Speed = Vector3.zero;
    bool separate = true;



    [Space, Header("Blood Effect")]
    public bool InfiniteDecal;
    public Light DirLight;
    public bool isVR = true;
    public GameObject BloodAttach;
    public GameObject[] BloodFX;
    public GameObject Canvas;
    bool firstTouch = false;

    public NetworkManager nm;
    public bool OnlineBegin = false;
    public bool ThereIsSpeed = false;
    private float DefTimePass = 0;
    float ServerPosUpdateDis;
    float ServerRotUpdateDis;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        OnlineBegin = false;
        ThereIsSpeed = false;
        StartCoroutine(StartSpider());
        Powers[0].slider.onValueChanged.AddListener(delegate {
            
            for(int i = 1; i < Powers.Length; i++)
            {
                if (!Powers[i].Ready())
                {
                    Powers[i].UpdateActiveTEnergy();
                }
            }
            SpeedUpdate();
        });

        AttackCollider.enabled = false;
        Hit = false;
        FreeLook.m_YAxis.Value = 0.7f;
        FreelookCameraYPos = 2;
        ServerPosUpdateDis = 0.05f;
        ServerRotUpdateDis = 2;

    }
    public void DefenceGetBug()
    {

        if (nm.Bugs.Count == 1)
        {
            StartCoroutine(Message("There is only you."));
        }
        else
        {
            float Shortest = Mathf.Infinity;
            int i0 = 0;
            for (int i = 0; i < nm.Bugs.Count; i++)
            {
                if (nm.Bugs[i].name != transform.parent.name)
                {
                    float distance = Vector3.Distance(transform.position, nm.Bugs[i].transform.GetChild(0).position);
                    if (distance < Shortest)
                    {
                        Shortest = distance;
                        i0 = i;
                    }

                }
            }

            NearestOpponent = nm.Bugs[i0].transform.Find("Tarantula").gameObject;
            if (!AI)
            {
                //Powers[0].slider.value -= Powers[2].requiredEnergy;
                StartCoroutine(Powers[2].Active());
                
            }
            StartCoroutine(Defence());
            DefTimePass = 0;


            //StartCoroutine(Defence(GC.Bugs[i0].transform.Find("Tarantula").gameObject));
        }
    }
    public void DefenceOff()
    {
        DefTimePass = 5;
    }
    public void DefenceIf()
    {
        //tek defence biti yani 1,2 yerine 1 koyarak yapılabilir mi acaba? öyle daha optimize olur
        if (DefenceCollider.enabled)
        {
            nm.Interaction(2);
            DefenceOff();
        }
        else if (/* && Powers[2].EnergyExist()&& */Powers[2].Ready())
        {
            nm.Interaction(1);
            DefenceGetBug();




        }
        
    }
    IEnumerator StartSpider()
    {
        yield return new WaitForSeconds(1);
        walkCollider.enabled = true;
        StartCoroutine(SpeedChangeTime());
        yield return new WaitForSeconds(2);
        OnlineBegin = true;
        
        //if (AI)
        //{
        //    Defence();


        //}

        //LegDead(Legs[0].gameObject);
        //LegDead(Legs[1].gameObject);
        //LegDead(Legs[2].gameObject);
        //LegDead(Legs[3].gameObject);
        //LegDead(Legs[4].gameObject);
        //LegDead(Legs[5].gameObject);

    }
    public IEnumerator Message(string message)
    {
        MessageBox.gameObject.SetActive(true);
        MessageText.text = message;
        yield return new WaitForSeconds(2.5f);
        MessageBox.gameObject.SetActive(false);
        MessageText.text = "";
    }
    
    private void Update()
    {
        
        if(FreeLook.m_YAxis.Value!= freelookYvalue)
        {
            FreeLook.m_YAxis.Value = Mathf.Lerp(FreeLook.m_YAxis.Value, freelookYvalue, 0.027f);
        }


        
        
    }
    public void PosCamera(InputAction.CallbackContext context)
    {
        switch (FreelookCameraYPos)
        {
            case 0:
                freelookYvalue = 1;
                break;
            case 1:
                freelookYvalue = 0.85f;
                break;
            case 2:
                freelookYvalue = 0.7f;
                break;
            case 3:
                freelookYvalue = 0.35f;
                break;
            case 4:
                freelookYvalue = 0;
                break;
        }

        //burası daha optimize yazılabilir(2 kere çağırıyor methodu)
        float MouseYPos = context.ReadValue<float>();
        if (MouseYPos > 0)
        {
            FreelookCameraYPos--;
        }
        else if (MouseYPos < 0)
        {
            FreelookCameraYPos++;
        }


        if (FreelookCameraYPos >= 5)
        {
            FreelookCameraYPos = 4;
        }
        else if (FreelookCameraYPos <= -1)
        {
            FreelookCameraYPos = 0;
        }




        
    }
    public void MidPosCamera()
    {
        FreelookCameraYPos = 2;
        freelookYvalue = 0.7f;
    }
    public void Jump()
    {

        if (!cliff && contactCount > 0 && !AI/* && Powers[0].slider.value >= JumpEnergy*/)
        {
            transform.position += transform.up * 0.6f;//0.9
            Speed += transform.up * gravityModifier * 15;

            separate = true;

            //Powers[0].slider.value -= JumpEnergy;

        }
    }
    private void SpeedUpdate()
    {
        WalkingModifier = 0.05f + 0.25f * Powers[0].slider.value / Powers[0].slider.maxValue;
    }
    IEnumerator SpeedDown()
    {
        //başka zamanlarda bug var mı
        yield return new WaitForSeconds(Powers[3].ActiveTime / 2f);
        SpeedUpdate();


        StartCoroutine(SpeedChangeTime());
    }
    public float slowDown = 5;
    
    IEnumerator SpeedChangeTime()
    {
        if (WalkingModifier <= 0.05f)
        {
            slowDown = 15;
            radiusMultip = 0.8f;
            walkCollider.transform.localPosition = Vector3.up * 0.05f;
            walkCollider.radius = 0.25f;
            yield break;
        }
        else if (WalkingModifier <= 0.1f)
        {
            slowDown = 9;
            radiusMultip = 0.8f;
            walkCollider.transform.localPosition = Vector3.up * 0.2f;
            walkCollider.radius = 0.4f;
            yield break;
        }
        yield return new WaitForSeconds(0.01f);

        if (WalkingModifier <= 0.2f && WalkingModifier > 0.1f)
        {
            slowDown = 9;
            radiusMultip = 0.8f;
            walkCollider.transform.localPosition = Vector3.up * 0.2f;
            walkCollider.radius = 0.4f;
            yield break;
        }
        else if (WalkingModifier <= 0.3f)
        {
            slowDown = 6;
            radiusMultip = 0.8f;
            walkCollider.transform.localPosition = Vector3.up * 0.2f;
            walkCollider.radius = 0.4f;
            yield break;
        }
        else if (WalkingModifier <= 0.4f)
        {
            slowDown = 2;
            radiusMultip = 0.8f;
            walkCollider.transform.localPosition = Vector3.up * 0.2f;
            walkCollider.radius = 0.4f;
            yield break;

        }
    }

    public void SpeedUp()
    {
        if (!AI && Powers[3].Ready()/* && Powers[3].EnergyExist()*/)
        {
            //Powers[0].slider.value -= Powers[3].requiredEnergy;
            WalkingModifier = WalkingHighSpeed;

            StartCoroutine(Powers[3].Active());
            StartCoroutine(SpeedDown());
            StartCoroutine(SpeedChangeTime());
            //defence duration decrease
        }

    }
    float mouseX;
    public void Rotate(InputAction.CallbackContext context)
    {
        mouseX= context.ReadValue<float>();
    }
    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        
    }
    
    Vector2 move;
    Vector3 OldPosition;
    Quaternion OldRotation;
    private void FixedUpdate()
    {
        //bunu alta koymak belki boşuna değildir
        //onlinebegin ile yapmam çok optimize olmayabilir
        if (OnlineBegin && Cam.activeSelf)//mouthcol bool'ü eklemek muhtemelen daha büyük trafik
        {

            if (Quaternion.Angle(transform.rotation, OldRotation) > ServerRotUpdateDis)
            {
                ThereIsSpeed = false;
                OldRotation = transform.rotation;
                nm.PosRot(transform.position, transform.rotation, ThereIsSpeed);
            }
            if (Vector3.Distance(OldPosition, transform.position) > ServerPosUpdateDis)
            {
                ThereIsSpeed = true;
                OldPosition = transform.position;
                nm.PosRot(transform.position, transform.rotation, ThereIsSpeed);


                //ThereIsSpeed = Speed.magnitude != 0;
                //ThereIsSpeed = true;
                //if (RotSpeed != 0 || ThereIsSpeed)
                //{
                //    nm.PosRot(transform.position, transform.rotation, ThereIsSpeed);
                //}

            }

        }





        if (!walkCollider.enabled)
            return;


        
        WalkColFront = Vector3.zero;

        contactCount = Physics.OverlapCapsuleNonAlloc(walkCollider.transform.position, walkCollider.transform.position + WalkColFront, walkCollider.radius, contactColliders, contactLayer);

        gravityRotation = Vector3.down;
        contactRotation = Vector3.zero;
        cliff = false;
        bool MouthColEn = MouthCollider.enabled;
        //if (walkCollider.radius == 0.25f)
        //{
        //    Debug.Log(0.25);
        //}
        
        if (contactCount >= 1)
        {
            //if (MouthColEn && walkCollider.radius == 0.15f)
            //{
            //    Hit = true;
            //    Debug.Log("Hit failed!");
            //    StartCoroutine(Message("Hit failed!"));
            //    RecoilJump(transform.forward, 0.15f);

            //}
            //else if(!AttackTime)
            //{

            //}
            //if (!AttackTime)
            //{
            //StartCoroutine(SpeedChangeTime());
            //}

            Speed = Vector3.zero;
            
            for (int index = 0; index < contactCount; index++)
            {
                Vector3 position = Vector3.zero;

                if (contactColliders[index].name == "Terrain")
                {

                    Ray ray = new Ray(walkCollider.transform.position + walkCollider.radius * Vector3.up, Vector3.down);
                    Debug.DrawRay(ray.origin, ray.direction, Color.black);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, walkCollider.radius * 4.5f, contactLayer))
                    {
                        position = hit.point + contactRadius * (Vector3.up - hit.normal);

                    }
                }

                else
                {
                    position = contactColliders[index].ClosestPoint(walkCollider.transform.position);
                }
                Vector3 Difference = position - walkCollider.transform.position;
                Vector3 ValueDirection = Difference.normalized;


                float ColContactRadius = Vector3.Distance(position, walkCollider.transform.position);


                if (index == 0)
                {

                    gravityRotation = Vector3.zero;
                }
                if (contactColliders[index].tag == "Cliff")
                {
                    transform.position += position - (walkCollider.transform.position + ValueDirection * walkCollider.radius)
                        /*+Vector3.down*0.1f*/;
                    cliff = true;
                    
                    
                }
                else
                {
                    gravityRotation += ValueDirection;

                    float slowDown0 = 1;
                    if (contactColliders[index].tag == "Tree" && contactCount > 1)
                    {
                        slowDown0 = slowDown;

                    }
                    transform.position += -transform.up * (ColContactRadius - contactRadius) / slowDown0;

                    //transform.Translate(Vector3.down * (ColContactRadius - contactRadius) / slowDown0);
                    if (separate)
                    {
                        transform.position += ValueDirection * (walkCollider.radius - contactRadius)*3f;
                    }

                }




            }

            if (cliff)
            {
                transform.LookAt(transform.position + transform.forward);
                if (contactCount == 1)
                {
                    move = Vector2.zero;
                }
            }
            if (move.y != 0 || move.x != 0|| DefenceCollider.enabled|| !firstTouch /*|| separate*/)
            {
                
                
                if(!cliff)
                {
                    transform.LookAt(transform.position + Vector3.Cross(gravityRotation, transform.right), -gravityRotation);

                    //GroundLook();



                }
                //transform.Translate(Vector3.down * (ColContact0Radius - contactRadius));
                //g.transform.position =walkCollider.transform.position+ Vector3.down * (ColContact0Radius - contactRadius);
                firstTouch = true;



                //bunu silersem bir etkisi olmayabilir dene!
                if (!AttackTime)
                {
                    //SpeedUpdate();
                    StartCoroutine(SpeedChangeTime());

                }
                //
            }

            float DefEnergy = 1;
            if (DefenceCollider.enabled/* && !AI*/)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(NearestOpponent.transform.position - transform.position, transform.up), 0.05f);
                DefEnergy = 0.5f;
            }
            //walkCollider.transform.LookAt(walkCollider.transform.position + (walkCollider.transform.forward * VertiMove + walkCollider.transform.right * HoriMove).normalized);
            separate = false;





            if (AI)
            {
                
                transform.Rotate(0.01f * Vector3.up * turningModifier);
                Vector3 Speed0 = transform.forward * WalkingModifier;
                //if (Energy > Speed0.magnitude * WalkEnergy)
                {
                    Speed = Speed0;
                    //EnergyChange(Speed0.magnitude);

                }
            }
            else
            {
                //if (Cam.activeSelf)
                //{
                //    RotSpeed = Mouse.current.delta.x.ReadValue();
                    
                //}
                
                transform.Rotate(mouseX * Vector3.up * turningModifier);
                
                //if (VertiMove != 0)
                {
                    Speed += transform.forward * move.y;
                }
                //if (HoriMove != 0)
                {
                    Speed += transform.right * move.x;
                }
                
                
                Speed *= WalkingModifier* DefEnergy/** Powers[0].slider.value*DefEnergy / Powers[0].slider.maxValue*/;
            }
            
        }
        else
        {
            //transform.position += Speed;
            //Speed += Vector3.down * gravityModifier;
            //transform.position += Vector3.down * FallSpeed + Vector3.right * XSpeed+Vector3.forward*ZSpeed;
            //FallSpeed += gravityModifier;
            if (!MouthColEn)
            {
                Speed += Vector3.down * gravityModifier;
                float WalkRadius = Speed.magnitude * 2f;
                if (walkCollider.radius != WalkRadius)
                {
                    walkCollider.radius = WalkRadius;
                }
                
            }
            firstTouch = false;


        }
        transform.position += Speed;
        
        


        CameraLate.position = transform.position;
        CameraLate.rotation = Quaternion.Lerp(CameraLate.rotation, transform.rotation, 0.05f);


    }
    //private void GroundLook()
    //{
    //    transform.LookAt(transform.position + Vector3.Cross(gravityRotation, transform.right), -gravityRotation);
    //}
    public IEnumerator AttackColGetOpponent()
    {
        yield return new WaitForSeconds(0.05f);
        AttackCollider.enabled = false;

        if (/*!MouthCollider.enabled*/!AttackTime/*!walkCollider*/)
        {
            StartCoroutine(Message("Opponent not close enough!"));
        }
    }
    public IEnumerator Attack(GameObject Target)
    {
        AttackTime = true;
        if (!AI)
        {
            //Powers[0].slider.value -= Powers[1].requiredEnergy;
            StartCoroutine(Powers[1].Active());
        }
        //Vector3 HitFailPoint = Vector3.zero;
        //RaycastHit hit;
        ////kısalt:
        //Ray ray = new Ray(MouthCollider.transform.position, Target.transform.position- MouthCollider.transform.position);
        //if (Physics.Raycast(ray, out hit, Vector3.Distance(MouthCollider.transform.position, Target.transform.position), attackLayer))
        //{

        //    if((1 << hit.transform.gameObject.layer) == contactLayer)
        //    {
        //        HitFailPoint = hit.point;
        //        Debug.Log("Will Fail");

        //    }
        //}

        //transform.position += transform.up * 0.3f;

        


        walkCollider.enabled = false;

        AttackCollider.enabled = false;

        float time = Time.deltaTime * 5f / 2f;
        //transform.LookAt(Target, transform.up);
        yield return new WaitForSeconds(0.05f * time);
        //CameraLate.rotation = transform.rotation;
        //AttackTime = true;
        transform.Translate(Vector3.up * 0.1f);
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.005f * time);
            transform.Translate(Vector3.back * 0.08f);
            Vector3 targetDirection = Target.transform.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.4f, 0.0f);
            //Quaternion Rot = Quaternion.LookRotation(newDirection);
            //transform.rotation = new Quaternion(Rot.x, Rot.y, transform.rotation.z, Rot.w);
            transform.LookAt(transform.position+ newDirection, transform.up);
        }
        int i0 = 0;
        float hop = 0;

        //hız azalmıyor ve hit failed


        //WalkColAttackPos:
        //slowDown = 15;
        //radiusMultip = 0.8f;
        //walkCollider.transform.localPosition = transform.up * 0.25f;
        //walkCollider.radius = 0.15f;


        //transform.Translate(Vector3.down * 0.3f);
        ServerPosUpdateDis = 0.005f;
        ServerRotUpdateDis = 0.2f;
        while (!Hit)
        {
            
            i0++;
            if (i0 == 19)
            {
                LegsActive(false, true);
            }
            if (i0 == 2)
            {

                MouthCollider.enabled = true;
                
            }
            yield return new WaitForSeconds(0.005f * time);
            Vector3 targetDirection = Target.transform.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.4f, 0.0f);
            //Quaternion Rot= Quaternion.LookRotation(newDirection);
            //transform.rotation = new Quaternion(Rot.x, Rot.y, transform.rotation.z, Rot.w);
            transform.LookAt(transform.position + newDirection,transform.up);




            //if (HitFailPoint != Vector3.zero)
            //{
            //    if (Vector3.Distance(MouthCollider.transform.position, HitFailPoint) < 0.3f)
            //    {
            //        RecoilJump(transform.forward, 0.15f);
            //        break;
            //    }
            //}
            float speed = 0.4f;
            transform.Translate(Vector3.forward * speed);
            hop += speed;
            if (hop > AttackCollider.radius * 4)
            {

                break;
            }
        }
        ServerPosUpdateDis = 0.05f;
        ServerRotUpdateDis = 2;
        walkCollider.enabled = true;
        LegsActive(true,true);
        AttackTime = false;

        MouthCollider.enabled = false;
        Hit = false;

        //SpeedUpdate();
        //StartCoroutine(SpeedChangeTime());

    }
    public void LegsHealthDown()
    {
        if (Cam.activeSelf)//gerek var mı
        {
            
            
            nm.Interaction(4);
            foreach (BugLeg Leg in Legs)
            {
                LegHealthDown(Leg, 1);

            }
        }
            
    }
    //sürekli tek başına leghealthdown'u göndermesi server'ı yavaşlatır
    public void LegHealthDown(BugLeg Leg, ushort lifetaken)
    {
        
        if (lifetaken == 3)//gerek var mı
        {
            for (int i = 0; i < Legs.Length; i++)
            {
                if (Legs[i] == Leg)
                {
                    nm.Interaction((byte)(i + 5));

                    //0 attack
                    //1 defence
                    //2 defence off
                    //3 death
                    //4 legshealthdown
                    //5 leghealthdown (BugLeg[0]) (3 life)
                    //6 leghealthdown (BugLeg[1]) (3 life)
                    //7 leghealthdown (BugLeg[2]) (3 life)
                    //8 leghealthdown (BugLeg[3]) (3 life)
                    //9 leghealthdown (BugLeg[4]) (3 life)
                    //10 leghealthdown (BugLeg[5]) (3 life)
                    //11 leghealthdown (BugLeg[6]) (3 life)
                    //12 leghealthdown (BugLeg[7]) (3 life)
                }
            }
        }
        
        if (Leg.Health > 0)
        {
            Leg.Health -= lifetaken;
            byte maxhealth = 3;
            if (!AI)
            {
                //maxhealth,lifetaken,Leg.Health ile formülize edip daha kısa code yazılabilir
                Powers[0].slider.value -= lifetaken;
                
                
            }
            if (Leg.Health <= 0)
            {
                Powers[0].slider.value -= -Leg.Health;
                //SpeedUpdate();

                Leg.Health = 0;
                //eğer bacaklarının canı azalınca bazı özelliklerine etki ediyorsa o zaman bacak canı azalması metodunu server'a göndermem gerekiyor
                LegDead(Leg.gameObject);
            }
            

            //if (!AI && Powers[0].slider.value <= 0)
            //{
            //    enabled = false;
            //    //StartCoroutine(Death(0));
            //}
        }
        //for (int i = 1; i < Powers.Length; i++)
        //{
        //    Powers[i].UpdateActiveTEnergy();
        //}
        //SpeedUpdate();
    }
    public void LegDead(GameObject Leg)
    {
        //Debug.Log("Leg stolen!");
        //for(int i = 0; i < Legs.Length; i++)
        //{
        //    if (Legs[i] == Leg)
        //    {
        //        nm.Interaction((byte)(4 + i));
        //        //0 attack
        //        //1 defence
        //        //2 defence off
        //        //3 death
        //        //4 leghealthdown (BugLeg[0])
        //        //5 leghealthdown (BugLeg[1])
        //        //6 leghealthdown (BugLeg[2])
        //        //7 leghealthdown (BugLeg[3])
        //        //8 leghealthdown (BugLeg[4])
        //        //9 leghealthdown (BugLeg[5])
        //        //10 leghealthdown (BugLeg[6])
        //        //11 leghealthdown (BugLeg[7])
        //    }
        //}
        

        StartCoroutine(Message("Leg stolen!"));
        //SpiderScript.Hit = true;
        //Spider OtherSpiderScript = other.transform.root.Find("Tarantula").GetComponent<Spider>();
        //bu tarz tanımlamalar optimize olmalı her spiderda olacak o ulaşacak

        //OtherSpiderScript.subjectCollider.enabled = false;


        //OtherSpiderScript.LegsActive(false);

        GameObject g = Leg;
        //Leg.GetComponent<BugLeg>().LegGrounded = !Leg.GetComponent<BugLeg>().LegGrounded;
        Vector3 frontRig = Vector3.zero;
        for (int i = 0; i < 7; i++)
        {
            if (g.name[12] == 'C')
            {
                frontRig = g.transform.position;
                break;
            }
            g = g.transform.parent.gameObject;

        }

        RecoilJump(frontRig, 0.15f);




        //BloodSplash(this);

        //BloodSplash(OtherSpiderScript);
        int side = -1;
        if (g.name[10] == 'l')
        {
            side = 1;
        }
        while (g.transform.childCount != 0)
        {
            //Debug.Log(g.name);

            g.transform.position += g.transform.right * 0.12f * side;
            
            g.SetActive(false);
            
            

            TakenLegLimbs.Add(g);
            g = g.transform.GetChild(0).gameObject;

        }
        //GameObject g0 = Leg.GetComponentInChildren<BugLeg>().gameObject;
        //for (int i = 0; i < 7; i++)
        //{
        //    Debug.Log(g0.name);
        //    g0.transform.position += g0.transform.right * 0.15f * side;
        //    g0.SetActive(false);

        //    TakenLegLimbs.Add(g0);
        //    g0 = g0.transform.parent.gameObject;
        //}


        
        
    }
    public void LegsActive(bool active, bool all)
    {
        for (int i = 0; i < Legs.Length; i++)
        {
            if (all)
            {
                //ActiveLeg(Legs[i], active);
                Legs[i].gameObject.SetActive(active);
            }
            else
            {
                if (i < 2 || i > 3 && i < 6)
                {
                    Legs[i].gameObject.SetActive(active);
                    //ActiveLeg(Legs[i], active);
                }
            }
            
        }
    }
    //public void ActiveLeg(BugLeg Leg, bool active)
    //{
    //    Leg.gameObject.SetActive(active);
    //}
    public GameObject NearestOpponent;
    public void AttackIf()
    {
        if (!AttackTime && !DefenceCollider.enabled/* && Powers[1].EnergyExist()*/ && Powers[1].Ready())
        {
            nm.Interaction(0);
            Attack();
        }
    }


    public void Attack()
    {
        AttackCollider.enabled = true;

        StartCoroutine(AttackColGetOpponent());
    }
    //public void AttackDefence(BaseEventData bed)
    //{
    //    PointerEventData ped = (PointerEventData)bed;
    //    if (!AI)
    //    {
    //        switch (ped.button.ToString())
    //        {
    //            case "Left"://Attack
    //                AttackIf();
    //                break;
    //            case "Right":
    //                DefenceIf();

    //                break;
    //        }
    //    }

    //}
    
    IEnumerator Defence()
    {
        //for (int i = 0; i < 300; i++)
        //{
        //    yield return new WaitForSeconds(0.01f);
        //    //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(NearestOpponent.transform.position - transform.position, transform.up), 0.05f);
        //    //GroundFit();

        //}
        //defence time duration decrease
        float speed = 10;
        for (int i = 0; i < speed; i++)
        {
            yield return new WaitForSeconds(0.01f);
            ParentBody.transform.Rotate(Vector3.up * 35 / speed);
            Body.transform.Rotate(Vector3.up * 35 / speed);
        }
        DefenceCollider.enabled = true;

        float DefDuration = 2;
        

        LegsActive(false, false);
        float time = 0.02f;
        
        while (!Hit)
        {
            yield return new WaitForSeconds(time);
            DefTimePass+= time;
            if(DefTimePass > DefDuration)
            {
                //RecoilJump(ParentBody.transform.position,4);
                break;
            }
        }
        for (int i = 0; i < speed; i++)
        {
            yield return new WaitForSeconds(0.01f);
            ParentBody.transform.Rotate(Vector3.down * 35 / speed);
            Body.transform.Rotate(Vector3.down * 35 / speed);
        }
        Hit = false;
        
        if (DefTimePass < DefDuration)
        {
            StartCoroutine(Defence(/*timePass*/));

        }
        else
        {
            DefenceCollider.enabled = false;
            LegsActive(true, false);
        }
        //Play test, Defence colliderın hit olunca defence olayı bitecek mi?

    }
    public void RecoilJump(Vector3 MainLegPos, float power)
    {


        transform.position += transform.up * 0.9f;

        Speed = (transform.position - MainLegPos).normalized * power + transform.up * 0.005f;


        LegsActive(true, true);
        
    }

    public IEnumerator Death(/*Spider OtherSpiderScript*/)
    {
        nm.Interaction(3);
        StartCoroutine(Message("He is dead!"));

        transform.Find("Tarantula_mesh").GetComponent<SkinnedMeshRenderer>().material = Black;
        WalkingModifier = 0;
        turningModifier = 0;
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    public void HealUp()
    {
        for (int i = 0; i < TakenLegLimbs.Count; i++)
        {
            GameObject limb = TakenLegLimbs[i];
            limb.SetActive(true);
            int side = 1;
            if (limb.name[10] == 'l')
            {
                side = -1;
            }

            limb.transform.position += limb.transform.right * 0.12f * side;
            //limb.tag = "Leg";
        }
        foreach(BugLeg Leg in Legs)
        {
            Leg.Health = 3;
            //Powers[0].slider.value = Powers[0].slider.maxValue;
        }
        Powers[0].slider.value = Powers[0].slider.maxValue;
        TakenLegLimbs.Clear();
        //bütün leglerin canını full yap
    }
    public void BloodSplash(Spider otherSpider)
    {
        float angle = Vector3.Angle(otherSpider.transform.position, transform.position)/*Mathf.Atan2(hit.normal.x, hit.normal.z) * Mathf.Rad2Deg + 180*/;

        if (effectIdx == BloodFX.Length) effectIdx = 0;

        var instance = Instantiate(BloodFX[effectIdx], otherSpider.transform.position, Quaternion.Euler(0, angle + 90, 0));
        effectIdx++;

        var settings = instance.GetComponent<BFX_BloodSettings>();

        settings.LightIntensityMultiplier = DirLight.intensity;


        var nearestBone = GetNearestObject(otherSpider.transform, otherSpider.transform.position);
        if (nearestBone != null)
        {
            var attachBloodInstance = Instantiate(BloodAttach);
            var bloodT = attachBloodInstance.transform;
            bloodT.position = otherSpider.transform.position;
            bloodT.localRotation = Quaternion.identity;
            bloodT.localScale = Vector3.one * Random.Range(0.75f, 1.2f);
            bloodT.LookAt(otherSpider.transform.position + transform.forward, direction);
            bloodT.Rotate(90, 0, 0);
            bloodT.transform.parent = otherSpider.transform;//nearestBone
        }

    }



    Transform GetNearestObject(Transform hit, Vector3 hitPos)
    {
        var closestPos = 100f;
        Transform closestBone = null;
        var childs = hit.GetComponentsInChildren<Transform>();

        foreach (var child in childs)
        {
            var dist = Vector3.Distance(child.position, hitPos);
            if (dist < closestPos)
            {
                closestPos = dist;
                closestBone = child;
            }
        }

        var distRoot = Vector3.Distance(hit.position, hitPos);
        if (distRoot < closestPos)
        {
            closestPos = distRoot;
            closestBone = hit;
        }
        return closestBone;
    }

    public Vector3 direction;
    int effectIdx;



    public float CalculateAngle(Vector3 from, Vector3 to)
    {

        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;

    }
  

}
