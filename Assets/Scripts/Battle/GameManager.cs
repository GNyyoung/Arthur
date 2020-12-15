using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Main;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Player player = null;
    [SerializeField]
    MonsterSpawner spawner = null;
    private readonly PlayerReward playerReward = new PlayerReward();

    public static GameMode CurrentMode { get; private set; }

    public enum GameMode { Infinite, Boss }

    private void Awake()
    {
        GameResolution.SetResolution(Camera.main);
        
        if (Data.Instance.isLoaded == false)
        {
            Data.Instance.LoadJsonData();   
        }
        playerReward.Initialize();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log($"PauseStatus : {pauseStatus}");
        if (pauseStatus == true)
        {
            UINavigation.Push("BattleOption");
        }
        else
        {
            // GameResolution.SetResolution();
        }
    }

    private void Start()
    {
        player.Initialize();
        spawner.Initialize(this, player, playerReward);
        DefenceVariety.InitializeGemSprite();
        GameStart();
    }

    public void GameStart()
    {
        player.CurrentAction.StartAction();

        var stageLoadData = (StageLoadData) InformationReceiver.Instance.InformationDictionary["Stage"];
        CurrentMode = stageLoadData.gameMode;
        spawner.StartSpawn(stageLoadData.stageName);
    }

    public void GameOver()
    {
        Debug.Log("게임오버");
        BattleSceneManager.Instance.InputInformation();
        BattleSceneManager.Instance.ResetData();
        SceneManager.LoadScene("Main");
    }

    public void GameWin()
    {
        Debug.Log("게임 승리");
        BattleSceneManager.Instance.InputInformation();
        BattleSceneManager.Instance.ResetData();
        SceneManager.LoadScene("Main");
    }
}
