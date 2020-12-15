﻿using System;
using System.Collections.Generic;
using Main;
using UnityEngine;

namespace DefaultNamespace
{
    public class MainSceneManager : MonoBehaviour
    {
        private MainSceneManager(){}
        private static MainSceneManager _instance;

        public static MainSceneManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<MainSceneManager>();
                    if (instances.Length == 0)
                    {
                        var newInstance = GameObject.Find("Main Camera")?.AddComponent<MainSceneManager>();
                        _instance = newInstance;
                    }
                    else if (instances.Length >= 1)
                    {
                        for (int i = 1; i > instances.Length; i++)
                        {
                            Destroy(instances[i]);
                        }

                        _instance = instances[0];
                    }
                }

                return _instance;
            }
        }
        
        private List<IInfoProvider> _battleInfoProviderList = new List<IInfoProvider>();
        [SerializeField] private PlayerInfo playerInfo = null;
        private void Awake()
        {
            GameResolution.SetResolution(Camera.main);
            
            if (Data.Instance.isLoaded == false)
            {
                Data.Instance.LoadJsonData();   
            }
        }

        private void Start()
        {
            MoneyManager.Instance.SetGold();
            
            if (InformationReceiver.Instance.InformationDictionary.ContainsKey("Reward"))
            {
                Debug.Log("보상 화면 띄우기");
                UINavigation.Push("Reward");
            }
        }

        public void InputBattleInfo()
        {
            var informationDictionary = new Dictionary<string, object>();
            foreach (var battleInfoProvider in _battleInfoProviderList)
            {
                var informationArray = battleInfoProvider.GetInfo();
                foreach (var info in informationArray)
                {
                    informationDictionary.Add(info.Key, info.Value);   
                }
            }

            InformationReceiver.Instance.SetDic(informationDictionary);
        }

        public void ResetData()
        {
            UINavigation.PopToRoot();
            InstanceProvider.ReceiverList.Clear();
        }

        public void AddBattleInfoProvider(IInfoProvider provider)
        {
            if (_battleInfoProviderList.Contains(provider) == false)
            {
                _battleInfoProviderList.Add(provider);
            }
        }

        /// <summary>
        /// 스테이지 시작을 위한 조건이 갖춰졌는지 확인합니다.
        /// </summary>
        public bool CheckStageStartCondition()
        {
            foreach (var swordIndex in playerInfo.EquipSwordIndexList)
            {
                if (swordIndex != -1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}