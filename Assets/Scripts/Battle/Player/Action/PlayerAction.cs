using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class PlayerAction : MonoBehaviour, IPlayerAction
    {
        protected Player Player { get; private set; }
        private BoxCollider2D _playerCollider;

        protected void Start()
        {
            // enable 가능하게 하려고 생성함.
        }

        public virtual void InitializeAction(Player player)
        {
            Player = player;
            _playerCollider = player.transform.Find("PlayerSprite").GetComponent<BoxCollider2D>();
        }

        public virtual void StartAction()
        {
            Debug.Log($"액션 시작 : {GetStatus()}");
            this.enabled = true;
            Player.Animator.SetTrigger(GetStatus().ToString());
        }

        public virtual void StopAction()
        {
            this.enabled = false;
        }
        public abstract PlayerStatus GetStatus();

        public bool IsPossibleChange(IPlayerAction currentAction)
        {
            if (currentAction.GetStatus() >= GetStatus())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public GameObject[] GetRaycastHitMonsters(float range)
        {
            int layerMask = 1 << LayerMask.NameToLayer("Monster");
            var colliderSize = _playerCollider.size;
            var hits = Physics2D.RaycastAll(
                _playerCollider.transform.position - Vector3.right * 0.2f, 
                Vector2.right, 
                (colliderSize.x / 2) + range, layerMask);
            Debug.DrawRay(_playerCollider.transform.position - Vector3.right * 0.2f, Vector3.right * ((colliderSize.x / 2) + range), Color.red, 0.1f);

            var collidedMonsters = new GameObject[hits.Length];
            for (int i = 0; i < collidedMonsters.Length; i++)
            {
                collidedMonsters[i] = hits[i].collider.gameObject;
            }

            return collidedMonsters;
        }

        public GameObject GetRaycastFrontMonster(float range)
        {
            int layerMask = 1 << LayerMask.NameToLayer("Monster");
            var colliderSize = _playerCollider.size;
            var hit = Physics2D.Raycast(
                _playerCollider.transform.position - Vector3.right * 0.2f, 
                Vector2.right, 
                (colliderSize.x / 2) + range, layerMask);
            Debug.DrawRay(_playerCollider.transform.position - Vector3.right * 0.2f, Vector3.right * ((colliderSize.x / 2) + range), Color.red, 0.1f);

            if (hit.collider == null)
            {
                return null;
            }
            else
            {
                return hit.transform.gameObject;   
            }
        }
    }    
}


