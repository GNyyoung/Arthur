using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestSkill1 : MonsterSkill
    {
        protected override IEnumerator Skill()
        {
            SkillDirection = (AttackDirection)Enum.Parse(typeof(AttackDirection), UnityEngine.Random.Range(1, 4).ToString());
            transform.Find("Canvas").GetComponent<MonsterDebugUI>().SkillDirectionText.text = $"Skl:{SkillDirection.ToString()}";
            
            Monster.ChangeDefenceDirection(AttackDirection.None);
            ShowDirectionEffect(SkillDirection);
            
            while (ActiveProgress < PreDelay)
            {
                ActiveProgress += Time.fixedDeltaTime;
                DebugAttackTiming(ActiveProgress);
                yield return new WaitForFixedUpdate();
            }

            Debug.Log("몬스터 스킬 사용");
            SkillCastAction.AttackPlayer(2, SkillDirection);
            SkillDirection = AttackDirection.None;
                
            while (ActiveProgress < PreDelay + PostDelay)
            {
                ActiveProgress += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
                
            Monster.ChangeDefenceDirection();
            EndSkill();
        }

        protected override bool IsPauseCooldown()
        {
            if (Monster.CurrentAction?.GetStatus() == MonsterStatus.Idle)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        void DebugAttackTiming(float time)
        {
            transform.Find("Canvas").GetComponent<MonsterDebugUI>().AttacktimingSlider.value = time / PreDelay;
        }
    }
}