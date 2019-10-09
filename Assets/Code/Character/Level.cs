using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]

public class Level {
    public int MaxNum;
    public int[] traps;
    public int state;

    public Level(int max, int _state,int[] _traps )
    {
        MaxNum = max;
        traps = _traps;
        state = _state;
    }

    
}
