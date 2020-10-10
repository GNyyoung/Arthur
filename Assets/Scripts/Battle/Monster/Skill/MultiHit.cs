using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class MultiHit : MonsterSkill
    {
        protected override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();

            for (int i = 0; i < 4; i++)
            {
                float time = 0;
                
                ShowDirectionEffect(AttackDirection.Slash);
                while (time < 0.33f)
                {
                    time += Time.fixedDeltaTime;
                    yield return waitForFixedUpdate;
                }
            }
            
            for (int i = 0; i < 4; i++)
            {
                ActiveProgress = 0;
                while (ActiveProgress < PreDelay)
                {
                    ActiveProgress += Time.fixedDeltaTime;
                    yield return waitForFixedUpdate;
                }
                
                SkillCastAction.AttackPlayer(2, AttackDirection.Slash);
            }
            
            while (ActiveProgress < PreDelay + PostDelay)
            {
                ActiveProgress += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            
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
    }
}