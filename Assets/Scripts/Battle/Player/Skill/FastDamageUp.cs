using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class FastDamageUp : PlayerSkill
    {
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/RaiseSword");
        }

        public override IEnumerator Skill()
        {
            player.CharacterEffect.AddEffect(Effect.DamageUp, GetTotalDamage());
            ApplySkillAnimation();
            yield return new WaitForSeconds(skillAnim.length);
            
            EndSkill();
        }
    }
}