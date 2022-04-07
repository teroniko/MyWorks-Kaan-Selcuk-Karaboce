using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift.Client.Unity;
using DarkRift;
using Tags;
using DarkRift.Client;
using UnityEngine.InputSystem;

public class NetworkManager : MonoBehaviour
{
    public UnityClient client;
    public GameObject Bug;
    public Transform[] PlayerBegin;
    public Light DirLight;

    public List<GameObject> Bugs;
    //server açýk deðilken baðlanýnca hata veriyor engellemeye çalýþýlabilir
    IEnumerator Connection()
    {
        for(int i = 0; i < 20; i++)
        {
            if (i >= 19)
            {
                Application.Quit();
            }
            if (client.ConnectionState == ConnectionState.Connecting)
            {
                
                client.MessageReceived += OnMessageReceived;
                break;
            }
            yield return new WaitForSeconds(1000);
        }
        
    }
    
   
    private void Awake()
    {

        //StartCoroutine(Connection());

        client.MessageReceived += OnMessageReceived;
        DontDestroyOnLoad(gameObject);
        //farklý oyuncular baðlandýðýnda onlarý ayný oyun üzerine koymam lazým ondan sonra pozisyon update
        //bir player giriþ yapýnca her playera o spawnlanýcak ve client.idleri doðru olacak

        //Chat chat =new Chat();
        //using (Message message = Message.Create((ushort)Models.Tags0.PlayerSpawn, chat))
        //{
        //    client.SendMessage(message, SendMode.Reliable);

        //}
        
    }
    
    private void OnDestroy()
    {

        client.MessageReceived -= OnMessageReceived;
    }
    public void AttackDefenceLegdeath(byte Condition)//legdeathi de içine koyunca sorun yaratýr mý? sanýrým hayýr
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(client.ID);
            writer.Write(Condition);

            using (Message message = Message.Create((ushort)Models.Tags0.AttackDefenceLegdeath, writer))
            {
                client.SendMessage(message, SendMode.Reliable);
            }
        }

    }
    public void PosRot(Vector3 Pos, Quaternion Rot, bool ThereIsPos)
    {
        XYZ Position=null;

        if (ThereIsPos)
        {
            Position = new XYZ();
            Position.X = Pos.x;
            Position.Y = Pos.y;
            Position.Z = Pos.z;
        }
        XYZ Rotation = new XYZ();
        Rotation.X = Rot.x;
        Rotation.Y = Rot.y;
        Rotation.Z = Rot.z;
        float RotW = Rot.w;




        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(client.ID);

            var tag = Models.Tags0.Rot;
            if (ThereIsPos)
            {
                tag = Models.Tags0.PosRot;
                writer.Write(Position);
            }

            writer.Write(Rotation);
            writer.Write(RotW);
            
            
            using (Message message = Message.Create((ushort)tag, writer))
            {
               
                client.SendMessage(message, SendMode.Unreliable);
                //Add more data here
            }



        }
        //Set fields here
        //Message playerMessage = Message.Create((ushort)Models.Tags0.Position, Position);
        
    }
    public int PlayerId;
    public ushort PlayerCount;
    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            using (DarkRiftReader reader = message.GetReader())
            {
                if (message.Tag == (ushort)Models.Tags0.AttackDefenceLegdeath)
                {
                    ushort Id = reader.ReadUInt16();
                    GameObject tarantula = Bugs[Id].transform.GetChild(0).gameObject;
                    byte ADL = reader.ReadByte();
                    switch (ADL)
                    {
                        case 0: tarantula.GetComponent<Spider>().Attack(); break;
                        case 1:
                            tarantula.GetComponent<Spider>().Defence(); break;
                        case 2:break;
                    }
                    

                }


                byte ThereIsPos=0;
                if(message.Tag == (ushort)Models.Tags0.PosRot)
                {
                    ThereIsPos = 1;


                }
                else if(message.Tag == (ushort)Models.Tags0.Rot)
                {
                    ThereIsPos = 2;

                }
                if (ThereIsPos != 0)
                {
                    ushort Id = reader.ReadUInt16();
                    
                    GameObject tarantula = Bugs[Id].transform.GetChild(0).gameObject;

                    if (ThereIsPos == 1)
                    {
                        XYZ Position = reader.ReadSerializable<XYZ>();
                        tarantula.transform.position = new Vector3(Position.X, Position.Y, Position.Z);

                    }
                        
                    
                    XYZ Rotation = reader.ReadSerializable<XYZ>();
                    float RotW = reader.ReadSingle();
                    tarantula.transform.rotation = new Quaternion(Rotation.X, Rotation.Y, Rotation.Z, RotW);





                    //tarantula.transform.rotation = new Quaternion(posrot.RotX, posrot.RotY, posrot.RotZ,posrot.RotW);
                    //Debug.Log("Id: " + posrot.Id);
                    //Debug.Log("Bugs.Count : "+ Bugs.Count);

                }

                //if (message.Tag == (ushort)Models.Tags0.Rot)
                //{
                //    PosRot posrot = reader.ReadSerializable<PosRot>();
                //    GameObject tarantula = Bugs[posrot.Id].transform.GetChild(0).gameObject;
                //    tarantula.transform.rotation = new Quaternion(posrot.RotX, posrot.RotY, posrot.RotZ, posrot.RotW);
                //    Debug.Log("Id: " + posrot.Id);
                //    Debug.Log("Bugs.Count : " + Bugs.Count);

                //}

                if (message.Tag == (ushort)Models.Tags0.NewPlayer)
                {
                    PlayerCount = (ushort)(reader.ReadUInt16() + 1);
                    //Debug.Log("PlayerCount : " + PlayerCount);
                    PlayerId = Bugs.Count;//Player No
                    //Debug.Log("PlayerId : " + PlayerId);
                    while (PlayerId < PlayerCount)
                    {

                        Debug.Log("Player Spawn" + PlayerId);

                        GameObject bug = Instantiate(Bug, PlayerBegin[PlayerId].position, Quaternion.identity, null);
                        
                        bug.name = "Bug" + PlayerId;
                        
                        Spider spider;
                        spider = bug.GetComponentInChildren<Spider>();
                        spider.nm = this;
                        spider.DirLight = DirLight;
                        //spider.transform.position = PlayerBegin[PlayerId].position;
                        //PosChange(spider.gameObject.transform.position);
                        if (client.ID != PlayerId)
                        {
                            //spider.enabled = false;
                            bug.transform.Find("Camera").gameObject.SetActive(false);
                            bug.transform.Find("CM FreeLook1").gameObject.SetActive(false);
                            bug.transform.Find("Camera Late").gameObject.SetActive(false);
                            bug.transform.Find("Canvas").gameObject.SetActive(false);
                            bug.GetComponentInChildren<PlayerInput>().enabled = false;
                        }
                        Bugs.Add(bug);
                        PlayerId++;
                    }
                }
            }
        }
    }
}



//using (Message message = Message.Create((ushort)Models.Tags0.Position, Position))
//{
//    client.SendMessage(message, SendMode.Unreliable);
//}





//Message playerMessage = Message.Create((ushort)Models.Tags0.Position, Position);




//using (DarkRiftWriter writer = new DarkRiftWriter())
//{
//    //Add data here
//    writer.Write(player);
//    //Add more data here
//}
//using (DarkRiftWriter writer = DarkRiftWriter.Create())
//{
//    writer.Write(Position);

//    using (Message message = Message.Create((ushort)Models.Tags0.Position, Position))
//    {
//        client.SendMessage(message, SendMode.Unreliable);
//    }
//}