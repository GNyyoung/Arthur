using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    MonsterSpawner spawner;

    GameMode mode;

    public enum GameMode { Infinite, Boss }

    private void Start()
    {
        if (Data.Instance.isLoaded == false)
        {
            Data.Instance.LoadJsonData();   
        }
        player.Initialize();
        spawner.Initialize();
        GameStart();
    }

    public void GameStart()
    {
        player.CurrentAction.StartAction();
        spawner.StartSpawn(player);
    }

    public void GameOver()
    {

    }
}
