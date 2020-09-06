using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string levelName;
    public int starScore;

    public LevelData(MenuSystem menuSys)
    {
        levelName = menuSys.GetLevelName();
        starScore = menuSys.GetStarValue();
    }
}