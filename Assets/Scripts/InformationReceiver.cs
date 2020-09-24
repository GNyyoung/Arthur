using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class InformationReceiver : MonoBehaviour
    {
        private InformationReceiver(){}
        private static InformationReceiver _instance;

        public static InformationReceiver Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<InformationReceiver>();
                    if (instances.Length == 0)
                    {
                        var newInstance = GameObject.Find("InformationReceiver")?.AddComponent<InformationReceiver>();
                        if (newInstance == null)
                        {
                            var informationReceiver = new GameObject("informationReceiver");
                            newInstance = informationReceiver.AddComponent<InformationReceiver>();
                        }
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

        public Dictionary<string, object> InformationDictionary { get; set; }

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void SetDic(Dictionary<string, object> dic)
        {
            InformationDictionary = dic;
            Debug.Log("데이터 받음");
        }
    }
}