using System.Net;
using System.Net.Sockets;
using System.Linq;
using TelegramServer.Net.IO;

namespace TelegramServer;

class Program
{
    static List<Client> _clients;
    static TcpListener? _listener;
    static void Main(string[] args)
    {
        _clients = new List<Client>();
        _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
        _listener.Start();

        while (true)
        {
            var client = new Client(_listener.AcceptTcpClient());
            _clients.Add(client);

            BroadcastConnection();
        }


    }
    
    static void BroadcastConnection()
    {
        foreach (var client in _clients)
        {
            foreach(var cl in _clients) 
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(1);
                broadcastPacket.WriteMessage(cl.Username);
                broadcastPacket.WriteMessage(cl.UID.ToString());
                client.ClientSocket.Client.Send(broadcastPacket.GetPacketSize());
            }
        }
    }

    public static void BroadcastMessage(string message)
    {
        foreach(var client in _clients)
        {
            var msgPacket = new PacketBuilder();
            msgPacket.WriteOpCode(5);
            msgPacket.WriteMessage(message);
            client.ClientSocket.Client.Send(msgPacket.GetPacketSize());
        }
    }

    public static void BroadcastDisconnect(string msg)
    {
        var disconnectedClient = _clients.Where(x => x.UID.ToString() == msg).FirstOrDefault();
        Console.WriteLine($"Here it is {msg}");
        _clients.Remove(disconnectedClient);
        foreach (var client in _clients)
        {
            int i = 1;
            Console.WriteLine($"{i} {client.Username}");
            i++;
        }
        foreach (var client in _clients)
        {
            try
            {
                Console.WriteLine("We are in broadcast dis");
                var disconnectPacket = new PacketBuilder();
                Console.WriteLine("We are in broadcast dis 1");
                disconnectPacket.WriteOpCode(10);
                Console.WriteLine("We are in broadcast dis 2");
                disconnectPacket.WriteMessage(msg);
                Console.WriteLine("We are in broadcast dis 3");
                client.ClientSocket.Client.Send(disconnectPacket.GetPacketSize());
                Console.WriteLine("We are in broadcast dis 4");
            } catch(Exception e) 
            {
                Console.WriteLine(e.ToString());
            }
        }
        Console.WriteLine("We are in broadcast dis 222");
        BroadcastMessage($"{disconnectedClient.Username} has disconnected");
    }
}
