using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class UpperAxe : MonsterSkill
    {
        protected override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();

            var skillDirections = new[] {AttackDirection.UpperSlash};
            yield return StartCoroutine(ShowSkillDirectionAlarm(skillDirections));
            
            yield return new WaitForSeconds(0.5f);
            Monster.DefenceDirection = DefenceVariety.NotDefence(Monster);
            PlaySkillAnimation();
            
            int damage = Mathf.FloorToInt(2 * Monster.DamageMultiple);
            yield return StartCoroutine(AttackPlayer(skillDirections, damage));

            yield return StartCoroutine(WaitForPostDelay());
            EndSkill();
        }

        protected override bool IsPauseCooldown()
        {
            return SkillCooldownCondition.IsPauseCooldownWhenIdle(Monster);
        }

        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>($"Animations/{Monster.MonsterStat.Name}/UpperAxe");
        }
    }
}