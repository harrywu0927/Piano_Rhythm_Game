using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;
using UnityEngine.SceneManagement;
using Assets.Scripts.UI;

public class SelectMusic : MonoBehaviour
{
    public Button mItemPrefab;
    private Transform mContentTransform;
    public Dropdown selectAuthor;
    public InputField songName;
    public Button searchBtn, backBtn;
    public KeyCode keyCode;
    public static bool getSearchResult = false;
    public static bool isChallengeMusic = false;
    int lastindex;
    void Start()
    {
        mContentTransform = this.transform.Find("Scroll View/Viewport/Content");
        backBtn.onClick.AddListener(BackClick);
        searchBtn.onClick.AddListener(SearchClick);
        SearchClick();
        lastindex = -1;
        selectAuthor.onValueChanged.AddListener(DropdownListClick);

    }

    void ShowItems(MainPack pack)
    {
        DestroySongList();
        for (int i = 0; i < pack.Userssong.Count; i++)
        {
            Text[] texts = mItemPrefab.GetComponentsInChildren<Text>();
            texts[0].text = pack.Userssong[i].Author;
            texts[1].text = "难度:" + pack.Userssong[i].Difficulty.ToString();
            texts[2].text = "最高分:" + (pack.Userssong[i].Scorerecord*100).ToString()+"%";
            texts[3].text = "世界排名:" + pack.Userssong[i].Rank.ToString();
            texts[4].text = pack.Userssong[i].Songname;
            Button item = Instantiate(mItemPrefab, transform.position, transform.rotation);
            item.tag = "songlist";
            item.transform.SetParent(mContentTransform);
            item.onClick.AddListener(StartGame);
            item.name = pack.Userssong[i].Songname;
        }
    }
    void DestroySongList()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("songlist"))
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
        for (int i = 0; i < Client.mainPack.Authors.Count; i++)
        {
            authors.Add(Client.mainPack.Authors[i]);
            if (Client.mainPack.Authors[i] == Client.mainPack.Searchsongpack.Author)
                find = true;
            if (!find)
                index++;

        }
        selectAuthor.AddOptions(authors);
        if (Client.mainPack.Searchsongpack.Author != "")
            selectAuthor.value = index;
    }
    void DropdownListClick(int index)
    {
        if (index != lastindex)
        {
            Client.mainPack.Actioncode = ActionCode.SearchSongs;
            Client.mainPack.Requestcode = RequestCode.UserControl;

            SearchSongPack searchSongPack = new SearchSongPack();
            searchSongPack.Author = selectAuthor.options[index].text;
            searchSongPack.SongName = songName.text;
            Client.mainPack.Searchsongpack = searchSongPack;
            lastindex = index;
            Client.Send(Client.mainPack);
        }

    }

    void BackClick()
    {
        SceneController.Pop();
    }

    void StartGame()
    {
        var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        
        if (!isChallengeMusic)
        {
            Debug.Log("start:"+buttonSelf.name);
            Notes.songName = buttonSelf.name;
            SceneManager.LoadScene("GameScene");
        }
        else //从挑战界面而来
        {
            isChallengeMusic = false;
            ChallengePlayer.songName = buttonSelf.name;
            ChallengePlayer.author = buttonSelf.GetComponentsInChildren<Text>()[0].text;
            ChallengePlayer.musicSelected = true;
            SceneController.Pop();
        }
    }
    void SearchClick()
    {
        Client.mainPack.Actioncode = ActionCode.SearchSongs;
        Client.mainPack.Requestcode = RequestCode.UserControl;

        SearchSongPack searchSongPack = new SearchSongPack();
        searchSongPack.Author = selectAuthor.options[selectAuthor.value].text;
        searchSongPack.SongName = songName.text;
        Client.mainPack.Searchsongpack = searchSongPack;

        Client.Send(Client.mainPack);

    }
    private void Update()
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
    }
}
