using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class DashStab : PlayerSkill
    {
        const float DASH_DISTANCE = 1.5f;
        private const float ATTACK_COUNT = 3;
        
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/DashStab");
        }

        public override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            float dashStartTime = skillAnim.length * 17 / 40;
            float dashEndTime = skillAnim.length * 31 / 40;
            float dashDistancePerFrame = DASH_DISTANCE / (dashEndTime - dashStartTime) * Time.fixedDeltaTime;
            float attackIndex = 0;
            float attackInterval = (dashEndTime - dashStartTime) / ATTACK_COUNT;

            ActiveProgress = 0;
            ApplySkillAnimation();
            while (ActiveProgress < skillAnim.length)
            {
                if (ActiveProgress >= dashStartTime &&
                    ActiveProgress < dashEndTime)
                {
                    PushPosition(player.gameObject, dashDistancePerFrame);
                    var approachedMonsters = MonsterApproach.Instance.GetAllApproachedMonsters();
                    foreach (var approachedMonster in approachedMonsters)
                    {
                        PushPosition(approachedMonster.gameObject, dashDistancePerFrame);
                    }

                    if (ActiveProgress >= dashStartTime + attackInterval * attackIndex)
                    {
                        var hitMonsters = skillCastAction.GetRaycastHitMonsters(GetTotalRange());
                        foreach (var monsterObject in hitMonsters)
                        {
                            monsterObject.GetComponent<Monster>().TakeDamage(player, GetTotalDamage(), AttackDirection.Stab);
                        }
                        attackIndex += 1;
                    }
                }
                
                ActiveProgress += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            
            EndSkill();
        }
    }
}