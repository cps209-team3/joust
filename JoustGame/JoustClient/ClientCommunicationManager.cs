using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JoustModel;

namespace JoustClient
{
    public class ClientCommunicationManager
    {
        const int PORT = 5500;

        TcpClient tcpClient;
        StreamReader rd;
        StreamWriter wr;

        public ClientCommunicationManager()
        {
            tcpClient = new TcpClient();
        }

        public async Task ConnectToServerAsync(string host)
        {
            await tcpClient.ConnectAsync(host, PORT);
            rd = new StreamReader(tcpClient.GetStream());
            wr = new StreamWriter(tcpClient.GetStream());
        }

        public async Task<ResponseMessage> SendMessageAsync(RequestMessage msg)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string serializedMessage = JsonConvert.SerializeObject(msg, settings);
            await wr.WriteLineAsync(serializedMessage);
            await wr.FlushAsync();
            string response = await rd.ReadLineAsync();
            ResponseMessage responseMsg = JsonConvert.DeserializeObject(response, settings) as ResponseMessage;
            return responseMsg;

        }
    }
}
