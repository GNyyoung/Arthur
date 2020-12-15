using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Smash : PlayerSkill
    {
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/Smash");
        }
        
        public override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            bool isDamage = false;
            ActiveProgress = 0;
            
            ApplySkillAnimation();

            while (ActiveProgress < skillAnim.length)
            {
                if (isDamage == false &&
                    ActiveProgress > skillAnim.length * 0.611f)
                {
                    var hitMonsters = skillCastAction.GetRaycastHitMonsters(GetTotalRange());
                    foreach (var monsterObject in hitMonsters)
                    {
                        monsterObject.GetComponent<Monster>().TakeDamage(player, GetTotalDamage(), AttackDirection.Slash);
                    }
                    isDamage = true;
                }
                
                ActiveProgress += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            
            EndSkill();
        }
    }
}