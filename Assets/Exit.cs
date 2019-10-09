using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{

    public string LoadScene;
    public int SetLevel = -1;
    private Player pl;
    private void Start()
    {
       if(GameObject.Find("Player")!=null) pl = GameObject.Find("Player").GetComponent<Player>();
        if (PlayerPrefs.GetInt(name + "Death") == 1) Destroy(gameObject);

    }
    void Update()
    {
        if (pl != null)
        {
            if (!pl.Options)
            {
                if (SetLevel > -1)
                {
                    Player pl = GameObject.Find("Player").GetComponent<Player>();
                    if (pl.enter_b && pl.GetLegscollob().Contains(gameObject))
                    {
                        PlayerPrefs.SetInt("DateLevel", SetLevel);
                        SceneManager.LoadScene(LoadScene);
                    }
                }
                else
                {
                    SceneManager.LoadScene(LoadScene);
                }
            }
        }
        else
        {
            SceneManager.LoadScene(LoadScene);
        }

    }
}
