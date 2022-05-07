using System;
using DarkRift.Server;
using DarkRift;
using Tags;

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
                if (message.Tag == (ushort)Models.Tags0.InteractionType)
                {
                    foreach (IClient client in ClientManager.GetAllClients())
                    {
                        if (client.ID != e.Client.ID)
                        {
                            client.SendMessage(message, SendMode.Reliable);


                        }
                    }

                }





            }
        }
    }
}

