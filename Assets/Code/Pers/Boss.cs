using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {
	private Animator anim;
	public int MaxState =2;
	public int Timerdeley = 5;
	private float Timer, TimerDeley;
    private Menu menu;
    public float DeathTimer { get; set; }
    private GameObject[] Bosss;
	void Awake()
	{
        Bosss = GameObject.FindGameObjectsWithTag("Boss");

        anim = GetComponent<Animator> ();
        menu = GameObject.Find("Player").GetComponent<Menu>();
    }

	// Update is called once per frame
	void Update () {
        if (!menu.Options)
        {
            if (DeathTimer > Time.fixedTime)
            {
                for(int i =0;i< Bosss.Length;i++)
                    if (Bosss[i].GetComponent<Animator>() != null) Bosss[i].GetComponent<Animator>().SetBool("Damage", true);
                anim.SetBool("Damage", true);
            }
            else
            {
                for (int i = 0; i < Bosss.Length; i++)
                {
                    if(Bosss[i]!=null)
                    if (Bosss[i].GetComponent<Animator>() != null) Bosss[i].GetComponent<Animator>().SetBool("Damage", false);
                }

                anim.SetBool("Damage", false);

                anim.StopPlayback();
                if (PlayerPrefs.GetInt(name + "Start") == 1)
                    anim.SetBool("Start", true);

                PlayerPrefs.SetInt(name + "Start", 1);


                if (Timer < Time.fixedTime && TimerDeley > Time.fixedTime)
                {

                    anim.SetInteger("State", Random.Range(0, MaxState));
                    Timer = Time.fixedTime + Timerdeley;
                    TimerDeley = Time.fixedTime - 1;
                }

                if (Timer < Time.fixedTime&& TimerDeley < Time.fixedTime)
                {
                    anim.SetInteger("State", -1);
                    TimerDeley = Time.fixedTime + 1f;
                }



            }
        }
        else anim.StartPlayback(); 

    }
}
