using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerIdle : PlayerAction
    {
        public override void StartAction()
        {
            StartCoroutine(CheckMonsterCollision());
            Player.CurrentStatus = PlayerStatus.Idle;
        }

        public override void StopAction()
        {
            StopCoroutine(CheckMonsterCollision());
        }

        public override PlayerStatus GetStatus()
        {
            return PlayerStatus.Idle;
        }

        private IEnumerator CheckMonsterCollision()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            while(true)
            {
                var hit = Player.GetRaycastHitMonster(0);
                if (hit.collider == null)
                {
                    Player.ChangeStatus(Player.GetAction(PlayerStatus.Move));
                    break;
                }

                yield return waitForFixedUpdate;
            }
        }
    }
}