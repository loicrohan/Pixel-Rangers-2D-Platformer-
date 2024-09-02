using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection_UI : MonoBehaviour
{
    [SerializeField] private LevelButton_UI buttonPrefab;
    [SerializeField] private Transform buttonsParent;

    [SerializeField] private bool[] levelsUnlocked;

    public void Start()
    {
        LoadLevelsInfo();
        CreateLevelButtons();
    }

    private void CreateLevelButtons()
    {
        int levelsAmount = SceneManager.sceneCountInBuildSettings - 2;

        for (int i = 1; i < levelsAmount; i++)
        {
            if (IsLevelUnlocked(i) == false)
                return;

            LevelButton_UI newButton = Instantiate(buttonPrefab, buttonsParent);
            newButton.SetupButton(i);
        }

    }
    //SaveLevelVoids
    private bool IsLevelUnlocked(int levelIndex) => levelsUnlocked[levelIndex];

    private void LoadLevelsInfo()
    {
        int levelsAmount = SceneManager.sceneCountInBuildSettings - 2;

        levelsUnlocked = new bool[levelsAmount];

        for (int i = 1; i < levelsAmount; i++)
        {
            bool levelUnlocked = PlayerPrefs.GetInt("Level" + i + "Unlocked", 0) == 1;

            if (levelUnlocked)
            {
                levelsUnlocked[i] = true;
                Debug.Log("Level is unlocked");
            }
        }

        levelsUnlocked[1] = true;
    }
}