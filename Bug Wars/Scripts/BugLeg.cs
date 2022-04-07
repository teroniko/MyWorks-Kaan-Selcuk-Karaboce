
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugLeg : MonoBehaviour
{
    /// <summary>
    /// Chain length of bones
    /// </summary>
    public int ChainLength = 2;

    /// <summary>
    /// Target the chain should bent to
    /// </summary>
    public Transform Target;
    public Transform Pole;

    /// <summary>
    /// Solver iterations per update
    /// </summary>
    [Header("Solver Parameters")]
    public int Iterations = 10;

    /// <summary>
    /// Distance when the solver stops
    /// </summary>
    public float Delta = 0.001f;

    /// <summary>
    /// Strength of going back to the start position.
    /// </summary>
    [Range(0, 1)]
    public float SnapBackStrength = 1f;


    protected float[] BonesLength; //Target to Origin
    protected float CompleteLength;
    protected Transform[] Bones;
    protected Vector3[] Positions;
    protected Vector3[] StartDirectionSucc;
    protected Quaternion[] StartRotationBone;
    protected Quaternion StartRotationTarget;
    protected Transform Root;
    private Vector3 LockedPos;
    //public Transform FixedPosition;
    private GameObject Spider;
    public int stepPriority;
    private float StepTime = 0;
    public float StepDistance = 0.5f;
    public GameObject OtherSideLeg;//spiderleg yap
    //private Vector3 NewStepPosFinder;
    //Vector3 newStepUpper;
    public bool LegGrounded = false;
    private Vector3 zigzag;

    float StepTimeY;
    float StepTimeStep = 0.04f;
    public Vector3 LegNormal;
    GameObject[] LegPosFinder;
    Spider SpiderScript;
    Transform MainLeg;

    public int Health;
    void Awake()
    {
        MainLeg = transform.parent.parent.parent.parent.parent;
        Init();
        Spider = MainLeg.parent.parent.parent.parent.gameObject;
        
        SpiderScript = GetComponentInParent<Spider>();

        Target.position = transform.position;
        LockedPos = transform.position;

        

        LockedPos -= (Spider.transform.position - transform.position) * 0.2f;
        Target.position = LockedPos;


        LegGrounded = true;
        zigzag += stepPriority * Spider.transform.forward * StepDistance / 4f;
        


        StartCoroutine(LegPositionFinder());
    }
    IEnumerator LegPositionFinder()
    {
        yield return new WaitForSeconds(0.4f);
        LegPosFinder = new GameObject[6];

        float angle = 50;
        LegPosFinder[0] = new GameObject("LegPosFinder" + 0);
        LegPosFinder[0].transform.parent = MainLeg;
        LegPosFinder[0].transform.position = MainLeg.position;
        LegPosFinder[0].transform.rotation = MainLeg.rotation;

        LegPosFinder[1] = new GameObject("LegPosFinder" + 1);
        LegPosFinder[1].transform.parent = LegPosFinder[0].transform;

        LegPosFinder[1].transform.position = LegPosFinder[0].transform.position + SpiderScript.transform.up / 5f;

        LegPosFinder[1].transform.rotation = LegPosFinder[0].transform.rotation;
        LegPosFinder[0].transform.Rotate(Vector3.forward, angle);



        

        for (int i = 2; i < LegPosFinder.Length; i++)
        {
            LegPosFinder[i] = new GameObject("LegPosFinder" + i);
            LegPosFinder[i].transform.parent = LegPosFinder[i-1].transform;

            LegPosFinder[i].transform.position = LegPosFinder[i-1].transform.position + (LegPosFinder[i - 1].transform.position - LegPosFinder[i-2].transform.position);
            LegPosFinder[i].transform.rotation = LegPosFinder[i - 1].transform.rotation;
            LegPosFinder[i - 1].transform.Rotate(Vector3.forward, angle);
        }



        //for (int i = 2; i < LegPosFinder.Length; i++)
        //{
        //    LegPosFinder[i] = new GameObject("LegPosFinder"+i);
        //    LegPosFinder[i].transform.parent = LegPosFinder[i-1].transform;

        //    LegPosFinder[i].transform.position = LegPosFinder[i - 1].transform.position+(LegPosFinder[i - 1].transform.position- LegPosFinder[i - 2].transform.position);

        //    LegPosFinder[i-1].transform.Rotate((transform.position - MainLeg.localPosition), 40);
        //}



        //LegPosFinder[0].transform.rotation = Quaternion.RotateTowards(LegPosFinder[0].transform.rotation, Quaternion.LookRotation(MainLeg.Find("tarantula_l_PatellaI_bone").Find("tarantula_l_TibiaI_bone").position- MainLeg.position), 10);

        //LegPosFinder[0].transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(LegPosFinder[1].transform.position-MainLeg.position, MainLeg.position-MainLeg.Find("tarantula_l_PatellaI_bone").Find("tarantula_l_TibiaI_bone").position, 0.001f, 0));




        //Vector3.Lerp(LegPosFinder[0].transform.position + (LegPosFinder[0].transform.position + (LegPosFinder[0].transform.position - SpiderScript.transform.up)) * 1.1f, Pole.position, 1f);

        //Vector3.RotateTowards(LegPosFinder[0].transform.position + SpiderScript.transform.up, Pole.position, 0.1f, 0); 

        // Vector3.Lerp(LegPosFinder[0].transform.position + SpiderScript.transform.up * 1.1f, Pole.position, 1f);


        //LegPosFinder[2].transform.position = Vector3.Lerp(LegPosFinder[1].transform.position + (LegPosFinder[1].transform.position - LegPosFinder[0].transform.position) * 1.1f, Quaternion.AngleAxis(-45, Pole.position - LegPosFinder[1].transform.position)* Vector3.one, 1f);





        //LegPosFinder[2].transform.position = Vector3.RotateTowards(transform.position + SpiderScript.transform.up, Pole.position, 0.1f, 0);
        //LegPosFinder[0].transform.position + (Pole.position - LegPosFinder[0].transform.position) * 0.5f;
        //LegPosFinder[2].transform.position = LegPosFinder[1].transform.position+(Pole.position- LegPosFinder[1].transform.position)*0.5f;
        //ince dallarda yürüyebilecek şekilde yapmalıyım belkide
        //LegPosFinder[2].transform.position = Vector3.RotateTowards(LegPosFinder[1].transform.position, Pole.position, 0.5f, 0);

    }
    public bool LegTaken = false;
    //Vector3 lockedPosHitNormal;
    private void Update()
    {


        //ilk geri olan ayağı yarım adım atacak sonra sürekli tam
        //dönünce bacak konumları 0lanıyor zigzigı yeniden aktif et dönünce

        //newStepUpper = /*Spider.transform.forward * StepDistance * 0.5f +*/ Spider.transform.up * 1.2f;

        


        //if (!SpiderScript.FreezeLegs)
        {
            if (LegPosFinder != null)
            {
                if (Target.position != LockedPos)
                {
                    ResolveIK();
                }
                
                StepTimeStep = SpiderScript.WalkingModifier * 1.6f;

                if (!LegTaken)
                {
                    Target.position = LockedPos;

                }

                if (SpiderScript.AttackTime)
                    return;


                Vector3 start, end;
                bool targetFound = false;
                RaycastHit hit = new RaycastHit();
                bool Hit = false;
                float distance = 5;
                for (int i = 0; i <= LegPosFinder.Length - 2; i++)
                {
                    start = LegPosFinder[i].transform.position;
                    end = LegPosFinder[i + 1].transform.position;
                    Debug.DrawRay(start, end - start, Color.red);

                    if (Physics.Raycast(start, end - start, out hit, Vector3.Distance(end, start), SpiderScript.contactLayer))
                    {
                        //float angle = Vector3.Angle(Spider.transform.forward, lockedPosHitNormal)-90f;
                        //Debug.Log(angle);
                        //Debug.DrawLine(LockedPos, hit.point, Color.green);
                        //Debug.DrawLine(LockedPos, hit.point + Spider.transform.up * Mathf.Sin(angle) * Vector3.Distance(LockedPos, hit.point), Color.green);

                        Hit = true;
                        distance = Vector3.Distance(LockedPos, hit.point);
                       
                        if (distance >= StepDistance && (OtherSideLeg.GetComponent<BugLeg>().LegGrounded|| OtherSideLeg) /*&& (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Mouse X") != 0)*/)
                        {
                            targetFound = true;

                        }
                        break;
                    }
                    //else if(i == LegPosFinder.Length - 2&&!Hit)
                    //{
                    //    LockedPos = LegPosFinder[3].transform.position;
                    //}
                }

                if (!Hit)
                {
                    LockedPos = LegPosFinder[5].transform.position;
                    Target.position = LockedPos;
                    
                }
                if (targetFound)
                {
                    //Debug.Log(name + " : " + StepTime);
                    //Debug.Log(hitPoint);



                    //LegNormal = hit.normal;


                    //if (hit.distance > 1.5f || hit.distance < 0.7f)
                    //{
                    //    SlopeLeg = Spider.transform.right * 0.4f;
                    //    return;
                    //}
                    LegGrounded = false;
                    StepTime += StepTimeStep;

                    Vector3 difference = hit.point - LockedPos;
                    if (distance > StepDistance * 4)
                    {
                        StepTime = 0;
                        LockedPos = hit.point;
                        //lockedPosHitNormal = hit.normal;
                        LegGrounded = true;
                        zigzag = Vector3.zero;
                    }


                    Vector3 a = new Vector3(difference.x * StepTime, difference.y * StepTime/*+ animCurve.Evaluate(StepTime)*/, difference.z * StepTime);
                    if (StepTime >= 0.5f)
                    {
                        StepTimeY = StepTime - StepTimeStep * 2;
                    }
                    a += Spider.transform.up * StepTimeY * 0.1f;
                    Target.position += a;

                    if (StepTime >= 1)
                    {
                        StepTime = 0;
                        LockedPos = hit.point;
                        //lockedPosHitNormal = hit.normal;
                        LegGrounded = true;
                        zigzag = Vector3.zero;

                    }
                }
            }
        }






    }
    void Init()
    {
        //initial array
        Bones = new Transform[ChainLength + 1];
        Positions = new Vector3[ChainLength + 1];
        BonesLength = new float[ChainLength];
        StartDirectionSucc = new Vector3[ChainLength + 1];
        StartRotationBone = new Quaternion[ChainLength + 1];

        //find root
        Root = transform;
        for (var i = 0; i <= ChainLength; i++)
        {
            if (Root == null)
                throw new UnityException("The chain value is longer than the ancestor chain!");
            Root = Root.parent;
        }



        //init target
        if (Target == null)
        {
            Target = new GameObject(gameObject.name + " Target").transform;
            SetPositionRootSpace(Target, GetPositionRootSpace(transform));
        }
        StartRotationTarget = GetRotationRootSpace(Target);


        //init data
        var current = transform;
        CompleteLength = 0;
        for (var i = Bones.Length - 1; i >= 0; i--)
        {
            Bones[i] = current;
            StartRotationBone[i] = GetRotationRootSpace(current);

            if (i == Bones.Length - 1)
            {
                //leaf
                StartDirectionSucc[i] = GetPositionRootSpace(Target) - GetPositionRootSpace(current);
            }
            else
            {
                //mid bone
                StartDirectionSucc[i] = GetPositionRootSpace(Bones[i + 1]) - GetPositionRootSpace(current);
                BonesLength[i] = StartDirectionSucc[i].magnitude;
                CompleteLength += BonesLength[i];
            }

            current = current.parent;
        }



    }

    // Update is called once per frame
    void LateUpdate()
    {
        //ResolveIK();
    }

    private void ResolveIK()
    {
        if (Target == null)
            return;

        if (BonesLength.Length != ChainLength)
            Init();

        //Fabric

        //  root
        //  (bone0) (bonelen 0) (bone1) (bonelen 1) (bone2)...
        //   x--------------------x--------------------x---...

        //get position
        for (int i = 0; i < Bones.Length; i++)
            Positions[i] = GetPositionRootSpace(Bones[i]);

        var targetPosition = GetPositionRootSpace(Target);
        var targetRotation = GetRotationRootSpace(Target);

        //1st is possible to reach?
        if ((targetPosition - GetPositionRootSpace(Bones[0])).sqrMagnitude >= CompleteLength * CompleteLength)
        {
            //Debug.Log("Far");
            //just strech it
            var direction = (targetPosition - Positions[0]).normalized;
            //set everything after root
            for (int i = 1; i < Positions.Length; i++)
                Positions[i] = Positions[i - 1] + direction * BonesLength[i - 1];

        }
        else
        {

            for (int i = 0; i < Positions.Length - 1; i++)
                Positions[i + 1] = Vector3.Lerp(Positions[i + 1], Positions[i] + StartDirectionSucc[i], SnapBackStrength);

            for (int iteration = 0; iteration < Iterations; iteration++)
            {
                //https://www.youtube.com/watch?v=UNoX65PRehA
                //back
                for (int i = Positions.Length - 1; i > 0; i--)
                {
                    if (i == Positions.Length - 1)
                        Positions[i] = targetPosition; //set it to target
                    else
                        Positions[i] = Positions[i + 1] + (Positions[i] - Positions[i + 1]).normalized * BonesLength[i]; //set in line on distance
                }

                //forward
                for (int i = 1; i < Positions.Length; i++)
                    Positions[i] = Positions[i - 1] + (Positions[i] - Positions[i - 1]).normalized * BonesLength[i - 1];

                //close enough?
                if ((Positions[Positions.Length - 1] - targetPosition).sqrMagnitude < Delta * Delta)
                    break;
            }
        }

        //move towards pole
        if (Pole != null)
        {
            var polePosition = GetPositionRootSpace(Pole);
            for (int i = 1; i < Positions.Length - 1; i++)
            {
                var plane = new Plane(Positions[i + 1] - Positions[i - 1], Positions[i - 1]);
                var projectedPole = plane.ClosestPointOnPlane(polePosition);
                var projectedBone = plane.ClosestPointOnPlane(Positions[i]);
                var angle = Vector3.SignedAngle(projectedBone - Positions[i - 1], projectedPole - Positions[i - 1], plane.normal);
                Positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (Positions[i] - Positions[i - 1]) + Positions[i - 1];
            }
        }

        //set position & rotation
        for (int i = 0; i < Positions.Length; i++)
        {
            if (i == Positions.Length - 1)
                SetRotationRootSpace(Bones[i], Quaternion.Inverse(targetRotation) * StartRotationTarget * Quaternion.Inverse(StartRotationBone[i]));
            else
                SetRotationRootSpace(Bones[i], Quaternion.FromToRotation(StartDirectionSucc[i], Positions[i + 1] - Positions[i]) * Quaternion.Inverse(StartRotationBone[i]));
            SetPositionRootSpace(Bones[i], Positions[i]);
        }
    }

    private Vector3 GetPositionRootSpace(Transform current)
    {
        if (Root == null)
            return current.position;
        else
            return Quaternion.Inverse(Root.rotation) * (current.position - Root.position);
    }

    private void SetPositionRootSpace(Transform current, Vector3 position)
    {
        if (Root == null)
            current.position = position;
        else
            current.position = Root.rotation * position + Root.position;
    }

    private Quaternion GetRotationRootSpace(Transform current)
    {
        //inverse(after) * before => rot: before -> after
        if (Root == null)
            return current.rotation;
        else
            return Quaternion.Inverse(current.rotation) * Root.rotation;
    }

    private void SetRotationRootSpace(Transform current, Quaternion rotation)
    {
        if (Root == null)
            current.rotation = rotation;
        else
            current.rotation = Root.rotation * rotation;
    }

}