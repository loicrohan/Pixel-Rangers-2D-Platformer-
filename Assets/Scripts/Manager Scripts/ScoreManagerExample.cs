using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagerExample : MonoBehaviour
{
    public string levelName;
    
    [ContextMenu("Save Value")]
    public void SaveValue() => PlayerPrefs.SetInt("LevelUnlocked", 1);

    [ContextMenu("Load Value")]
    public void LoadValue()
    {
        bool levelUnlocked = PlayerPrefs.GetInt("LevelUnlocked", 0) == 1;

        if (levelUnlocked)
            print("Level 1 is Unlocked");
    }
}