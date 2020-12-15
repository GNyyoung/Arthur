using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class AttackDisplay : MonoBehaviour, ICooldownObserver
    {
        public Image[] cooldownFilter;
        private IEnumerator _currentEnumerator;
        
        public void DisplayCooldown(Sword sword)
        {
            _currentEnumerator = ActiveCooldown(sword);
            StartCoroutine(_currentEnumerator);
        }

        private IEnumerator ActiveCooldown(Sword sword)
        {
            var enumerator = _currentEnumerator;
            foreach (var filter in cooldownFilter)
            {
                filter.gameObject.SetActive(true);
            }
            while (sword.IsUsable == false && enumerator.Equals(_currentEnumerator) == true)
            {
                foreach (var filter in cooldownFilter)
                {
                    filter.fillAmount = sword.CooldownRest / sword.GetFinalAttackCooldown();
                }

                yield return null;
            }

            if (enumerator.Equals(_currentEnumerator) == true)
            {
                foreach (var filter in cooldownFilter)
                {
                    filter.gameObject.SetActive(true);
                }
            }
        }
    }
}