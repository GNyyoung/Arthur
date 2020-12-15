using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageCreator
{
    public enum MonsterName
    {
        Normal,
        Tiny,
        Changer,
        Boss1,
        Snake,
        FastHand
    }
    
    [ExecuteInEditMode]
    public class MonsterCreateInfo : MonoBehaviour
    {
        private MonsterName originalMonsterName;
        
        public MonsterName monsterName;
        public bool isStopSpawn;
        // [HideInInspector]
        public GameObject modelObject;

        private void Start()
        {
            StageCreateManager.Instance.monsterList.Add(this);
            originalMonsterName = monsterName;
        }

        private void Update()
        {
            if (originalMonsterName != monsterName)
            {
                if (Data.Instance.isLoaded == false)
                {
                    Debug.LogError("먼저 Json 데이터를 불러오세요.");
                    monsterName = originalMonsterName;
                }
                else
                {
                    if (modelObject != null)
                    {
                        DestroyImmediate(modelObject);    
                    }
                
                    // var newModel = Resources.Load<GameObject>($"Prefabs/Monster/{monsterName}");
                    Debug.Log(monsterName.ToString());
                    var newModel = Resources.Load<GameObject>(Data.Instance.GetMonster(monsterName.ToString()).ImagePath);
                    modelObject = Instantiate(newModel, this.transform);
                    originalMonsterName = monsterName;
                }
            }
        }
    }
}
