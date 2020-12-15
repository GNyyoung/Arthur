using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class FastDraw : PlayerSkill
    {
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/FastDraw");
        }

        public override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            bool isDamage = false;
            
            ApplySkillAnimation();

            ActiveProgress = 0;
            while (ActiveProgress < skillAnim.length)
            {
                if (isDamage == false &&
                    ActiveProgress > skillAnim.length * 0.25f)
                {
                    var hitMonsters = skillCastAction.GetRaycastHitMonsters(GetTotalRange());
                    foreach (var monsterObject in hitMonsters)
                    {
                        monsterObject.GetComponent<Monster>().TakeDamage(player, GetTotalDamage(), AttackDirection.UpperSlash);
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