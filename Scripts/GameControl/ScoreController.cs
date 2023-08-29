using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ScoreController : MonoBehaviour
    {
        public static int perfectHit, greatHit, goodHit, missHit;
        public static double totalScore;
        public static double ratioScore;
        public static int combo;
        public static int maxCombo;
        public static double realTimeScore;

        public Text scoreText, comboText;
        public static string score = "";
        public static void UpdateScore()
        {
            CalculateRatio();
            score = (ratioScore * 100).ToString() + "%";
        }
        private void Update()
        {
            scoreText.text = "Score: " + score;
            comboText.text = "Combo: " + combo;
        }

        public enum Hit
        {
            Perfect=0,Great=1,Good=2,Miss=3
        }
        public static Dictionary<Hit, int> scoreMap = new Dictionary<Hit, int>()
        {
            {Hit.Perfect,5 },{Hit.Great,4},{Hit.Good,3},{Hit.Miss,0}
        };
        public static void Initialize()
        {
            perfectHit = 0;
            greatHit = 0;
            goodHit = 0;
            missHit = 0;
            ratioScore = 0;
            combo = 0;
            maxCombo = 0;
            realTimeScore = 0;
            CalculateMaxTotalScore();
            UpdateScore();
        }


        // 需要在Sheet调用ReadSheet()后使用
        public static void CalculateMaxTotalScore() 
        {
            int totalNotes = Sheet.SimplifiedNotes.Count;
            totalScore = scoreMap[Hit.Perfect] * (1 * 10 + 1.2 * 20 + 1.3 * (totalNotes - 30));
            
        }

        //实时分数
        public static void CalculateRealTimeScore(Hit hit)
        {
            if (combo < 10)
            {
                realTimeScore += scoreMap[hit];
            }else if (combo < 30)
            {
                realTimeScore += scoreMap[hit] * 1.2;
            }
            else
            {
                realTimeScore += scoreMap[hit] * 1.3;
            }
        }

        public static void CalculateRatio()
        {
            ratioScore = Math.Round(realTimeScore / totalScore, 4);//保留4位小数
        }

    }
}
