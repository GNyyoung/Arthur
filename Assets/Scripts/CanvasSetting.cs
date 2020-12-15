﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class CanvasSetting : MonoBehaviour
    {
        private void Awake()
        {
            var canvasScaler = GetComponent<CanvasScaler>(); 
            // canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
            canvasScaler.matchWidthOrHeight = 1;
            Debug.Log($"해상도 : {Screen.width}, {Screen.height}");
            Debug.Log($"캔버스 크기 : {GetComponent<RectTransform>().rect}");
        }
    }
}