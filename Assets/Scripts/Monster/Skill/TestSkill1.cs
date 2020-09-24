using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestSkill1 : MonsterSkill
    {
        public override void Active()
        {
            base.Active();
            if (IsUsable == true)
            {
                SkillCoroutine = StartCoroutine(TestSkill());
            }
        }

        private IEnumerator TestSkill()
        {
            SkillDirection = (AttackDirection)Enum.Parse(typeof(AttackDirection), UnityEngine.Random.Range(1, 4).ToString());
            transform.Find("Canvas").GetComponent<MonsterDebugUI>().DefenceDirectionText.text = SkillDirection.ToString();
            
            while (ActiveProgress < PreDelay)
            {
                ActiveProgress += Time.fixedDeltaTime;
                DebugAttackTiming(ActiveProgress);
                yield return new WaitForFixedUpdate();
            }

            Debug.Log("몬스터 스킬 사용");
            SkillCastAction.AttackPlayer(2, SkillDirection);
            SkillDirection = AttackDirection.None;
                
            while (ActiveProgress < PostDelay)
            {
                ActiveProgress += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
                
            SkillCastAction.EndSkill();
            
            StartCooldown();
        }

        void DebugAttackTiming(float time)
        {
            transform.Find("Canvas").GetComponent<MonsterDebugUI>().AttacktimingSlider.value = time / PreDelay;
        }
    }
}