using System.Collections.Generic;
using UnityEngine;

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

        public void SetInstance(object obj)
        {
            
        }

        public void OpenInventory()
        {
            UINavigation.Push("Inventory");
        }

        public void CloseInventory()
        {
            UINavigation.PopTo("Inventory");
        }

        public void OpenSelectStage()
        {
            UINavigation.Push("SelectStage");
        }

        public void CloseSelectStage()
        {
            UINavigation.PopTo("SelectStage");
        }
    }
}