﻿using System;
 using System.Collections;
using DefaultNamespace;
 using Unity.Collections;
 using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private const float STAGE_SWITCH_TIME = 2.0f;
    [SerializeField]
    private GameObject monsterPrefab = null;
    private ObjectPool monsterPool;
    private Coroutine spawnCoroutine;
    private bool isPlay;
    private Player player;
    private PlayerReward playerReward;
    private GameManager gameManager;
    

    public void Initialize(GameManager gameManager, Player player, PlayerReward playerReward)
    {
        this.gameManager = gameManager;
        this.player = player;
        this.playerReward = playerReward;
        var monsterFolder = new GameObject("MonsterFolder");
        monsterPool = GetComponent<ObjectPool>();
        monsterPool.InitializePool(monsterPrefab, monsterFolder, 10);
    }
    
    public void StartSpawn(string stageType)
    {
        isPlay = true;
        switch (GameManager.CurrentMode)
        {
            case GameManager.GameMode.Infinite:
                spawnCoroutine = StartCoroutine(SpawnInfiniteMode(stageType));
                break;
            case GameManager.GameMode.Boss:
                spawnCoroutine = StartCoroutine(SpawnBossMode(stageType));
                break;
            default:
                Debug.LogError($"{GameManager.CurrentMode} 스폰이 설정되지 않았습니다.");
                break;
        }
        
    }

    public void PauseSpawn()
    {
    }

    public void EndSpawn()
    {
        isPlay = false;
        StopCoroutine(spawnCoroutine);
    }

    private IEnumerator SpawnInfiniteMode(string stageType)
    {
        int stageNum = 0;
        var waitForFixedUpdate = new WaitForFixedUpdate();
        bool isActiveSpawn = true;
        
        while (isPlay)
        {
            string stageName;
            var stageData = Data.Instance.GetStageSpawn($"{stageType}{stageNum + 1}");

            CameraMove.Instance.ActiveCameraMove(false);
            yield return new WaitForSeconds(STAGE_SWITCH_TIME);
            CameraMove.Instance.ActiveCameraMove(true);
            
            if (stageData == null)
            {
                stageName = $"{stageType}{stageNum}";
                stageData = Data.Instance.GetStageSpawn(stageName);
            }
            else
            {
                stageName = $"{stageType}{stageNum + 1}";
            }
            
            Debug.Log(stageName);
            float spawnTime = 0;
            var stageInfo = Data.Instance.GetStageInfo(stageName);

            foreach (var spawnData in stageData)
            {
                while (spawnTime < spawnData.SpawnTime ||
                       isActiveSpawn == false)
                {
                    spawnTime += Time.fixedDeltaTime;
                    yield return waitForFixedUpdate;
                }
            
                Debug.Log("몬스터 소환");
                var spawnedMonster = SpawnMonster(spawnData.MonsterName, stageInfo);
                if (spawnData.IsStopSpawn == true)
                {
                    isActiveSpawn = false;
                    yield return new WaitWhile(() => spawnedMonster.gameObject.activeSelf == true);
                    Debug.Log("True");
                    isActiveSpawn = true;
                }
            }
        
            stageNum += 1;
        }
    }

    private IEnumerator SpawnBossMode(string stageType)
    {
        var waitForFixedUpdate = new WaitForFixedUpdate();
        bool isActiveSpawn = true;
        Monster spawnedMonster = null;
        
        var stageData = Data.Instance.GetStageSpawn($"{stageType}");
            
        CameraMove.Instance.ActiveCameraMove(false);
        yield return new WaitForSeconds(1.5f);
        CameraMove.Instance.ActiveCameraMove(true);
        
        float spawnTime = 0;
        var stageInfo = Data.Instance.GetStageInfo(stageType);

        foreach (var spawnData in stageData)
        {
            while (spawnTime < spawnData.SpawnTime ||
                   isActiveSpawn == false)
            {
                spawnTime += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            Debug.Log("몬스터 소환");
            spawnedMonster = SpawnMonster(spawnData.MonsterName, stageInfo);
            
            if (spawnData.IsStopSpawn == true)
            {
                isActiveSpawn = false;
                yield return new WaitUntil(() => spawnedMonster.gameObject.activeSelf == false);
                isActiveSpawn = true;
            }
        }

        // yield return new WaitForSeconds(1.0f);
        yield return new WaitWhile(() => spawnedMonster.gameObject.activeSelf == true);
        gameManager.GameWin();
    }

    private Monster SpawnMonster(string monsterName, JsonStageInfo stageInfo)
    {
        Monster spawnedMonster = null;
        var monsterObject = monsterPool.GetObject();
        var monsterData = Data.Instance.GetMonster(monsterName);
        if (monsterData != null)
        {
            monsterObject.SetActive(true);
            spawnedMonster = monsterObject.GetComponent<Monster>();
            spawnedMonster.Initialize(monsterData, player, playerReward, stageInfo);
        }
        else
        {
            Debug.LogWarning($"몬스터 데이터가 할당되지 않았습니다. : {monsterName}");
        }

        return spawnedMonster;
    }
}