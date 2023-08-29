using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.UI;
using SocketGameProtocol;
using System;

public class ChallengePlayer : MonoBehaviour
{
    public Text scoreText,selectedMusic, musicAuthor, ratioText, baseScoreText;
    public Button backBtn, selectMusicBtn, sendChallengeBtn, myChallengeBtn;
    public Slider baseScoreSlider, ratioSlider;
    public static bool musicSelected = false, playerNotExist = false, challengeToMany = false;
    public static double ratio, baseScore;
    public static string songName, author;
    public static bool challenging = false, gameCompleted;
    public static double challengerScore;
    void Start()
    {
        scoreText.text = "你的积分:" + Client.mainPack.User.Scores.ToString();
        backBtn.onClick.AddListener(BackClick);
        selectMusicBtn.onClick.AddListener(SelectMusicClick);
        sendChallengeBtn.onClick.AddListener(SendChallengeClick);
        myChallengeBtn.onClick.AddListener(MyChallengeClick);
        ratioSlider.onValueChanged.AddListener(delegate { ratioValueChanged(); });
        baseScoreSlider.onValueChanged.AddListener(delegate { baseScoreValueChanged(); });
        selectedMusic.enabled = false;
        musicAuthor.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (musicSelected)
        {
            selectedMusic.enabled = true;
            musicAuthor.enabled = true;
            selectedMusic.text = songName;
            musicAuthor.text = author;
            selectMusicBtn.GetComponentInChildren<Text>().text = "更改";
            ratioText.text = "x" + ratio.ToString();
            baseScoreText.text = baseScore.ToString();
            ratioSlider.value = (float)ratio*10;
            baseScoreSlider.value = (float)baseScore;
        }
        if(challengeToMany)
        {
            
        }
        if (gameCompleted)
        {
            gameCompleted = false;
            SendChallengePack();
        }
    }

    void BackClick()
    {
        SceneController.Pop();
    }
    void SelectMusicClick()
    {
        musicSelected = false;
        SelectMusic.isChallengeMusic = true;
        SceneController.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("SelectMusic");
        
    }
    void SendChallengeClick()
    {
        if (selectedMusic.text == "")
        {
            Debug.Log("song empty");
            return;
        }
        //if (UnityEditor.EditorUtility.DisplayDialog("", "发起后，你需要立即完成游戏，确定吗？", "确认", "等一下"))
        //{
            StartGame();
        //}
    }
    void StartGame()
    {
        Debug.Log("start:" + songName);
        Notes.songName = songName;
        challenging = true;
        SceneController.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("GameScene");
    }
    void SendChallengePack()//玩家完成游戏后，向服务器上传此次挑战信息
    {
        challenging = false;
        ChallengePack challengePack = new ChallengePack
        {
            BaseScore = baseScore,
            Ratio = ratio,
            Challenger = Client.userid,
            SongName = selectedMusic.text,
            ChallengerScore = challengerScore,
            Author = author
        };
        Client.mainPack.Challengepack.Clear();
        Client.mainPack.Challengepack.Add(challengePack);
        Client.mainPack.Requestcode = RequestCode.ChallengeControl;
        Client.mainPack.Actioncode = ActionCode.SendChallenge;
        //Debug.Log("已发送：" + challengePack);
        Client.Send(Client.mainPack);
    }
    void MyChallengeClick()
    {
        SceneController.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("MyChallenge");
    }
    void ratioValueChanged()
    {
        ratio = Math.Round(ratioSlider.value / 10, 1);
        ratioText.text = "x" + ratio.ToString();
    }
    void baseScoreValueChanged()
    {
        baseScore = baseScoreSlider.value;
        baseScoreText.text = baseScore.ToString();
    }
}
