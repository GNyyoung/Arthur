using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Reward
    {
        public int gold;
        public List<string> itemNameList = new List<string>();
        
        public Reward(int gold, string itemName)
        {
            this.gold = gold;
            itemNameList.Add(itemName);
        }

        public Reward(int gold, string[] itemNames)
        {
            this.gold = gold;
            foreach (var itemName in itemNames)
            {
                itemNameList.Add(itemName);
            }
        }
        
    }
    /*
    public struct Reward : IInfoProvider
    {
        public int gold;
        public string itemName;
        
        public Reward(int gold, string itemName)
        {
            this.gold = gold;
            this.itemName = itemName;
        }

        public void AddProvider()
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<string, object>[] GetInfo()
        {
            throw new NotImplementedException();
        }
    }
    */
    // 테스트용으로 Monobehavior 추가한 것. 나중에 지우기 바람.
    public class PlayerReward : IInfoProvider
    {
        private int totalGold = 0;
        private List<string> totalItems = new List<string>();

        public void Initialize()
        {
            AddProvider();
        }

        public void RaiseReward(Reward reward)
        {
            totalGold += reward.gold;
            if (reward.itemNameList.Count > 0)
            {
                foreach (var itemName in reward.itemNameList)
                {
                    totalItems.Add(itemName);
                }
            }
        }

        public void AddProvider()
        {
            BattleSceneManager.Instance.AddInfoProvider(this);
        }

        public KeyValuePair<string, object>[] GetInfo()
        {
            Reward reward = new Reward(totalGold, totalItems.ToArray());
            var info = new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>("Reward", reward)
            };

            return info;
        }
    }
}