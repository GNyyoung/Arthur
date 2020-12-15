using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Immortality : PlayerSkill
    {
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/Immortality");
        }
        
        public override IEnumerator Skill()
        {
            player.CharacterEffect.AddEffect(Effect.Immortality, GetTotalDuration());
            ApplySkillAnimation();
            yield return new WaitForSeconds(skillAnim.length);
            
            EndSkill();
        }
    }
}