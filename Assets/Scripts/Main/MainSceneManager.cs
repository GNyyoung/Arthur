using System;
using System.Collections.Generic;
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
        
        public List<IBattleInfoProvider> BattleInfoProviderList {get; private set; } = new List<IBattleInfoProvider>();
        [SerializeField]
        private PlayerInfo playerInfo;
        private void Awake()
        {
            Data.Instance.LoadJsonData();
        }

        public void InputBattleInfo()
        {
            var informationDictionary = new Dictionary<string, object>();
            foreach (var battleInfoProvider in BattleInfoProviderList)
            {
                var info = battleInfoProvider.GetBattleInfo();
                informationDictionary.Add(info.Key, info.Value);
            }

            InformationReceiver.Instance.InformationDictionary = informationDictionary;   
        }
    }
}