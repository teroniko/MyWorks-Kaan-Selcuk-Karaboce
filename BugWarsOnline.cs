using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift.Server;
using DarkRift;
using Tags;
using System.Threading;

namespace Bug_Wars_Online
{
    public class BugWarsOnline : Plugin
    {
        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public BugWarsOnline(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            ClientManager.ClientConnected += OnClientConnected;
            ClientManager.ClientDisconnected += OnClientDisconnected;
        }
        
        //List<IClient> Players=new List<IClient>();
        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Console.WriteLine("Connected");

            

            e.Client.MessageReceived += OnMessageReceived;

            using (DarkRiftWriter writer = DarkRiftWriter.Create())
            {
                writer.Write(e.Client.ID);
                writer.Write(0);
                using (Message message = Message.Create((ushort)Models.Tags0.NewPlayer, writer))
                {

                    foreach (IClient client in ClientManager.GetAllClients())
                    {
                        //if (client.ID != e.Client.ID)
                        {

                            client.SendMessage(message, SendMode.Reliable);
                            Console.WriteLine("Player" + client.ID + " Connected");
                            Console.WriteLine("ClientManager.Count" + ClientManager.Count);

                        }
                    }
                }
            }
        }
        
        private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            Console.WriteLine("Disconnected");
            //destroy et!




        }
        
        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            //using (Message message = e.GetMessage())
            //{
            //    using (DarkRiftReader reader = message.GetReader())
            //    {
            //        ushort ID=reader.ReadUInt16();
            //        XYZ pos = reader.ReadSerializable<XYZ>();
            //        Console.WriteLine(pos.X);

            //    }


            //}
            using (Message message = e.GetMessage())
            {
                if (message.Tag == (ushort)Models.Tags0.PosRot|| message.Tag == (ushort)Models.Tags0.Rot)
                {
                    foreach (IClient client in ClientManager.GetAllClients())
                    {
                        if (client.ID != e.Client.ID)
                        {
                            client.SendMessage(message, SendMode.Unreliable);


                        }
                    }

                }
                if (message.Tag == (ushort)Models.Tags0.AttackDefenceLegdeath)
                {
                    foreach (IClient client in ClientManager.GetAllClients())
                    {
                        if (client.ID != e.Client.ID)
                        {
                            client.SendMessage(message, SendMode.Reliable);


                        }
                    }

                }





                //rot ve posrot'u birleştirip tek bir tag eklemek daha optimize olabilir



                //using (DarkRiftReader reader = message.GetReader())
                //{
                //    if (message.Tag == (ushort)Models.Tags0.Position)
                //    {

                //        foreach (IClient client in ClientManager.GetAllClients())
                //        {
                //            if (client.ID != e.Client.ID)
                //            {

                //                client.SendMessage(message, SendMode.Unreliable);
                //                Console.WriteLine("Server toke " + client.ID);


                //            }
                //        }
                //    }

                //}


            }
        }
    }
}

//string textMessage=reader.ReadString();
//Console.WriteLine(textMessage);






//using (DarkRiftReader reader = message.GetReader())
//{
//Chat chat = reader.ReadSerializable<Chat>();


//else if(message.Tag == (ushort)Models.Tags0.MessageSender)
//{
//    foreach (ushort Player in Players)
//    {
//        Console.WriteLine(Player);
//        //chat.chatMessage = "send message";
//        using (Message message0 = Message.Create((ushort)Models.Tags0.MessageTaker, chat))
//        {
//            e.Client.SendMessage(message0, SendMode.Reliable);//unreliable yap,dene

//        }
//    }
//}
//}





//foreach (IClient Player in Players)
//{
//    //using (Message message0 = Message.Create((ushort)Models.Tags0.MessageTaker, chat))
//    //{
//    //    Player.SendMessage(message0, SendMode.Unreliable);//unreliable yap,dene
//    //    //Console.WriteLine(Player);
//    //    Console.WriteLine(Player.ID);
//    //    Console.WriteLine(chat.PosX);

//    //}
//    using (Message message0 = Message.Create((ushort)Models.Tags0.PlayerSpawn, chat))
//    {
//        Player.SendMessage(message0, SendMode.Reliable);//unreliable yap,dene
//        //Console.WriteLine(Player);
//        Console.WriteLine(Player.ID);


//    }
//}



//chat.chatMessage = "take message";
//Console.WriteLine(chat.chatMessage);