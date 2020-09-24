using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterPrefab;
    private ObjectPool monsterPool;
    private Coroutine spawnCoroutine;
    private bool isPlay;

    public void Initialize()
    {
        monsterPool = GetComponent<ObjectPool>();
        monsterPool.InitializePool(monsterPrefab, 10);
    }
    
    public void StartSpawn(ICombatant player)
    {
        isPlay = true;
        spawnCoroutine = StartCoroutine(Spawn(player));
    }

    public void EndSpawn()
    {
        isPlay = false;
        StopCoroutine(spawnCoroutine);
    }

    private IEnumerator Spawn(ICombatant player)
    {
        float spawnTime = 0;
        int stageNum = 0;
        var waitForFixedUpdate = new WaitForFixedUpdate();

        // 테스트용 코드
        Debug.Log($"InfiniteStage{stageNum + 1}");
        var stageData = Data.Instance.GetStage($"InfiniteStage{stageNum + 1}");
        
        foreach (var spawnData in stageData)
        {
            while (spawnTime < spawnData.SpawnTime)
            {
                spawnTime += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
        
            Debug.Log("몬스터 소환");
            SpawnMonster(spawnData.MonsterName, player);
        }
        // 코드 끝
        
        // 에러나서 잠깐 주석처리
        // while (isPlay)
        // {
        //     Debug.Log($"InfiniteStage{stageNum + 1}");
        //     var stageData = Data.Instance.StageDatas[$"InfiniteStage{stageNum + 1}"];
        //     
        //     foreach (var spawnData in stageData)
        //     {
        //         while (spawnTime < spawnData.SpawnTime)
        //         {
        //             spawnTime += Time.fixedDeltaTime;
        //             yield return waitForFixedUpdate;
        //         }
        //     
        //         Debug.Log("몬스터 소환");
        //         SpawnMonster(spawnData.MonsterName, player);
        //     }
        //
        //     stageNum += 1;
        // }
    }

    private void SpawnMonster(string monsterName, ICombatant player)
    {
        var monsterObject = monsterPool.GetObject();
        var monsterData = Data.Instance.GetMonster(monsterName);
        if (monsterData != null)
        {
            monsterObject.SetActive(true);    
            monsterObject.GetComponent<Monster>().InitializeStat(monsterData, player);
        }
        else
        {
            Debug.LogWarning($"몬스터 데이터가 할당되지 않았습니다. : {monsterName}");
        }
    }
}