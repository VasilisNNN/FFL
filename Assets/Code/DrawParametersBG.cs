using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawParametersBG : MonoBehaviour {
    public GameObject[] Targets;
    public string PrefsNames;
    public bool DestroyIfNotEquel;
    public float TimerVertMove = 0;
    public float VertMoveBorder =1;
    private float[] YStart;
    public bool DestroyVertUsed;
    private void Start()
    {
        if (TimerVertMove != 0)
        {
            YStart = new float[Targets.Length];
            for (int i = 0; i < Targets.Length; i++)
            {
                YStart[i] = Targets[i].transform.position.y;
            }
        }
    }
    // Update is called once per frame
    void Update() {
        for (int i = 0; i < Targets.Length; i++)
        {
            if (TimerVertMove == 0)
            {
                if (!DestroyIfNotEquel)
                {
                    if (TimerVertMove == 0)
                    {
                        if (PlayerPrefs.GetInt(PrefsNames) >= i)
                            Targets[i].GetComponent<SpriteRenderer>().enabled = true;
                        else
                            Targets[i].GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
                else
                {

                    if (PlayerPrefs.GetInt(PrefsNames) != i) Destroy(Targets[i]);
                }
            }

            else
            {
                if (i * TimerVertMove +TimerVertMove < Time.fixedTime)
                {
                    if (i < Targets.Length - 1)
                    {
                        if (Targets[i].transform.position.y > YStart[i] - VertMoveBorder)
                            Targets[i].transform.position = new Vector3(Targets[i].transform.position.x, Targets[i].transform.position.y - 0.01f, Targets[i].transform.position.z);
                        else if (DestroyVertUsed) Destroy(Targets[i]);

                        if (Targets[i+1].transform.position.y< YStart[i+1] + VertMoveBorder)
                            Targets[i + 1].transform.position = new Vector3(Targets[i + 1].transform.position.x, Targets[i + 1].transform.position.y + 0.01f, Targets[i + 1].transform.position.z);
                    }
                }
            }
            
        }
	}
}
