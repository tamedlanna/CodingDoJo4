using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CodingDoJo4.Connection
{
    class Client
    {

        byte[] buffer = new byte[152];
        Socket clientsocket;
        Action<string> MessageInformer;
        Action AbortInformer;

        public Client(string ip, int port, Action<string> messageInformer, Action abortInformer) {
            try {
                this.AbortInformer = abortInformer;
                this.MessageInformer = messageInformer;
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(ip), port);
                clientsocket = client.Client;
                StartReceiving();
            }
            catch (Exception) {
                messageInformer("Server not ready");
                AbortInformer();
            }
        }

        private void StartReceiving()
        {
            Task.Factory.StartNew(Receiving);
        }

        private void Receiving()
        {
            string message = "";
            while (!message.Contains("@quit")){
                int length = clientsocket.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);
                MessageInformer(message);
            }
            Close();
        }

        public void Send(object p)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            clientsocket.Close();
            AbortInformer();
        }

        public void Send(string message) {
            if (clientsocket != null) {
                clientsocket.Send(Encoding.UTF8.GetBytes(message));
            }
        }
    }
}
