using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerMove : PlayerAction
    {
        private Coroutine moveCoroutine;
        private Coroutine _retreatCoroutine;
        
        public override void StartAction()
        {
            Debug.Log("플레이어 이동 시작");
            base.StartAction();
            Player.SpeedController.UpdatePlayerSpeed(Player.SpeedController.MaxSpeed / 2.0f);
            moveCoroutine = StartCoroutine(Move());
        }

        public override void StopAction()
        {
            base.StopAction();
            Player.SpeedController.UpdatePlayerSpeed(0);
            StopCoroutine(moveCoroutine);
        }

        public override PlayerStatus GetStatus()
        {
            return PlayerStatus.Move;
        }

        private IEnumerator Move()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            while (true)
            {
                Player.transform.position += Vector3.right * (Player.SpeedController.PlayerSpeed * Time.fixedDeltaTime);   
                yield return waitForFixedUpdate;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Player.CurrentAction.Equals(this) == true)
            {
                Player.StopCurrentStatus();    
            }
        }

        public void StartRetreat()
        {
            if (_retreatCoroutine == null)
            {
                _retreatCoroutine = StartCoroutine(Retreat());
            }
        }
        
        private IEnumerator Retreat()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            
            while (MonsterApproach.Instance.IsExistCollide() == true)
            {
                transform.position += Vector3.left * (Player.SpeedController.GetRetreatSpeed() * Time.fixedDeltaTime);
                yield return waitForFixedUpdate;
            }

            _retreatCoroutine = null;
        }
    }
}