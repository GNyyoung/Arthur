using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Stun : PlayerSkill
    {
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/Stun");
        }

        public override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            bool isDamage = false;
            
            ApplySkillAnimation();
            var hitMonster = skillCastAction.GetRaycastFrontMonster(GetTotalRange());
            hitMonster.GetComponent<Monster>().CharacterEffect.AddEffect(Effect.Stun, GetTotalDuration());

            ActiveProgress = 0;
            while (ActiveProgress < skillAnim.length)
            {
                ActiveProgress += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            
            EndSkill();
        }
    }
}