using UnityEngine;
using System.Collections;

public class Days_Draw : MonoBehaviour {

	private AudioSource AU;
	
    public bool[] Delete;
	void Awake()
	{
		AU = GetComponent<AudioSource>();

	}
	
	void Update()
	{
        for (int i = 0; i < Delete.Length; i++)
        {
            if (Delete[i]&&PlayerPrefs.GetInt("CurrentLevel") ==i)
                Destroy(gameObject);
                }

        if (PlayerPrefs.GetInt("CurrentLevel") >= Delete.Length&& Delete[Delete.Length-1])
            Destroy(gameObject);



    }
	
	
	
	
}
