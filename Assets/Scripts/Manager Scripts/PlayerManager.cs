using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay;
    public Player player;
    [SerializeField] private int currentLevelIndex;
    private RespawnEvent respawnEvent;

    #region Awake,Start & Update
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    [ContextMenu("Find RespawnPoint")]
    private void Update()
    {
        //Useful to often avoid manually setting the gameobject
        RespawnPosition();

    }
    #endregion
    #region Respawn Methods
    private void RespawnPosition()
    {
        if (respawnPoint == null)
            respawnPoint = FindFirstObjectByType<RespawnEvent>().transform;
    }

    public void UpdateRespawnPosition(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;


    public void RespawnPlayer()
    {
        DifficultyManager difficultyManager = DifficultyManager.instance;

        if (difficultyManager != null && difficultyManager.difficulty == DifficultyType.Hard)
            return;

        StartCoroutine(RespawnCoroutine());
    }

    public IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();

        CinemachineVirtualCamera vCam = FindFirstObjectByType<CinemachineVirtualCamera>();

        if (vCam != null)
        {
            vCam.Follow = newPlayer.transform;
            vCam.LookAt = newPlayer.transform;
        }

    }
    #endregion
}