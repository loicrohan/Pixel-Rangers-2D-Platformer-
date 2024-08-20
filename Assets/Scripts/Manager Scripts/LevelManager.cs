//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LevelManager : MonoBehaviour
//{
//    public static LevelManager instance;
//    private InGame_UI uiInGame;

//    [Header("Level Management")]
//    [SerializeField] private float levelTimer;

//    private void Start()
//    {
//        uiInGame = InGame_UI.instance;
//    }

//    private void Awake()
//    {
//        DontDestroyOnLoad(this.gameObject);

//        if (instance == null)
//            instance = this;
//        else
//            Destroy(this.gameObject);
//    }

//    public void Update()
//    {
//        levelTimer += Time.deltaTime;

//        uiInGame.UpdateTimerUI(levelTimer);
//    }
//}