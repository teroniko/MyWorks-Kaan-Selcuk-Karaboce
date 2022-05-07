using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    //These trees placed like hexagons to the TreeGenerator's middle
    public int treeHorizontal;
    public int treeVertical;
    public int playerCount;
    public Terrain Ter;
    public GameObject LogPiece;
    public GameObject SphereLog;
    public GameObject CapsuleLog;
    public int logPieceCount;
    int treeDividingLimit = 12;
    GameObject Tree;
    //Every tree has 2 bug
    void Start()
    {
        treeHorizontal = (int)Mathf.Sqrt(playerCount);
        treeVertical = (int)Mathf.Sqrt(playerCount);

        //CreateBranch("Tree", transform.position);
        


        //CreateTreeNodes(true, transform.position, 0, 0,10);

        Tree = new GameObject("Tree");
        Tree.transform.position = new Vector3(transform.position.x, Ter.SampleHeight(transform.position), transform.position.z);
        StartCoroutine("CreateTreeNodes", new object[5] { true, transform.position, Quaternion.identity, 0f, 0f });

        //diğer ağaçlara değmeyecek şekilde bükülmesi lazım
        //yani başka obje olmayana kadar döndür hiç boş yer yoksa iptal et

        DontDestroyOnLoad(Tree);


        //for (int i = 0; i < treeHorizontal; i++)
        //{
        //    for(int i2 = 0; i2 < treeVertical; i2++)
        //    {

        //        SpawnPos = transform.position + new Vector3(4*i, Ter.SampleHeight(SpawnPos) - 1.5f, 4*i2);

        //    }

        //}


        //float ratio = 7;
        //if (logHeight < 1.3f)
        //{
        //    ratio = 5;
        //}
        //x=logHeight * ratio / 4f



        //SpawnPos += new Vector3(0/*logHeight * Mathf.Cos(90 - turnPossibilityAngle)*/, /*Mathf.Sin(turnPossibilityDirection)* */(logHeight * ratio / 4f), -Mathf.Sin(turnPossibilityDirection)* (logHeight * ratio / 4f));
    }
    List<Vector3> BranchDirections = new List<Vector3>();
    List<Quaternion> BranchRotations = new List<Quaternion>();
    GameObject Oldg1;
    GameObject Oldg2;
    GameObject OldNode;
    Quaternion OldBranchRotation = Quaternion.identity;
    int RotationReset = 0;
    bool reset = false;
    IEnumerator CreateTreeNodes(object[] parms)
    {
        
        bool type/*true:tree,false:branch*/= (bool)parms[0];
        Vector3 Pos = (Vector3)parms[1];
        
        Quaternion Rotation = (Quaternion)parms[2];
        float treeWidth = (float)parms[3]; 
        float logHeight = (float)parms[4];
        
        GameObject Node = new GameObject("TreeNode");
        Node.transform.parent = Tree.transform;
        Node.transform.position = Pos;
        float turnAngleZ = 0;
        float turnAngleX = 0;
        float DisBetweenBranches=0;
        float AngleBetweenbranches = 0;

        int treeDividing;

        Vector2 RandomRot;
        float treeLength = 0;
        float newBrachingDistance;
        

        Vector3 BranchDirection;
        Quaternion BranchRotation= Quaternion.identity;
        if (type)
        {
            Pos = new Vector3(Pos.x, Ter.SampleHeight(Pos) - 2, Pos.z);
            Node.transform.position = Pos;

            treeWidth = Random.Range(8f, 12f);
            logHeight = 9;

            newBrachingDistance = 8;//6.5  yeni dal olmasına başlayan nokta
            treeDividing = Random.Range(4, 9);
            treeDividing = 5;
            BranchDirection = Vector3.forward;

        }
        else
        {
            treeDividing = /*Random.Range(1, 4)*/4;

            newBrachingDistance = 4;

            AngleBetweenbranches = 55;//2 katı gibi bir değer yazıyor
            
            BranchDirection = Vector3.forward;
            DisBetweenBranches = Random.Range(5, 10f);

            ////collide ediyormu onu ölç contact pointten yap

            //if (BranchDirections.Count >=2)
            //{
            //    BranchDirection = Quaternion.AngleAxis(AngleBetweenbranches, Vector3.up) * BranchDirections[1];
            //}
            //else if (BranchDirections.Count == 1)
            //{
            //    BranchDirection = Quaternion.AngleAxis(AngleBetweenbranches, Vector3.up) * BranchDirections[0];

            //}



            //if (BranchRotations.Count == 1)
            //{
            //    BranchRotation = Quaternion.Euler(Vector3.right * 30);
            //}
            //else if(BranchRotations.Count >= 2)
            //{
            //    BranchRotation = BranchRotations[BranchRotations.Count - 1]* Quaternion.Euler(Vector3.right * 30);
            //}
            //if (!reset)
            //{
            //    reset = true;
            //    OldBranchRotation = Quaternion.identity;
            //    BranchRotation = OldBranchRotation;
            //}
            //else
            //{
            //    BranchRotation = OldBranchRotation * Quaternion.Euler(Vector3.right * 30);
            //}
            

            switch (RotationReset)
            {
                case 0: BranchRotation = Rotation; break;
                case 1: BranchRotation = Rotation * Quaternion.Euler(Vector3.right * AngleBetweenbranches); break;
                case 2: BranchRotation = Rotation * Quaternion.Euler(Vector3.left * AngleBetweenbranches); break;
                case 3: BranchRotation = Rotation * Quaternion.Euler(Vector3.forward * AngleBetweenbranches); break;
                case 4: BranchRotation = Rotation * Quaternion.Euler(Vector3.back * AngleBetweenbranches); break;
            }
            RotationReset++;
            //for (int i = 0; i < BranchDirections.Count - 1; i++)
            //{
            //    BranchDirection = BranchDirections[i - 1];
            //}
            //lookat kullanmalı mıyım?
            //***********bir dalı diğeri ekseninde döndürmem için onun childı yapmam gerekiyor************
        }
        treeWidth -= 1.5f;
        
        while (true)
        {
            yield return new WaitForSeconds(0.1f);//0.05f
            OldNode = null;

            GameObject g = Instantiate(LogPiece, Pos, Quaternion.identity,Node.transform);
            g.transform.localScale = new Vector3(treeWidth, logHeight, treeWidth);
            if (type)
            {
                g.transform.Translate(Vector3.down * 4);
            }
            //g.transform.LookAt(Pos + new Vector3(Mathf.Abs(RandomRot.x), Mathf.Abs(RandomRot.y), Mathf.Abs(RandomRot.z)));
            //g.transform.LookAt(Pos + BranchDirection);
            g.transform.rotation = BranchRotation;
            
            //g.transform.rotation = Quaternion.AngleAxis(30,Vector3.right);
            //g.transform.LookAt(Pos + Random.onUnitSphere);
            Pos = g.transform.Find("Top").position;
            //if (Oldg1 != null)
            //{
            //    Oldg1.transform.parent = g.transform;
            //}
            //Oldg1 = g;

            GameObject g2 = Instantiate(SphereLog, Pos, Quaternion.identity, Node.transform);
            g2.transform.localScale = new Vector3(treeWidth, treeWidth, treeWidth);
            treeLength += logHeight;
            //if (Oldg2 != null)
            //{
            //    Oldg2.transform.parent = g2.transform;
            //}
            //Oldg2 = g2;
            //logHeight -= 0.005f;


            if (treeLength > newBrachingDistance || treeWidth < 1.5f)
            {
                if (treeDividingLimit > 0)
                {
                    //if (type)
                    //{
                    //    treeWidth = treeWidth / 1.5f;
                    //}
                    //else
                    //{
                    //    treeWidth = treeWidth / 2.5f;
                    //}
                    //treeWidth = treeWidth / (1+Random.Range(0.1f,0.9f));
                    
                    for (int i4 = 0; i4 < treeDividing; i4++)
                    {
                        yield return new WaitForSeconds(0.05f);
                        //if (i4!=0)
                        //{
                        //    Node.transform.parent = OldNode.transform;

                        //}
                        //if (OldNode != null&&i4!=0)
                        //{
                        //    Node.transform.parent = OldNode.transform;
                        //}

                        //OldNode = Node.gameObject;



                        //Debug.Log(i4);
                        treeDividingLimit--;
                        //Debug.Log("treeWidth: " + treeWidth);
                        
                        StartCoroutine("CreateTreeNodes", new object[5] { false, Pos, BranchRotation, treeWidth, logHeight});
                        //OldBranchRotation = BranchRotation;
                        //BranchDirections.Add(BranchDirection);
                        //BranchRotations.Add(BranchRotation);
                        //Debug.Log(BranchDirections.Count);
                        //Debug.Log(BranchRotations.Count);
                    }
                    RotationReset = 0;
                    //reset = false;
                    //BranchRotations = new List<Quaternion>();
                    //BranchDirections = new List<Vector3>();
                }
                //OldNode = null;
                break;
            }
        }
    }
    //void CreateBranch(string Name,Vector3 BranchPos)
    //{
    //    GameObject Tree = new GameObject(Name);
    //    switch (Name)
    //    {
    //        case "Tree":
    //            BranchPos = new Vector3(BranchPos.x, Ter.SampleHeight(BranchPos) - 2, BranchPos.z);
    //            Tree.transform.position = BranchPos;
    //            break;
    //    }
    //    Debug.Log("height : " + Ter.SampleHeight(BranchPos));
    //    Debug.Log("logpiece.y : " + LogPiece.transform.Find("GameObject").GetComponent<Renderer>().bounds.size.y);

        

    //    float treeWidth = Random.Range(2.8f, 5.8f);
    //    float logHeight = 2f;
    //    int treeDividing = 0;


    //    float turnAngleZ = 0;
    //    float turnAngleX = 0;
    //    float treeLength = 0;

    //    Vector3 DividingPoint = Vector3.zero;

    //    for (int i3 = 0; i3 < logPieceCount; i3++)
    //    {

    //        //if (Random.Range(0, 6) == 0)
    //        //{
    //        //    turnAngleZ = Random.Range(-30f, 30);
    //        //    turnAngleX = Random.Range(-30f, 30);
    //        //}
            
    //        Debug.Log("d : "+ i3);
    //        GameObject g = Instantiate(LogPiece, BranchPos, Quaternion.identity, Tree.transform);
    //        g.transform.localScale = new Vector3(treeWidth, logHeight, treeWidth);

    //        turnAngleZ = Random.Range(-30f, 30);
    //        turnAngleX = Random.Range(-30f, 30);
            
    //        g.transform.localEulerAngles = new Vector3(turnAngleX, 0, turnAngleZ);

    //        BranchPos = g.transform.Find("Top").position;

    //        GameObject g2 = Instantiate(SphereLog, BranchPos, Quaternion.identity, Tree.transform);
    //        g2.transform.localScale = new Vector3(treeWidth, treeWidth, treeWidth);

    //        treeLength += logHeight;

    //        logHeight -= 0.05f;
    //        treeWidth -= 0.05f;
    //        if (treeLength > 5f)
    //        {
    //            treeDividing = 2;
    //            treeDividingLimit--;
    //            treeWidth /= treeDividing;
    //            if (Name == "Tree")
    //            {
    //                DividingPoint = BranchPos;
    //            }
    //            for (int i4 = 0; i4 < treeDividing && treeDividingLimit > 0; i4++)
    //            {
    //                Debug.Log("v");
    //                if (Name == "Branch")
    //                {
    //                    CreateBranch("Branch", BranchPos);
    //                }
    //                else
    //                {
    //                    CreateBranch("Branch", DividingPoint);
                        
    //                }
                    
    //            }
    //            break;


    //        }

    //        //if (treeWidth < 0.05f || logHeight < 0.05f)
    //        //{

    //        //    break;
    //        //}
    //        //log'u yamuk yapmak için generate mesh videoları izle(https://www.youtube.com/watch?v=eJEpeUH1EMg)
    //        //yada blenderdan yap*
    //    }
    //}
}
