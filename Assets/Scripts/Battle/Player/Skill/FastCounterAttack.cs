using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class FastCounterAttack : PlayerSkill
    {
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/Counter");
        }

        public override IEnumerator Skill()
        {
            player.CharacterEffect.AddEffect(Effect.Counter, GetTotalDuration());
            ApplySkillAnimation();
            yield return new WaitForSeconds(skillAnim.length);
            
            EndSkill();
        }
    }
}