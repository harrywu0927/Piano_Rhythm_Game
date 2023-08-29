using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{    
    class Note
    {
        //音符对应的音频文件
        public static Dictionary<string, string> NotesMap = new Dictionary<string, string> {
            {"C1", "C1" },
            {"D1", "D1" },
            {"E1", "E1" },
            {"F1", "F1" },
            {"G1", "G1" },
            {"A1", "A1" },
            {"B1", "B1" },
          {  "C2",  "a49" },
          {  "D2",  "a50" },
          {  "E2",  "a51" },
          {  "F2",  "a52" },
          {  "G2",  "a53" },
          {  "A2",  "a54" },
          {  "B2",  "a55" },
          {  "C3",  "a56" },
          {  "D3",  "a57" },
          {  "E3",  "a48" },
          {  "F3",  "a81" },
          {  "G3",  "a87" },
          {  "A3",  "a69" },
          {  "B3",  "a82" },
          {  "C4",  "a84" },
          {  "D4",  "a89" },
          {  "E4",  "a85" },
          {  "F4",  "a73" },
          {  "G4",  "a79" },
          {  "A4",  "a80" },
          {  "B4",  "a65" },
          {  "C5",  "a83" },
          {  "D5",  "a68" },
          {  "E5",  "a70" },
          {  "F5",  "a71" },
          {  "G5",  "a72" },
          {  "A5",  "a74" },
          {  "B5",  "a75" },
          {  "C6",  "a76" },
          {  "D6",  "a90" },
          {  "E6",  "a88" },
          {  "F6",  "a67" },
          {  "G6",  "a86" },
          {  "A6",  "a66" },
          {  "B6",  "a78" },
          {  "C7",  "a77" },
            {"D7", "D7" },
            {"E7", "E7" },
            {"F7", "F7" },
            {"G7", "G7" },
            {"A7", "A7" },
            {"B7", "B7" },
            {"C8", "C8" },
          {"C#1", "C1#" },
            {"D#1", "D1#" },
            {"F#1", "F1#" },
            {"G#1", "G1#" },
            {"A#1", "A1#" },
          {  "C#2",  "b49" },
          {  "D#2",  "b50" },
          {  "F#2",  "b52" },
          {  "G#2",  "b53" },
          {  "A#2",  "b54" },
          {  "C#3",  "b56" },
          {  "D#3",  "b57" },
          {  "F#3",  "b81" },
          {  "G#3",  "b87" },
          {  "A#3",  "b69" },
          {  "C#4",  "b84" },
          {  "D#4",  "b89" },
          {  "F#4",  "b73" },
          {  "G#4",  "b79" },
          {  "A#4",  "b80" },
          {  "C#5",  "b83" },
          {  "D#5",  "b68" },
          {  "F#5",  "b71" },
          {  "G#5",  "b72" },
          {  "A#5",  "b74" },
          {  "C#6",  "b76" },
          {  "D#6",  "b90" },
          {  "F#6",  "b67" },
          {  "G#6",  "b86" },
          {  "A#6",  "b66" },
          {"C#7", "C7#" },
            {"D#7", "D7#" },
            {"F#7", "F7#" },
            {"G#7", "G7#" },
            {"A#7", "A7#" },
        };

        public int id { get; }
        public string audio { get; set; }//音频文件名
        public double activeTime { get; set; }//应被按下的时间
        public double duration { get; set; }//持续时间
        //public double[] duration { get; set; }//持续时间
        public float volume { get; set; }//音量
        //public float[] volume { get; set; }//音量 
        public string[] pitch { get; set; }//音高
        public string mainPitch { get; set; }//主音
        public bool final { get; set; }//是否为最后一个音符
        public bool isChord { get; set; }//是否为和弦

        public Note(string audio, double activeTime, double duration, int volume, string mainPitch, int id)
        {
            this.audio = audio;
            this.activeTime = activeTime;
            this.duration = duration;
            this.volume = VolumeConvert(volume);
            this.mainPitch = mainPitch;
            this.id = id;
            this.final = false;
            this.isChord = false;
        }
        public static float VolumeConvert(int volume)  //将音量范围线性调整至0-1
        {
            float scale = 1f / 128f;  //0<=volume<=128
            return volume * scale;
        }
        //public static int GetMainNoteIndex(Note[] chord)
        //{
        //    int index = 0;
        //    for(int i = 0; i < chord.Length && chord[i] != null; i++)
        //    {
        //        if (chord[i].isMain)
        //        {
        //            index = i;
        //            break;
        //        }
        //    }
        //    return index;
        //}
        public Note() { }
    }
}
