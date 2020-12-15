using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestSkill : PlayerSkill
    {
        protected override void SetSkillAnimation()
        {
            Cooldown = 8f;
        }

        public override void Active(PlayerSkillCast skillCastAction)
        {
            base.Active(skillCastAction);
            if (IsUsable == true)
            {
                SkillCoroutine = StartCoroutine(SkillName());
            }
        }

        public override IEnumerator Skill()
        {
            throw new NotImplementedException();
        }

        private IEnumerator SkillName()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            
            yield return waitForFixedUpdate;
            
            Debug.Log("스킬 사용");
            
            skillCastAction.EndSkill();
        }
    }
}