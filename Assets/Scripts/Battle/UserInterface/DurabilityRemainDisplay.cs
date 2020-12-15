using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class DurabilityRemainDisplay : MonoBehaviour
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void ChangeRemain(float currentDurability, float maxDurability)
        {
            _image.fillAmount = currentDurability / (float)maxDurability;
        }
    }
}