using System;
using Main;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class RewardPanelUI : MonoBehaviour, IPanelUI
    {
        private RewardPanelUI(){}
        private static RewardPanelUI _instance;

        public static RewardPanelUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<RewardPanelUI>();
                    if (instances.Length == 0)
                    {
                        var newInstance = UINavigation.GetView("Inventory")?.gameObject.AddComponent<RewardPanelUI>();
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

        public Text resultText;
        public Text goldRewardText;

        public void ShowPanelData()
        {
            if ((bool)InformationReceiver.Instance.InformationDictionary["Result"] == true)
            {
                resultText.text =  "전투보상!";
                MainSound.Instance.OutputRewardJingle();
            }
            else
            {
                resultText.text = "패배...";
                MainSound.Instance.OutPutStageFailJingle();
            }
            goldRewardText.text = (InformationReceiver.Instance.InformationDictionary["Reward"] as Reward).gold.ToString();
        }

        public void CloseRewardPanel()
        {
            UINavigation.PopTo("Reward");
            MainSound.Instance.OutputPanelCloseSound();
        }
    }
}