﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class MonsterIdle : MonsterAction
    {
        private bool isActive = false;
        private Coroutine _pushCoroutine;
        
        public override void StartAction()
        {
            Debug.Log($"{gameObject.GetInstanceID()}몬스터 대기 시작");
            this.enabled = true;
            isActive = true;
            StartCoroutine(CheckIdleCondition());
            if (_pushCoroutine == null)
            {
                _pushCoroutine = StartCoroutine(PushPlayer());    
            }
        }

        public override void StopAction()
        {
            isActive = false;
            this.enabled = false;
        }

        public override void TerminateAction()
        {
            if (_pushCoroutine != null)
            {
                StopCoroutine(_pushCoroutine);
                _pushCoroutine = null;    
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
            while (isActive)
            {
                if (MonsterApproach.Instance.IsExistCollide() == false)
                {
                    Monster.StopCurrentStatus();
                    break;
                }

                yield return waitForFixedUpdate;
            }
        }
        
        private IEnumerator PushPlayer()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            
            while (MonsterApproach.Instance.IsExistCollide() == true)
            {
                transform.position +=
                    Vector3.left * (Player.SpeedController.GetRetreatSpeed() * Time.fixedDeltaTime);
                yield return waitForFixedUpdate;
            }

            _pushCoroutine = null;
        }
    }
}