using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]

public class  Character{
    public int[] smiles;
    public int MaxNum;
    public Character(int Max , int[] _smiles)
    {
        smiles = _smiles;
        MaxNum = Max;
    }

    
}
