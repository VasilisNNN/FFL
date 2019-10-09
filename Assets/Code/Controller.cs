using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Controller : MonoBehaviour
{
    private bool _horizontal_button, Exiting, _vertical_button;
    private int _horizontalScroll_Timer;
    public float Enterdeley { get; set; }

    public float _horizontal { get; set; }
    public float _vertical { get; set; }
    
    public bool exit_b { get; set; }
    public bool enter_axis { get; set; }

    private bool menu_b;

    public bool enter_b { get; set; }
    private float horizontal_b, scrollV;


    public bool joystick = false;
    private float ControllTextureAlpha;
    private int[] JINT;
    private int ChoisedSec, ChoiseY, Choise, ChoisedSmile;

    private Texture2D[] MainChoise;
    private Texture2D[] SecondChoise;
    private Texture2D ChoiseT, LOVET,FG_HP, BG_HP;
    private Texture2D[] Smiles;

    private float X_max,Y_max, CurrentSeg, ChoiseTimer,SmileTimer, LoveTimer, NotLoveTimer;
    private OpponentController OppC;
    private float width, YY;
    private Vector2 UIBorders,ScreenBorder;
    private bool Question, Smile, LoveAdded;
    public bool AnswerSmile { get; set; }
    public bool PlayerMove { get; set; }
    private float Timer;
    private int LOVE,HP;
    private GUISkin skin;
    private Menu _menu;
    private float WidthBorder;
    private Animator anim;
    private ItemDatabase ID;

    private AudioSource AU;
    private AudioClip[] Clicks;
    private AudioClip[] EnterClips;

    void Start()
    {
        HP = 5;
        FG_HP = Resources.Load<Texture2D>("Sprites/UI/Level_FG");
        BG_HP = Resources.Load<Texture2D>("Sprites/UI/Level_BG");

        _menu = GetComponent<Menu>();
        ID = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        skin = Resources.Load<GUISkin>("Sprites/UI/New GUISkin");
        PlayerMove = Question=true;
        width = Screen.width / 20;
        UIBorders = Camera.main.WorldToScreenPoint(transform.position);
        WidthBorder = Screen.width / 10;
        YY = Screen.height/10;

        Smiles = new Texture2D[2];
        for (int i = 0; i < Smiles.Length; i++)
            Smiles[i] = Resources.Load<Texture2D>("Sprites/UI/Smile_" + i);

        JINT = new int[20];
        if (name == "Player")
        {
            GameObject pers = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Pers/Pers_" + PlayerPrefs.GetInt("DateLevel")));
            pers.name = "Pers_" + PlayerPrefs.GetInt("DateLevel");
        }

        OppC = GameObject.Find("Pers_" + PlayerPrefs.GetInt("DateLevel")).GetComponent<OpponentController>();
        OppC.name = "Pers_" + PlayerPrefs.GetInt("DateLevel");

        MainChoise = new Texture2D[5];
        SecondChoise = new Texture2D[3];

        for (int i = 0; i < 5; i++)
        {
            MainChoise[i] = Resources.Load<Texture2D>("Sprites/UI/MainChoise" + i);
           
        }

        

        ChoiseT = Resources.Load<Texture2D>("Sprites/UI/Choise");
        LOVET = Resources.Load<Texture2D>("Sprites/UI/Love");
        X_max = 4;
        Y_max = MainChoise.Length;

        ScreenBorder = new Vector2(Screen.width/10,Screen.height/10);
        anim = GetComponent<Animator>();

        AU = GetComponent<AudioSource>();

        Clicks = new AudioClip[3];
        for(int i =0;i< Clicks.Length;i++)
        Clicks[i] = Resources.Load<AudioClip>("Sound/UI/Click_"+i);

        EnterClips = new AudioClip[3];
        for (int i = 0; i < EnterClips.Length; i++)
        EnterClips[i] = Resources.Load<AudioClip>("Sound/UI/Enter_" + i);

    }

    // Update is called once per frame
    void Update()
    {
        InputSets();
        if (LoveTimer > Time.fixedTime) anim.SetBool("Love", true);
        else anim.SetBool("Love", false);
        if (NotLoveTimer > Time.fixedTime) anim.SetBool("NotLove", true);
        else anim.SetBool("NotLove", false);

        if (menu_b) _menu.Options = !_menu.Options;

        if (!_menu.Options)
        {
            if (LOVE >= 5)
            {
                SceneManager.LoadScene("BossLoad");
                PlayerPrefs.SetInt(OppC.name+"Death", 1);
            }
            if (HP <= -5)
            {
                SceneManager.LoadScene("Home");
                PlayerPrefs.SetInt(OppC.name + "Death", 1);
            }

            if (OppC.AnswerSmile && X_max == 2 && !LoveAdded)
            {
                if (OppC.AnswerINT == Choise&& ID.items[ChoiseY].count[Choise] != 1)
                {
                    LOVE++;
                    if(Choise>0)
                    ID.items[ChoiseY].count[Choise] = 1;
                    

                    LoveTimer = Time.fixedTime + 1;
                    LoveAdded = true;
                }
                if (OppC.AnswerINT != Choise)
                {
                    HP--;
                    NotLoveTimer = Time.fixedTime + 1;
                    LoveAdded = true;
                    ID.items[ChoiseY].count[Choise] = 1;
                }
            }

            


          
            if (PlayerMove&&LoveTimer < Time.fixedTime && NotLoveTimer < Time.fixedTime)
            {
                if (Choise == 0)
                {
                    if (_vertical < 0 && _vertical_button && ChoiseY < Y_max - 1)
                    {
                        ChoiseY++;
                        PlayAudio(Clicks);
                    }
                    if (_vertical > 0 && _vertical_button && ChoiseY > 0)
                    {
                        ChoiseY--;
                        PlayAudio(Clicks);
                    }
                }
                if (Choise > X_max - 1) Choise = (int)(X_max - 1);


                if (_horizontal > 0 && _horizontal_button && Choise < X_max - 1)
                {
                    Choise++;
                    PlayAudio(Clicks);
                }
                if (_horizontal < 0 && _horizontal_button && Choise > 0)
                {
                    Choise--;
                    PlayAudio(Clicks);
                }
            }

            
            float ff = 10;

            if (PlayerMove)
            {
                for (int j = 0; j < SecondChoise.Length; j++)
                    SecondChoise[j] = Resources.Load<Texture2D>("Sprites/UI/SecondChoise" + ChoiseY + "_" + j);


                if (Question)
                {
                    LoveAdded = false;
                    X_max = 4;
                    Y_max = MainChoise.Length;
                    OppC.SetChoiseY(ChoiseY);
                    OppC.SetChoiseX(Choise);

                    if (enter_b && Choise > 0&& ID.items[ChoiseY].count[Choise] !=1)
                    {
                        PlayAudio(EnterClips);
                        CurrentSeg = (Mathf.Pow(ff, Choise) / 10);
                        ChoisedSec = Choise - 1;
                        Choise = 0;
                        Question = false;
                        OppC.AnswerSmile = false;
                        Smile = true;
                        
                    }
                }
                else if (Smile)
                {
                    X_max = 2;
                    Y_max = 1;

                    if (enter_b)
                    {
                        PlayAudio(EnterClips);
                        OppC.SetTimer(1);
                        ChoisedSmile = Choise;
                        PlayerMove = false;

                        OppC.AnswerSmile = true;
                    }

                }
                else if (AnswerSmile)
                {
                    X_max = 2;
                    Y_max = 1;
                    LoveAdded = false;
                    if (enter_b)
                    {
                        PlayAudio(EnterClips);
                        ChoisedSmile = Choise;
                        AnswerSmile = false;
                        OppC.AnswerSmile = true;
                        Timer = Time.fixedTime + 5;
                    }

                }
                else
                {
                    if (OppC.AnswerSmile)
                    {
                        if (Timer > Time.fixedTime)
                        {
                            if (Timer - 1 < Time.fixedTime)
                            {
                                Question = true;
                                OppC.AnswerSmile = false;
                            }
                        }

                    }
                }

            }
            else
            {
                Question = false;
                Smile = false;
                AnswerSmile = false;
            }



        }
    }

    private void OnGUI()
    {
        

        if (OppC.AnswerSmile)
        {
            if (Timer > Time.fixedTime)
            {
                if (Timer - 1 > Time.fixedTime)
                {
                    GUI.DrawTexture(new Rect(UIBorders.x + width * 2 + WidthBorder/2, YY + width, width, width), Smiles[ChoisedSmile]);
                }
            }
        }
            if (PlayerMove)
            {

                if(Question) DrawUI();
                if(Smile) DrawSmile();
                if(AnswerSmile) DrawSmile();
         
            }
            else
            {
                if (OppC.AnswerSmile)
                {
                    GUI.DrawTexture(new Rect(UIBorders.x + width + WidthBorder/2, YY + width, width, width), SecondChoise[ChoisedSec]);
                    GUI.DrawTexture(new Rect(UIBorders.x + width * 2 + WidthBorder/2, YY + width, width, width), Smiles[ChoisedSmile]);
                }
            }


        GUI.DrawTexture(new Rect(ScreenBorder.x, ScreenBorder.y, width, width), LOVET);
        GUI.Box(new Rect(ScreenBorder.x+width, ScreenBorder.y, width, width), LOVE.ToString(),skin.customStyles[0]);

        float HP_Width = width * 2;
        GUI.DrawTexture(new Rect(ScreenBorder.x + width * 2, ScreenBorder.y, HP_Width / 5 * HP, Screen.height / 35), BG_HP);
        GUI.DrawTexture(new Rect(ScreenBorder.x+width*2, ScreenBorder.y, HP_Width, Screen.height / 35), FG_HP);
    }
    void DrawSmile()
    {
      

        float width = Screen.width / 20;
       
        Vector2 UIBorders = Camera.main.WorldToScreenPoint(transform.position);
        float YY = Screen.height / 10;

        for (int i = 0; i < 2; i++)
            GUI.DrawTexture(new Rect(UIBorders.x + width * i+ WidthBorder/2, YY + width, width, width), Smiles[i]);
        GUI.DrawTexture(new Rect(UIBorders.x + width * Choise+ WidthBorder/2, YY + width, width, width), ChoiseT);


    }
    void DrawUI()
    {

        float width = Screen.width / 20;
        Vector2 UIBorders = Camera.main.WorldToScreenPoint(transform.position);
        float YY = Screen.height / 10;

        for (int i = 0; i < MainChoise.Length; i++)
         GUI.DrawTexture(new Rect(UIBorders.x + WidthBorder, YY + width * i, width, width), MainChoise[i]);



        for (int i = 0; i < 3; i++)
        {
            if (ID.items[ChoiseY].count[i+1] != 1)
                GUI.DrawTexture(new Rect(UIBorders.x + width + width * i + WidthBorder, YY + width * ChoiseY, width, width), SecondChoise[i]);
        }
        GUI.DrawTexture(new Rect(UIBorders.x + width * Choise+ WidthBorder, YY + width * ChoiseY, width, width), ChoiseT);



    }

    void InputSets()
    {
      

        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (Input.GetJoystickNames()[i] != "")
                JINT[i] = 1;
            else JINT[i] = 0;

            if (JINT.Sum() == 0)
            {
                joystick = false;
                if (PlayerPrefs.GetFloat("DrawKeyboard") != 1)
                {
                    PlayerPrefs.SetFloat("DrawKeyboard", 1);
                    PlayerPrefs.SetFloat("DrawGamepad", 0);
                    ControllTextureAlpha = 1;
                }
            }
            else
            {
                joystick = true;
                if (PlayerPrefs.GetFloat("DrawGamepad") != 1)
                {
                    PlayerPrefs.SetFloat("DrawGamepad", 1);
                    PlayerPrefs.SetFloat("DrawKeyboard", 0);
                    ControllTextureAlpha = 1;
                }
            }

        }


        if (!joystick)
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");

            _vertical_button = Input.GetButtonDown("Vertical");
            _horizontal_button = Input.GetButtonDown("Horizontal");
            
            
            //atack_b = Input.GetButtonDown ("Atack");
            enter_b = Input.GetButtonDown("Enter");
            enter_axis = Input.GetButton("Enter");
            exit_b = Input.GetButtonDown("Exit");
            menu_b = Input.GetButtonDown("Menu");

        }
        else
        {
            _horizontal = Input.GetAxis("Horizontal_J");
            _vertical = Input.GetAxis("Vertical_J");
            
            _vertical_button = Input.GetButtonDown("Vertical_J");
            

            enter_b = Input.GetKeyDown(KeyCode.JoystickButton2);
            enter_axis = Input.GetKey(KeyCode.JoystickButton2);
            exit_b = Input.GetKey(KeyCode.JoystickButton0);
            menu_b = Input.GetKeyDown(KeyCode.JoystickButton7);
            

        }


    }

    void PlayAudio(AudioClip[] clips)
    {
        if (!AU.isPlaying)
        {
            AU.clip = clips[Random.Range(0, clips.Length)];
            AU.Play();
        }
    }
    public int GetChoisedSmile()
    { return ChoisedSmile; }
    public int GetChoisedSec()
    {
        return ChoisedSec;
    }

}
