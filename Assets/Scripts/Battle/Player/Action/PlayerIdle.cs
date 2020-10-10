using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerIdle : PlayerAction
    {
        private GameObject collidedMonster;
        private Coroutine _currentCoroutine;
        
        public override void StartAction()
        {
            base.StartAction();
            _currentCoroutine = StartCoroutine(CheckIdleCondition());
        }

        public override void StopAction()
        {
            base.StopAction();
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);   
            }
        }

        public override PlayerStatus GetStatus()
        {
            return PlayerStatus.Idle;
        }

        private IEnumerator CheckIdleCondition()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            while(MonsterApproach.Instance.IsExistCollide() == true)
            {
                yield return waitForFixedUpdate;
            }
            
            Player.StopCurrentStatus();
        }
    }
}