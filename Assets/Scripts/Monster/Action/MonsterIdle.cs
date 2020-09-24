using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class MonsterIdle : MonsterAction
    {
        private bool isActive = false;
        public override void StartAction()
        {
            Debug.Log("플레이어 대기 시작");
            isActive = true;
            StartCoroutine(CheckPlayerCollision());
        }

        public override void StopAction()
        {
            isActive = false;
        }

        public override MonsterStatus GetStatus()
        {
            return MonsterStatus.Idle;
        }

        private IEnumerator CheckPlayerCollision()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            while(isActive)
            {
                var hit = GetRaycastHitPlayer(0);
                if (hit.collider == null)
                {
                    Monster.ChangeStatus(Monster.GetAction(MonsterStatus.Move));
                    break;
                }

                yield return waitForFixedUpdate;
            }
        }
    }
}