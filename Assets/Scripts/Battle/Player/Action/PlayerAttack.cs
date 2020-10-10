using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerAttack : PlayerAction
    {
        public AttackDirection AttackDirection { get; set; } = AttackDirection.None;
        private Coroutine currentCoroutine;
            
        public override void StartAction()
        {
            base.StartAction();
            Player.Animator.SetTrigger(AttackDirection.ToString());
            Player.Animator.speed = 1.0f / Player.CurrentSword.CooldownTime;
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

        private IEnumerator Attack()
        {
            //검에서 스탯 가져와서 공격하기

            var waitForFixedUpdate = new WaitForFixedUpdate();
            float time = 0;
            bool isDamage = false;
            var sword = Player.CurrentSword; 
            
            Player.CurrentSword.StartCooldown();
            while (time < sword.CooldownTime)
            {
                if (isDamage == false && time >= sword.DamageTime)
                {
                    var hits = GetRaycastHitMonsters(sword.Length);
                    Debug.Log($"공격타겟 수 : {hits.Length}");
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].collider.GetComponent<ICombatant>()
                            .TakeDamage(Player, Mathf.FloorToInt(sword.Damage), AttackDirection) == true)
                        {
                            Player.CurrentSword.DamageByAttack(Player);
                        }
                        else
                        {
                            Player.CurrentSword.DamageByBadAttack(Player);
                        }
                    }
                    
                    isDamage = true;
                }

                time += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            
            Player.StopCurrentStatus();
        }
    }
}