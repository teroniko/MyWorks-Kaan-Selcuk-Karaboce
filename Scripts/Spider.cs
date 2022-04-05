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
    public LayerMask contactLayer = 1;

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
    public SphereCollider MouthCollider;

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

    int FreelookCameraYPos = 1;
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
    
  

}
