using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
	public List<Item> items = new List<Item>();
	
	void Awake()
	{
	 items.Add (new Item(new int[4]{0,0,0, 0 }));
        items.Add(new Item(new int[4] { 0, 0, 0, 0 }));
        items.Add(new Item(new int[4] { 0, 0, 0, 0 }));
        items.Add(new Item(new int[4] { 0, 0, 0, 0 }));
        items.Add(new Item(new int[4] { 0, 0, 0, 0 }));
        items.Add(new Item(new int[4] { 0, 0, 0, 0 }));
    }



}
