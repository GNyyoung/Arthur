using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class MultiHit : MonsterSkill
    {
        private const int ATTACK_COUNT = 3;

        protected override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();

            var skillDirectionList = new List<AttackDirection>();
            for (int i = 0; i < ATTACK_COUNT; i++)
            {
                skillDirectionList.Add(AttackDirection.Slash);
            }
            
            yield return StartCoroutine(ShowSkillDirectionAlarm(skillDirectionList.ToArray()));

            PlaySkillAnimation();
            Monster.DefenceDirection = DefenceVariety.NotDefence(Monster);
            yield return new WaitForSeconds(1.0f);

            int damage = Mathf.FloorToInt(2 * Monster.DamageMultiple);
            yield return StartCoroutine(AttackPlayer(skillDirectionList.ToArray(), damage));

            yield return StartCoroutine(WaitForPostDelay());
            
            EndSkill();
        }

        protected override bool IsPauseCooldown()
        {
            return SkillCooldownCondition.IsPauseCooldownWhenIdle(Monster);
        }

        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>($"Animations/{Monster.MonsterStat.Name}/MultiHit");
        }
    }
}