using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;
using UnityEngine.SceneManagement;
using Assets.Scripts.UI;
using System;


public class MyChallenge : MonoBehaviour
{
    public Button mItemPrefab;
    public Transform mContentTransform;
    public Dropdown selectAuthor;
    public InputField songName, challenger;
    public Button searchBtn, backBtn;
    public KeyCode keyCode;
    public static bool getSearchResult = false, acceptSuccess = false, accepterChallenging = false, accepterCompleted = false;
    public static double accepterScore;
    public static int currentChallengeId;
    int lastindex;
    void Start()
    {
        mContentTransform = this.transform.Find("Scroll View/Viewport/Content");
        backBtn.onClick.AddListener(BackClick);
        searchBtn.onClick.AddListener(SearchClick);
        InitDropDownList();
        SearchClick();
        selectAuthor.onValueChanged.AddListener(DropdownListClick);
        lastindex = -1;
    }
    void ShowItems(MainPack pack)
    {
        DestroySongList();
        for (int i = 0; i < pack.Challengepack.Count; i++)
        {
            Text[] texts = mItemPrefab.GetComponentsInChildren<Text>();
            texts[0].text = pack.Challengepack[i].Author;
            texts[1].text = pack.Challengepack[i].SongName;
            texts[2].text = "该玩家得分: " + (pack.Challengepack[i].ChallengerScore*100).ToString() + "%";
            texts[3].text = "你的历史最高分: " + (pack.Userssong[i].Scorerecord*100).ToString() + "%";
            texts[4].text = "由 " + pack.Challengepack[i].ChallengerName + " 发起";
            texts[5].text = "倍率: " + pack.Challengepack[i].Ratio.ToString();
            texts[6].text = "底分: " + pack.Challengepack[i].BaseScore.ToString();
            Button item = Instantiate(mItemPrefab, transform.position, transform.rotation);
            item.tag = "challengelist";
            item.transform.SetParent(mContentTransform);
            item.onClick.AddListener(StartGame);

            item.name = pack.Challengepack[i].SongName;
        }
    }
    void DestroySongList()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("challengelist"))
        {
            Destroy(obj);
        }
    }
    void InitDropDownList()
    {
        List<string> authors = new List<string>();
        selectAuthor.ClearOptions();
        bool find = false;
        int index = 1;
        authors.Add("");
        
        if (Client.mainPack.Challengepack.Count > 0)
        {
            for (int i = 0; i < Client.mainPack.Authors.Count; i++)
            {
                authors.Add(Client.mainPack.Authors[i]);
                if (Client.mainPack.Authors[i] == Client.mainPack.Challengepack[0].Author)
                    find = true;
                if (!find)
                    index++;
            }
            selectAuthor.value = index;
        }
        else
        {
            for (int i = 0; i < Client.mainPack.Authors.Count; i++)
            {
                authors.Add(Client.mainPack.Authors[i]);
            }
        }
        selectAuthor.AddOptions(authors);

    }
    void DropdownListClick(int index)
    {
        if (index != lastindex)
        {
            Client.mainPack.Actioncode = ActionCode.SearchChallenges;
            Client.mainPack.Requestcode = RequestCode.ChallengeControl;
            
            ChallengePack challengePack = new ChallengePack
            {
                Author = selectAuthor.options[selectAuthor.value].text,
                SongName = songName.text,
            };
            Client.mainPack.Challengepack.Clear();
            Client.mainPack.Challengepack.Add(challengePack);
            lastindex = index;
            Client.Send(Client.mainPack);
        }

    }
    void StartGame()
    {
        var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        //if (UnityEditor.EditorUtility.DisplayDialog("", "确认挑战吗？你将立即开始游戏", "确认", "取消"))
        //{
            ChallengePack challengePack = new ChallengePack
            {
                ChallengerName = buttonSelf.GetComponentsInChildren<Text>()[4].text.Split(' ')[1],
                Accepter = Client.userid,
                SongName = buttonSelf.name,
                Author = buttonSelf.GetComponentsInChildren<Text>()[0].text
            };
            Client.mainPack.Challengepack.Clear();
            Client.mainPack.Challengepack.Add(challengePack);
            Client.mainPack.Requestcode = RequestCode.ChallengeControl;
            Client.mainPack.Actioncode = ActionCode.AcceptChallenge;
            Client.Send(Client.mainPack);
            Debug.Log("start:" + buttonSelf.name);
            Notes.songName = buttonSelf.name;
            
        //}
        
    }
    void BackClick()
    {
        SceneController.Pop();
    }

    void SearchClick()
    {
        Client.mainPack.Actioncode = ActionCode.SearchChallenges;
        Client.mainPack.Requestcode = RequestCode.ChallengeControl;

        ChallengePack challengePack = new ChallengePack
        {
            Author = selectAuthor.options[selectAuthor.value].text,
            SongName = songName.text,
        };
        if (challenger.text != "")
        {
            challengePack.Challenger = Convert.ToInt32(challenger.text);
        }
        else challengePack.Challenger = 0;
        Client.mainPack.Challengepack.Clear();
        Client.mainPack.Challengepack.Add(challengePack);

        Client.Send(Client.mainPack);

    }
    void ChallengeComplete()
    {
        accepterChallenging = false;
        ChallengePack challengePack = new ChallengePack
        {
            Accepter = Client.userid,
            AccepterScore = accepterScore,
            ChallengeId = currentChallengeId
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
        if (Input.GetKeyUp(keyCode))
        {
            SearchClick();
        }
        if (getSearchResult)
        {
            ShowItems(Client.mainPack);
            getSearchResult = false;
            InitDropDownList();
        }
        if (acceptSuccess)
        {
            acceptSuccess = false;
            accepterChallenging = true;
            SceneController.Push(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("GameScene");
        }
        if (accepterCompleted)
        {
            accepterCompleted = false;
            ChallengeComplete();
        }
    }
}
