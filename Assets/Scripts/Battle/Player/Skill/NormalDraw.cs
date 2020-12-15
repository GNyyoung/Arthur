using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class NormalDraw : PlayerSkill
    {
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/NormalDraw");
        }

        public override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            
            ApplySkillAnimation();

            float time = 0;
            while (time < skillAnim.length)
            {
                time += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            EndSkill();
        }
    }
}