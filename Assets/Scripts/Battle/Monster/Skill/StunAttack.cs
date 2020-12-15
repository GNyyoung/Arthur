using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class StunAttack : MonsterSkill
    {
        private bool isUseEffect = false;
        protected override IEnumerator Skill()
        {
            var skillDirections = new[] {AttackDirection.UpperSlash, AttackDirection.Slash};
            yield return StartCoroutine(ShowSkillDirectionAlarm(skillDirections));

            PlaySkillAnimation();
            Monster.DefenceDirection = DefenceVariety.NotDefence(Monster);
            yield return new WaitForSeconds(0.5f);

            // 공격 또는 효과 발동
            isUseEffect = false;
            int damage = Mathf.FloorToInt(1 * Monster.DamageMultiple);
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
            skillAnim = Resources.Load<AnimationClip>($"Animations/{Monster.MonsterStat.Name}/StunAttack");
        }

        public override void UseHitEffect()
        {
            if (isUseEffect == false)
            {
                Monster.Player.CharacterEffect.AddEffect(Effect.Stun, 3.0f);
                isUseEffect = true;   
            }
        }
    }
}