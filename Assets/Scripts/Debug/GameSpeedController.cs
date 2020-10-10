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
        private Text timeScaleText;

        private Coroutine timeCoroutine;
        private IEnumerator testt;
        private void Update()
        {
            float wheel = Input.GetAxis("Mouse ScrollWheel");
            if (wheel != 0)
            {
                currentTimeScale += wheel;
                if (currentTimeScale <= 0)
                {
                    currentTimeScale = 0.05f;
                }
                else if (currentTimeScale > 1)
                {
                    currentTimeScale = 1;
                }

                Time.timeScale = currentTimeScale;
                testt = ShowTimeScale();
                timeCoroutine = StartCoroutine(ShowTimeScale());
            }
        }

        private IEnumerator ShowTimeScale()
        {
            var thisCoroutine = testt;

            Debug.Log(thisCoroutine);
            timeScaleText.text = $"TimeScale : {currentTimeScale:F}";
            yield return new WaitForSecondsRealtime(1.0f);

            Debug.Log(thisCoroutine == testt);
            if (testt.Equals(thisCoroutine))
            {
                timeScaleText.text = null;    
            }
        }
    }
}