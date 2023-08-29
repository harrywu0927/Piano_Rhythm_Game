using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

    public class ExitGameDialog : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                //if (UnityEditor.EditorUtility.DisplayDialog("", "确认结束吗？", "确认", "取消"))
                //{
                    TimeTable.GameOver();
                //}
            }
        }
        
    }

