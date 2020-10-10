using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestSkill : PlayerSkill
    {
        private void Awake()
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
        
        private IEnumerator SkillName()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            
            yield return waitForFixedUpdate;
            
            Debug.Log("스킬 사용");
            
            SkillCastAction.EndSkill();
        }
    }
}