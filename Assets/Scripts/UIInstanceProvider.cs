using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace DefaultNamespace
{
    public interface IInstanceReceiver
    {
        void SetInstance(Object obj);
    }
    
    public static class UIInstanceProvider
    {
        public static List<IInstanceReceiver>UIList = new List<IInstanceReceiver>();

        public static void SendInstance(Object obj)
        {
            Debug.Log(obj.GetType());
            Debug.Log(UIList.Count);
            for (int i = UIList.Count - 1; i >= 0; i--)
            {
                if (UIList[i] == null)
                {
                    UIList.RemoveAt(i);
                }
                else
                {
                    UIList[i].SetInstance(obj);
                }
            }
        }

        public static void Add(IInstanceReceiver receiver)
        {
            Debug.Log("추가");
            UIList.Add(receiver);
        }
    }
}