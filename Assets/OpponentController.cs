using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : MonoBehaviour
{
    private int ChoiseY, Choise,CN;

    private Texture2D[] MainChoise;
    private Texture2D[] SecondChoise;
    private Texture2D[] Smiles;
    private Texture2D ChoiseT;
    private float X_max, Y_max, CurrentSeg, ChoiseTimer, Timer, RNDTimerMax,ChoiseTimerMax;
    private Controller pl;
    private CharacterDatabase C_data;
    public bool AnswerSmile { get; set; }
    public bool Question { get; set; }

    private float width,YY;
    private Vector2 UIBorders;
    public int AnswerINT { get; set; }
    private float WidthBorder;

    private AudioSource AU;
    private AudioClip[] Clicks;
    private AudioClip[] EnterClips;

    void Start()
    {
        ChoiseTimerMax = 0.5f;
        width = Screen.width / 20;
        WidthBorder = Screen.width / -5;
        UIBorders = Camera.main.WorldToScreenPoint(transform.position);
        YY = Screen.height/10;

        pl = GameObject.Find("Player").GetComponent<Controller>();
        MainChoise = new Texture2D[5];
        SecondChoise = new Texture2D[3];
        Smiles = new Texture2D[2];

        C_data = GameObject.Find("CharacterDatabase").GetComponent<CharacterDatabase>();

        for (int i = 0; i < MainChoise.Length; i++)
        {
            MainChoise[i] = Resources.Load<Texture2D>("Sprites/UI/MainChoise" + i);

           
        }



        ChoiseT = Resources.Load<Texture2D>("Sprites/UI/Choise");
        for (int i = 0; i < Smiles.Length; i++)
        Smiles[i] = Resources.Load<Texture2D>("Sprites/UI/Smile_" + i);

        X_max = 4;
        Y_max = MainChoise.Length;
        RNDTimerMax = 4;


        AU = GetComponent<AudioSource>();

        Clicks = new AudioClip[3];
        for (int i = 0; i < Clicks.Length; i++)
            Clicks[i] = Resources.Load<AudioClip>("Sound/UI/Click_" + i);

        EnterClips = new AudioClip[3];
        for (int i = 0; i < EnterClips.Length; i++)
            EnterClips[i] = Resources.Load<AudioClip>("Sound/UI/Enter_" + i);
    }

    // Update is called once per frame
    void Update()
    {
        CN = 1;
        for (int i = 0; i < 2 - (Choise - 1); i++)
            CN *= 10;
        if ((C_data.character[PlayerPrefs.GetInt("DateLevel")].smiles[ChoiseY] / CN) % 10 == 0)
            GetComponent<Animator>().SetBool("Exited",false);
        if ((C_data.character[PlayerPrefs.GetInt("DateLevel")].smiles[ChoiseY] / CN) % 10 == 1)
            GetComponent<Animator>().SetBool("Exited", true);

        for (int j = 0; j < 3; j++)
            SecondChoise[j] = Resources.Load<Texture2D>("Sprites/UI/SecondChoise" + ChoiseY + "_" + j);

        if (!pl.PlayerMove)
            {

            if (Timer > Time.fixedTime)
            {

               
                if (Question)
                {
                    X_max = 4;
                    Y_max = MainChoise.Length;
                    if (Timer - RNDTimerMax / 2 > Time.fixedTime)
                    {
                        int PM = Random.Range(0, 2);

                        if (ChoiseTimer < Time.fixedTime)
                        {
                            if (PM == 0)
                            {
                                if (ChoiseY > 0)
                                {
                                    ChoiseY--;
                                    PlayAudio(Clicks);
                                }
                            }
                            else
                            {
                                if (ChoiseY < Y_max - 1)
                                {
                                    ChoiseY++;
                                    PlayAudio(Clicks);
                                }
                            }
                            ChoiseTimer = Time.fixedTime + ChoiseTimerMax;
                        }

                    }
                    else
                    {
                        if (ChoiseTimer < Time.fixedTime)
                        {
                            int PM = Random.Range(0, 2);

                            if (Choise < 1) Choise = 1;

                            if (PM == 0)
                            {
                                if (Choise > 1)
                                {
                                    Choise--;
                                    PlayAudio(Clicks);
                                }
                            }
                            else
                            {
                                if (Choise < X_max - 1)
                                {
                                    Choise++;
                                    PlayAudio(Clicks);
                                }
                            }

                            ChoiseTimer = Time.fixedTime + ChoiseTimerMax;
                        }


                    }
                }

            }
            else
            {
                if (Question)
                {
                    pl.PlayerMove = true;
                    pl.AnswerSmile = true;
                    Question = false;
                }
                if (AnswerSmile)
                {
                    Question = true;
                    AnswerSmile = false;
                    Timer = Time.fixedTime + RNDTimerMax;
                }


            }

        }
           
        

    }

    private void OnGUI()
    {
        
        
        if (AnswerSmile)
        {
            
            DrawSmile((C_data.character[PlayerPrefs.GetInt("DateLevel")].smiles[ChoiseY] / CN) % 10);
            AnswerINT = (C_data.character[PlayerPrefs.GetInt("DateLevel")].smiles[ChoiseY] / CN) % 10;
        }

        if(Question)DrawUI();

        if (pl.PlayerMove)
        {
            if (pl.AnswerSmile||AnswerSmile)
            {
                GUI.DrawTexture(new Rect(UIBorders.x + width+ WidthBorder, YY + width, width, width), SecondChoise[Choise - 1]);
            }
        }
            

    }
    void DrawSmile(int i)
    {
        //GUI.DrawTexture(new Rect(UIBorders.x , YY + width, width, width), SecondChoise[Choise - 1]);
        GUI.DrawTexture(new Rect(UIBorders.x + width*2+ WidthBorder, YY + width, width, width), Smiles[i]);
    }
    void DrawUI()
    {
        float width = Screen.width / 20;
      
        Vector2 UIBorders = Camera.main.WorldToScreenPoint(transform.position);
        float YY = Screen.height / 10;

        for (int i = 0; i < MainChoise.Length; i++)
            GUI.DrawTexture(new Rect(UIBorders.x+ WidthBorder, YY + width * i, width, width), MainChoise[i]);

        for (int i = 0; i < 3; i++)
            GUI.DrawTexture(new Rect(UIBorders.x + width + width * i+ WidthBorder, YY + width * ChoiseY, width, width), SecondChoise[i]);

        GUI.DrawTexture(new Rect(UIBorders.x + width * Choise+ WidthBorder, YY + width * ChoiseY, width, width), ChoiseT);



    }

    void PlayAudio(AudioClip[] clips)
    {
        if (!AU.isPlaying)
        {
            AU.clip = clips[Random.Range(0, clips.Length)];
            AU.Play();
        }
    }

    public void SetTimer(float Plus)
    {
        Timer = Time.fixedTime + Plus;
    }
    

    public void SetChoiseY(int CY)
    { ChoiseY = CY; }

    public void SetChoiseX(int CX)
    { Choise = CX; }
    

}
