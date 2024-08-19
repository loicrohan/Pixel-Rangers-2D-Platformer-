using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_UI : MonoBehaviour
{
    public string firstLevelName;
    public GameObject menuCharacter;
    [SerializeField] private GameObject[] uiElements;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow, new RefreshRate() { numerator = 60, denominator = 1 });
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

    public void PlayerSelectionEnabled()
    {
        menuCharacter.SetActive(true);
    }

    public void PlayerSelectionDisabled()
    {
        menuCharacter.SetActive(false);
    }

    public void ClickSelect()
    {
        AudioManager.instance.PlaySFX(4);
        SceneManager.LoadScene(firstLevelName);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
        print("Quitting Game");
    }
}