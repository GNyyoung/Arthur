using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class MonsterSkillSample : MonsterSkill
    {
        protected override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            
            PlaySkillAnimation();

            //// 스킬 경고 및 선딜
            // for (int i = 0; i < 공격횟수; i++)
            // {
            //     float time = 0;
            //     
            //     ShowDirectionEffect(AttackDirection.Slash);
            //     while (time < PreDelay)
            //     {
            //         time += Time.fixedDeltaTime;
            //         yield return waitForFixedUpdate;
            //     }
            // }

            //// 공격 또는 효과 발동
            // SkillDirection = AttackDirection.Slash;
            // for (int i = 0; i < ATTACK_COUNT; i++)
            // {
            //     ActiveProgress = 0;
            //     while (ActiveProgress < PreDelay)
            //     {
            //         ActiveProgress += Time.fixedDeltaTime;
            //         // DebugAttackTiming();
            //         yield return waitForFixedUpdate;
            //     }
            //     
            //     SkillCastAction.AttackPlayer(Mathf.FloorToInt(2 * Monster.DamageMultiple), SkillDirection);
            // }
            
            //// 후딜
            // while (ActiveProgress < PreDelay + PostDelay)
            // {
            //     ActiveProgress += Time.fixedDeltaTime;
            //     yield return new WaitForFixedUpdate();
            // }

            // 복사 시 제거
            yield return null;
            
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

        protected override void SetSkillAnimation()
        {
            
        }
    }
}