using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialog : MonoBehaviour {

	private Rect pos;
	private Rect dialogpos;

	public Sprite[] SPRTS;
    private float timer;
    private int CurrentSPRT;
    private GameObject g;
    private bool Dead;
    private void Start()
    {
       g = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Dialog"));
       
    }
    private void FixedUpdate()
    {
        if (!Dead)
        {
            DrawDialog();
        }
        else Destroy(g);
    }
    void DrawDialog()
        {
            if (timer < Time.fixedTime)
            {
                if (CurrentSPRT < SPRTS.Length - 1) CurrentSPRT++;
                else CurrentSPRT = 0;

                timer = Time.fixedTime + 10;
            }
            g.transform.position = new Vector3(transform.position.x + 0.45f, transform.position.y + 0.1f, 1);
            g.GetComponent<SpriteRenderer>().enabled = GetComponent<SpriteRenderer>().enabled;
            g.GetComponent<SpriteRenderer>().sprite = SPRTS[CurrentSPRT];
            g.transform.position = new Vector3(transform.position.x + 0.45f, transform.position.y + 0.1f, 1);


        }
    public void SetDead(bool _dead)
    {
        Dead = _dead;

    }
    /* private void OnGUI()
     {
         Vector2 pos = new Vector2(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y);

         GUI.DrawTexture(new Rect(pos.x,Screen.height - pos.y,200,100),);
     }*/
}
