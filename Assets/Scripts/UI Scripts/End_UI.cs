using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class End_UI : MonoBehaviour
{
    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}