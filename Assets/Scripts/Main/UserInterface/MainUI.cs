using System;
using System.Collections.Generic;
using Main;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class MainUI : MonoBehaviour, IInstanceReceiver
    {
        private MainUI(){}
        private static MainUI _instance;

        public static MainUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<MainUI>();
                    if (instances.Length == 0)
                    {
                        var newInstance = GameObject.Find("Canvas").AddComponent<MainUI>();
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

        GameObject InventoryPanel;
        public Text goldText;
        public StageMessage stageMessage;

        private void Awake()
        {
            UINavigation.baseTransform = gameObject.transform;
        }

        public void SetInstance(object obj)
        {
            
        }

        public void OpenInventory()
        {
            UINavigation.Push("Inventory");
            MainSound.Instance.OutputPanelOpenSound();
        }

        public void CloseInventory()
        {
            UINavigation.PopTo("Inventory");
            MainSound.Instance.OutputPanelCloseSound();
        }

        public void OpenSelectStage()
        {
            UINavigation.Push("SelectStage");
            MainSound.Instance.OutputMapOpen();
        }

        public void CloseSelectStage()
        {
            UINavigation.PopTo("SelectStage");
            MainSound.Instance.OutputPanelCloseSound();
        }
    }
}