using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsSprite : MonoBehaviour
{
    public string PrefName;
    public Sprite[] SPRT;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = SPRT[PlayerPrefs.GetInt(PrefName)];
    }

}
