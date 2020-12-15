using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class DrawSkillDisplay : MonoBehaviour, ICooldownObserver
    {
        private IEnumerator _currentEnumerator;
        private Image _imageComponent;
        
        public Text cooldownText;
        public Sprite cooldownFinishSprite;
        public Sprite cooldownProgressSprite;

        private void Awake()
        {
            _imageComponent = GetComponent<Image>();
        }

        public void DisplayCooldown(Sword sword)
        {
            if (sword.DrawSkill != null)
            {
                _currentEnumerator = ActiveCooldown(sword.DrawSkill);
                StartCoroutine(_currentEnumerator);
                Debug.Log(StackTraceUtility.ExtractStackTrace());   
            }
        }

        private IEnumerator ActiveCooldown(PlayerSkill skill)
        {
            var enumerator = _currentEnumerator;
            _imageComponent.sprite = cooldownProgressSprite;
            while (skill.IsUsable == false && enumerator.Equals(_currentEnumerator) == true)
            {
                cooldownText.text = Mathf.FloorToInt(skill.CooldownRest).ToString();
                yield return null;
            }

            if (enumerator.Equals(_currentEnumerator) == true)
            {
                // 쿨타임이 끝난 뒤의 효과
                cooldownText.text = "";
                _imageComponent.sprite = cooldownFinishSprite;
            }
        }
    }
}