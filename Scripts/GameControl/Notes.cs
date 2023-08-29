using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class Notes : MonoBehaviour
{
    private float speed = 5;
    public static bool start = false;
    public static ArrayList activeNotes = new ArrayList();
    public ParticleSystem explosion;
    private readonly Dictionary<string, int> LaneMap = new Dictionary<string, int>() {
        {"C",0 },{"C#",1},{"D",2},{"D#",3},{"E",4},{"F",6},{"F#",7},{"G",8},{"G#",9},{"A",10}
        ,{"A#",11},{"B",12}
    };

    public Texture2D tex;
    private Sprite mySprite;
    private SpriteRenderer sr;
    public static string songName;
    private static Sheet.Mode mode { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        mode = Sheet.Mode.simple;
        Sheet.ReadSheet(songName, mode);
        //Debug.Log(songName);
        ScoreController.Initialize();
    }
    void CreateNotes()//创建Notes
    {
        if (Sheet.SimplifiedNotes.Peek().activeTime - 1f < TimeTable.time)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<SpriteRenderer>();
            sr = obj.GetComponent<SpriteRenderer>();
            sr.size = new Vector2(0.8f, 0.9f);
            sr.color = Color.white;
            mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, 256, 256), new Vector2(0.5f, 0.5f), 100.0f);
            sr.transform.localScale = new Vector3(0.7f, 0.4f, 0f);
            sr.sprite = mySprite;
            obj.tag = "notes";
            Note note = Sheet.SimplifiedNotes.Dequeue();
            if (note.isChord)
            {
                Color32 color = new Color32(255, 215, 0, 255);
                sr.color = color;
            }
            obj.name = note.id.ToString();
            obj.transform.position = new Vector2(getXPosition(note), 20f);
            
            if(Sheet.SimplifiedNotes.Count == 0)
            {
                note.final = true;
            }
            activeNotes.Add(note);
        }
        
    }
    float getXPosition(Note note)//计算初始化Note时的X坐标
    {
        try
        {
            if (note.mainPitch.Contains("#"))
            {
                return 1.7f * LaneMap[note.mainPitch.Substring(0, 2)];
            }
            else
            {
                return 1.7f * LaneMap[note.mainPitch.Substring(0, 1)];
            }
        }
        catch(KeyNotFoundException)
        {
            return 0f;
        }
        
    }

    void FixedUpdate()
    {
        if (start)
        {
            GameObject[] notes = GameObject.FindGameObjectsWithTag("notes");
            foreach (GameObject note in notes){
                note.transform.position -= new Vector3(0f, speed * Time.fixedDeltaTime, 0f);
                if(note.transform.position.y < -5)
                {
                    ScoreController.missHit++;
                    ScoreController.combo = 0;
                    DestroyNote(note,ScoreController.Hit.Miss);
                }
            }
            CreateNotes();
            
        }
    }

    //销毁Note，释放资源；同时将其从activeNotes中删除并计分
    public static void DestroyNote(GameObject obj, ScoreController.Hit hit)
    {
        Note note;
        for (int i = 0; i < activeNotes.Count; i++)
        {
            note = activeNotes[i] as Note;
            if(note.id.ToString() == obj.name)
            {
                ScoreController.CalculateRealTimeScore(hit);
                ScoreController.UpdateScore();
                ScoreController.combo++;
                
                if (ScoreController.combo > ScoreController.maxCombo)
                {
                    ScoreController.maxCombo = ScoreController.combo;
                }
                if(hit == ScoreController.Hit.Miss)
                {
                    ScoreController.combo = 0;
                }
                Destroy(obj);
                activeNotes.RemoveAt(i);
            }
        }
    }
    


}
