using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf;
using SocketGameProtocol;
using System.IO;


namespace SocketServer.Tools
{
    class Message
    {
        public static byte[] Serialize(MainPack pack)
        {
            return pack.ToByteArray();
        }

        public static MainPack Deserialize(byte[] data)
        {
            IMessage message = MainPack.Descriptor.Parser.ParseFrom(data);
            return message as MainPack;
        }
    }
}
