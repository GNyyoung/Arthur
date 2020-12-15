using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerMove : PlayerAction
    {
        private Coroutine moveCoroutine;
        private Coroutine _retreatCoroutine;
        private GameObject mainCamera;
        public bool isMove = true;

        private void Awake()
        {
            mainCamera = Camera.main.gameObject;
        }

        public override void StartAction()
        {
            Debug.Log("플레이어 이동 시작");
            base.StartAction();
            Player.SpeedController.UpdatePlayerSpeed(MoveSpeedController.PlayerMaxSpeed);
            if (moveCoroutine == null)
            {
                moveCoroutine = StartCoroutine(Move());   
            }
        }

        public override void StopAction()
        {
            base.StopAction();
            // Player.SpeedController.UpdatePlayerSpeed(0);
            // StopCoroutine(moveCoroutine);
        }

        public override PlayerStatus GetStatus()
        {
            return PlayerStatus.Move;
        }

        private IEnumerator Move()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            while (MonsterApproach.Instance.IsExistCollide() == false)
            {
                if (isMove == true)
                {
                    var distance = Vector3.right * (Player.SpeedController.PlayerSpeed * Time.fixedDeltaTime);
                    Player.transform.position += distance;
                }
                
                yield return waitForFixedUpdate;
            }

            Debug.Log("이동 취소");
            _retreatCoroutine = StartCoroutine(Retreat());
            moveCoroutine = null;
            if (Player.CurrentAction.GetStatus() == PlayerStatus.Move)
            {
                Player.StopCurrentStatus();
            }
        }

        public void StartRetreat()
        {
            if (_retreatCoroutine == null)
            {
                // _retreatCoroutine = StartCoroutine(Retreat());
            }
        }
        
        private IEnumerator Retreat()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            float delayProgress = 0;
            bool isActiveSpeed = false;
            
            Debug.Log("후퇴 활성화");
            Player.SpeedController.ActiveRetreatSpeed(false);
            
            while (MonsterApproach.Instance.IsPush == true)
            {
                if (delayProgress < MonsterApproach.RETREAT_DELAY)
                {
                    delayProgress += Time.fixedDeltaTime;
                }
                else
                {
                    if (isActiveSpeed == false)
                    {
                        Player.SpeedController.ActiveRetreatSpeed(true);
                    }
                    transform.position += Vector3.left * (Player.SpeedController.GetRetreatSpeed() * Time.fixedDeltaTime);
                }
                
                yield return waitForFixedUpdate;
            }
        
            Debug.Log("후퇴 종료");
            _retreatCoroutine = null;
        }

        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     throw new NotImplementedException();
        // }
    }
}