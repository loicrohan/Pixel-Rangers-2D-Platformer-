using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager instance;

    private InGame_UI uiInGame;

    [Header("Level Management")]
    [SerializeField] private float levelTimer;
    [SerializeField] private int currentLevelIndex, nextLevelIndex;
    //[SerializeField] private string mainMenuScreen;

    [Header("Fruits Management")]
    public bool fruitsAreRandom;
    public int fruitsCollected;
    public int totalFruits;
    public Transform fruitParent;

    [Header("Checkpoints")]
    public bool canReactivate;

    [Header("Managers")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private SkinManager skinManager;
    [SerializeField] private DifficultyManager difficultyManager;
    #endregion
    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        uiInGame = InGame_UI.instance;
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        //mainMenuScreen = SceneManager.

        nextLevelIndex = currentLevelIndex + 1;
        CollectFruitsInfo();
        CreateManagersIfNeeded();
    }

    private void Update()
    {
        if (currentLevelIndex >= 1)
            EnableCountDown();
        else
            return;
    }

    private void EnableCountDown()
    {
        levelTimer += Time.deltaTime;
        uiInGame.UpdateTimerUI(levelTimer);
    }

    private void CreateManagersIfNeeded()
    {
        if (AudioManager.instance == null)
            Instantiate(audioManager);

        if (PlayerManager.instance == null && currentLevelIndex != 0)
            Instantiate(playerManager);

        if (SkinManager.instance == null)
            Instantiate(skinManager);

        if (DifficultyManager.instance == null)
            Instantiate(difficultyManager);
    }

    #region Fruits Methods
    private void CollectFruitsInfo()
    {
        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruits = allFruits.Length;

        if(currentLevelIndex >= 1)
            uiInGame.UpdateFruitUI(fruitsCollected, totalFruits);

        PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalFruits", totalFruits);
    }

    [ContextMenu("Parent All Fruits")]
    private void ParentAllTheFruits()
    {
        if (fruitParent == null)
            return;

        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);

        foreach (Fruit fruit in allFruits)
        {
            fruit.transform.parent = fruitParent;
        }
    }
    public void AddFruit()
    {
        fruitsCollected++;
        uiInGame.UpdateFruitUI(fruitsCollected, totalFruits);
    }

    public void RemoveFruit()
    {
        fruitsCollected--;
        uiInGame.UpdateFruitUI(fruitsCollected, totalFruits);
    }

    public int FruitsCollected() => fruitsCollected;

    public bool FruitsHaveRandomLook() => fruitsAreRandom;
    #endregion
    public void RestartLevel()
    {
        InGame_UI.instance.GameOverUI();
        Invoke("LoadCurrentScene", 3f);
    }

    public void LoadCurrentScene() => SceneManager.LoadScene(currentLevelIndex);

    //After Level Selection & Save Level Progression voids
    public void LevelFinished()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SaveLevelProgression();
        SaveBestTime();
        SaveFruitsInfo();
    }

#region Save Level Progression voids
    private void SaveFruitsInfo()
    {
        int fruitsCollectedBefore = PlayerPrefs.GetInt("Level" + currentLevelIndex + "FruitsCollected");

        if (fruitsCollectedBefore < fruitsCollected)
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "FruitsCollected", fruitsCollected);

        int totalFruitsInBank = PlayerPrefs.GetInt("TotalFruitsAmount");
        PlayerPrefs.SetInt("TotalFruitsAmount", totalFruitsInBank + fruitsCollected);
    }
    private void SaveBestTime()
    {
        float lastTime = PlayerPrefs.GetFloat("Level" + currentLevelIndex + "BestTime", 99);

        if (levelTimer < lastTime)
            PlayerPrefs.SetFloat("Level" + currentLevelIndex + "BestTime", levelTimer);
    }
    private void SaveLevelProgression()
    {
        PlayerPrefs.SetInt("Level" + nextLevelIndex + "Unlocked", 1);

        if (NoMoreLevels() == false)
            PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);
    }
    private bool NoMoreLevels()
    {
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 2; // We have main menu and The End scene, that's why we use number 2
        bool noMoreLevels = currentLevelIndex == lastLevelIndex;

        return noMoreLevels;
    }
    #endregion
}