//-----------------------------------------------------------
//File:   ServerCommunicationManager.cs
//Desc:   Listens for incoming connections and handles requests
//        from clients.
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using JoustModel;
using Newtonsoft.Json;

namespace JoustServer
{
    class ServerCommunicationManager
    {

        public const int PORT = 5500;
        private MainWindow window;
        private TcpListener listener;
        GameController ctrl = new GameController();

        public ServerCommunicationManager(MainWindow window)
        {
            this.window = window;
            listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();
        }

        public async Task<TcpClient> WaitForClientAsync()
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            return client;
        }

        public async Task HandleClientAsync(TcpClient tcpClient)
        {
            try
            {
                using (tcpClient)
                {
                    string clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
                    window.Log("Received connection request from " + clientEndPoint);

                    NetworkStream networkStream = tcpClient.GetStream();
                    StreamReader reader = new StreamReader(networkStream);
                    StreamWriter writer = new StreamWriter(networkStream);

                    string request = await reader.ReadLineAsync();
                    while (request != null)
                    {
                        window.Log("Received data: " + request);

                        string response = ProcessMessage(request);

                        window.Log("Transmitting data: " + response);
                        await writer.WriteLineAsync(response);
                        await writer.FlushAsync();
                        request = await reader.ReadLineAsync();
                    }
                }

                // Client closed connection
                window.Log("Client closed connection.");
            }
            catch (Exception ex)
            {
                window.Log(ex.Message);
            }
        }

        private string ProcessMessage(String requestMsgStr)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            RequestMessage requestMsg = JsonConvert.DeserializeObject(requestMsgStr, settings) as RequestMessage;
            ResponseMessage responseMsg = requestMsg.Execute(ctrl);
            return JsonConvert.SerializeObject(responseMsg, settings);
        }
    }
}
