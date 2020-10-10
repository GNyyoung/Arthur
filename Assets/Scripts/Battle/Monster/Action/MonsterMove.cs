﻿﻿﻿using System;
 using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class MonsterMove : MonsterAction
    {
        private bool isMove = false;
        public override void StartAction()
        {
            this.enabled = true;
            isMove = true;
            StartCoroutine(Move());
        }

        public override void StopAction()
        {
            Debug.Log("이동 중지");
            isMove = false;
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
            while (isMove)
            {
                Monster.transform.position += Vector3.left * ( Player.SpeedController.MonsterSpeed * Time.fixedDeltaTime);
                yield return waitForFixedUpdate;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"충돌 : {other.gameObject.name}");
            switch (other.gameObject.tag)
            {
                case "Player":
                    Debug.Log("플레이어와 충돌");
                    if (MonsterApproach.Instance.IsExistCollide() == false)
                    {
                        MonsterApproach.Instance.FirstApproachedMonster = this.gameObject;
                        // if (Monster.IsPush == true)
                        // {
                        //     other.GetComponent<Player>().Retreat();
                        // }
                    }
                    
                    MonsterApproach.Instance.AddApproach(this.gameObject);
                
                    Monster.StopCurrentStatus();
                    break;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (this.isActiveAndEnabled == true && 
                MonsterApproach.Instance.IsLastApproachedMonster(other.gameObject) == true)
            {
                MonsterApproach.Instance.AddApproach(this.gameObject);
                Monster.StopCurrentStatus();  
            }
        }
    }
}