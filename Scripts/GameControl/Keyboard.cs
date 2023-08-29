using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using Assets.Scripts;

public class Keyboard : MonoBehaviour
{

    public KeyCode keyToPress;
    //public AudioSource audio ;
    public SpriteRenderer SR;
    public ParticleSystem explosion;
    private bool keyDown = false; //控制每次按下只可有一个音符消除
    
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))//键盘按下
        {
            
            if (CompareTag("white"))
            {
                SR.color = new Color32(57, 204, 255, 255);
            }
            else if (CompareTag("black"))
            {
                SR.color = new Color32(65, 63, 138, 255);
            }
            AudioSource audio = GetComponent<AudioSource>();
            GameObject[] objs = GameObject.FindGameObjectsWithTag("notes");
            for(int i = 0; i < Notes.activeNotes.Count; i++)
            {
                Note note = Notes.activeNotes[i] as Note;
                if (Math.Abs(note.activeTime + 3f - TimeTable.time) < 0.5f && (!keyDown)) //进入判定范围
                {
                    for (int j = 0;j<objs.Length;j++)
                    {
                        if (objs[j].name == note.id.ToString())//遍历现存Notes，和activeNotes中Note的id比对
                        {
                            if ((note.mainPitch.Contains("#")&& name.Contains("#")&&note.mainPitch
                                .Contains(name))||(!note.mainPitch.Contains("#") && !name.Contains("#") && note.mainPitch
                                .Contains(name)))
                            {
                                if (note.isChord)
                                {
                                    for (int k = 0; k < note.pitch.Length; k++)
                                    {
                                        AudioClip clip = Resources.Load(Note.NotesMap[note.pitch[k]]) as AudioClip;
                                        audio.PlayOneShot(clip, note.volume);
                                    }
                                }
                                else
                                {
                                    AudioClip clip = Resources.Load(note.audio) as AudioClip;
                                    audio.PlayOneShot(clip, note.volume);
                                }
                                if (Math.Abs(note.activeTime + 3f - TimeTable.time) < 0.4f)//Great
                                {
                                    if (Math.Abs(note.activeTime + 3f - TimeTable.time) < 0.3f)//Perfect
                                    {
                                        ScoreController.perfectHit++;
                                        Notes.DestroyNote(objs[j], ScoreController.Hit.Perfect);
                                        explosion.Play();
                                        keyDown = true;
                                        break;
                                    }
                                    Notes.DestroyNote(objs[j], ScoreController.Hit.Great);
                                    ScoreController.greatHit++;
                                    keyDown = true; explosion.Play();
                                    break;
                                }
                                Notes.DestroyNote(objs[j], ScoreController.Hit.Good);
                                ScoreController.goodHit++; explosion.Play();
                                keyDown = true;
                                break;
                            }
                            
                        }
                    }

                }

            }



        }
        if (Input.GetKeyUp(keyToPress))//键盘松开
        {
            
            keyDown = false;
            if (CompareTag("white"))
            {
                SR.color = new Color32(255, 255, 255, 255);
            }else if (CompareTag("black"))
            {
                SR.color = new Color32(0, 0, 0, 255);
            }
            //audio.Stop();
        }

        
    }
    
}
