using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Revard : MonoBehaviour
{
   private int MoneyForCharacter, FineForCharacter, Damage, HPPlus, Money;
    private List<Sprite> Numbs = new List<Sprite>();
    private float ExitTimer, ExitTimerMax;
    private GUISkin skin;
    private bool AddedMoney,FineMoney;
    private bool enter_b;
    private bool joystick;
    private Texture2D BlockTexture, DamageTexture, MoneyTexture, Wood,Coal,Diamond, RevardTexture;
   
    void Start()
    {
        MoneyTexture = Resources.Load<Texture2D>("Sprites/UI/Money");
        DamageTexture = Resources.Load<Texture2D>("Sprites/UI/HPReduce");
        BlockTexture = Resources.Load<Texture2D>("Sprites/UI/ComfortUp1");

        Wood = Resources.Load<Texture2D>("Sprites/UI/RevardWood");
        Coal = Resources.Load<Texture2D>("Sprites/UI/RevardCoal");
        Diamond = Resources.Load<Texture2D>("Sprites/UI/RevardDiamont");

        ExitTimerMax = 6;
        ExitTimer = Time.fixedTime;
        skin = Resources.Load<GUISkin>("Prefabs/New GUISkin");
        for (int i = 0; i < 10; i++)
        {
            Numbs.Add(Resources.Load<Sprite>("Sprites/UI/Nums/" + i));
        }
            MoneyForCharacter = 10;

        Damage = PlayerPrefs.GetInt("Damage" + PlayerPrefs.GetInt("CurrentLevel"));
        HPPlus = PlayerPrefs.GetInt("HPPlus" + PlayerPrefs.GetInt("CurrentLevel"));
        Money = PlayerPrefs.GetInt("Money" + PlayerPrefs.GetInt("CurrentLevel"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerPrefs.GetFloat("JoyStickOn") == 1)
            joystick = true;
        else
            joystick = false;

        if (!joystick)
        {
            enter_b = Input.GetButtonDown("Enter");
        }
        else
        {
            if (PlayerPrefs.GetFloat("JoyStickType") == 0)
            {
                enter_b = Input.GetKeyDown(KeyCode.JoystickButton2);

            }
            if (PlayerPrefs.GetFloat("JoyStickType") == 1)
            {
                enter_b = Input.GetKeyDown(KeyCode.JoystickButton3);

            }
        }

        if (enter_b)
        {
          if (ExitTimer + ExitTimerMax > Time.fixedTime&& ExitTimer + ExitTimerMax < Time.fixedTime + ExitTimerMax - 0.2f) ExitTimer -= ExitTimerMax / 3;
        }


        if (ExitTimer + ExitTimerMax < Time.fixedTime && enter_b)
        {
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
            SceneManager.LoadScene("Home");
        }
    }

    private void OnGUI()
    {
        int PCount = PlayerPrefs.GetInt("PCount");
        int Death = PlayerPrefs.GetInt("Death");

        /* int finalmoney = MoneyForCharacter * PCount;
         int fine = FineForCharacter * PlayerPrefs.GetInt("DeadCharacters");*/

        int finalmoney = MoneyForCharacter * PCount;
        int fine = 8*Death;

        
        
        float w = Screen.height / 7;
        float XPos = 30;
        float YPos = w;
     

        int Final = (int)(HPPlus - Damage + Money/50);



        if (Final<0) RevardTexture = Wood;

        if (Final>=0&& Final<5) RevardTexture = Coal;

        if (Final>=5) RevardTexture = Diamond;

        if (ExitTimer + ExitTimerMax / 4 < Time.fixedTime)
        {
            GUI.DrawTexture(new Rect(XPos, YPos, w, w), DamageTexture);
            GUI.Box(new Rect(XPos+w, YPos, w, w), "x " + Damage, skin.customStyles[1]);


        }
        if (ExitTimer + ExitTimerMax / 3 < Time.fixedTime)
        {
            GUI.DrawTexture(new Rect(XPos, YPos*2, w, w), BlockTexture);
            GUI.Box(new Rect(XPos+w, YPos*2, w, w), "x " + HPPlus, skin.customStyles[1]);
            
        }

        if (ExitTimer + ExitTimerMax / 2 < Time.fixedTime)
        {
            GUI.DrawTexture(new Rect(XPos, YPos * 3, w, w), MoneyTexture);
            GUI.Box(new Rect(XPos + w, YPos * 3, w, w), "x " + Money, skin.customStyles[1]);
        }
        
        if (ExitTimer + ExitTimerMax / 1.5f < Time.fixedTime)
        GUI.DrawTexture(new Rect(Screen.width / 2 - w*2, Screen.height / 2 - w*2, w * 4, w * 4 ), RevardTexture);
        

        if (ExitTimer + ExitTimerMax < Time.fixedTime)
           GUI.Box(new Rect(Screen.width/2-50, YPos * 5 + 10, 100, 100), "e - Exit", skin.customStyles[1]);
           
        
       // GUI.DrawTexture(new Rect(10, 10, w, w), MoneyTexture);
       // GUI.Box(new Rect(20+w, 10, w, w), " x " + PlayerPrefs.GetInt("Money") + "₴", skin.customStyles[1]);

       
    }

}
