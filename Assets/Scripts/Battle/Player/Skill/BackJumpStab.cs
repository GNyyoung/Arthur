using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class BackJumpStab : PlayerSkill
    {
        private const float JumpDistance = 1.5f;
        private const float JumpTimeRate = 0.5f;
        
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/BackJumpStab");
        }
        public override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            ApplySkillAnimation();

            ActiveProgress = 0;
            bool isDamage = false;
            float jumpDistPerFrame = JumpDistance / (skillAnim.length * JumpTimeRate);
            Debug.Log(jumpDistPerFrame);
            while (ActiveProgress < skillAnim.length)
            {
                if (ActiveProgress < skillAnim.length * JumpTimeRate)
                {
                    transform.position += Vector3.left * jumpDistPerFrame;
                }
                
                if (isDamage == false && 
                    ActiveProgress > skillAnim.length * 0.66f)
                {
                    isDamage = true;
                    var monsters = skillCastAction.GetRaycastHitMonsters(GetTotalRange());
                    foreach (var monster in monsters)
                    {
                        monster.GetComponent<Monster>().TakeDamage(player, GetTotalDamage(), AttackDirection.Stab);
                    }
                }
                ActiveProgress += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            
            EndSkill();
        }
    }
}