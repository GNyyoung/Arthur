using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class BackJumpHeal : MonsterSkill
    {
        private Vector3 jumpDistPerFrame;
        private float healTime = 2.0f;
        private float healTick;
        private float healRatePerTick = 0.01f;

        protected override void Awake()
        {
            base.Awake();
            jumpDistPerFrame = Vector3.right * MoveSpeedController.MonsterMaxSpeed * 6 * Time.fixedDeltaTime;
            healTick = 0.25f;
        }

        protected override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            float progress = 0;
            
            PlaySkillAnimation();
            
            Monster.DefenceDirection = DefenceVariety.NotDefence(Monster);

            while (progress < PreDelay)
            {
                transform.position += jumpDistPerFrame;
                progress += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }

            progress = 0;
            float healProgress = 0;
            while (progress < healTime)
            {
                if (healProgress > healTick)
                {
                    Debug.Log("체력 회복");
                    Monster.CurrentHP += Monster.MaxHP * healRatePerTick;
                    if (Monster.CurrentHP > Monster.MaxHP)
                    {
                        Monster.CurrentHP = Monster.MaxHP;
                    }
                    healProgress = 0;
                }
                else
                {
                    healProgress += Time.fixedDeltaTime;
                }
                
                progress += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            
            // 후딜
            yield return StartCoroutine(WaitForPostDelay());
            
            EndSkill();
        }

        protected override bool IsPauseCooldown()
        {
            return false;
        }

        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>($"Animations/{Monster.MonsterStat.Name}/BackJumpHeal");
        }
    }
}