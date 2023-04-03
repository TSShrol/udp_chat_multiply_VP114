using System;

namespace server_udp
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatServer server = new ChatServer(8007);
            server.StartServer();
        }
    }
}
