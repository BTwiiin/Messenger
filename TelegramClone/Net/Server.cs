using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TelegramClient.Net.IO;

namespace TelegramClient.Net
{
    class Server
    {
        TcpClient _tcpClient;

        public PacketReader _packetReader;

        public event Action connectionEvent;
        public event Action msgEvent;
        public event Action disconnectionEvent;

        public Server()
        {
            _tcpClient = new TcpClient();
        }
        
        public void ConnectToServer(string username)     
        {
            
            if (!_tcpClient.Connected) 
            {
                _tcpClient.Connect("127.0.0.1", 7891);
                _packetReader = new PacketReader(_tcpClient.GetStream());
               
                if (!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteMessage(username);
                    _tcpClient.Client.Send(connectPacket.GetPacketSize());
                }
                ReadPackets();
            }
        }

        private void ReadPackets() 
        {
            Task.Run(() =>
            {
                while(true)
                {
                    var opcode = _packetReader.ReadByte();
                    switch(opcode)
                    {
                        case 1:
                            connectionEvent?.Invoke();
                            break;
                        case 5:
                            msgEvent?.Invoke();
                            break;
                        case 10:
                            Console.WriteLine("Opcode 10 Server.cs");
                            disconnectionEvent?.Invoke();
                            break;
                        default: break;
                    }
                }
            });
        }

        public void SendMessage(string msg)
        {
            var sendingPacket = new PacketBuilder();
            sendingPacket.WriteOpCode(5);
            sendingPacket.WriteMessage(msg);
            _tcpClient.Client.Send(sendingPacket.GetPacketSize());
        }
    }
}
