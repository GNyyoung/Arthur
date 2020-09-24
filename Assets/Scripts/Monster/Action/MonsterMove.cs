using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class MonsterMove : MonsterAction
    {
        private bool isMove = false;
        public override void StartAction()
        {
            isMove = true;
            StartCoroutine(Move());
        }

        public override void StopAction()
        {
            isMove = false;
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
                var hit = GetRaycastHitPlayer(0);
                if (hit.collider != null)
                {
                    Monster.StopCurrentStatus();
                    break;
                }
                else
                {
                    Monster.transform.position += Vector3.left * 0.5f * Time.fixedDeltaTime;    
                }
                yield return waitForFixedUpdate;
            }
        }
    }
}