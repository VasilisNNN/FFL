using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public UnityEngine.Audio.AudioMixer mg;

    // private Rect[] controller_rect = new Rect[4];

    private bool Sound, Visuals, SetALLOptions;
    private int controllernum;
    private Rect[] rect_options = new Rect[10];
    public Sprite[] ControllerDrawSprt;
    private string[] OptionNamesInLine;
    private GUISkin skin;

    private int ChoisePosition, ChoisePositionY, ChoisePositionScreen, ChoisePositionFullScreen, language;
    private int ChoiseYMax, ChoiseXMax;
    private float timer_e;

    public bool joystick = false;

    public float _horizontal { get; set; }
    public float _vertical { get; set; }

    private bool/* _vertical_button_Up, _vertical_button_Down,*/ _horizontalScroll_button, _verticalScroll_button;
    private float _horizontalScroll_axis, _verticalScroll_axis;

    public bool exit_b { get; set; }
    public bool enter_b { get; set; }
    public bool Options { get; set; }
    // private bool Extras;

    private int[] Sc_width, Sc_height;

    private float MasterV, BGV, MusicV, InDoorV;

    private bool MoveCh, FullScreen = true;

    private string[] ScreenResStrings, FullScreenString, LanguageString;


    private float starttimer;

    private Texture ArrowLeft, ArrowRight, Lock;
    private float ControllTV, ControllTH;
    //	private float MaxInMenuOptions;
    private int[] SoundVolume;
    private Texture[] InnerOptions;
    private float EnterDeley;
    private AudioSource AU;
    private AudioClip EnterClip, ChoiseClip;

    private Texture[] OptionsTextures, MenuTextures, VisualTextures, SoundTextures, SmallMenuTextures;

    private Texture BlackFG, MarginalAct, SmallBG, Choise;

    void Start()
    {
        Lock = Resources.Load<Texture>("Sprites/UI/Lock");
        BlackFG = Resources.Load<Texture>("Sprites/UI/MenuSmallBG");
        SmallBG = Resources.Load<Texture>("Sprites/UI/BGScreen");
        Choise = Resources.Load<Texture>("Sprites/UI/Choise");

        MarginalAct = Resources.Load<Texture>("Sprites/UI/Logo_MA_Pixel_Light");

      

        OptionsTextures = new Texture[3];
        for (int i = 0; i < 2; i++)
            OptionsTextures[i] = Resources.Load<Texture>("Sprites/UI/OptionsTexture" + i);
        OptionsTextures[OptionsTextures.Length - 1] = Resources.Load<Texture>("Sprites/UI/Back");


        MenuTextures = new Texture[3];
        for (int i = 0; i < MenuTextures.Length; i++)
            MenuTextures[i] = Resources.Load<Texture>("Sprites/UI/MenuTexture" + i);

        SmallMenuTextures = new Texture[3];
        SmallMenuTextures[0] = Resources.Load<Texture>("Sprites/UI/MenuTexture0");
        SmallMenuTextures[1] = Resources.Load<Texture>("Sprites/UI/MenuTexture1");
        SmallMenuTextures[2] = Resources.Load<Texture>("Sprites/UI/Back");


        VisualTextures = new Texture[3];
        for (int i = 0; i < 2; i++)
            VisualTextures[i] = Resources.Load<Texture>("Sprites/UI/VisualTexture" + i);
        VisualTextures[VisualTextures.Length - 1] = Resources.Load<Texture>("Sprites/UI/Back");

        SoundTextures = new Texture[4];
        for (int i = 0; i < 3; i++)
            SoundTextures[i] = Resources.Load<Texture>("Sprites/UI/SoundTexture" + i);
        SoundTextures[SoundTextures.Length - 1] = Resources.Load<Texture>("Sprites/UI/Back");

        if (SceneManager.GetActiveScene().name == "StartMenu") Options = true;
        EnterClip = Resources.Load<AudioClip>("Sound/UI/Accept");
        ChoiseClip = Resources.Load<AudioClip>("Sound/UI/Click");


        InnerOptions = MenuTextures;
        SoundVolume = new int[3] { 8, 7, 7 };
        FullScreenString = new string[2];
        OptionNamesInLine = new string[5];


        if (Cursor.visible) Cursor.visible = false;

        //ArrowUp = Resources.Load<Texture>("Sprites/UI/Arrow_Up");
        //ArrowDown  = Resources.Load<Texture>("Sprites/UI/Arrow_Down");
        ArrowLeft = Resources.Load<Texture>("Sprites/UI/LeftArrow");
        ArrowRight = Resources.Load<Texture>("Sprites/UI/RightArrow");


        ScreenResStrings = new string[3] { "1366 * 768", "1280 * 720", "1024 * 768" };


        LanguageString = new string[2] { "RU", "EN" };
        //AuC = new string[3] { "Master", "Background","Objects"};

        Sc_width = new int[4] { 1920, 1366, 1280, 1024 };
        Sc_height = new int[4] { 1080, 768, 720, 768 };

        /*En = Resources.Load<Texture>("Sprites/UI/English");
        Ru = Resources.Load<Texture>("Sprites/UI/Russian");*/

        skin = Resources.Load<GUISkin>("Prefabs/New GUISkin");

        if (GetComponent<AudioSource>() == null)
        {
            print("NO_AUDIO_ON_START! IN MENU");
        }
        AU = GetComponent<AudioSource>();

        for (int i = 0; i < 10; i++)
            rect_options[i] = new Rect(200f, 80f + 40f * i, 400f, 60f);
        Load();
        if (PlayerPrefs.GetFloat("Language") == 0) PlayerPrefs.SetFloat("Language", language);

    }


    void Update()
    {

        InputSets();

        if (Options)
        {
            ControllerGeneral();
        }


    }





    void ControllerGeneral()
    {

        if (ChoisePositionY > ChoiseYMax)
            ChoisePositionY = ChoiseYMax;
        if (ChoisePosition > ChoiseXMax)
            ChoisePosition = ChoiseXMax;

        if (_horizontalScroll_button)
        {
            if (ChoisePosition < ChoiseXMax && _horizontalScroll_axis > 0)
            {
                ChoisePosition++;
                AU.clip = ChoiseClip;
                AU.Play();
            }
            else if (ChoisePosition > 0 && _horizontalScroll_axis < 0)
            {
                ChoisePosition--;
                AU.clip = ChoiseClip;
                AU.Play();
            }
        }
        if (_verticalScroll_button)
        {
            if (ChoisePositionY < ChoiseYMax && _verticalScroll_axis < 0)
            {
                ChoisePositionY++;
                AU.clip = ChoiseClip;
                AU.Play();
            }
            else if (ChoisePositionY > 0 && _verticalScroll_axis > 0)
            {
                ChoisePositionY--;
                AU.clip = ChoiseClip;
                AU.Play();
            }

        }


        SetOptions();
    }


    void SetOptions()
    {
        int MovePositions = 0;
        if (SceneManager.GetActiveScene().name == "StartMenu") MovePositions = 0;
        else MovePositions = -1;
        if (Options)
        {
            if (enter_b)
            {
                if (ChoisePositionY == 0 && !Visuals && !Sound && !SetALLOptions && EnterDeley < Time.fixedTime)
                {
                    if (SceneManager.GetActiveScene().name != "Home")
                    {
                        SceneManager.LoadScene("Home");

                        if (!AU.isPlaying)
                        {
                            AU.clip = EnterClip;
                            AU.Play();
                        }
                    }
                    else
                    {

                        SceneManager.LoadScene("Home");
                    }

                    if (!AU.isPlaying)
                    {
                        AU.clip = EnterClip;
                        AU.Play();
                    }
                }
                if (ChoisePositionY == 1 && !SetALLOptions && !Visuals && !Sound  && EnterDeley < Time.fixedTime)
                {
                    if (!AU.isPlaying)
                    {
                        AU.clip = EnterClip;
                        AU.Play();
                    }
                    SetALLOptions = true;
                    EnterDeley = Time.fixedTime + 0.1f;
                }
               
                if (ChoisePositionY == 2 && !Visuals && !Sound  && !SetALLOptions && EnterDeley < Time.fixedTime)
                {

                    if (SceneManager.GetActiveScene().name == "Home")
                        Application.Quit();
                    else Options = false;

                    if (!AU.isPlaying)
                    {
                        AU.clip = EnterClip;
                        AU.Play();
                    }
                    EnterDeley = Time.fixedTime + 0.1f;

                }






            }
            if (SetALLOptions) SetOuterOptions();
            if (Sound) SetSounds();
            if (Visuals) SetVisuals();

        }
    }





    void DrawOP()
    {
        if (SceneManager.GetActiveScene().name == "StartMenu")
            InnerOptions = MenuTextures;
        else InnerOptions = SmallMenuTextures;

        DrawBoxInnerOptions(InnerOptions.Length - 1, false, InnerOptions.Length, 1, false, false);
    }


    void OnGUI()
    {
        if (Options)
        {

            if (SceneManager.GetActiveScene().name != "StartMenu")
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), BlackFG);

            GUI.DrawTexture(new Rect(100, Screen.height - 100, 350, 40), MarginalAct);



            if (Sound) DrawSound();
            else if (SetALLOptions) DrawOptions();
            else if (Visuals) DrawVisuals();
            else DrawOP();
        }

    }

    //VISUALS
    void SetVisuals()
    {
        if (ChoisePositionY == 0)
        {

            ChoiseXMax = 3;
            ChoisePositionScreen = SetInHorPlusMinus(ChoisePositionScreen);

        }
        if (ChoisePositionY == 1)
        {

            ChoiseXMax = 1;
            ChoisePositionFullScreen = SetInHorPlusMinus(ChoisePositionFullScreen);
            if (ChoisePosition == 0) FullScreen = true;
            else FullScreen = false;

        }

        if (ChoisePositionY == InnerOptions.Length - 1 && enter_b && EnterDeley < Time.fixedTime)
        {
            if (!AU.isPlaying)
            {
                AU.clip = EnterClip;
                AU.Play();
            }
            PlayerPrefs.SetFloat("FullScreen", ChoisePositionFullScreen);
            PlayerPrefs.SetFloat("ScreenSize", ChoisePositionScreen);
            Screen.SetResolution(Sc_width[ChoisePositionScreen], Sc_height[ChoisePositionScreen], FullScreen, 1);
            Visuals = false;
            SetALLOptions = true;
            EnterDeley = Time.fixedTime + 0.1f;
        }

    }
    void DrawVisuals()
    {
        if (Screen.fullScreen)
            VisualTextures[1] = Resources.Load<Texture>("Sprites/UI/VisualTexture1_0");
        else
            VisualTextures[1] = Resources.Load<Texture>("Sprites/UI/VisualTexture1_1");

        InnerOptions = VisualTextures;
        DrawBoxInnerOptions(InnerOptions.Length - 1, true, InnerOptions.Length, 1, true, false);

    }


    //SOUND

    void SetSounds()
    {
        SetMixer();

        if (ChoisePositionY == 0)
        {
            //	print (SoundVolume [ChoisePositionY]);
            ChoiseXMax = 9;
            SoundVolume[ChoisePositionY] =
        SetInHorPlusMinus(SoundVolume[ChoisePositionY]);
        }
        if (ChoisePositionY == 1)
        {
            ChoiseXMax = 9;
            SoundVolume[ChoisePositionY] =
                SetInHorPlusMinus(SoundVolume[ChoisePositionY]);
        }
        if (ChoisePositionY == 2)
        {
            ChoiseXMax = 9;
            SoundVolume[ChoisePositionY] =
                SetInHorPlusMinus(SoundVolume[ChoisePositionY]);
        }

        if (ChoisePositionY == InnerOptions.Length - 1 && enter_b && EnterDeley < Time.fixedTime)
        {
            AU.clip = EnterClip;
            AU.Play();
            Sound = false;
            SetALLOptions = true;
            EnterDeley = Time.fixedTime + 0.1f;
        }

    }

    void DrawSound()
    {
        InnerOptions = SoundTextures;
        DrawBoxInnerOptions(InnerOptions.Length - 1, true, InnerOptions.Length, 1, false, true);
    }

    void SetOuterOptions()
    {


        if (ChoisePositionY == 0 && enter_b && EnterDeley < Time.fixedTime)
        {
            Visuals = true;
            SetALLOptions = false;
            AU.clip = EnterClip;
            AU.Play();
            EnterDeley = Time.fixedTime + 0.1f;
        }
        if (ChoisePositionY == 1 && enter_b && EnterDeley < Time.fixedTime)
        {
            Sound = true;
            SetALLOptions = false;
            AU.clip = EnterClip;
            AU.Play();
            EnterDeley = Time.fixedTime + 0.1f;
        }

        if (ChoisePositionY == InnerOptions.Length - 1 && enter_b && EnterDeley < Time.fixedTime)
        {
            AU.clip = EnterClip;
            AU.Play();
            SetALLOptions = false;
            EnterDeley = Time.fixedTime + 0.1f;
        }


    }
    void DrawOptions()
    {
        InnerOptions = OptionsTextures;
        DrawBoxInnerOptions(InnerOptions.Length - 1, false, InnerOptions.Length, 1, false, false);
    }



    void DrawBoxInnerOptions(int MaxArrows, bool DrawArrows, int YPosMax, int XPosMax, bool DrawScreen, bool DrawAUDIO)
    {

        float SH = Screen.height / 6;
        float Width = Screen.height / 6;
        int XPos = 0;
        int YPos = 0;
        float LeftBorder = -Width/2;
        ChoiseYMax = YPosMax - 1;
        ChoiseXMax = XPosMax - 1;

        GUI.DrawTexture(new Rect(Screen.width / 2 + LeftBorder - Width / 4, SH - Width / 2, Width * XPosMax + Width / 2, Width * (YPosMax + 1)), SmallBG);


        GUI.DrawTexture(new Rect(Screen.width / 2 + LeftBorder + Width * ChoisePosition, SH + Width * ChoisePositionY, Width, Width), Choise);

        for (int i = 0; i < InnerOptions.Length; i++)
        {
            Color color = GUI.color;
            if (i != ChoisePosition + ChoisePositionY * (YPosMax))
                GUI.color = new Color(1f, 1, 1, 0.7f);
            else GUI.color = color;
            GUI.DrawTexture(new Rect(Screen.width / 2 + LeftBorder + Width * XPos, SH + Width * YPos, Width, Width), InnerOptions[i]);

            if (PlayerPrefs.GetInt("Unlocked_" + i) != 1 && YPosMax == 4 && XPosMax == 4 && i < InnerOptions.Length - 1)
                GUI.DrawTexture(new Rect(Screen.width / 2 + LeftBorder + Width * XPos, SH + Width * YPos, Width, Width), Lock);

            XPos++;
            if (XPos >= XPosMax)
            {
                YPos++;
                XPos = 0;
            }
            GUI.color = color;
        }

        if (DrawScreen)
        {
            GUI.Box(new Rect(Screen.width / 2 + LeftBorder + Width * XPos, SH, Width, Width), Sc_width[ChoisePositionScreen].ToString() + " * " + Sc_height[ChoisePositionScreen].ToString(), skin.customStyles[12]);
        }
        if (DrawAUDIO)
        {
            for (int i = 0; i < 3; i++)
                GUI.Box(new Rect(Screen.width / 2 + LeftBorder + Width * XPos, SH + Width * i, Width, Width), SoundVolume[i].ToString(), skin.customStyles[12]);

        }

        if (DrawArrows)
        {
            if (ChoisePositionY < MaxArrows)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - Width + LeftBorder, SH + Width * ChoisePositionY, Width, Width), ArrowLeft);
                GUI.DrawTexture(new Rect(Screen.width / 2 + Width + LeftBorder, SH + Width * ChoisePositionY, Width, Width), ArrowRight);
            }
        }

    }


    void Load()
    {
        mg = Resources.Load<UnityEngine.Audio.AudioMixer>("Sound/NewAudioMixer");


        int j = 0;
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {

            if (Input.GetJoystickNames()[i] == "")
            {
                j++;
            }
        }
        if (j == Input.GetJoystickNames().Length)
        {
            PlayerPrefs.SetFloat("JoyStickOn", 0);
            joystick = false;
        }

        if (PlayerPrefs.GetFloat("FullScreen") == 0)
            FullScreen = true;
        else
            FullScreen = false;

        Screen.SetResolution(Sc_width[(int)PlayerPrefs.GetFloat("ScreenSize")], Sc_height[(int)PlayerPrefs.GetFloat("ScreenSize")], FullScreen, 1);


        if (PlayerPrefs.GetFloat("Master_V") != 0)
            SoundVolume[0] = (int)PlayerPrefs.GetFloat("Master_V") / 10 + 8;
        if (PlayerPrefs.GetFloat("BG_V") != 0)
            SoundVolume[1] = (int)PlayerPrefs.GetFloat("BG_V") / 10 + 8;
        if (PlayerPrefs.GetFloat("Objects_V") != 0)
            SoundVolume[2] = (int)PlayerPrefs.GetFloat("Objects_V") / 10 + 8;

        mg.SetFloat("Master", PlayerPrefs.GetFloat("Master_V"));
        mg.SetFloat("BG", PlayerPrefs.GetFloat("BG_V"));
        mg.SetFloat("Objects", PlayerPrefs.GetFloat("Objects_V"));


        if (PlayerPrefs.GetFloat("Language") < 1)
            language = 0;
        else
            language = 1;

        ChoisePositionFullScreen = (int)PlayerPrefs.GetFloat("FullScreen");

        if (ChoisePositionFullScreen == 0) FullScreen = true;
        else if (ChoisePositionFullScreen == 1) FullScreen = false;

        ChoisePositionScreen = (int)PlayerPrefs.GetFloat("ScreenSize");

    }

    void SetMixer()
    {
        print("PlayerPrefs.GetFloat(Master_V)" + PlayerPrefs.GetFloat("Master_V"));
        mg.SetFloat("Master", -80 + SoundVolume[0] * 10);
        PlayerPrefs.SetFloat("Master_V", -80 + SoundVolume[0] * 10);

        mg.SetFloat("BG", -80 + SoundVolume[1] * 10);
        PlayerPrefs.SetFloat("BG_V", -80 + SoundVolume[1] * 10);

        mg.SetFloat("Objects", -80 + SoundVolume[2] * 10);
        PlayerPrefs.SetFloat("Objects_V", -80 + SoundVolume[2] * 10);
    }


    int SetInHorPlusMinus(int t)
    {
        if (t > ChoiseXMax)
            t = ChoiseXMax;
        if (_horizontalScroll_button)
        {
            if (t < ChoiseXMax && _horizontalScroll_axis > 0)
                t++;
            else if (t > 0 && _horizontalScroll_axis < 0)
                t--;

        }
        return t;
    }

    void InputSets()
    {

        _horizontalScroll_button = Input.GetButtonDown("Horizontal");
        _horizontalScroll_axis = Input.GetAxis("Horizontal");
        _verticalScroll_button = Input.GetButtonDown("Vertical");
        _verticalScroll_axis = Input.GetAxis("Vertical");


        enter_b = Input.GetButtonDown("Enter");

        exit_b = Input.GetButtonDown("Exit");



    }
    /* public void Forget()
     {
         PlayerPrefs.DeleteAll();

         if (language <= 0) PlayerPrefs.SetFloat("Language", -1);
         if (language == 1) PlayerPrefs.SetFloat("Language", 1);

         PlayerPrefs.SetFloat("FullScreen", ChoisePositionFullScreen);
         PlayerPrefs.SetFloat("ScreenSize", ChoisePositionScreen);


         SetMixer();
     }*/
}




