using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelDatabase : MonoBehaviour {
	public List<Level> level = new List<Level>();

    //1 - MINE
    //2 - BLOCK
    //3 - Money
    //4 - PUSH
    //5 - GUN
    //6 - Beam
    //7 - HPplus
    //8 - Stable



       // 1 - Sate
       
    //1 - MINE
    //2 - BLOCK
    //3 - Pillar
    //4 - PUSH
    //5 - GUN
    //6 - Beam
    //7 - HPplus
    //8 - PillarBlow
    //9 - Pillar999


    void Awake()
    {
        level.Add(new Level(2,0, new int[15]
        {
       01,
       10,
       01,
       01,
       10,
       10,
       01,
       10,
       01,
       10,
       01,
       10,
       01,
       10,
       01})); 

        level.Add(new Level(2, 0, new int[15]
                {
       01,
       10,
       01,
       01,
       10,
       10,
       01,
       10,
       01,
       10,
       01,
       10,
       01,
       10,
       01}));

        level.Add(new Level(2, 0, new int[15]
                {
       01,
       10,
       01,
       01,
       10,
       10,
       01,
       10,
       01,
       10,
       01,
       10,
       01,
       10,
       01}));

        level.Add(new Level(2, 0, new int[15]
                {
       01,
       10,
       01,
       01,
       10,
       10,
       01,
       10,
       01,
       10,
       01,
       10,
       01,
       10,
       01}));


    }



}
