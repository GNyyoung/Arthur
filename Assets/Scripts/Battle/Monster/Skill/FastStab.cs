using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class FastStab : MonsterSkill
    {   
        protected override IEnumerator Skill()
        {
            var skillDirections = new[] {AttackDirection.Stab};
            yield return StartCoroutine(ShowSkillDirectionAlarm(skillDirections));

            yield return new WaitForSeconds(0.2f);
            Monster.DefenceDirection = DefenceVariety.NotDefence(Monster);
            PlaySkillAnimation();

            // 공격 또는 효과 발동
            int damage = Mathf.FloorToInt(3 * Monster.DamageMultiple);
            yield return StartCoroutine(AttackPlayer(skillDirections, damage));
            
            // 후딜
            yield return StartCoroutine(WaitForPostDelay());
            
            EndSkill();
        }
        
        protected override bool IsPauseCooldown()
        {
            return SkillCooldownCondition.IsPauseCooldownWhenIdle(Monster);
        }

        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>($"Animations/{Monster.MonsterStat.Name}/FastStab");
        }
    }
}