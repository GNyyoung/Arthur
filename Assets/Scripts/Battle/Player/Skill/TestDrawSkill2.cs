using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestDrawSkill2 : PlayerSkill
    {
        protected override void SetSkillAnimation()
        {
            Cooldown = 5f;
        }

        public override void Active(PlayerSkillCast skillCastAction)
        {
            base.Active(skillCastAction);
            if (IsUsable == true)
            {
                SkillCoroutine = StartCoroutine(DrawSkillName());
            }
        }

        public override IEnumerator Skill()
        {
            throw new NotImplementedException();
        }

        private IEnumerator DrawSkillName()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            
            Debug.Log("발도 스킬 사용");
            
            skillCastAction.EndSkill();
            
            yield return waitForFixedUpdate;
        }
    }
}