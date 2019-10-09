using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    public List<Transform> Parts = new List<Transform>();
    public float Speed=0.1f;
    private float XMax, XMin;
    public float Way = 1;
    private Player pl;
    public bool MoveWithTrain = true;
    void Start()
    {
        pl = GameObject.Find("Player").GetComponent<Player>();
      
        for (int i = 0; i < transform.childCount; i++)
        {
            Parts.Add(transform.GetChild(i));
        }

        XMax = Parts[Parts.Count - 1].transform.position.x;
        XMin = Parts[0].transform.position.x;

        Destroy(Parts[0].gameObject);
        Parts.RemoveAt(0);
        
    }
    void FixedUpdate()
    {
        if (!pl.Options && !pl._gameover)
        {
          Move();
        }
    }


    private void Move()
    {
        for (int i = 0; i < Parts.Count; i++)
        {
            float s = (Speed + Mathf.Abs(transform.position.z) / 100) * Way;
            //if (s < 0) s = 0.01f* Way;
            Parts[i].position = new Vector3(Parts[i].position.x + s, Parts[i].position.y, Parts[i].position.z);
            if (Way == 1)
            {
                if (Parts[i].position.x > XMax)
                    Parts[i].position = new Vector3(XMin, Parts[i].position.y, Parts[i].position.z);
            }
            else
            {
                if (Parts[i].position.x < XMin)
                    Parts[i].position = new Vector3(XMax, Parts[i].position.y, Parts[i].position.z);

            }
    }
    
}
}
