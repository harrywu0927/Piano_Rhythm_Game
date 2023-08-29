using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    class ResponseProcessor
    {
        public void Login()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                Debug.Log("Login Success");
                LoginPanel.isLogin = true;
                Client.userid = Client.mainPack.User.Userid;
            }
            else
            {
                Debug.Log("Login Failed");
            }
        }
        public void Signup()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                Debug.Log("Signup Success");
                SignupPanel.signuped = true;
            }
            else if(Client.mainPack.Returncode == ReturnCode.UserExists)
            {
                Debug.Log("User is already existed");
            }
            else
            {
                Debug.Log("Server exception,Signup failed");
            }
        }
        public void Logout()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                Debug.Log("Logout Success");
                MainMenu.logoutSuccess = true;
            }
            else
            {
                Debug.Log("Logout Failed");
            }
        }
        public void SearchSongs()
        {
            if(Client.mainPack.Returncode == ReturnCode.Succeed && Client.mainPack.Requestcode == RequestCode.SongControl)
            {
                //Debug.Log(Client.mainPack.Requestcode);
                MusicShop.getSearchResult = true;
            }
            else if (Client.mainPack.Returncode == ReturnCode.Succeed && Client.mainPack.Requestcode == RequestCode.UserControl)
            {
                SelectMusic.getSearchResult = true;
                MyChallenge.getSearchResult = true;
                Debug.Log(Client.mainPack.Requestcode);
            }
            else
            {
                Debug.Log("search fail");
            }
        }
        public void Buysongs()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                MusicShop.buySuccess = true;
                Debug.Log("Buy success");
            }
            else if(Client.mainPack.Returncode == ReturnCode.GoldcoinNotEnough)
            {
                Debug.Log("Goldcoins not enough");
            }
        }
        public void GameSettlement()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                GameResult.getResult = true;
                Debug.Log(Client.mainPack.Gameresultpack);
            }
            else
            {
                Debug.Log("Game settlement fail");
            }
        }
        public void SendChallenge()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                //UnityEditor.EditorUtility.DisplayDialog("", "发起挑战成功", "确认");
                Debug.Log("发起挑战成功");
            }
            else if(Client.mainPack.Returncode == ReturnCode.ChallengeTooMany)
            {
                //UnityEditor.EditorUtility.DisplayDialog("", "同时进行的挑战数过多!", "确认");
                Debug.Log("同时进行的挑战数过多!");
            }
            else if (Client.mainPack.Returncode == ReturnCode.UserNotExist)
            {
                //UnityEditor.EditorUtility.DisplayDialog("", "被挑战的ID不存在!请检查", "确认");
                Debug.Log("被挑战的ID不存在");
            }
        }
        public void GetAllChallenges()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                MyChallenge.getSearchResult = true;
                Debug.Log("搜索挑战成功");
            }
            else
            {
                Debug.Log("!");
            }
        }
        public void SearchChallenges()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                MyChallenge.getSearchResult = true;
                Debug.Log("搜索挑战成功");
            }
            else
            {
                Debug.Log("!");
            }
        }
        public void AcceptChallenge()
        {
            if(Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                MyChallenge.acceptSuccess = true;
                MyChallenge.currentChallengeId = Client.mainPack.Challengepack[0].ChallengeId;
                Debug.Log("应答成功");
            }
            else
            {
                Debug.Log("应答失败");
            }
        }
        public void CompleteChallenge()
        {

        }
        public void GetChallengeResult()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                MainMenu.getNotice = true;
                Debug.Log("获取成功");
            }
            else
            {
                Debug.Log("无新的通知");
            }
        }
    }
}
