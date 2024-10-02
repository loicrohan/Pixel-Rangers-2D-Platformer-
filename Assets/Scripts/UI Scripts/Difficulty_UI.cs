using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty_UI : MonoBehaviour
{
    private DifficultyManager difficultyManager;

    private void Start()
    {
        difficultyManager = DifficultyManager.instance;
    }

    public void SetEasyMode() => difficultyManager.SetDifficulty(DifficultyType.Easy);
    public void SetNormalMode() => difficultyManager.SetDifficulty(DifficultyType.Normal);
    public void SetHardMode() => difficultyManager.SetDifficulty(DifficultyType.Hard);
}