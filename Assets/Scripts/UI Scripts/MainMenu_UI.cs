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
}