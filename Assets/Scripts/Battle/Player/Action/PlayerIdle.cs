using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerIdle : PlayerAction
    {
        private GameObject collidedMonster;
        private Coroutine _idleCoroutine;
        
        public override void StartAction()
        {
            base.StartAction();
            if (_idleCoroutine == null)
            {
                _idleCoroutine = StartCoroutine(CheckIdleCondition());   
            }
        }

        public override void StopAction()
        {
            base.StopAction();
        }

        public override PlayerStatus GetStatus()
        {
            return PlayerStatus.Idle;
        }

        private IEnumerator CheckIdleCondition()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            Debug.Log($"몬스터 충돌 : {MonsterApproach.Instance.IsExistCollide()}");
            while(MonsterApproach.Instance.IsExistCollide() == true ||
                  Player.CurrentAction.GetStatus() != PlayerStatus.Idle)
            {
                // if (MonsterApproach.Instance.IsPush == true)
                // {
                //     transform.position += Vector3.left * (Player.SpeedController.GetRetreatSpeed() * Time.fixedDeltaTime);   
                // }
                yield return waitForFixedUpdate;
            }
            
            _idleCoroutine = null;
            if (Player.CurrentAction.GetStatus() == PlayerStatus.Idle)
            {
                Player.StopCurrentStatus();   
            }
        }
    }
}