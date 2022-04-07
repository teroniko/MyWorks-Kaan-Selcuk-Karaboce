using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_Camera : MonoBehaviour
{

    public GameObject Soldier;
    public GameObject Gaint;
    public GameObject Archer;
    public GameObject Selected;
    public GameObject TargetTower;
    public GameObject Arrow;
    public GameObject Ball;
    public GameObject CannonBall;
    public GameObject ArcherTower;
    public GameObject King;
    public GameObject Message;
    bool GaintMessage = false;
    //public static int CannonBallCount = 1;
    //public static int ArcherTowerCount = 1;

    //public int soldiercount = 20;
    //public int achercount = 20;
    //public int gaintcount = 3;

    //Prefabs:
    public Attackable SoldierAttackable;
    public Attackable ArcherAttackable;
    public Attackable GaintAttackable;
    public Attackable ArcherTowerAttackable;
    public Attackable CannonAttackable;
    public Attackable KingAttackable;

    //tower attackableı gir
    //health barı düzelt

    public bool newTarget = false;
    bool putting = false;
    bool firstPut = false;
    //public (Animator,ReferencedScript) Anim(Animator anim, ReferencedScript script)
    //{
    //    anim = anim.gameObject.transform.Find("erika_archer_bow_arrow@Running").GetComponent<Animator>();

    //    script = anim.gameObject.GetComponent<ReferencedScript>();
    //    anim.SetFloat("ShootMultiplier", script.at.attackSpeed / 8);
    //    return (anim, script);
    //}
    
    Attackable GetAttackable(float power, float speed, float life)
    {
        Attackable at = new Attackable();
        at.attackPower = power;
        at.attackSpeed = speed;
        at.life = life;
        at.firstLife = life;
        return at;
    }
    ObjectPool Warriors;
    ObjectPool Archers;
    ObjectPool Gaints;
    ObjectPool ArcherTowers;
    ObjectPool CannonBalls;
    ObjectPool Kings;

    public Attackable[] attackables;
    public int[] count;
    public int unitNumber = 0;
    public static int ObjectType = 1;//1soldier 2Tower
    public static bool JustGame = false;//true yapma
    public bool GameStarted = false;
    private void Awake()
    {
        firstPut = false;
        ObjectType = 1;
        attackables = new Attackable[3];
        count = new int[3];
        SceneManager.LoadScene("Level" + Strategy0.level, LoadSceneMode.Additive);
        for (int i = 0; i < count.Length; i++)
        {
            attackables[i] = new Attackable();
            attackables[i].attackPower = 0.1f;
            attackables[i].attackSpeed = 0.1f;
            attackables[i].life = 0.1f;
            attackables[i].firstLife = 0.1f;
            count[i] = 0;
        }
        

        
        SoldierAttackable = GetAttackable(1.0f, 5, 20);
        ArcherAttackable = GetAttackable(2f, 10, 10);
        GaintAttackable = GetAttackable(1.5f, 4, 50);

        CannonAttackable = GetAttackable(5, 7, 60);


        ArcherTowerAttackable = GetAttackable(5, 7, 20);
        KingAttackable = GetAttackable(0, 0, 40);
        Debug.Log("Attackables Got");

        //tower attackable değişmesin yani birbirinin aynı olsun

        if (!JustGame)
        {
            Debug.Log("Soldier's got from Strategy Scene");
            unitNumber = PlayerPrefs.GetInt("UnitNumber");

            for (int i = 0; i < /*attackables.Length*/3; i++)
            {
                
                attackables[i].attackPower = PlayerPrefs.GetFloat("AttackPower" + i);
                attackables[i].attackSpeed = PlayerPrefs.GetFloat("AttackSpeed" + i);
                attackables[i].life = PlayerPrefs.GetFloat("Life" + i);
                attackables[i].firstLife = PlayerPrefs.GetFloat("Life" + i);
                count[i] = PlayerPrefs.GetInt("Count" + i);
                //Debug.Log(i);
                //özellik yenilemeyi yap
            }

        }
        SoldierAttackable = GetAttackable(attackables[0].attackPower, attackables[0].attackSpeed, attackables[0].life);
        ArcherAttackable = GetAttackable(attackables[1].attackPower, attackables[1].attackSpeed*2.0f, attackables[1].life/2.0f);
        GaintAttackable = GetAttackable(attackables[2].attackPower, attackables[2].attackSpeed/3.0f, attackables[2].life*4.0f);
        
        //for (int i = 0; i < 5; i++)
        //{
        //    SetAttackable(i);
        //}





        //GetComponent<UI>().Soldier();
    }
    
    IEnumerator Start0()
    {
        Soldier = Resources.Load("Prefabs/Warrior", typeof(GameObject)) as GameObject;
        Archer = Resources.Load("Prefabs/Archer", typeof(GameObject)) as GameObject;
        Gaint = Resources.Load("Prefabs/Gaint", typeof(GameObject)) as GameObject;
        Arrow = Resources.Load("Prefabs/Sphere", typeof(GameObject)) as GameObject;
        CannonBall = Resources.Load("Prefabs/CannonBall", typeof(GameObject)) as GameObject;
        ArcherTower = Resources.Load("Prefabs/ArrowMaker", typeof(GameObject)) as GameObject;
        Ball = Resources.Load("Prefabs/Ball", typeof(GameObject)) as GameObject;
        King = Resources.Load("Prefabs/King", typeof(GameObject)) as GameObject;
        Debug.Log("Prefabs Taked");
        yield return new WaitForSeconds(1);
        Warriors = new ObjectPool(Soldier, SoldierAttackable, count[0]);
        yield return new WaitForSeconds(0.1f * count[0]);
        Archers = new ObjectPool(Archer, ArcherAttackable, count[1]);
        yield return new WaitForSeconds(0.1f * count[1]);
        Gaints = new ObjectPool(Gaint, GaintAttackable, count[2]);
        yield return new WaitForSeconds(0.1f * count[2]);
        ObjectType = 2;
        Debug.Log(SoldierAttackable.attackPower);
        GameObject[] ArcherTowersPlace = GameObject.FindGameObjectsWithTag("Archer Tower");
        GameObject[] CannonPlace = GameObject.FindGameObjectsWithTag("Cannon Ball");
        GameObject[] KingPlace = GameObject.FindGameObjectsWithTag("King");
        CannonBalls = new ObjectPool(CannonBall, CannonAttackable, CannonPlace.Length);
        yield return new WaitForSeconds(0.5f);
        ArcherTowers = new ObjectPool(ArcherTower, ArcherTowerAttackable, ArcherTowersPlace.Length);
        Kings = new ObjectPool(King, KingAttackable, KingPlace.Length);
        Debug.Log("Prefabs Has Made");
        ObjectType = 2;
        yield return new WaitForSeconds(3);

        //GameObject[] TowersPlace = GameObject.FindGameObjectsWithTag("Towers Place");
        
        for (int i = 0; i < ArcherTowers.Objects.Count; i++)
        {
            //if (ArcherTowersPlace[i2].gameObject.name == "Archer Tower")
            {
                //ArcherTowersPlace[i2].gameObject.transform.position = ArcherTowersPlace[i2].transform.position;
                
                 

                //ArcherTower0.gameObject.transform.Find("Life").transform.Find("Canvas").transform.Find("Health Bar").gameObject.SetActive(true);
                ArcherTowers.ActivateObject(ArcherTowersPlace[i].transform.position);

                yield return new WaitForSeconds(0.5f);
                ArcherTowers.Objects[i].transform.Find("Tower").GetComponent<Target>().self =GetAttackable(ArcherTowerAttackable.attackPower, ArcherTowerAttackable.attackSpeed, ArcherTowerAttackable.life);
                ArcherTowers.Objects[i].name = ArcherTowers.Objects[i].name + i;
                ArcherTowers.Objects[i].transform.position = new Vector3(ArcherTowersPlace[i].transform.position.x,0.86f, ArcherTowersPlace[i].transform.position.z);
                
                Destroy(ArcherTowersPlace[i].gameObject);

            }

        }



        
        for (int i = 0; i < CannonBalls.Objects.Count; i++)
        {
            //if (ArcherTowersPlace[i2].gameObject.name == "Archer Tower")
            {
                //ArcherTowersPlace[i2].gameObject.transform.position = ArcherTowersPlace[i2].transform.position;
                
                Vector3 position = CannonPlace[i].transform.position;
                Destroy(CannonPlace[i]);

                //ArcherTower0.gameObject.transform.Find("Life").transform.Find("Canvas").transform.Find("Health Bar").gameObject.SetActive(true);

                CannonBalls.ActivateObject(new Vector3(position.x, 0.86f, position.z));

                yield return new WaitForSeconds(0.5f);
                CannonBalls.Objects[i].transform.Find("Tower").GetComponent<Target>().self=GetAttackable(CannonAttackable.attackPower, CannonAttackable.attackSpeed, CannonAttackable.life);
                CannonBalls.Objects[i].name = CannonBalls.Objects[i].name + i;
                

            }

        }


        for (int i = 0; i < Kings.Objects.Count; i++)
        {
            //if (ArcherTowersPlace[i2].gameObject.name == "Archer Tower")
            {
                //ArcherTowersPlace[i2].gameObject.transform.position = ArcherTowersPlace[i2].transform.position;

                Vector3 position = KingPlace[i].transform.position;
                Destroy(KingPlace[i]);

                //ArcherTower0.gameObject.transform.Find("Life").transform.Find("Canvas").transform.Find("Health Bar").gameObject.SetActive(true);

                Kings.ActivateObject(new Vector3(position.x, 0.86f, position.z));

                yield return new WaitForSeconds(0.5f);
                Kings.Objects[i].transform.Find("Tower").GetComponent<Target>().self = GetAttackable(KingAttackable.attackPower, KingAttackable.attackSpeed, KingAttackable.life);
                Kings.Objects[i].name = Kings.Objects[i].name + i;


            }

        }

        Message = GameObject.FindGameObjectWithTag("Message");
        GameStarted = true;
        Debug.Log("Towers is placed");

    }
    public void PointerEnter()
    {
        putting = true;
        StartCoroutine(Put0());
    }
    public void PointerExit()
    {
        putting = false;
    }
    private void Start()
    {
        TargetTower = null;

        StartCoroutine(Start0());

        //sırayla yap:
        //warrior spawnlanırken yana dönük spawnlanıyor doğru hedefe dönerek spawnlansın
        //saldırırken her vuruş doğru zamanda olmuyor

        //yapıldı:
        //perfect block click
        



    }
    
    void Update()
    {
        if (GameStarted&&(GameObject.FindGameObjectsWithTag("King").Length == 0|| firstPut&& GameObject.FindGameObjectsWithTag("Soldier").Length == 0&& GameObject.FindGameObjectsWithTag("King").Length == 0))
        {
            GameStarted = false;
            Message.GetComponent<Text>().text = "Level Completed";
            StartCoroutine(WaitAndLoadMenu());
        }
    }
    IEnumerator WaitAndLoadMenu()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public IEnumerator Put0()
    {
        firstPut = true;
        while (putting)
        {
            Put();
            yield return new WaitForSeconds(0.2f);
            
            
        }
        
    }
    public void Put()
    {
        RaycastHit hit;
        //Vector3 v = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        Vector3 v = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z)).direction;
        
        Ray rayToCameraPos = new Ray(Camera.main.transform.position, transform.position- Camera.main.transform.position);
        Debug.Log(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        //RaycastHit hit/* = new RaycastHit()*/;
        //bool put = true;
        //Physics.Raycast(ray, out hit)
        //Physics.Raycast(ray, out hit,1000, 1 << 8)
        int plane = -1;
        Debug.Log(v);
        //Camera.main.ScreenToWorldPoint(Input.mousePosition)
        RaycastHit[] hits = Physics.RaycastAll(transform.position - v * 500, v, 5000);
        for (int i = hits.Length - 1; i >= 0; i--)
        {
            Debug.Log(hits[i].transform.gameObject.name);
            if (hits[i].transform.gameObject.tag == "Tower")
            {
                Debug.Log("tower");
                TargetTower = hits[i].transform.gameObject;
                i = -1;
                break;
            }
            if (hits[i].transform.name == "Plane")
            {
                plane = i;
            }
            else if (hits[i].transform.gameObject.name == "Block Click")
            {
                plane = -1;
                break;
            }
            //switch (hits[i].transform.gameObject.name)
            //{
            //    case "Plane":

            //        Debug.Log("Plane");

            //        break;

            //    case "Block Click":
            //        plane = -1;
            //        i = -2;
            //        Debug.Log("Block Click");
            //        break;
            //}

        }
        //int layerMask = 1 << 11;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;

        //RaycastHit hit;
        //// Does the ray intersect any objects excluding the player layer
        //if (Physics.Raycast(transform.position, v, out hit, Mathf.Infinity, layerMask))
        //{
        //    TargetTower = hit.transform.gameObject;
        //    Debug.DrawRay(transform.position, v * hit.distance, Color.yellow);
        //    Debug.Log("Did Hit");
        //    Debug.Log(hit.transform.name);
        //}
        //else
        //{
        //    Debug.DrawRay(transform.position, v * 1000, Color.white);
        //    Debug.Log("Did not Hit");
        //}
        if (plane != -1 && hits[plane].point.y < 0.5f && hits[plane].point.y > -0.5f && hits[plane].transform.gameObject.tag == "Plane")
        {


            switch (unitNumber)
            {
                case 0:
                    if (Warriors.objectnumber < Warriors.objectcount)
                    {
                        GameObject Warrior = Warriors.ActivateObject(hits[plane].point).transform.Find("Soldier").gameObject;
                        Warrior.GetComponent<Target>().self.Reset();
                        Warrior.GetComponent<Target>().MainCamera0 = gameObject;
                        Warrior.GetComponent<PathFind>().FindClosestEdge();
                        //Warrior.SetActive(true);
                        //Warrior.SetActive(false);
                    }

                    break;
                case 1:
                    if (Archers.objectnumber < Archers.objectcount)
                    {
                        GameObject Archer = Archers.ActivateObject(hits[plane].point).transform.Find("Soldier").gameObject;
                        Archer.GetComponent<Target>().self.Reset();
                        Archer.GetComponent<Target>().MainCamera0 = gameObject;
                        Archer.GetComponent<PathFind>().FindClosestEdge();
                        //Archer.SetActive(true);
                        //Archer.SetActive(false);
                    }

                    break;
                case 2:
                    if (Gaints.objectnumber < Gaints.objectcount)
                    {
                        GameObject Gaint = Gaints.ActivateObject(hits[plane].point).transform.Find("Soldier").gameObject;
                        Gaint.GetComponent<Target>().self.Reset();
                        Gaint.GetComponent<PathFind>().MainCamera0 = gameObject;

                        if (!GaintMessage)
                        {
                            Message.GetComponent<Text>().text = "Click a Tower";
                            StartCoroutine(Message0());
                            GaintMessage = true;
                        }


                        //if (TargetTower != null)
                        //{
                        //    Gaint.GetComponent<PathFind>().TargetTower = TargetTower;
                        //    Gaint.GetComponent<PathFind>().OwnPath();
                        //}

                        //Gaint.SetActive(true);
                        //Gaint.SetActive(false);
                        //StartCoroutine(OwnPath(Gaint));
                    }
                    break;
            }


            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Vector3 v = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
            ////RaycastHit hit/* = new RaycastHit()*/;
            ////bool put = true;
            ////Physics.Raycast(ray, out hit)
            ////Physics.Raycast(ray, out hit,1000, 1 << 8)
            //int plane = -1;
            //RaycastHit[] hits = Physics.RaycastAll(transform.position, v, 500);
            //for (int i = hits.Length - 1; i > 0; i--)
            //{
            //    if (hits[i].transform.gameObject.tag == "Tower")
            //    {
            //        TargetTower = hits[i].transform.gameObject;
            //        i = -1;
            //        break;
            //    }
            //    switch (hits[i].transform.gameObject.name)
            //    {
            //        case "Plane":
            //            plane = i;

            //            break;

            //        case "Block Click":
            //            plane = -1;
            //            i = -1;
            //            Debug.Log("Block Click");
            //            break;
            //    }

            //}
            //if (plane != -1 && hits[plane].point.y < 0.5f && hits[plane].point.y > -0.5f && hits[plane].transform.gameObject.tag == "Plane")
            //{


            //    switch (unitNumber)
            //    {
            //        case 0:
            //            if (Warriors.objectnumber < Warriors.objectcount)
            //            {
            //                GameObject Warrior = Warriors.ActivateObject(hits[plane].point).transform.Find("Soldier").gameObject;
            //                Warrior.GetComponent<Target>().self.Reset();
            //                Warrior.GetComponent<Target>().MainCamera0 = gameObject;
            //                Warrior.GetComponent<PathFind>().FindClosestEdge();
            //                //Warrior.SetActive(true);
            //                //Warrior.SetActive(false);
            //            }

            //            break;
            //        case 1:
            //            if (Archers.objectnumber < Archers.objectcount)
            //            {
            //                GameObject Archer = Archers.ActivateObject(hits[plane].point).transform.Find("Soldier").gameObject;
            //                Archer.GetComponent<Target>().self.Reset();
            //                Archer.GetComponent<Target>().MainCamera0 = gameObject;
            //                Archer.GetComponent<PathFind>().FindClosestEdge();
            //                //Archer.SetActive(true);
            //                //Archer.SetActive(false);
            //            }

            //            break;
            //        case 2:
            //            if (Gaints.objectnumber < Gaints.objectcount)
            //            {
            //                GameObject Gaint = Gaints.ActivateObject(hits[plane].point).transform.Find("Soldier").gameObject;
            //                Gaint.GetComponent<Target>().self.Reset();
            //                Gaint.GetComponent<PathFind>().MainCamera0 = gameObject;
            //                //if (TargetTower != null)
            //                //{
            //                //    Gaint.GetComponent<PathFind>().TargetTower = TargetTower;
            //                //    Gaint.GetComponent<PathFind>().OwnPath();
            //                //}

            //                //Gaint.SetActive(true);
            //                //Gaint.SetActive(false);
            //                //StartCoroutine(OwnPath(Gaint));
            //            }
            //            break;
            //    }


            //*devin radiusu ayarla
            //*arrowa destroy yazmışım o pooling olcak
            //*soldier geliyormu kontrol et
            //*2 soldier bir kuleye vururken kule öldükten sonra bir soldier sıradaki objeyi seçemiyor
            //*archer hedef alamıyor
            //*archer hedef alınca kulenin canı azalmıyor
            //*2 dev aynı kuleye dalarken kule yok olunca 1 dev yeni hedefi belirleyemiyor
            //*ilk başta hata targettower seçili olmadığı için dev basarken hata veriyor
            //yazılımı temizlemeyi unutma




            //towers pooling
            //*devler towerların çok yakınına bırakılmıyor basılan ve basılmayan plane yap
            //towerlar en yakındaki adama ateş edecekler
            //Main Menuyu yap
            //basılı tutunca asker spawnlansın
            //*2 dev aynı anda 2 hedefe saldıramıyor
            //dev hedef almadan önce maincamera.targettower null olduğu için hata veriyor
            //*archer hedef seçerken arada kalıp yanlış hedefe gidebiliyor
            //*nav mesh agent hedefe gelmeden yavaşlıyor sonra hedefe geliyor aslında hızlarını aynı yapabilirim
            //koşma hızını ayarla
            //*archer saldırdığı anda kulenin canı düşüyor ok ona ulaştıktan sonra canının azalması lazım
            //*canlar doğru yere baksınlar ve 
            //hepsini tek canvastan yap
            //sphere ler kalıyor archer gidince
            //*okçular birden fazla olunca kule yok olduktan sonra kuleye takılı kalıyorlar


            //Duvar yap
            //*towerlar bir soldierı öldürdüğünde diğer soldierların targettowerları null oluyor
            //healthbarı ortala
            //*archerın okları bazen hedefe ulaşmıyor yere düşüp yok oluyor bunun nedeni virgüllü sayıların unityde yuvarlandığından olabilir

            //kulenin içerisindeki colliderdanda yararlanabilirim
            //navmeshmodifier ile de seçim yapılıyor olabilir
            //navmesh modifier ile ilgili video izle yararlımı bak
            //devdeki navmesh agent methodunu archer ve soldiera uygulayabilirsin
            //diğer colliderlarada değmemesi gerekiyor
        }

    }
    IEnumerator Message0()
    {
        yield return new WaitForSeconds(0.9f);
        Message.GetComponent<Text>().text = "";
    }
    

    
    //IEnumerator OwnPath(GameObject g)
    //{
    //    yield return new WaitForSeconds(1);
    //    g=OwnPath0(g);
    //}
    //GameObject OwnPath0(GameObject g)
    //{


    //    return g;
    //}
}
