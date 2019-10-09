using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterDatabase : MonoBehaviour {
	public List<Character> character = new List<Character>();

    void Awake()
    {

        character.Add(new Character(3,new int[5]
        {
       010,
       100,
       000,
       110,
       001
       }));

        character.Add(new Character(3, new int[5]
        {
       100,
       101,
       000,
       010,
       100
       }));

        character.Add(new Character(3, new int[5]
        {
       011,
       100,
       000,
       001,
       100
       }));

        character.Add(new Character(3, new int[5]
        {
       001,
       100,
       000,
       101,
       010
       }));

        character.Add(new Character(3, new int[5]
        {
       000,
       100,
       101,
       000,
       101
       }));

        character.Add(new Character(3, new int[5]
        {
       100,
       100,
       010,
       101,
       001
       }));
    }



}
