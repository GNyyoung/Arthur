using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class BatDrawAttack : PlayerSkill
    {
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/PowerSlash");
        }

        public override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            bool isDamage = false;
            
            ApplySkillAnimation();

            float time = 0;

            while (time < skillAnim.length)
            {
                if (isDamage == false &&
                    time > skillAnim.length * 0.5f)
                {
                    var hitMonsters = skillCastAction.GetRaycastHitMonsters(GetTotalRange());
                    foreach (var monsterObject in hitMonsters)
                    {
                        monsterObject.GetComponent<Monster>().TakeDamage(player, GetTotalDamage(), AttackDirection.Slash);
                    }
                    isDamage = true;
                }
                
                time += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            
            EndSkill();
        }
    }
}