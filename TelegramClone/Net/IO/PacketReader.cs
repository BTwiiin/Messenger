using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TelegramClient.Net.IO
{
    class PacketReader : BinaryReader
    {
        NetworkStream _stream;
        public PacketReader(NetworkStream stream) : base(stream)
        {
            _stream = stream;
        }

        public string ReadMessage()
        {
            byte[] buffer = new byte[ReadInt32()];
            _stream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

    }
}
