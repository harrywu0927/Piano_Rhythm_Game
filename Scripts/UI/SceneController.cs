using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    class SceneController
    {
        public static Stack<string> Scenes = new Stack<string>();
        public static void Pop()
        {
            SceneManager.LoadScene(Scenes.Pop());
            while(Scenes.Count!=0 && Scenes.Peek() == SceneManager.GetActiveScene().name)
            {
                Scenes.Pop();
            }
        }
        public static void Push(string sceneName)
        {
            Scenes.Push(sceneName);
        }
        public static void Top()
        {
            SceneManager.LoadScene(Scenes.Peek());
        }
    }
}
