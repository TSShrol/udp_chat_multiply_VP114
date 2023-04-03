using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server_udp
{
    public class ChatServer
    {
        private IPEndPoint clientEndPoint = null;
        private UdpClient server;
        private List<string> chatHistory = new List<string>();
        private HashSet<IPEndPoint> clients = new HashSet<IPEndPoint>();

        public ChatServer(short port) {
            server = new UdpClient(port);
        }

        public void Join(IPEndPoint endPoint) {
            clients.Add(endPoint);
        }

        public void QuitChat(IPEndPoint endPoint)
        {
            clients.Remove(endPoint);
        }

        public async void SendMessage(string message) {
            chatHistory.Add(message);
            byte[] data = Encoding.Unicode.GetBytes(message);
            foreach (var client in clients)
            {
              await  server.SendAsync(data, data.Length, client);
            }
        }

        public void StartServer() {
            while (true) {
                Console.WriteLine("Server run. Waiting for the requests...");
                var request = server.Receive(ref clientEndPoint);
                string message = Encoding.Unicode.GetString(request);
                Console.WriteLine($"Gota msg: {message} from {clientEndPoint} ");
                switch (message) {
                    case "<|join|>":
                        Join(clientEndPoint);
                        break;
                    case "<|quit|>":
                        QuitChat(clientEndPoint);
                        break;
                    default:
                        SendMessage($"{clientEndPoint.Address} {clientEndPoint.Port} ({DateTime.Now.ToShortDateString()}): {message}");
                        break;
                }
            }
        }

    }
}
