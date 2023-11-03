using System.Net.Sockets;
using TelegramServer.Net.IO;

namespace TelegramServer
{
    class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }
        PacketReader _packetReader;
        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(ClientSocket.GetStream());

            var opcode = _packetReader.ReadByte();
            Username = _packetReader.ReadMessage();

            Console.WriteLine($"{Username} has connected to the server");

            Task.Run(() => Process());
        }

        void Process()
        {
            while (true)
            {
                
                try
                {
                    var opcode = _packetReader.ReadByte();
                    Console.WriteLine($"Opcode {opcode}");
                    switch (opcode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"{msg}: received!");
                            Program.BroadcastMessage(msg);
                            break;
                        default:
                            break;
                    }
                }
                catch(Exception)
                {  
                    Console.WriteLine($"Sth weird");
                    Console.WriteLine($"Opcode");
                    Console.WriteLine($"{UID.ToString()} - Disconnected");
                    Console.WriteLine($"After Disconnected");
                    Program.BroadcastDisconnect(UID.ToString());
                    Console.WriteLine($"After Broadcast");
                    ClientSocket.Close();
                    Console.WriteLine($"After Close");
                    break;
                }
            }
        } 
    }
}
