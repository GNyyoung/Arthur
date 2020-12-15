﻿using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MobileControl : MonoBehaviour
    {
#if UNITY_ANDROID
        private void Update()
        {
            if (Input.backButtonLeavesApp)
            {
                if (UINavigation.Pop() == null)
                {
                    Application.Quit();
                }
            }
        }  
#endif
    }
}