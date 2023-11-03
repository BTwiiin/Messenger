using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramServer.Net.IO
{
    class PacketBuilder
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        // Writing operation codes for Client to help it interpret messages from Servcer side
        // it's in a byte form just to reduce the size of a packet that is sent to the Server side
        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg)
        {
            _ms.Write(BitConverter.GetBytes(msg.Length));
            _ms.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketSize()
        {
            return _ms.ToArray();
        }
    }
}
