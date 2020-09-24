using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerMove : PlayerAction
    {
        private bool isMove = false;
        public override void StartAction()
        {
            Debug.Log("플레이어 이동 시작");
            isMove = true;
            StartCoroutine(Move());
        }

        public override void StopAction()
        {
            isMove = false;
            StopCoroutine(Move());
        }

        public override PlayerStatus GetStatus()
        {
            return PlayerStatus.Move;
        }

        private IEnumerator Move()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            while (isMove)
            {
                var hit = Player.GetRaycastHitMonster(0);
                if (hit.collider != null)
                {
                    Player.StopCurrentStatus();
                    break;
                }
                else
                {
                    Player.transform.position += Vector3.right * Time.fixedDeltaTime;    
                }
                yield return waitForFixedUpdate;
            }
        }
    }
}