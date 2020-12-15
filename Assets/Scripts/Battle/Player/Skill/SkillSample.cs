using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class SkillSample : PlayerSkill
    {
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/스킬 애니메이션 이름");
        }

        public override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            bool isDamage = false;
            
            ApplySkillAnimation();

            float time = 0;
            while (time < skillAnim.length)
            {
                // if (isDamage == false &&
                //     time > skillAnim.length * 공격 타이밍)
                // {
                //     var hitMonsters = skillCastAction.GetRaycastHitMonsters(CalcRange());
                //     foreach (var monsterObject in hitMonsters)
                //     {
                //         monsterObject.GetComponent<ICombatant>().TakeDamage(player, CalcDamage(), 공격방향);
                //     }
                //     isDamage = true;
                // }
                
                time += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            EndSkill();
        }
    }
}