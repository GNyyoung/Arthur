using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class InstanceProvider
    {
        public static readonly List<IInstanceReceiver>ReceiverList = new List<IInstanceReceiver>();

        public static void SendInstance(object obj)
        {
            Debug.Log(obj.GetType());
            Debug.Log(ReceiverList.Count);
            for (int i = ReceiverList.Count - 1; i >= 0; i--)
            {
                if (ReceiverList[i] == null)
                {
                    ReceiverList.RemoveAt(i);
                }
                else
                {
                    ReceiverList[i].SetInstance(obj);
                }
            }
        }

        public static void Add(IInstanceReceiver receiver)
        {
            Debug.Log("추가");
            ReceiverList.Add(receiver);
        }
    }
}