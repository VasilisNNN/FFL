using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollList : MonoBehaviour {

    private List<GameObject> coll_obj = new List<GameObject>();
    public bool Pushed { get; set; }

   
    public List<GameObject> GetCollList()
        {
        return coll_obj;
        }
  

    public void SetCollListNull()
    {
        coll_obj = new List<GameObject>();
    }
    
    private void OnTriggerStay2D(Collider2D c)
    {

        if (!coll_obj.Contains(c.gameObject))
        {
            coll_obj.Add(c.gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D c)
    {

        if (coll_obj.Contains(c.gameObject))
        {
            coll_obj.Remove(c.gameObject);
        }

    }
}
