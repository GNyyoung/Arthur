using System;
using System.Collections.Generic;
using Main;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class SelectStagePanelUI : MonoBehaviour, IPanelUI, IInfoProvider
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

        private StageLoadData _stageLoadData;

        private void Awake()
        {
            AddProvider();
        }

        public void ShowPanelData()
        {
            // 갱신할 내용들
        }
        
        public void StartStage(StageLoadData stageLoadData)
        {
            if (MainSceneManager.Instance.CheckStageStartCondition() == true)
            {
                _stageLoadData = stageLoadData;
                MainSceneManager.Instance.InputBattleInfo();
                MainSceneManager.Instance.ResetData();
                SceneManager.LoadScene("Battle");    
            }
            else
            {
                MainUI.Instance.stageMessage.ShowMessage(StageMessage.StageMessageType.EmptyEquippedSword);
            }
        }

        public void AddProvider()
        {
            MainSceneManager.Instance.AddBattleInfoProvider(this);
        }

        public KeyValuePair<string, object>[] GetInfo()
        {
            Debug.Log("스테이지 정보 수집");
            // var info = new KeyValuePair<string, object>("Stage", _selectedStageName);
            var info = new[]{new KeyValuePair<string, object>("Stage", _stageLoadData)};
            return info;
        }
    }
}