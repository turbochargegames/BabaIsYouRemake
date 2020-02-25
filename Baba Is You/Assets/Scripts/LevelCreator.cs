using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementTypes
{
    Empty=0,
    Baba,
    Wall,
    Rock,
    Flag,
    Goop,

    IsWord = 99,
    BabaWord =100,
    WallWord,
    FlagWord,
    RockWord,
    GoopWord,

    

    YouWord=200,
    PushWord,
    WinWord,
    StopWord,
    SinkWord,

}

[CreateAssetMenu()][System.Serializable]
public class LevelCreator : ScriptableObject
{
    
    [SerializeField]
    public List<ElementTypes> level = new List<ElementTypes>();

    public LevelCreator()
    {
        level = new List<ElementTypes>();
    }
}
