using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_UI : MonoBehaviour
{
    public string firstLevelName;
    public GameObject menuCharacter, gameTitle;
    [SerializeField] private GameObject[] uiElements;
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        if (HasLevelProgression())
            continueButton.SetActive(true);
    }
    
    public void SwitchUI(GameObject uiToEnable)
    {
        foreach (GameObject ui in uiElements)
        {
            ui.SetActive(false);
        }

        uiToEnable.SetActive(true);

        AudioManager.instance.PlaySFX(4);

        
    }
    #region Enable/Disable Methods
    public void PlayerUISelectionEnabled() => menuCharacter.SetActive(true);

    public void PlayerUISelectionDisabled() => menuCharacter.SetActive(false);

    public void GameTitleEnabled() => gameTitle.SetActive(true);

    public void GameTitleDisabled() => gameTitle.SetActive(false);
    #endregion
    #region Level Progress Methods
    public void ClickSelect()
    {
        AudioManager.instance.PlaySFX(4);
        SceneManager.LoadScene(firstLevelName);
    }

    private bool HasLevelProgression()
    {
        bool hasLevelProgression = PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;

        return hasLevelProgression;
    }

    public void ContinueGame()
    {
        int difficultyIndex = PlayerPrefs.GetInt("GameDifficulty", 1);
        int levelToLoad = PlayerPrefs.GetInt("ContinueLevelNumber", 0);

        DifficultyManager.instance.LoadDifficulty(difficultyIndex);
        SceneManager.LoadScene("Level " + levelToLoad);
        AudioManager.instance.PlaySFX(4);
    }
    #endregion
}