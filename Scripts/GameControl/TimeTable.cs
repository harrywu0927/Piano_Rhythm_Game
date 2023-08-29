using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using SocketGameProtocol;
public class TimeTable : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    public static double time;
    public Sprite mySprite;
    public SpriteRenderer sr;

    private double endTime;
    private float timeLineLen = 66.61f;
    private float speed;
    void Start()
    {
        //StartCoroutine(Timer());
        endTime = Sheet.SimplifiedNotes.ToArray()[Sheet.SimplifiedNotes.ToArray().Length - 1].activeTime + 5;
        time = 0;
        speed = timeLineLen / (float)endTime;
    }
    //IEnumerator Timer() //协程方式的TimeTable
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        //Debug.Log(string.Format("Timer2 is up !!! time=${0}", Time.time));
    //        text.text = Time.time.ToString();
    //    }
    //}
    //Update is called once per frame
    public static void GameOver()//游戏结束
    {
        Client.mainPack.Actioncode = ActionCode.GameSettlement;
        Client.mainPack.Requestcode = RequestCode.GameResultControl;
        GameResultPack gameResultPack = new GameResultPack {
            Userid = Client.userid,
            Song = Notes.songName,
            Combo = ScoreController.maxCombo,
            Perfect = ScoreController.perfectHit,
            Great = ScoreController.greatHit,
            Good = ScoreController.goodHit,
            Miss = ScoreController.missHit,
            Gamescore = ScoreController.ratioScore

        };
        Client.mainPack.Gameresultpack = gameResultPack;
        Client.Send(Client.mainPack);

        SceneManager.LoadScene("GameResult");
    }
    void FixedUpdate()
    {
        text.text = time.ToString();
        time += Time.fixedDeltaTime;
        if (endTime != 0 && time > endTime ) //暂以此标准作为游戏结束的判定
        {
            GameOver();
        }

        //进度条
        sr.gameObject.transform.position += new Vector3(speed * Time.fixedDeltaTime, 0f, 0f);
    }
}
