﻿﻿﻿using System;
 using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class MonsterMove : MonsterAction
    {
        private Coroutine moveCoroutine;

        public override void StartAction()
        {
            Debug.Log($"몬스터{gameObject.GetInstanceID()} 이동 시작");
            this.enabled = true;
            Monster.Animator.SetTrigger("Move");
            moveCoroutine = StartCoroutine(Move());
        }

        public override void StopAction()
        {
            StopCoroutine(moveCoroutine);
            Monster.Animator.SetTrigger("Stop");
            this.enabled = false;
        }

        public override void TerminateAction()
        {
            StopAction();
        }

        public override MonsterStatus GetStatus()
        {
            return MonsterStatus.Move;
        }

        private IEnumerator Move()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            while (true)
            {
                Monster.transform.position += Vector3.left * ( MoveSpeedController.MonsterMaxSpeed * Time.fixedDeltaTime);
                yield return waitForFixedUpdate;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Monster.IsCheckCollision == true)
            {
                switch (other.gameObject.tag)
                {
                    case "Player":
                        Debug.Log($"몬스터{gameObject.GetInstanceID()} : 플레이어와 충돌");
                        if (Monster.isCollided == false)
                        {
                            MonsterApproach.Instance.AddApproach(this.gameObject);
                            Monster.isCollided = true;
                            if (Monster.CurrentAction.GetStatus() == MonsterStatus.Move)
                            {
                                Monster.StopCurrentStatus();   
                            }  
                        }
                        break;  
                }   
            }

            if (other.tag.Equals("DeadLine") == true)
            {
                StartCoroutine(Monster.Die());
            }
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (Monster.IsCheckCollision == true)
            {
                var lastApproachedMonster = MonsterApproach.Instance.GetLastApproachedMonsterWithType(Monster.MonsterType);
        
                if (Monster.isCollided == false &&
                    lastApproachedMonster != null &&
                    other.gameObject.Equals(lastApproachedMonster))
                {
                    Debug.Log($"{gameObject.GetInstanceID()} 체크");
                    MonsterApproach.Instance.AddApproach(this.gameObject);
                    Monster.isCollided = true;
                    Monster.StopCurrentStatus();
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                MonsterApproach.Instance.RemoveApproach(this.gameObject);
            }
        }
    }
}