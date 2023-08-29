using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;
using UnityEngine.SceneManagement;
using Assets.Scripts.UI;
using System;

public class MainMenu : MonoBehaviour
{
    public Button selectMusicBtn,challengeBtn,buyMusicBtn,logOutBtn,exitBtn;
    public Button mItemPrefab;

    public static bool logoutSuccess = false, getNotice = false;
    public Text welcomeBack, goldCoins, scores;
    private Transform mContentTransform;

    void Start()
    {
        mContentTransform = this.transform.Find("Scroll View/Viewport/Content");
        welcomeBack.text += Client.mainPack.User.Usrname;
        goldCoins.text += Client.mainPack.User.Goldcoins.ToString();
        scores.text += Client.mainPack.User.Scores.ToString();
        selectMusicBtn.onClick.AddListener(SelectMusicClick);
        //personalDataBtn.onClick.AddListener(PersonalDataClick);
        challengeBtn.onClick.AddListener(ChallengeClick);
        buyMusicBtn.onClick.AddListener(BuyMusicClick);
        logOutBtn.onClick.AddListener(LogOutClick);
        exitBtn.onClick.AddListener(ExitClick);
        StartCoroutine(CheckChallenge());
    }
    void ExitClick()
    {
        Client.Close();
        Application.Quit();
    }
    void ShowItems(MainPack pack)
    {
        DestroyNoticeList();
        Debug.Log(pack.ChallengeNoticePack.Count);
        for (int i = 0; i < pack.ChallengeNoticePack.Count; i++)
        {
            Text[] texts = mItemPrefab.GetComponentsInChildren<Text>();
            DateTime dateTime = pack.ChallengeNoticePack[i].StartTime.ToDateTime();
            if (pack.ChallengeNoticePack[i].IsWin == 1)
            {
                texts[0].text = "获得" + pack.ChallengeNoticePack[i].Score + "分";
                texts[1].text = "你的得分:" + pack.ChallengeNoticePack[i].ChallengerScore * 100 + "%";
                texts[2].text = "对手得分:" + pack.ChallengeNoticePack[i].AccepterScore * 100 + "%";
                texts[4].text = "你在你发起的挑战中战胜了"+pack.ChallengeNoticePack[i].AccepterName;
            }
            else
            {
                texts[0].text = "损失" + pack.ChallengeNoticePack[i].Score + "分";
                texts[1].text = "你的得分:" + pack.ChallengeNoticePack[i].ChallengerScore * 100 + "%";
                texts[2].text = "对手得分:" + pack.ChallengeNoticePack[i].AccepterScore * 100 + "%";
                texts[4].text = "你在你发起的挑战中被" + pack.ChallengeNoticePack[i].AccepterName+"击败";
            }
            
            Button item = Instantiate(mItemPrefab, transform.position, transform.rotation);
            item.tag = "noticelist";
            item.transform.SetParent(mContentTransform);
            item.onClick.AddListener(BuySong);
            item.name = pack.ChallengeNoticePack[i].AccepterName;
        }
    }
    void BuySong()
    {

    }
    void DestroyNoticeList()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("noticelist"))
        {
            Destroy(obj);
        }
    }
    void SelectMusicClick()
    {
        SceneController.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("SelectMusic");
    }
    //void PersonalDataClick()
    //{
    //    SceneController.Push(SceneManager.GetActiveScene().name);
    //    SceneManager.LoadScene("PersonalDataScene");
    //}
    void ChallengeClick()
    {
        SceneController.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("ChallengeScene");
    }
    void BuyMusicClick()
    {
        SceneController.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("MusicShop");
    }
    void LogOutClick()
    {
        Client.mainPack.Actioncode = ActionCode.Logout;
        Client.mainPack.Requestcode = RequestCode.UserControl;
        Client.mainPack.User.Userid = Client.userid;
        Client.Send(Client.mainPack);
    }
    
    IEnumerator CheckChallenge() //每隔一定时间向服务器发送获取挑战结果的请求
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            Debug.Log(string.Format("Timer2 is up !!! time=${0}", Time.time));
            Client.mainPack.Actioncode = ActionCode.GetChallengeResult;
            Client.mainPack.Requestcode = RequestCode.UserControl;
            Client.mainPack.User.Userid = Client.userid;
            Client.Send(Client.mainPack);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (logoutSuccess)
        {
            Client.ClearPack();
            LoginPanel.isLogin = false;
            logoutSuccess = false;
            Client.Close();
            SceneController.Pop();
        }
        if (getNotice)
        {
            getNotice = false;
            ShowItems(Client.mainPack);
        }
    }
}
