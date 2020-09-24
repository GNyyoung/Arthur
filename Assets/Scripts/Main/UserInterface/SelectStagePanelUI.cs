using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class SelectStagePanelUI : MonoBehaviour, IPanelUI, IBattleInfoProvider
    {
        private SelectStagePanelUI(){}
        private static SelectStagePanelUI _instance;

        public static SelectStagePanelUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<SelectStagePanelUI>();
                    if (instances.Length == 0)
                    {
                        var newInstance = UINavigation.GetView("SelectStage")?.gameObject.AddComponent<SelectStagePanelUI>();
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

        private string _selectedStageName; 

        private void Awake()
        {
            MainSceneManager.Instance.BattleInfoProviderList.Add(this);
        }

        public void UpdateData()
        {
            // 갱신할 내용들
        }
        
        public void StartStage(string stageName)
        {
            _selectedStageName = stageName;
            MainSceneManager.Instance.InputBattleInfo();
            SceneManager.LoadScene("Battle");
        }

        public KeyValuePair<string, object> GetBattleInfo()
        {
            Debug.Log("스테이지 정보 수집");
            var info = new KeyValuePair<string, object>("Stage", _selectedStageName);
            return info;
        }
    }
}