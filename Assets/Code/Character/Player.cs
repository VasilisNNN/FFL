using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public bool CutSceenMode { get; set; }
    public float HP { get; set; }
    public bool Options { get; set; }
    private List<GameObject> coll_obj = new List<GameObject>();
    private List<GameObject> Legscoll_obj = new List<GameObject>();
    private List<GameObject> Fightcoll_obj = new List<GameObject>();
    private int[] JINT;

    //private GameObject[] LevelingSprites;
    
    private CharacterController2D _controller;
    private float _normalHSpeed, _normalVSpeed;

    private float MaxSpeed = 0.2f;
    private Animator anim;

    public bool ChoiseInterface { get; set; }
    public bool _gameover { get; set; }

    private Transform _transform;

    private float startscale;
    public float _horizontal { get; set; }
    public float _vertical { get; set; }
    public bool _vertical_button { get; set; }


    private bool _vertical_button_Up, _vertical_button_Down,Exiting, _horizontal_button_Push, _vertical_button_Push,RageHit;
    public float Enterdeley { get; set; }
    
    public bool exit_b { get; set; }
    public bool enter_axis { get; set; }

    private bool menu_b;
    private float side;

    public bool enter_b { get; set; }
    private float horizontal_b;
    public bool joystick = false;
    private float StartAlpha = 1;
    private float SleepAlpha = 0.5f;

    private Menu menu;
    private GUISkin skin;
    
    private AudioSource WalkAU;
    private AudioClip[] Steps;

    private Texture2D SaveTexture;
    private float SaveAlpha;
    public float Money { get; set; }

    private float DrawParameterChange = 1;
   

    private float MoneyChange;
    private float InvisTimer, SleepTimer, SleepDeley, secondstimer, seconds;
   
    private Texture2D BGScreen, SleepTexture,MoneyTexture;
    private float BlockTimer,PunchTimer;
    public bool DrawCh { get; set; }

    private float ControllTextureAlpha;
    private Texture2D ChoiseTexture;

    private string ExitingString;
    private bool _Character_Punch;
    private float _Character_Block;
    public bool PunchDelay;
    public bool AlwaysMove;
    private List<Vector3> Positions = new List<Vector3>();
    public int MaxPositions;
    public int YPos { get; set; }
    public int XPos { get; set; }
    private DeathAnim DeathAn;
    private TrapGenerator TG;
    private LevelDatabase LevelData;
    private float WBorder, HBorder, RageCount, LastFrameMoney;
    private Texture FG_HP, BG_HP, StartHead, FinishtHead, GamepadT, KeyboardT, BG_HP_Black, BG_HP_White;
    public float MaxSeconds { get; set; }
    private Sprite FinishtHeadSPRT;

    private int HPPlus, MaxRage;
    private AudioSource Damage;
    private AudioSource Hit;
    private AudioSource BossDamage;

    private List<AudioClip> DamageClips = new List<AudioClip>();
    private List<AudioClip> HitClips = new List<AudioClip>();
    private List<AudioClip> BossDamageClips = new List<AudioClip>();

    private void Awake()
    {
        WalkAU = GetComponent<AudioSource>();
        Damage = transform.Find("LegsZone").GetComponent<AudioSource>();
        Hit = transform.Find("BodySPRT").GetComponent<AudioSource>();
        BossDamage = transform.Find("RightSound").GetComponent<AudioSource>();

        Steps = new AudioClip[4];
        for (int i = 0; i < Steps.Length; i++)
            Steps[i] = Resources.Load<AudioClip>("Sound/Steps/WaterSteps_" + i);

        if (SceneManager.GetActiveScene().name == "Home") MaxSpeed = 0.1f;
         MaxRage = 150;
        YPos = 0;
        FG_HP = Resources.Load<Texture>("Sprites/UI/Level_FG");
        BG_HP = Resources.Load<Texture>("Sprites/UI/Level_BG");
        GamepadT = Resources.Load<Texture2D>("Sprites/UI/Gamepad_icon");
        KeyboardT = Resources.Load<Texture2D>("Sprites/UI/KeyBoard_icon");
        BG_HP_Black = Resources.Load<Texture>("Sprites/UI/Level_BG_Black");
        BG_HP_White = Resources.Load<Texture>("Sprites/UI/Level_BG_White");
        side = 1;
        JINT = new int[20];

        for (int i = 0; i < 3; i++)
            DamageClips.Add(Resources.Load<AudioClip>("Sound/Explosions/SmallExplosion"));

        for (int i = 0; i < 3; i++)
            HitClips.Add(Resources.Load<AudioClip>("Sound/Explosions/SmallExplosion"));

        for (int i = 0; i < 3; i++)
            BossDamageClips.Add(Resources.Load<AudioClip>("Sound/Explosions/SmallExplosion"));


        if (PlayerPrefs.GetInt("CurrentLevel")>=2) Money = 10;

        if (AlwaysMove)
        {
            LevelData = GameObject.Find("LevelDatabase").GetComponent<LevelDatabase>();
            TG = GameObject.Find("TrapGenerator").GetComponent<TrapGenerator>();

           
            for (int i = 0; i < 4; i++)
                Positions.Add(GameObject.Find("Position (" + i + ")").transform.position);
        }
        DeathAn = GetComponent<DeathAnim>();
        //if (AlwaysMove) transform.position = Positions[YPos];

        if (SceneManager.GetActiveScene().name == "Market")
        {
            PlayerPrefs.SetInt("Death", 0);
            PlayerPrefs.SetInt("PCount", 0);
        }
        
        StartHead = Resources.Load<Texture2D>("Sprites/Pers/Faces/PlayerFace_Transperent");
        FinishtHead = Resources.Load<Texture2D>("Sprites/Pers/Faces/Face_" + PlayerPrefs.GetInt("CurrentLevel"));
        FinishtHeadSPRT = Resources.Load<Sprite>("Sprites/Pers/Faces/Face_" + PlayerPrefs.GetInt("CurrentLevel"));

        ChoiseTexture = Resources.Load<Texture2D>("Sprites/UI/ChoiseIcon");
        
        SleepDeley = 140;
        if (SceneManager.GetActiveScene().name !="Level7") SleepTimer = Time.fixedTime + SleepDeley;
       else SleepTimer = Time.fixedTime + 30f;

        BGScreen = Resources.Load<Texture2D>("Sprites/UI/BGScreen");
        SleepTexture = Resources.Load<Texture2D>("Sprites/UI/BGScreen");
        
        MoneyTexture = Resources.Load<Texture2D>("Sprites/UI/MoneyBorder");

        skin = Resources.Load<GUISkin>("Sprites/UI/New GUISkin");
        if (SceneManager.GetActiveScene().name == "CutSceen") CutSceenMode = true;
        
        SaveTexture = Resources.Load<Texture2D>("Sprites/UI/Flopy");
        if (PlayerPrefs.GetFloat("Language") == 0)
        {
            PlayerPrefs.SetFloat("Language", 1);
        }
        _transform = transform;
        anim = GetComponent<Animator>();
        if (!CutSceenMode)
            _controller = GetComponent<CharacterController2D>();

       
        menu = GetComponent<Menu>();
        startscale = _transform.localScale.x;
        
            PlayerPrefs.SetInt("Death", 0);
            PlayerPrefs.SetInt("PCount", 0);

        HP = 3;
        if (transform.Find("QButtonPush") != null)
        {
            if (transform.Find("QButtonPush").gameObject != null &&
            PlayerPrefs.GetInt("CurrentLevel") != 2) Destroy(transform.Find("QButtonPush").gameObject);
        }

        if (transform.Find("EButtonBlock") != null)
        {
            if (transform.Find("EButtonBlock").gameObject != null &&
            PlayerPrefs.GetInt("CurrentLevel") != 1) Destroy(transform.Find("EButtonBlock").gameObject);
        }

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("LevelAnim").Length; i++)
            GameObject.FindGameObjectsWithTag("LevelAnim")[i].GetComponent<Animator>().SetInteger("Level", PlayerPrefs.GetInt("CurrentLevel"));

        if (SceneManager.GetActiveScene().name == "Home")
        PlayerPrefs.SetInt("Lips", -1);
        if (PlayerPrefs.GetInt("CurrentLevel") < 0) PlayerPrefs.SetInt("CurrentLevel", 0);
        MaxSeconds = 100;
    }


    void Update()
    {
        

        if (GameObject.Find("FightBox")!=null)
        Fightcoll_obj = GameObject.Find("FightBox").GetComponent<CollList>().GetCollList();

        

        if (PlayerPrefs.GetInt("CurrentLevel") < 0) PlayerPrefs.SetInt("CurrentLevel", 0);
        LastFrameMoney = Money;
        if (SceneManager.GetActiveScene().name == "Level0")
        {
            if (transform.Find("EButtonPush") != null)
            {
                if (transform.Find("EButtonPush").gameObject != null && PunchTimer > Time.fixedTime) Destroy(transform.Find("EButtonPush").gameObject);


            }
            if (transform.Find("QButtonBlock") != null)
            {
                if (transform.Find("QButtonBlock").gameObject != null && BlockTimer > Time.fixedTime) Destroy(transform.Find("QButtonBlock").gameObject);
            }
        }else {
            if (transform.Find("QButtonPush") != null)
                Destroy(transform.Find("QButtonPush").gameObject);
            if (transform.Find("EButtonBlock") != null)
                Destroy(transform.Find("EButtonBlock").gameObject);
              }


    Options = menu.Options;

        WBorder = Screen.width / 10;
        HBorder = Screen.height / 10;

        if(LevelData!=null)
        MaxPositions = LevelData.level[PlayerPrefs.GetInt("DateLevel")].MaxNum;
        
        if (HP <= 0) DeathAn.Dead = true;

        if (Input.GetKeyDown(KeyCode.F12))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (PlayerPrefs.GetInt("DateLevel") > 0 && Input.GetKeyDown(KeyCode.Minus))
        {
            PlayerPrefs.SetInt("DateLevel", PlayerPrefs.GetInt("DateLevel") - 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Equals) && PlayerPrefs.GetInt("DateLevel") < 5)
        {
            PlayerPrefs.SetInt("DateLevel", PlayerPrefs.GetInt("DateLevel") + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (TG != null)
        {
            if (PlayerPrefs.GetInt("CurrentLevel") > TG.MaxLevels) PlayerPrefs.SetInt("CurrentLevel", TG.MaxLevels - 1);
        }



        if (!_gameover)
        {
            if (InvisTimer > Time.fixedTime) transform.Find("Head").GetComponent<SpriteRenderer>().color = GameObject.Find("Player (1)").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            else transform.Find("Head").GetComponent<SpriteRenderer>().color = GameObject.Find("Player (1)").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
         
        }
        else
        {
            GameObject.Find("Head").GetComponent<SpriteRenderer>().color = GameObject.Find("Player (1)").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            anim.SetBool("Death", true);
            
        }


        InputSets();

        if (!CutSceenMode)
        {
            if (PlayerPrefs.GetInt("FirstRun") != 1)
                PlayerPrefs.SetInt("FirstRun", 1);
            Anim();
        }
        Exit();
        HandleInput();

        if (!_gameover && !Options)
        {

            GameObject[] expl = GameObject.FindGameObjectsWithTag("Explosion");

            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            GameObject[] mines = GameObject.FindGameObjectsWithTag("Mine");
            GameObject[] money = GameObject.FindGameObjectsWithTag("Money");
            GameObject[] block = GameObject.FindGameObjectsWithTag("Block");
            GameObject[] push = GameObject.FindGameObjectsWithTag("Push");
            GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
            GameObject[] hpplus = GameObject.FindGameObjectsWithTag("HPPlus");

            if (HP > 0 && !CutSceenMode)
            {
                
                    
                if (PunchDelay && PunchTimer < Time.fixedTime) PunchDelay = false;

                float HP_Width = Screen.width - WBorder * 8;
                if (TG != null)
                {
                    if (TG.BOSSHP <= 0)
                    {
                        for (int m = 0; m < bosses.Length; m++)
                            BlowThis(bosses[m]);
                    }
                    else
                    {
                        for (int m = 0; m < bosses.Length; m++)
                        {
                            if (Fightcoll_obj.Contains(bosses[m])&&PunchTimer>Time.fixedTime&& !PunchDelay)
                            {
                                TG.BOSSHP--;
                                _transform.position = GameObject.Find("PlayerStart").transform.position;
                                DestroyAllTraps();
                                for (int b = 0; b < mines.Length; b++)
                                Destroy(mines[b]);
                                
                                    if (!BossDamage.isPlaying)
                                {
                                    BossDamage.clip = BossDamageClips[Random.Range(0, BossDamageClips.Count)];
                                    BossDamage.Play();
                                }
                                PunchDelay = true;
                            }

                        }
                    }
                }
                
                for (int i = 0; i < expl.Length; i++)
                if (!expl[i].GetComponent<SpriteRenderer>().enabled) Destroy(expl[i]);

                
                for (int i = 0; i < hpplus.Length; i++)
                {
                    if (Legscoll_obj.Contains(hpplus[i]))
                    {
                      HP++;
                      Destroy(hpplus[i]); }
                }

                for (int i = 0; i < mines.Length; i++)
                {
                   
                        if (Legscoll_obj.Contains(mines[i]))
                    HPDamage(mines[i]);
                }

                for (int i = 0; i < bullets.Length; i++)
                {
                    if (Fightcoll_obj.Contains(bullets[i]) && PunchTimer > Time.fixedTime && !PunchDelay)
                    {
                        BlowThis(bullets[i]);
                    }
                    //if (PushTimer <= Time.fixedTime)
                    if (Legscoll_obj.Contains(bullets[i]))
                    {
                        HPDamageNoBlow(bullets[i]);
                    }
                   // else PushObject(bullets[i]);
                }

                for (int i = 0; i < block.Length; i++)
                {
                    if (Fightcoll_obj.Contains(block[i]) && PunchTimer > Time.fixedTime && !PunchDelay)
                    {
                        BlowThis(block[i]);
                    }

                    if (block[i].transform.Find("EButtonBlock") != null)
                    {
                        if (PlayerPrefs.GetInt("CurrentLevel") != 1) Destroy(block[i].transform.Find("EButtonBlock").gameObject);
                    }

                    if (BlockTimer <= Time.fixedTime)
                    {
                        if (Legscoll_obj.Contains(block[i])) HPDamage(block[i]);

                        if (Legscoll_obj.Contains(block[i])) DropFromTheTop(block[i].transform,false);
                    }
                    else if (Legscoll_obj.Contains(block[i]))
                    {
                        HPPlus++;
                        DropFromTheTop(block[i].transform,true);
                        Destroy(block[i]);
                    }
                    
                }

                for (int i = 0; i < push.Length; i++)
                {

                    if (Fightcoll_obj.Contains(push[i]) && PunchTimer > Time.fixedTime && !PunchDelay)
                    {
                        BlowThis(push[i]);
                    }

                    if (BlockTimer <= Time.fixedTime)
                    {
                        if (Legscoll_obj.Contains(push[i])) HPDamage(push[i]);

                        if (Legscoll_obj.Contains(push[i])) DropFromTheTop(push[i].transform, false);
                    }
                    
                }

                for (int i = 0; i < money.Length; i++)
                {

                    if (Legscoll_obj.Contains(money[i]))
                    {
                        Money++;
                        Destroy(money[i]);
                    }

                     

                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameObject.Find("ENDHead") != null)
            GameObject.Find("ENDHead").GetComponent<SpriteRenderer>().sprite = FinishtHeadSPRT;
        if (HP > 0 && !CutSceenMode && !Options&&!_gameover)
        {
            if (AlwaysMove)
            {

                float SpeedPos = 0.1f;
                float SpeedX = 0.3f;

                if (transform.position.y != Positions[YPos].y)
                {
                    if (transform.position.y < Positions[YPos].y)
                        transform.position = new Vector3(transform.position.x, transform.position.y + SpeedPos, transform.position.z);
                    if (transform.position.y > Positions[YPos].y)
                        transform.position = new Vector3(transform.position.x , transform.position.y - SpeedPos, transform.position.z);

                }
               
                    if (transform.position.x < SpeedX * XPos)
                    {
                        InvisTimer = Time.fixedTime + 0.1f;
                        transform.position = new Vector3(transform.position.x + SpeedPos, transform.position.y, transform.position.z);
                    }
                    if (transform.position.x > SpeedX * XPos)
                    {
                        InvisTimer = Time.fixedTime + 0.1f;
                        transform.position = new Vector3(transform.position.x - SpeedPos, transform.position.y, transform.position.z);
                    }
                
            }
        }

        /* GameObject[] pillar = GameObject.FindGameObjectsWithTag("Pillar");
         for (int i = 0; i < pillar.Length; i++)
         {

             Transform pilT = pillar[i].transform;
             if (pilT.childCount >= 1)
             {
                 if (pilT.GetChild(0).position.y > pilT.position.y)
                 {
                     for (int ch = 0; ch < pilT.childCount; ch++)
                         pilT.GetChild(ch).position = new Vector3(pilT.GetChild(ch).position.x, pilT.GetChild(ch).position.y - 0.1f, pilT.GetChild(ch).position.z);

                 }
             }
             if (pilT.childCount > 0)
             {
                 float currentchild = ((pilT.position.y * 10) * -1) / 3;

                 if (pilT.position.x < pilT.parent.position.x + 0.1f * currentchild-0.1f)
                 {
                     pilT.position = new Vector3(pilT.position.x + 0.1f, pilT.position.y, pilT.position.z);
                 }
                 else
                 {
                     pilT.position = new Vector3(pilT.parent.position.x + 0.1f * currentchild, pilT.position.y, pilT.position.z);
                     if (pilT.childCount > 0)
                         pilT.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
                 }
             }
             else Destroy(pillar[i]);
         }
         */


    }

    private void LateUpdate()
    {
        if (LastFrameMoney != Money)
        {
            // print("MoneyDraw");
            MoneyChange = Money - LastFrameMoney;
            DrawParameterChange = 1.3f;
        }

        if (Options)
        anim.enabled =false;
        else anim.enabled = true;
       /* string BossName = "Boss_Level_" + PlayerPrefs.GetInt("CurrentLevel") + "(Clone)";
        if (GameObject.Find(BossName) != null)
        {
            if (Options || _gameover)
                GameObject.Find(BossName).GetComponent<Animator>().enabled = false;
            else GameObject.Find(BossName).GetComponent<Animator>().enabled = true;
        }*/

        if (RageCount > 0)
        {
            RageCount-=0.4f;
            Time.timeScale = 0.3f;
        }else Time.timeScale = 1;
        

        if (RageHit && Money >= 150&& RageCount<=0)
        {
            Money -= 150;
            RageCount = 150;
            if (!Hit.isPlaying)
            {
                Hit.clip = HitClips[Random.Range(0, HitClips.Count)];
                Hit.Play();
            }
        }
        
        else GameOverControll();
          
        
        if (enter_b && _gameover)
        {
            ExitingString = SceneManager.GetActiveScene().name;
            Exiting = true;
        }
        if (TG != null)
        {
            if (TG.BOSSHP <= 0)
            {
                ExitingString = "Home";
                Exiting = true;
            }
        }
    }
    //INPUT
    private void HandleInput()
    {

        if (PunchTimer < Time.fixedTime)
        {
            anim.SetBool("Punch", false);
            if (_Character_Punch) PunchTimer = Time.fixedTime + 0.25f;
        }
        else anim.SetBool("Punch", true);

        if (BlockTimer < Time.fixedTime) anim.SetBool("Block", false);

        for (int i = 0; i < coll_obj.Count; i++)
        {
            if (coll_obj[i] != null)
            {
                if (coll_obj[i].GetComponent<BoxCollider2D>() != null)
                {
                    if (!coll_obj[i].GetComponent<BoxCollider2D>().enabled)
                    {
                        coll_obj.RemoveAt(i);

                    }
                }
            }
            else coll_obj.RemoveAt(i);
        }
        for (int i = 0; i < Legscoll_obj.Count; i++)
        {
            if (Legscoll_obj[i] != null)
            {
                if (Legscoll_obj[i].GetComponent<BoxCollider2D>() != null)
                {
                    if (!Legscoll_obj[i].GetComponent<BoxCollider2D>().enabled)
                    {
                        Legscoll_obj.RemoveAt(i);

                    }
                }
            }
            else Legscoll_obj.RemoveAt(i);
        }
        
        if (Application.loadedLevelName != "StartMenu")
            PlayerPrefs.SetInt("Continue", 1);
        
             if (MaxSpeed != 0&&!Options)Flip();

           

        if (Options)
        {
            _normalHSpeed = _normalVSpeed = 0;
        }
        else
        {
            if (AlwaysMove)
            {

               
                    if (_vertical > 0 && YPos > 0 && _vertical_button_Push)
                    {
                        YPos--;
                        print("YPos " + YPos);
                        WalkAU.pitch = 1 + 0.1f * YPos;
                        WalkAU.clip = Steps[Random.Range(0, Steps.Length)];
                        WalkAU.Play();
                    }
                    if (_vertical < 0 && YPos < MaxPositions - 1 && _vertical_button_Push)
                    {
                        YPos++;
                        print("YPos " + YPos);
                        WalkAU.pitch = 1 - 0.1f * YPos;
                        WalkAU.clip = Steps[Random.Range(0, Steps.Length)];
                        WalkAU.Play();
                    }

                if (_horizontal < 0 && XPos > 0 && _horizontal_button_Push)
                {
                    XPos--;
                    WalkAU.pitch = 1 + 0.1f * XPos;
                    WalkAU.clip = Steps[Random.Range(0, Steps.Length)];
                    WalkAU.Play();
                }
                if (_horizontal > 0 && XPos < 10 && _horizontal_button_Push)
                {
                    XPos++;
                    WalkAU.pitch = 1 - 0.1f * XPos;
                    WalkAU.clip = Steps[Random.Range(0, Steps.Length)];
                    WalkAU.Play();
                }

                for (int i = 0; i < 8; i++)
                {
                    if (GameObject.Find("Position (" + i + ")") != null && i > MaxPositions - 1)
                        Destroy(GameObject.Find("Position (" + i + ")"));
                }
            }
            if (!AlwaysMove)
            {
                _normalHSpeed = 15 * _horizontal;
                _normalVSpeed = 15 * _vertical;

                if (_normalHSpeed != 0 || _normalVSpeed != 0)
                {
                    if (!WalkAU.isPlaying)
                    {
                        WalkAU.clip = Steps[Random.Range(0, Steps.Length)];
                        WalkAU.Play();
                    }
                }
            }

        }

        if (!CutSceenMode&&!AlwaysMove) {
        _controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, _normalHSpeed * MaxSpeed, 10));
        _controller.SetVerticalForce(Mathf.Lerp(_controller.Velocity.y, _normalVSpeed * MaxSpeed, 10));
        }

        
      // _transform.position = new Vector3(_transform.position.x+ _normalHSpeed/100, _transform.position.y+ _normalVSpeed/100, _transform.position.z);

		if (menu_b&&Enterdeley<Time.fixedTime){

            menu.Options = !menu.Options;
            Enterdeley = Time.fixedTime + 0.18f;

        }
        
       
    }

    private void Flip()
    {
        
        if (!joystick)
        {
            if (_horizontal < 0)
                side = -1;
            else if (_horizontal > 0)
                side = 1;
        }
        else
        {
            if (_horizontal < -0.3)
                side = -1;
            else if (_horizontal > 0.3)
                side = 1;
        }

        _transform.localScale = new Vector3(side * startscale, _transform.localScale.y, _transform.localScale.z);
        
    }

    private void Anim()
    {

        GameObject[] expl = GameObject.FindGameObjectsWithTag("Explosion");
        


        for (int i = 0; i < expl.Length; i++)
        {
            if (coll_obj.Contains(expl[i]))
            {
                anim.SetBool("Death", true);
            }
        }

        if (!AlwaysMove)
        {
            if (_normalHSpeed != 0 || _normalVSpeed != 0)
                anim.SetBool("Walk", true);
            else anim.SetBool("Walk", false);
        }else anim.SetBool("Walk", true);


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


        if (!joystick) {
			_horizontal = Input.GetAxis ("Horizontal");
			_vertical = Input.GetAxis ("Vertical");
            _vertical_button_Push = Input.GetButtonDown("Vertical");
            _horizontal_button_Push = Input.GetButtonDown("Horizontal");

            _vertical_button = Input.GetButtonDown("Vertical");

            _Character_Punch = Input.GetButtonDown("Punch");
            _Character_Block = Input.GetAxis("Block");
            

            if (_vertical > 0)
            {
                _vertical_button_Up = true;
                _vertical_button_Down = false;
            }

            if (_vertical < 0)
            {
                _vertical_button_Up = false;
                _vertical_button_Down = true;
            }

            //atack_b = Input.GetButtonDown ("Atack");
            enter_b = Input.GetButtonDown("Enter");
			enter_axis= Input.GetButton("Enter");
			exit_b = Input.GetButtonDown ("Exit");
			menu_b  = Input.GetButtonDown ("Menu");
            RageHit = Input.GetButtonDown("Space");

        } else {
			_horizontal = Input.GetAxis ("Horizontal_J");
			_vertical = Input.GetAxis ("Vertical_J");
            
		
			if (_vertical < 0)
            {

                _vertical_button_Up = true;
                _vertical_button_Down = false;
            }

            if (_vertical > 0)
            {
                _vertical_button_Up = false;
                _vertical_button_Down = true;
            }
            _vertical_button_Push = Input.GetButtonDown("Vertical_J");
            _horizontal_button_Push = Input.GetButtonDown("Horizontal_J");
            //atack_b = Input.GetKeyDown(KeyCode.JoystickButton2);
            _Character_Punch = Input.GetButtonDown("Punch_J");
            _Character_Block = Input.GetAxis("Block_J");
            
            
				enter_b = Input.GetKeyDown(KeyCode.JoystickButton2);
				enter_axis = Input.GetKey(KeyCode.JoystickButton2);
				exit_b = Input.GetKey(KeyCode.JoystickButton0);
			menu_b =  Input.GetKeyDown(KeyCode.JoystickButton7);
            RageHit = Input.GetKeyDown(KeyCode.JoystickButton5);








        }
		

	}

    void ParamChange(float param,Texture addcomment,float PChalpha)

    {
        if (param != 0)
        {
            Vector3 p = Camera.main.WorldToScreenPoint(_transform.position);
            string pm = "";
            Color guiColor = GUI.color; // Save the current GUI color

            float a =0;
            if (PChalpha > 1) a = 1;
            else a = PChalpha;


            float MoveVector = 1;
            if (param > 0) MoveVector = 1;
            else MoveVector = -1;

            GUI.color = new Color(1, 1, 1, a);
            if (param > 0) pm = "+ ";
            else pm = "";
            float w = 50;
            GUI.Box(new Rect(p.x, Screen.height - p.y + (PChalpha * 30* MoveVector)- 30 * 1.3f * MoveVector - w/2, w, w), pm + param, skin.customStyles[0]);
            GUI.DrawTexture(new Rect(p.x-w, Screen.height - p.y + (PChalpha * 30* MoveVector) - 30 * 1.3f * MoveVector - w / 2, w, w), addcomment);
            GUI.color = guiColor;
            
        }
    }
 
    private void OnGUI()

	{
        float GPADWidth = Screen.width / 8;

        if (PlayerPrefs.GetFloat("DrawGamepad") > 0 && ControllTextureAlpha > 0)
        {
            Color guiColor = GUI.color;
            GUI.color = new Color(1, 1, 1, ControllTextureAlpha);

            GUI.DrawTexture(new Rect(10, 10, GPADWidth, GPADWidth), GamepadT);
            ControllTextureAlpha -= 0.02f;


            GUI.color = guiColor; // Get back to previous GUI color
        }

        if (PlayerPrefs.GetFloat("DrawKeyboard") > 0 && ControllTextureAlpha > 0)
        {
            Color guiColor = GUI.color;
            GUI.color = new Color(1, 1, 1, ControllTextureAlpha);
            GUI.DrawTexture(new Rect(10, 10, GPADWidth, GPADWidth), KeyboardT);
            ControllTextureAlpha -= 0.02f;
            GUI.color = guiColor;
        }

        if (AlwaysMove)
        {
            


            float HP_Width = Screen.width - WBorder * 8;
            
                GUI.DrawTexture(new Rect(Screen.width - WBorder- HP_Width, HBorder, HP_Width / MaxSeconds*TG.BOSSHP, Screen.height / 35), BG_HP);
                GUI.DrawTexture(new Rect(Screen.width - WBorder- HP_Width, HBorder, HP_Width, Screen.height / 35), FG_HP);

            GUI.DrawTexture(new Rect( WBorder , HBorder, HP_Width / 3 * HP, Screen.height / 35), BG_HP);
            GUI.DrawTexture(new Rect(WBorder , HBorder, HP_Width, Screen.height / 35), FG_HP);

            /*if (RageCount <= 0)
            {
               
                for (int i = 0; i < (int)((Money / MaxRage)+1); i++)
                {
                    float W = 0;
                    if ((Money - 150* i) < MaxRage) W = (Money- MaxRage * i);
                    else W = MaxRage;
                    
                    GUI.DrawTexture(new Rect(Screen.width - WBorder - HP_Width, HBorder *2.4f+ (HBorder / 2)*i, (HP_Width / MaxRage) * MaxRage, Screen.height / 35), BG_HP_Black);
                    GUI.DrawTexture(new Rect(Screen.width - WBorder - HP_Width, HBorder *2.4f+ (HBorder/2)*i, (HP_Width / MaxRage) * W, Screen.height / 35), BG_HP);
                }
                
            }
            else
            {

                GUI.DrawTexture(new Rect(Screen.width - WBorder - HP_Width, HBorder * 2.4f, (HP_Width / MaxRage) * 150, Screen.height / 35), BG_HP_Black);
                GUI.DrawTexture(new Rect(Screen.width - WBorder - HP_Width, HBorder * 2.4f, (HP_Width / MaxRage) * RageCount, Screen.height / 35), BG_HP_White);
            }
            */


        }
        if (StartAlpha > 0&&Exiting)
        {
            Color gColor = GUI.color; // Save the current GUI color
            GUI.color = new Color(1, 1, 1, StartAlpha);

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), SleepTexture);

            GUI.color = gColor; // Get back to previous GUI color
        }

        if (!_gameover)
        {
            if (GameObject.Find("Coffee") != null)
            {
                if (_transform.Find("Head") != null)DrawSleep();

            }

        }

        if (_gameover)
        {
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 200), "GAME OVER", skin.customStyles[0]);
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 + 100, 300, 200), "e - to restart", skin.customStyles[0]);
        }
        
        if (DrawParameterChange >0)
        {
            ParamChange(MoneyChange, MoneyTexture, DrawParameterChange);
            DrawParameterChange -= 0.01f;
        }
      
        if (SaveAlpha > 0)
        {
           
            Color guiColor = GUI.color; // Save the current GUI color
            GUI.color = new Color(1, 1, 1, SaveAlpha);
            SaveAlpha -= 0.004f;
            GUI.DrawTexture(new Rect(Screen.width - 105f, Screen.height - 105f, 100, 100), SaveTexture);
            GUI.color = guiColor; // Get back to previous GUI color

           

        }
        
		

       

    }


	public List<GameObject> Getcollob()
	{
		return coll_obj;
	}
    public List<GameObject> GetLegscollob()
    {
        return Legscoll_obj;
    }
    

    void Exit()
    {
        
        if (Exiting)
        {
           // print("EXITING");
            if (StartAlpha < 1)
            {
                StartAlpha += 0.07f;
            }
            else
            {
               

                PlayerPrefs.SetInt("Money", (int)Money);
                SceneManager.LoadScene(ExitingString);
            }
        }

    }
    void BlowThis(GameObject g)
    {
        if (transform.Find("Explosion") == null)
        {
            GameObject blow = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Explosion"));
            blow.transform.position = g.transform.position;
            blow.name = "Explosion";
        }

        Destroy(g);

    }

    void GameOverControll()
    {
        if (HP <= 0) GameOver();
    }

    void GameOver()
    {
      _gameover = true;
    }

    void PushObject(GameObject obj)
    {

       // if (transform.position.x < obj.transform.position.x) { 
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(300, 0));
        //}
       
        
        if (obj.transform.childCount > 0)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
                Destroy(obj.transform.GetChild(i).gameObject);
        }
        if (obj.GetComponent<Animator>()!=null)
        {
            obj.GetComponent<Animator>().SetBool("Punch",true);
        }

        /*if (transform.position.x > obj.transform.position.x)
        {
            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 0));
            print("plus");
        }*/


        /* if (transform.Find("Explosion") == null)
         {
             GameObject blow = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Explosion"), transform);
             blow.name = "Explosion";
         }*/

    }

    void DropFromTheTop(Transform par,bool NullParent)
    {
        if (par.parent != null)
        {
            if (par.parent.tag == "Pillar")
            {
                for (int s = 0; s < TG.GetSegments().Count; s++)
                {
                    if (par.parent.parent.gameObject == TG.GetSegments()[s])
                    {
                        
                        if (s <= TG.GetSegments().Count-2)
                        {
                            par.parent.parent = TG.GetSegments()[s+1].transform;
                            break;
                        }
                        else { 
                            par.parent.parent = TG.GetSegments()[0].transform;
                            break;
                        }

                    }
                }
                if(NullParent)
                par.parent = null;

            }
        }
    }
    void ReactToPush(GameObject[]push, GameObject[] bosses, GameObject[] mines, GameObject[] block, GameObject[] pushing, int i)
    {
        if (push[i].GetComponent<CollList>() != null)
        {
            if (bosses != null)
            {
                for (int m = 0; m < bosses.Length; m++)
                {
                    if (push[i].GetComponent<CollList>().GetCollList().Contains(bosses[m]) && push[i].GetComponent<CollList>().Pushed)
                    {
                        BlowThis(push[i]);
                        GameObject.Find("Boss_Level_" + PlayerPrefs.GetInt("CurrentLevel") + "(Clone)").GetComponent<Boss>().DeathTimer = Time.fixedTime + 0.75f;
                        TG.BOSSHP--;
                    }

                }
            }

            if (mines != null)
            {
                for (int m = 0; m < mines.Length; m++)
                {
                    if (push[i].GetComponent<CollList>().GetCollList().Contains(mines[m]) && push[i].GetComponent<CollList>().Pushed)
                    {
                        BlowThis(push[i]);
                        Destroy(mines[m]);
                    }

                }
            }


            if (block != null)
            {
                for (int m = 0; m < block.Length; m++)
                {
                    if (push[i].GetComponent<CollList>().GetCollList().Contains(block[m]) && push[i].GetComponent<CollList>().Pushed)
                    {
                        BlowThis(push[i]);
                        Destroy(block[m]);
                    }

                }
            }
            if (pushing != null)
            {

                for (int m = 0; m < pushing.Length; m++)
                {
                    if (push[i].GetComponent<CollList>().GetCollList().Contains(pushing[m]) && push[i].GetComponent<CollList>().Pushed)
                    {
                        BlowThis(push[i]);

                        if (pushing[m].GetComponent<CollList>()==null)
                        {
                            PushObject(pushing[m]);
                            DropFromTheTop(pushing[m].transform, false);
                            pushing[m].AddComponent<CollList>();
                            pushing[m].GetComponent<CollList>().Pushed = true;
                            // Money += 10;
                        }
                    }
                }
            }

        }
    }
    void DrawSleep()
    {
        
        Vector3 FaceV = Camera.main.WorldToScreenPoint(_transform.Find("Head").position);

        Color guiColor = GUI.color; // Save the current GUI color

        if (SleepTimer - Time.fixedTime < SleepDeley/2)
        SleepAlpha = 1-(SleepTimer - Time.fixedTime) / SleepDeley;
       

        GUI.color = new Color(1, 1, 1, SleepAlpha);

        GUI.DrawTexture(new Rect(0, (Screen.height -FaceV.y)- Screen.height + (Time.fixedTime - SleepTimer)*3, Screen.width, Screen.height), SleepTexture);

        GUI.DrawTexture(new Rect(0, (Screen.height - FaceV.y)- (Time.fixedTime - SleepTimer)*3, Screen.width, Screen.height), SleepTexture);

        GUI.color = guiColor; // Get back to previous GUI color
    }
    void DestroyAllTraps()
    {
        
        for (int i = 0; i < TG.GetSegments().Count; i++)
        {
            if (TG.GetSegments()[i].transform.childCount > 0)
            {
                for (int c = 0; c < TG.GetSegments()[i].transform.childCount; c++)
                    Destroy(TG.GetSegments()[i].transform.GetChild(c).gameObject);
            }

        }

    }
    void HPDamageNoBlow(GameObject ob)
    {
        if (InvisTimer < Time.fixedTime)
        {
            HP--;

            Damage.clip = DamageClips[Random.Range(0, DamageClips.Count)];
            Damage.Play();

            InvisTimer = Time.fixedTime + 3;
            
        }
    }
        void HPDamage(GameObject ob)
    {

        if (InvisTimer < Time.fixedTime) {
            HP--;

            Damage.clip = DamageClips[Random.Range(0, DamageClips.Count)];
            Damage.Play();

            InvisTimer = Time.fixedTime + 3;

            if (ob.GetComponent<ParentDestroy>() != null)
                BlowThis(ob.GetComponent<ParentDestroy>().GetParentDestroy());
            else 
            BlowThis(ob);
        }

    }

    public float GetMaxSeconds()
    {
        return MaxSeconds;
    }
    public float GetSeconds()
    {
        return seconds;
    }
    public List<Vector3> GetPositions()
    {
        return Positions;
    }
    public int GetMaxPositions()
    {
        return MaxPositions;
    }
    
   

    private void OnTriggerStay2D(Collider2D c)
	{
		
		if(!Legscoll_obj.Contains(c.gameObject))
		{
            Legscoll_obj.Add(c.gameObject);
		}
		
	}
	
	private void OnTriggerExit2D(Collider2D c)
	{
		
		if(Legscoll_obj.Contains(c.gameObject))
		{
            Legscoll_obj.Remove(c.gameObject);
		}
		
	}

}
