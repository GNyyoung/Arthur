using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class DrawSkillDisplay : MonoBehaviour, ICooldownObserver
    {
        public Text cooldownText;
        private IEnumerator _currentEnumerator;
        
        public void DisplayCooldown(Sword sword)
        {
            _currentEnumerator = ActiveCooldown(sword.DrawSkill);
            StartCoroutine(_currentEnumerator);
            Debug.Log(StackTraceUtility.ExtractStackTrace());
        }

        private IEnumerator ActiveCooldown(PlayerSkill skill)
        {
            var enumerator = _currentEnumerator;
            while (skill.IsUsable == false && enumerator.Equals(_currentEnumerator) == true)
            {
                cooldownText.text = Mathf.FloorToInt(skill.CooldownRest).ToString();
                yield return null;
            }

            if (enumerator.Equals(_currentEnumerator) == true)
            {
                // 쿨타임이 끝난 뒤의 효과
                cooldownText.text = "";    
            }
        }
    }
}