using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameSpeedController : MonoBehaviour
    {
        private float currentTimeScale = 1;
        [SerializeField] 
        private Text timeScaleText = null;

        private Coroutine timeCoroutine;
        private IEnumerator testt;
        private void Update()
        {
            float wheel = Input.GetAxis("Mouse ScrollWheel");
            if (wheel != 0)
            {
                currentTimeScale += wheel;
                if (currentTimeScale <= 0.1f)
                {
                    currentTimeScale = 0.1f;
                }
                else if (currentTimeScale > 2)
                {
                    currentTimeScale = 2;
                }

                Time.timeScale = currentTimeScale;
                testt = ShowTimeScale();
                timeCoroutine = StartCoroutine(ShowTimeScale());
            }
        }

        private IEnumerator ShowTimeScale()
        {
            var thisCoroutine = testt;
            timeScaleText.text = $"TimeScale : {currentTimeScale:F}";
            yield return new WaitForSecondsRealtime(1.0f);
            if (testt.Equals(thisCoroutine))
            {
                timeScaleText.text = null;    
            }
        }
    }
}