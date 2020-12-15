using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

// 전투모드 전체 관리
public class BattleSceneManager : MonoBehaviour
{
    private BattleSceneManager(){}
    private static BattleSceneManager _instance;

    public static BattleSceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var instances = FindObjectsOfType<BattleSceneManager>();
                if (instances.Length == 0)
                {
                    var newInstance = GameObject.Find("EventSystem")?.AddComponent<BattleSceneManager>();
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
    
    private List<IInfoProvider> infoProviderList = new List<IInfoProvider>();

    public void ResetData()
    {
        UINavigation.PopToRoot();
        InstanceProvider.ReceiverList.Clear();
        MonsterApproach.Instance.ResetData();
    }

    public void AddInfoProvider(IInfoProvider infoProvider)
    {
        if (infoProviderList.Contains(infoProvider) == false)
        {
            infoProviderList.Add(infoProvider);
        }
    }
    public void InputInformation()
    {
        var informationDictionary = new Dictionary<string, object>();
        foreach (var infoProvider in infoProviderList)
        {
            var informationArray = infoProvider.GetInfo();
            foreach (var info in informationArray)
            {
                informationDictionary.Add(info.Key, info.Value);   
            }
        }

        InformationReceiver.Instance.SetDic(informationDictionary);
    }
    
}
