using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerAttack : PlayerAction
    {
        public AttackDirection AttackDirection { get; set; } = AttackDirection.None;
        private Coroutine currentCoroutine;

        private Dictionary<AttackDirection, PlayerSound.SoundType> swingSoundTypeSet =
            new Dictionary<AttackDirection, PlayerSound.SoundType>();

        public PlayerAttack()
        {
            swingSoundTypeSet.Add(AttackDirection.None, PlayerSound.SoundType.None);
            swingSoundTypeSet.Add(AttackDirection.Slash, PlayerSound.SoundType.Slash);
            swingSoundTypeSet.Add(AttackDirection.UpperSlash, PlayerSound.SoundType.UpperSlash);
            swingSoundTypeSet.Add(AttackDirection.Stab, PlayerSound.SoundType.Stab);
        }

        public override void StartAction()
        {
            base.StartAction();
            Player.Animator.SetTrigger(AttackDirection.ToString());
            Debug.Log($"{AttackDirection.ToString()}방향 공격 시작");
            currentCoroutine = StartCoroutine(Attack());
        }

        public override void StopAction()
        {
            Debug.Log("공격취소");
            base.StopAction();
            Player.Animator.speed = 1;
            StopCoroutine(currentCoroutine);
        }

        public override PlayerStatus GetStatus()
        {
            return PlayerStatus.Attack;
        }

        /// <summary>
        /// 애니메이션 도중 데미지를 입힌다.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Attack()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            float time = 0;
            bool isDamage = false;
            var sword = Player.CurrentSword; 
            
            Player.CurrentSword.StartCooldown();
            var audioSource = Player.GetComponent<AudioSource>();
            Player.SoundSet.OutputSound(swingSoundTypeSet[AttackDirection]);
            
            Player.Animator.speed = 1.0f / Player.CurrentSword.DamageTime;
            while (time < sword.AttackCooldown)
            {
                if (isDamage == false && time >= sword.DamageTime * 0.75f)
                {
                    var hitMonsters = GetRaycastHitMonsters(sword.Length);
                    Debug.Log($"공격타겟 수 : {hitMonsters.Length}");
                    float swordDamageRate = 0;
                    for (int i = 0; i < hitMonsters.Length; i++)
                    {
                        var monster = hitMonsters[i].GetComponent<Monster>();
                        // 몬스터가 방어한 방향을 공격하면 false 반환
                        if (monster.TakeDamage(Player, Mathf.FloorToInt(sword.Damage), AttackDirection) == true)
                        {
                            swordDamageRate = monster.SwordDamageRate;
                        }
                        else
                        {
                            swordDamageRate = monster.SwordDamageRate * 2;
                        }
                    }

                    Player.Animator.speed = 1.0f / (Player.CurrentSword.AttackCooldown - Player.CurrentSword.DamageTime);
                    Player.CurrentSword.DamageByAttack(Player, swordDamageRate);
                    isDamage = true;
                }

                time += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            
            Player.StopCurrentStatus();
        }
    }
}