using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_UI : MonoBehaviour
{
    public static InGame_UI instance;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI fruitText;
    [SerializeField] private GameObject gameOverObject;

    [SerializeField] private GameObject pauseUI;
    private bool isPaused;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        instance = this;
        gameOverObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            PauseButton();

        if (Input.GetKeyDown(KeyCode.Q))
            GoToMainMenu();

    }

    public void PauseButton()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;
            pauseUI.SetActive(false);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
            pauseUI.SetActive(true);
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void UpdateFruitUI(int collectedFruits, int totalFruits)
    {
        fruitText.text = collectedFruits + "/" + totalFruits;
    }

    public void UpdateTimerUI(float timer)
    {
        this.timerText.text = timer.ToString("0") + "s";
    }

    public void GameOverUI()
    {
        gameOverObject.SetActive(true);
        
        print("Game Over Restart Again");
        Invoke("ReloadScene", 3f);
    }

    public void ReloadScene()
    {
        GameManager.instance.LoadCurrentScene();
    }
}