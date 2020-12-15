﻿using DefaultNamespace;
using UnityEngine;

namespace Main
{
    public class MoneyManager
    {
        private MoneyManager(){}
        private static MoneyManager _instance;

        public static MoneyManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MoneyManager();
                }

                return _instance;
                
                
                // if (_instance == null)
                // {
                //     var instances = FindObjectsOfType<MoneyManager>();
                //     if (instances.Length == 0)
                //     {
                //         var newInstance = UINavigation.GetView("Inventory")?.gameObject.AddComponent<MoneyManager>();
                //         _instance = newInstance;
                //     }
                //     else if (instances.Length >= 1)
                //     {
                //         for (int i = 1; i > instances.Length; i++)
                //         {
                //             Destroy(instances[i]);
                //         }
                //     
                //         _instance = instances[0];
                //     }
                // }
                //
                // return _instance;
            }
        }
        
        private int gold;

        public bool SpendGold(int amount)
        {
            if (amount <= gold)
            {
                gold -= amount;
                MainUI.Instance.goldText.text = gold.ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetGold()
        {
            if (InformationReceiver.Instance.InformationDictionary.ContainsKey("Money"))
            {
                gold = (int)InformationReceiver.Instance.InformationDictionary["Money"];   
            }
            else
            {
                // 테스트용 코드
                gold = 100;
                // 코드 끝
            }

            if (InformationReceiver.Instance.InformationDictionary.ContainsKey("Reward"))
            {
                gold += (InformationReceiver.Instance.InformationDictionary["Reward"] as Reward).gold;
            }
            
            Debug.Log(MainUI.Instance);
            Debug.Log(MainUI.Instance.goldText);
            Debug.Log(gold.ToString());
            MainUI.Instance.goldText.text = gold.ToString();
        }
    }
}