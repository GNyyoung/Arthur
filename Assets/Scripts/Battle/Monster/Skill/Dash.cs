using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Dash : MonsterSkill
    {   
        protected override IEnumerator Skill()
        {
            const float duration = 2.0f;
            var waitForFixedUpdate = new WaitForFixedUpdate();
            float time = 0;
            int layerMask = 1 << LayerMask.NameToLayer("Player");
            var dashVelocity = Vector3.left * (MoveSpeedController.MaxSpeed * MoveSpeedController.StandardRetreatRate * 1.5f);
            
            PlaySkillAnimation();
            Monster.DefenceDirection = AttackDirection.Stab;
            
            while (time < duration)
            {
                var hit = Physics2D.Raycast(this.gameObject.transform.position, Vector2.left, 1.0f, layerMask);
                if (hit.collider != null)
                {
                    var moveDistance = dashVelocity * Time.fixedDeltaTime;
                    hit.collider.transform.position += moveDistance;
                    transform.position += moveDistance;
                }
                time += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            
            EndSkill();
        }

        protected override bool IsPauseCooldown()
        {
            return SkillCooldownCondition.IsPauseCooldownWhenIdle(Monster);
        }

        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>($"Animations/{Monster.MonsterStat.Name}/Dash");
        }
    }
}