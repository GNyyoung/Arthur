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
            
            // Monster.ChangeDefenceDirection(AttackDirection.None);
            Monster.DefenceDirection = DefenceVariety.NotDefence(Monster);
            // ShowDirectionEffect(SkillDirection);
            
            while (ActiveProgress < PreDelay)
            {
                ActiveProgress += Time.fixedDeltaTime;
                // DebugAttackTiming();
                yield return new WaitForFixedUpdate();
            }

            Debug.Log("몬스터 스킬 사용");
            SkillCastAction.AttackPlayer(2, SkillDirection);
                
            while (ActiveProgress < PreDelay + PostDelay)
            {
                ActiveProgress += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
                
            Monster.ChangeDefendDirection(Monster);
            EndSkill();
        }

        protected override bool IsPauseCooldown()
        {
            return SkillCooldownCondition.IsPauseCooldownWhenIdle(Monster);
        }

        protected override void SetSkillAnimation()
        {
            
        }
    }
}