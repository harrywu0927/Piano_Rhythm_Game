using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
namespace Assets.Scripts
{
    class Sheet
    {
        public static Queue<Note> Notes = new Queue<Note>();
        public static string basePath = "./Assets/Music/";
        public static Queue<Note> SimplifiedNotes = new Queue<Note>();
        public enum Mode { simple=0,medium=1,hard=2 }
        public static void ReadSheet(string song, Mode mode)
        {
            Notes.Clear();
            SimplifiedNotes.Clear();
            using (StreamReader sr = new StreamReader(basePath + song + ".txt"))
            {
                string line = "";
                int id = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(' ');
                    try
                    {
                        Note note = new Note(Note.NotesMap[words[2]], Convert.ToDouble(words[0]) + 4,
                            Convert.ToDouble(words[1]), Convert.ToInt32(words[3]), words[2], id);
                        Notes.Enqueue(note);
                    }
                    catch (KeyNotFoundException)
                    {
                        Debug.Log("Key: " + words[2] + " not exist in NotesMap");
                        Note note = new Note("", Convert.ToDouble(words[0]) + 4,
                            Convert.ToDouble(words[1]), Convert.ToInt32(words[3]), words[2], id);
                        Notes.Enqueue(note);
                    }
                    id++;

                }
            }
            if (mode == Mode.medium || mode == Mode.simple)
            {
                SheetSimplification();
            }
        
            
        }
        private static readonly Dictionary<string, int> PitchMap = new Dictionary<string, int>() {
        {"C",0 },{"C#",1},{"D",2},{"D#",3},{"E",4},{"F",6},{"F#",7},{"G",8},{"G#",9},{"A",10}
        ,{"A#",11},{"B",12}
        };
        public static Note Skyline(Note[] chord, int noteNum)  //对复杂和弦使用Skyline算法
        {
            Note mainNote = chord[0];  //暂时选取第一个为主音符
            int mainPitchIndex = 0;
            for(int i = 1; i < noteNum; i++)
            {
                //首先比较音域
                if (Convert.ToInt32(chord[i].mainPitch[chord[i].mainPitch.Length - 1]) >
                    Convert.ToInt32(mainNote.mainPitch[mainNote.mainPitch.Length - 1]))
                {
                    mainNote = chord[i];
                    mainPitchIndex = i;
                }
                //若音域相同，比较音级
                else if(Convert.ToInt32(chord[i].mainPitch[chord[i].mainPitch.Length - 1]) ==
                    Convert.ToInt32(mainNote.mainPitch[mainNote.mainPitch.Length - 1]))
                {
                    int level1, level2;
                    if (mainNote.mainPitch.Contains("#")) level1 = PitchMap[mainNote.mainPitch.Substring(0, 2)];
                    else level1 = PitchMap[mainNote.mainPitch.Substring(0, 1)];
                    if (chord[i].mainPitch.Contains("#")) level2 = PitchMap[chord[i].mainPitch.Substring(0, 2)];
                    else level2 = PitchMap[chord[i].mainPitch.Substring(0, 1)];

                    if (level1 < level2)
                    {
                        mainNote = chord[i];
                        mainPitchIndex = i;
                    }
                }
            }
            string[] pitches = new string[noteNum];
            for(int i = 0; i < noteNum; i++)//合成为一个和弦音符
            {
                pitches[i] = chord[i].mainPitch;
            }
            mainNote.pitch = pitches;
            mainNote.isChord = true;
            mainNote.mainPitch = mainNote.pitch[mainPitchIndex];
            return mainNote;
        }
        public static void SheetSimplification() 
        {
            Note[] tmp = new Note[100];
            tmp[0] = Notes.Dequeue();
            int n = 1; //tmp中的元素个数
            while (Notes.Count > 0)
            {
                if (Notes.Peek().activeTime == tmp[n - 1].activeTime)
                {
                    tmp[n] = Notes.Dequeue();
                    n++;
                }
                else 
                {
                    if (n == 1)
                    {
                        SimplifiedNotes.Enqueue(tmp[0]);
                    }
                    else if (n > 2) //和弦中音符数量大于2
                    {
                        SimplifiedNotes.Enqueue(Skyline(tmp, n));
                    }
                    else if (n == 2) //和弦中只有2个音符
                    {
                        SimplifiedNotes.Enqueue(tmp[0]);
                        SimplifiedNotes.Enqueue(tmp[1]);
                    }
                    Array.Clear(tmp, 0, n);
                    if (Notes.Count > 0)
                    {
                        tmp[0] = Notes.Dequeue();
                        n = 1;
                    }
                } 
            }
            //Debug.Log(SimplifiedNotes);
            /*Note[] tmp = new Note[100];
            tmp[0] = Notes.Dequeue();
            int n = 1; //tmp中的元素个数
            while (Notes.Count > 0)
            {
                //Debug.Log(Notes.Count);
                if(Notes.Peek().activeTime == tmp[n-1].activeTime)
                {
                    tmp[n] = Notes.Dequeue();
                    n++;
                }
                else if (n == 1)
                {
                    SimplifiedNotes.Enqueue(tmp);
                    Array.Clear(tmp, 0, 10);
                    if (Notes.Count > 0)
                    {
                        tmp[0] = Notes.Dequeue();
                        n = 1;
                    }
                }
                else if(n > 2) //和弦中音符数量大于2
                {
                    Note mainNote = Skyline(tmp, n);
                    for(int i = 0; i < n; i++)
                    {
                        if (tmp[i] == mainNote)
                        {
                            tmp[i].isMain = true;
                        }
                    }
                    SimplifiedNotes.Enqueue(tmp);
                    Array.Clear(tmp, 0, 100);
                    if (Notes.Count > 0)
                    {
                        tmp[0] = Notes.Dequeue();
                        n = 1;
                    }
                }
                else if(n == 2) //和弦中只有2个音符
                {
                    Note[] note1= new Note[1], note2 = new Note[1];
                    note1[0] = tmp[0];
                    note2[0] = tmp[1];
                    SimplifiedNotes.Enqueue(note1);
                    SimplifiedNotes.Enqueue(note2);
                    Array.Clear(tmp, 0, 20);
                    if (Notes.Count > 0)
                    {
                        tmp[0] = Notes.Dequeue();
                        n = 1;
                    }
                }
                
            }*/

        }
    }
}
