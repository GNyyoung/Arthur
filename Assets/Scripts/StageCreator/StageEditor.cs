using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using StageCreator;
using UnityEngine;

namespace StageCreator
{
    public class StageEditor : MonoBehaviour
    {
        [SerializeField]
        private GameObject monsterPrefab = null;
        [SerializeField]
        private GameObject monsterFolder = null;
        [SerializeField]
        private string stageName = null;
        private string currentStageName;
        
        public bool isLoadJson = false;
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void LoadStage()
        {
            if (Data.Instance.isLoaded == false)
            {
                Debug.LogError("먼저 Json 데이터를 불러오세요.");
            }
            else
            {
                if (stageName == "")
                {
                    Debug.LogWarning("스테이지 이름을 입력하세요.");
                    return;
                }
            
                var stageSpawnInfo = Data.Instance.GetStageSpawn(stageName);
                if (stageSpawnInfo == null)
                {
                    Debug.LogWarning($"해당 스테이지 파일이 존재하지 않습니다 : {stageName}");
                    return;
                }
            
                for (int i = monsterFolder.transform.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(monsterFolder.transform.GetChild(i).gameObject);
                }
            
                foreach (var spawnInfo in stageSpawnInfo)
                {
                    var newMonsterObj = Instantiate(monsterPrefab, monsterFolder.transform);
                    newMonsterObj.transform.position = new Vector3(spawnInfo.SpawnTime, 0, 0);
                    var newMonsterInfo = newMonsterObj.GetComponent<MonsterCreateInfo>();
                    newMonsterInfo.isStopSpawn = spawnInfo.IsStopSpawn;
                    newMonsterInfo.monsterName = (MonsterName)System.Enum.Parse(typeof(MonsterName), spawnInfo.MonsterName);
                    var monsterModel = Resources.Load<GameObject>(Data.Instance.GetMonster(spawnInfo.MonsterName).ImagePath);
                    newMonsterInfo.modelObject = Instantiate(monsterModel, newMonsterObj.transform);
                }
        
                Debug.Log($"{stageName}스테이지 불러오기 완료");
            }
        }

        public void Save()
        {
            var spawnList = new List<JsonStage>();
            for (int i = 0; i < monsterFolder.transform.childCount; i++)
            {
                var monsterTransform = monsterFolder.transform.GetChild(i);
                var monsterInfo = monsterTransform.GetComponent<MonsterCreateInfo>();
                
                var spawnInfo = new JsonStage();
                spawnInfo.MonsterName = monsterInfo.monsterName.ToString();
                spawnInfo.SpawnTime = monsterTransform.position.x;
                spawnInfo.IsStopSpawn = monsterInfo.isStopSpawn;
                
                spawnList.Add(spawnInfo);
            }
            
            var wrapperClass = new JsonWrapper<JsonStage>();
            wrapperClass.wrapper = spawnList.ToArray();

            var jsonText = JsonUtility.ToJson(wrapperClass);
            int wrapperTagLength = "{\"wrapper\":".Length;
            jsonText = jsonText.Substring(wrapperTagLength, jsonText.Length - wrapperTagLength - 1);
            
            File.WriteAllText($"Assets/Resources/Datas/{stageName}.json", jsonText);
            
            Debug.Log($"{stageName}스테이지 저장 완료");
        }

        public void LoadJson()
        {
            Data.Instance.RemoveAllJson();
            Data.Instance.LoadJsonData();
        }
    }
}

