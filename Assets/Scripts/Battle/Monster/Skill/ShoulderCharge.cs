﻿using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class ShoulderCharge : MonsterSkill
    {
        private GameObject collidedPlayer;
        private Vector3 moveDistPerFrame;
        private float knockbackTime = 0.3f;
        private float totalKnockbackDist = 1.5f;

        protected override void Awake()
        {
            base.Awake();
            moveDistPerFrame = 
                Vector3.left * MoveSpeedController.MonsterMaxSpeed * totalKnockbackDist / knockbackTime * Time.fixedDeltaTime;
        }

        protected override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            
            PlaySkillAnimation();
            Debug.Log("스킬 애니메이션");
            
            yield return new WaitForSeconds(PreDelay);
            DefenceVariety.UpdateDefenceInformation(Monster, AttackDirection.Stab);
            Debug.Log("대기 후 방어자세");
            
            while (true)
            {
                Debug.Log($"{moveDistPerFrame}만큼 이동");
                transform.position += moveDistPerFrame;
                if (collidedPlayer != null)
                {
                    Monster.Player.CharacterEffect.AddEffect(Effect.Knockback, knockbackTime);
                    Debug.Log("넉백 시작");
                    StartCoroutine(KnockbackPlayer());
                    int damage = Mathf.FloorToInt(3 * Monster.DamageMultiple);
                    SkillCastAction.AttackPlayer(damage, AttackDirection.None);
                    break;
                }
                
                yield return waitForFixedUpdate;
            }
            // 공격 또는 효과 발동
            
            Debug.Log("차지 완료");
            
            // 후딜
            yield return StartCoroutine(WaitForPostDelay());
            Debug.Log("후딜 완료");
            EndSkill();
        }

        protected override bool IsPauseCooldown()
        {
            return SkillCooldownCondition.IsPauseCooldownWhenIdle(Monster);
        }

        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>($"Animations/{Monster.MonsterStat.Name}/ShoulderCharge1");
            skillProgressAnim = Resources.Load<AnimationClip>($"Animations/{Monster.MonsterStat.Name}/ShoulderCharge2");
            skillFinishAnim = Resources.Load<AnimationClip>($"Animations/{Monster.MonsterStat.Name}/ShoulderCharge3");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag.Equals("Player") == true)
            {
                collidedPlayer = other.gameObject;
            }
        }

        IEnumerator KnockbackPlayer()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            float progress = 0;
            while (progress < knockbackTime)
            {
                collidedPlayer.transform.position += moveDistPerFrame;
                progress += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }

            collidedPlayer = null;
        }
    }
}