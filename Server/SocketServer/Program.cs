using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Google.Protobuf;
using SocketGameProtocol;
using SocketServer.Tools;
using System.Reflection;
using SocketServer.Controller;

namespace SocketServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint point = new IPEndPoint(ip, 6666);
            server.Bind(point);
            server.Listen(10);

            Thread thread = new Thread(Listen);
            thread.IsBackground = true;
            thread.Start(server);
            Console.WriteLine("TCP服务已启动");

            Console.Read();
        }

        static void Listen(object o)
        {
            var server = o as Socket;
            while (true)
            {
                Socket client = server.Accept();
                var clientIpoint = client.RemoteEndPoint.ToString();
                Thread thread = new Thread(Recive);
                thread.IsBackground = true;
                thread.Start(client);
            }
        }

        static void Recive(object o)
        {
            var client = o as Socket;
            while (true)
            {
                byte[] buffer = new byte[1024 * 1024 * 2];
                var effective = client.Receive(buffer);
                byte[] b2 = new byte[effective];
                Array.Copy(buffer, 0, b2, 0, effective); // 把数据拷贝给b2
                if (effective == 0)
                {
                    break;
                }
                MainPack mainPack = Message.Deserialize(b2);
                
                client.Send(Message.Serialize(HandleRequest(mainPack)));

            }
        }
        private static MainPack HandleRequest(MainPack pack)
        {
            if (pack.Requestcode == RequestCode.UserControl)
            {
                UserControl userControl = new UserControl();

                MethodInfo Method = userControl.GetType().GetMethod(pack.Actioncode.ToString());
                MainPack mainPack = Method.Invoke(userControl, new object[] { pack }) as MainPack;
                mainPack.Requestcode = RequestCode.UserControl;
                return mainPack;
            }
            else if(pack.Requestcode == RequestCode.SongControl)
            {
                SongControl songControl = new SongControl();

                MethodInfo Method = songControl.GetType().GetMethod(pack.Actioncode.ToString());
                MainPack mainPack = Method.Invoke(songControl, new object[] { pack }) as MainPack;
                mainPack.Requestcode = RequestCode.SongControl;
                return mainPack;
            }
            else if (pack.Requestcode == RequestCode.GameResultControl)
            {
                GameResultControl gameResultControl = new GameResultControl();

                MethodInfo Method = gameResultControl.GetType().GetMethod(pack.Actioncode.ToString());
                MainPack mainPack = Method.Invoke(gameResultControl, new object[] { pack }) as MainPack;
                mainPack.Requestcode = RequestCode.GameResultControl;
                return mainPack;
            }
            else if(pack.Requestcode == RequestCode.ChallengeControl)
            {
                ChallengeControl challengeControl = new ChallengeControl();
                MethodInfo Method = challengeControl.GetType().GetMethod(pack.Actioncode.ToString());
                MainPack mainPack = Method.Invoke(challengeControl, new object[] { pack }) as MainPack;
                mainPack.Requestcode = RequestCode.ChallengeControl;
                //Console.WriteLine(mainPack.Requestcode);
                return mainPack;
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
                return pack;
            }
            
            

        }
    }
}