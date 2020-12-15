﻿using UnityEngine;

namespace DefaultNamespace
{
    public static class GameResolution
    {
        private static bool isEdit = false;
        private static Vector2 originalResolution;
        public static float width;
        public static void SetResolution(Camera camera)
        {
            Debug.Log("Load Success Test3");
            Screen.orientation = ScreenOrientation.Landscape;
            camera.aspect = 1920 / 1080f;

            float widthRatio = ((float)Screen.height / Screen.width) * camera.aspect;
            float heightRatio = 1 / widthRatio;
            // float widthRatio = Screen.width / 1920f;
            // float heightRatio = Screen.height / 1080f;
            Debug.Log($"가로 픽셀 : {widthRatio}, {1 - (widthRatio - 1)}");
            if (widthRatio < 1)
            {
                camera.rect = new Rect((1 - widthRatio) * 0.5f, 0, widthRatio, 1);    
            }
            else
            {
                camera.rect = new Rect(0, (1 - heightRatio) * 0.5f, 1, heightRatio); 
            }
            Debug.Log($"카메라 : {camera.rect}");
            width = (widthRatio - 1);
            if (isEdit == false)
            {
                isEdit = true;
            }
            
        }
    }
}