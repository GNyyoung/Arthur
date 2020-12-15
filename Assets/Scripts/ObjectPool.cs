using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

// 탄환 오브젝트 관리하는 스크립트
public class ObjectPool : MonoBehaviour
{
    GameObject prefab;
    GameObject folder;
    private IInstanceReceiver _instanceReceiver;
    public int PoolSize { get; private set; }
    List<GameObject> createdObjectList = new List<GameObject>();
    int bulletIndex;

    /// <summary>
    /// 풀링할 오브젝트를 받아와서 생성함.
    /// </summary>
    /// <param name="prefab">풀링할 오브젝트</param>
    /// <param name="poolNum">생성할 오브젝트 수</param>
    public void InitializePool(GameObject prefab, GameObject folder, int poolNum)
    {
        InitializePool(prefab, folder, poolNum, null);
    }

    public void InitializePool(GameObject prefab, GameObject folder, int poolNum, IInstanceReceiver instanceReceiver)
    {
        this.prefab = prefab;
        this.folder = folder;
        PoolSize = poolNum;
        this._instanceReceiver = instanceReceiver;
        CreateObject(PoolSize);
    }

    // 생성한 오브젝트 하나를 반환함.
    public GameObject GetObject()
    {
        GameObject spawnedObject = null;

        if (createdObjectList[bulletIndex].activeSelf == false)
        {
            spawnedObject = createdObjectList[bulletIndex];
        }
        else
        {
            bulletIndex = createdObjectList.Count;
            CreateObject(5);
            spawnedObject = createdObjectList[bulletIndex];
        }

        bulletIndex = (bulletIndex + 1) % createdObjectList.Count;

        return spawnedObject;
    }

    public GameObject GetFrontObject()
    {
        GameObject spawnedObject = null;

        foreach (var createdObj in createdObjectList)
        {
            if (createdObj.activeSelf == false)
            {
                spawnedObject = createdObj;
                break;
            }
        }

        if (spawnedObject == null)
        {
            int createNum = 5;
            CreateObject(createNum);
            spawnedObject = createdObjectList[createdObjectList.Count - createNum];
        }

        return spawnedObject;
    }

    // num만큼 오브젝트를 생성함.
    public void CreateObject(int num)
    {
        for (int i = 0; i < num; i++)
        {
            createdObjectList.Add(Instantiate(prefab, folder.transform));
            _instanceReceiver?.SetInstance(createdObjectList[createdObjectList.Count - 1]);
            createdObjectList[createdObjectList.Count - 1].SetActive(false);
        }
    }
}
