using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End_UI : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            GoToMainMenu();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}