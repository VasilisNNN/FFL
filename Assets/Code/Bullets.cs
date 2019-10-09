using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Bullets : MonoBehaviour {

	private Vector2 ProdBoxMin,ProdBoxMax,ProdBoxCenter;
	private bool BulletCreated = false;
    
    
	public float XSpeed = 0;
	public float YSpeed =  0.01f;
	public string BulletName = "Bullet";
	
    private Menu menu;
    private TrapGenerator TG;

    private Player pl;
    private Texture FG_HP, BG_HP;
    private float TimeDeley;
    public float MaxTimer=4;
    private float width = 120;
    void Start () {
        pl = GameObject.Find("Player").GetComponent<Player>();
        TG = GameObject.Find("TrapGenerator").GetComponent<TrapGenerator>();
        

		ProdBoxMin = GetComponent<BoxCollider2D> ().bounds.min;
		ProdBoxMax = GetComponent<BoxCollider2D> ().bounds.max;
		ProdBoxCenter = GetComponent<BoxCollider2D> ().bounds.center;
        
        menu = GameObject.Find("Main Camera").GetComponent<Menu>();

        FG_HP = Resources.Load<Texture>("Sprites/UI/Level_FG");
        BG_HP = Resources.Load<Texture>("Sprites/UI/Level_BG");
    }

    void FixedUpdate()
    {
        if (!pl._gameover && !pl.Options&&!TG.END)
        {
            MoveBullet();

            CreateTimerBullets();
           
        }
        if (TG.END)
        {

            if (transform.childCount > 0)
            {
                if (transform.GetChild(0) != null)
                    Destroy(transform.GetChild(0).gameObject);
            }
        }
    }
    void ChangeAnimation()
    {
        if (TimeDeley > (MaxTimer / 4)*3)
            GetComponent<Animator>().SetInteger("Timer", 3);

        if (TimeDeley < (MaxTimer / 4) * 3&& TimeDeley > (MaxTimer / 2))
            GetComponent<Animator>().SetInteger("Timer", 2);

        if (TimeDeley < MaxTimer / 4)
            GetComponent<Animator>().SetInteger("Timer", 1);
        
        if (TimeDeley < MaxTimer / 50)
                GetComponent<Animator>().SetInteger("Timer", 0);

    }

    void CreateTimerBullets()
    {
        if (GetComponent<Animator>() != null)ChangeAnimation();
        MoveBullet();
        if (TimeDeley<Time.fixedTime)
        {
            
            SetBullet(0);
            TimeDeley = Time.fixedTime + MaxTimer;
        }
        TimeDeley -= 0.02f;
    }

    private void OnGUI()
    {
        
            Vector3 Pos = Camera.main.WorldToScreenPoint(transform.position);

            float W = (TimeDeley - Time.fixedTime) * width / MaxTimer;
            if (TimeDeley < Time.fixedTime) W = 0;

            float H = Screen.height / 40;
        /*
            GUI.DrawTexture(new Rect(Pos.x - width / 2, Screen.height - Pos.y - H*3, W, H), BG_HP);
            GUI.DrawTexture(new Rect(Pos.x - width / 2, Screen.height - Pos.y - H*3, width, H), FG_HP);*/
        
    }

    void MoveBullet()
    {
        for (int c = 0; c < transform.childCount; c++)
        {
            if (transform.GetChild(c).gameObject != null)
            {
                Transform ChildT = transform.GetChild(c).transform;
                ChildT.position = new Vector3(ChildT.position.x + XSpeed,
                    ChildT.position.y + YSpeed, ChildT.position.z);

                if (ChildT.position.x < transform.position.x - TG.GetSegmentsDistance() * 8
                || ChildT.position.x > transform.position.x + TG.GetSegmentsDistance() * 8)
                {

                    Destroy(transform.GetChild(c).gameObject);

                }

            }
        }
    }
    void SetBullet(int i )
	{
		GameObject mainObject;
		mainObject = (GameObject)Instantiate(Resources.Load("Prefabs/Bullets/" + BulletName),transform);
		mainObject.transform.position = transform.position;
		mainObject.name = name+BulletName+i;

	}
}
