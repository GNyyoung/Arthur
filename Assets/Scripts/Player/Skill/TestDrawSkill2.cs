using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestDrawSkill2 : PlayerSkill
    {
        private void Awake()
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
        
        private IEnumerator DrawSkillName()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            
            Debug.Log("발도 스킬 사용");
            
            SkillCastAction.EndSkill();
            
            yield return waitForFixedUpdate;
        }
    }
}