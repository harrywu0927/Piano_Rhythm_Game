using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using SocketGameProtocol;
using Assets.Scripts;
using System.Threading;
using System.Reflection;
public class Client
{
    public static Socket socket;
    public static MainPack mainPack;
    public static int userid;

    public Client()
    {
        socket = GetSocketConnection.getSocket();
        Thread thread = new Thread(Recive);
        thread.IsBackground = true;
        thread.Start(socket);
    }
    public static void ClearPack()
    {
        mainPack.Actioncode = ActionCode.ActionNone;
        mainPack.Requestcode = RequestCode.UserControl;
        mainPack.Returncode = ReturnCode.ReturnNone;
        mainPack.User = null;
        mainPack.Songs.Clear();
        mainPack.Loginpack = null;
        mainPack.Gameresultpack = null;
    }
    static void Recive(object o)
    {
        var client = o as Socket;
        while (true)
        {
            byte[] buffer = new byte[1024 *1024* 2];
            var effective = client.Receive(buffer);
            byte[] b2 = new byte[effective];
            Array.Copy(buffer, 0, b2, 0, effective);
            if (effective == 0)
            {
                break;
            }
            mainPack = Message.Deserialize(b2);

            Debug.Log(mainPack);
            HandleResponse(mainPack);
        }
    }
    public static void HandleResponse(MainPack pack)
    {
        ResponseProcessor responseProcessor = new ResponseProcessor();
        MethodInfo Method = responseProcessor.GetType().GetMethod(pack.Actioncode.ToString());
        Method.Invoke(responseProcessor, new object[] {  });
    }
    public static void Send(MainPack pack)
    {
        pack.Returncode = ReturnCode.ReturnNone;
        socket.Send(Message.Serialize(pack));
    }
    public static void Close()
    {
        socket.Close();
        Debug.Log("连接断开");
    }

}


