using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift.Client.Unity;
using DarkRift;
using Tags;
using DarkRift.Client;
using UnityEngine.InputSystem;
using System.Net;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public static string IP;
    public UnityClient client;
    public GameObject Bug;
    public Transform[] PlayerBegin;
    public Light DirLight;

    public List<GameObject> Bugs;
    public Material OtherTarantulaMaterial;
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

        //IPv6 = Dns.GetHostEntry(Dns.GetHostName())
        //     .AddressList.First(
        //         f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
        //Debug.Log(IPv6);

        //var host = Dns.GetHostEntry(Dns.GetHostName());
        //foreach (var ip in host.AddressList)
        //{
        //    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        //    {
        //        IPv6= ip;
        //    }
        //}
        //Debug.Log(IPv6);
        //client.Connect(IPv6, client.Port, true);

        //client.Host = ServerIP.IP.ToString();
        //client.Connect(IPAddress.Parse(ServerIP.IP), client.Port, true);

        //DontDestroyOnLoad(gameObject);

        string ip = IP.Substring(0, IP.Length - 1);//static stringi algýlamýyor, böyle yapýnca "static"ten kurtuluyor

        try
        {
            client.Connect(ip, client.Port, true);
        }
        catch (Exception e)
        {
            ServerIP.WrongIP = true;
            Debug.Log("Disconnected");
            SceneManager.LoadScene("ServerIP");
        }
        finally { client.MessageReceived += OnMessageReceived; }
        
    }
  

    private void OnDestroy()
    {

        client.MessageReceived -= OnMessageReceived;
    }
    public void Interaction(byte Condition)//legdeathi de içine koyunca sorun yaratýr mý? sanýrým hayýr
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(client.ID);
            writer.Write(Condition);
            
            using (Message message = Message.Create((ushort)Models.Tags0.InteractionType, writer))
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
               
            }



        }
        
    }
    public int PlayerId;
    public ushort PlayerCount;
    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            using (DarkRiftReader reader = message.GetReader())
            {
                if (message.Tag == (ushort)Models.Tags0.InteractionType)
                {
                    ushort Id = reader.ReadUInt16();
                    GameObject tarantula = Bugs[Id].transform.GetChild(0).gameObject;
                    byte Interaction = reader.ReadByte();
                    Spider s = tarantula.GetComponent<Spider>();



                    switch (Interaction)
                    {
                        case 0: s.Attack(); break;
                        case 1:
                            s.DefenceGetBug();
                            break;
                        case 2:
                            s.DefenceOff();
                            break;
                        case 3:
                            s.Death();
                            break;
                        case 4:
                            s.LegsHealthDown();
                            break;
                        default:
                            s.LegHealthDown(s.Legs[Interaction - 5], 3);
                            break;
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





                }


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
                            spider.GetComponentInChildren<SkinnedMeshRenderer>().material = OtherTarantulaMaterial;
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
