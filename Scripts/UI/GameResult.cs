using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;
using Assets.Scripts.UI;
using UnityEngine.SceneManagement;
using System;
public class GameResult : MonoBehaviour
{
    public Text Perfect, Great, Good, Miss, Combo, FinalScore, Goldcoins, Exp, challengeScore, result;
    public Button Next, Retry;
    public static bool getResult, getChallengeResult;
    double finalScore;

    // Start is called before the first frame update
    void Start()
    {
        Next.onClick.AddListener(NextClick);
        Retry.onClick.AddListener(RetryClick);
        challengeScore.enabled = false;
        //getResult = false;
    }
    void NextClick()
    {
        if (ChallengePlayer.challenging)
        {
            ChallengePlayer.challengerScore = finalScore;
            ChallengePlayer.gameCompleted = true;
            SceneController.Pop();
        }
        else if (MyChallenge.accepterChallenging)
        {
            //MyChallenge.accepterCompleted = true;
            //MyChallenge.accepterScore = finalScore;
            SceneController.Pop();
        }
        else
        {
            SceneManager.LoadScene("SelectMusic");
        }
        
    }
    void RetryClick()
    {
        SceneManager.LoadScene("GameScene");
    }
    void ChallengeComplete()
    {
        ChallengePack challengePack = new ChallengePack
        {
            Accepter = Client.userid,
            AccepterScore = finalScore,
            ChallengeId = MyChallenge.currentChallengeId
        };
        Client.mainPack.Challengepack.Clear();
        Client.mainPack.Challengepack.Add(challengePack);
        Client.mainPack.Requestcode = RequestCode.ChallengeControl;
        Client.mainPack.Actioncode = ActionCode.CompleteChallenge;
        Debug.Log("已发送：" + challengePack);
        Client.Send(Client.mainPack);
    }

    // Update is called once per frame
    void Update()
    {
        if (getResult)
        {
            InitResult();
            //Debug.Log("gameresult init");
            getResult = false;
        }
        if (getChallengeResult)
        {
            ChallengePack challengePack = Client.mainPack.Challengepack[0];
            challengeScore.enabled = true;
            if (challengePack.AccepterScore > challengePack.ChallengerScore)
            {
                result.text = "胜利";
                challengeScore.text = "获得:" + (challengePack.Ratio * challengePack.BaseScore).ToString() + "分";
            }
            else if (challengePack.AccepterScore < challengePack.ChallengerScore)
            {
                result.text = "败北";
                challengeScore.text = "损失:" + (challengePack.Ratio * challengePack.BaseScore).ToString() + "分";
            }
            else
            {
                result.text = "平局";
            }
        }
        if (ChallengePlayer.challenging || MyChallenge.accepterChallenging)
        {
            Retry.enabled = false;
        }
        else
        {
            Retry.enabled = true;
        }
        if (MyChallenge.accepterChallenging)
        {
            //MyChallenge.accepterCompleted = true;
            //MyChallenge.accepterScore = finalScore;
            //SceneController.Pop();
            ChallengeComplete();
        }
    }
    void InitResult()
    {
        Combo.text = Client.mainPack.Gameresultpack.Combo.ToString();
        Perfect.text = Client.mainPack.Gameresultpack.Perfect.ToString();
        Great.text = Client.mainPack.Gameresultpack.Great.ToString();
        Good.text = Client.mainPack.Gameresultpack.Good.ToString();
        Miss.text = Client.mainPack.Gameresultpack.Miss.ToString();
        FinalScore.text = (Client.mainPack.Gameresultpack.Gamescore * 100).ToString() + "%";
        finalScore = Client.mainPack.Gameresultpack.Gamescore;
        Goldcoins.text = Client.mainPack.Gameresultpack.Goldcoin.ToString();
        Exp.text = Client.mainPack.Gameresultpack.Experience.ToString();
    }
}
