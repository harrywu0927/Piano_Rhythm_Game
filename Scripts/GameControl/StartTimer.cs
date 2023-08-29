using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public Text startText;
    float currenttime;
    void Start()
    {
        currenttime = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currenttime += Time.fixedDeltaTime;
        if (currenttime > 0f)
        {
            startText.text = "3";
        }
        if (currenttime > 1f)
        {
            startText.text = "2";
        }
        if (currenttime > 2f)
        {
            startText.text = "1";
        }
        if (currenttime > 3f)
        {
            startText.text = "Start!";
            Notes.start = true;
        }
        if (currenttime > 4f)
        {
            startText.text = "";
            this.enabled = false;
        }
        
    }
}
