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
    [SerializeField] private int currentLevelIndex;
    //[SerializeField] private bool currentLevel;

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
    //[SerializeField] private LevelManager levelManager;
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

        //Useful to often avoid manually setting the gameobject 
        //if (respawnPoint == null)
        //{
        //    respawnPoint = FindFirstObjectByType<StartPoint>().transform;
        //}
        uiInGame = InGame_UI.instance;
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        //nextLevelIndex = currentLevelIndex + 1;
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

        if (PlayerManager.instance == null)
            Instantiate(playerManager);

        if (SkinManager.instance == null)
            Instantiate(skinManager);

        if (DifficultyManager.instance == null)
            Instantiate(difficultyManager);

        //if (LevelManager.instance == null)
        //    Instantiate(levelManager);
    }

    #region Fruits Methods
    private void CollectFruitsInfo()
    {
        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruits = allFruits.Length;

        if(currentLevelIndex >= 1)
            uiInGame.UpdateFruitUI(fruitsCollected, totalFruits);
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
    }

    public void LoadCurrentScene() => SceneManager.LoadScene(/*"Basic Tutorial Level" + */currentLevelIndex);
}