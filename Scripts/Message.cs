using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocol;
using Google.Protobuf;

  public class Message
    {
        public static byte[] Serialize(MainPack pack)//序列化
        {
            return pack.ToByteArray();
        }

        public static MainPack Deserialize(byte[] data)//反序列化
        {
            IMessage message = MainPack.Descriptor.Parser.ParseFrom(data);
            return message as MainPack;
        }
    

}

