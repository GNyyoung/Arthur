using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ActiveSkillDisplay : MonoBehaviour, ICooldownObserver
    {
        public Image cooldownFilter;
        private IEnumerator _currentEnumerator;
        
        public void DisplayCooldown(Sword sword)
        {
            if (sword.ActiveSkill != null)
            {
                _currentEnumerator = ActiveCooldown(sword.ActiveSkill);
                StartCoroutine(_currentEnumerator);   
            }
        }
        
        private IEnumerator ActiveCooldown(PlayerSkill skill)
        {
            var enumerator = _currentEnumerator;
            cooldownFilter.gameObject.SetActive(true);
            Debug.Log(skill);
            while (skill.IsUsable == false && enumerator.Equals(_currentEnumerator) == true)
            {
                cooldownFilter.fillAmount = skill.CooldownRest / skill.Cooldown;
                yield return null;
            }

            if (enumerator.Equals(_currentEnumerator) == true)
            {
                // 쿨타임이 끝난 뒤의 효과
                cooldownFilter.gameObject.SetActive(false);    
            }
        }
    }
}