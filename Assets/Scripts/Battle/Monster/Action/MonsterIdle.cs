﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class MonsterIdle : MonsterAction
    {
        private const float IDLE_CHANGE_DELAY = 0.1f;
        private bool isActive = false;
        private Coroutine _idleCoroutine;
        
        public override void StartAction()
        {
            Debug.Log($"{gameObject.GetInstanceID()}몬스터 대기 시작");
            this.enabled = true;
            isActive = true;
            if (_idleCoroutine == null)
            {
                _idleCoroutine = StartCoroutine(CheckIdleCondition());    
            }
        }

        public override void StopAction()
        {
            isActive = false;
            this.enabled = false;
        }

        public override void TerminateAction()
        {
            if (_idleCoroutine != null)
            {
                _idleCoroutine = null;
            }
            isActive = false;
            this.enabled = false;
        }

        public override MonsterStatus GetStatus()
        {
            return MonsterStatus.Idle;
        }

        private IEnumerator CheckIdleCondition()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            
            while (Monster.isCollided)
            {
                if (MonsterApproach.Instance.IsPush == true)
                {
                    transform.position += Vector3.left * (Player.SpeedController.GetRetreatSpeed() * Time.fixedDeltaTime);
                }
                yield return waitForFixedUpdate;
            }
            
            Debug.Log("몬스터 대기 중지");
            
            // yield return new WaitUntil(() => Monster.CurrentAction.GetStatus() == MonsterStatus.Idle);
            // _idleCoroutine = null;
            // Monster.StopCurrentStatus();

            _idleCoroutine = null;
            if (Monster.CurrentAction.GetStatus() == MonsterStatus.Idle)
            {
                Monster.StopCurrentStatus();
            }
        }
    }
}