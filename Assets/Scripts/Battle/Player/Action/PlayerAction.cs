using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class PlayerAction : MonoBehaviour, IPlayerAction
    {
        protected Player Player { get; private set; }

        protected void Start()
        {
            // enable 가능하게 하려고 생성함.
        }

        public virtual void InitializeAction(Player player)
        {
            Player = player;
        }

        public virtual void StartAction()
        {
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
        
        public RaycastHit2D[] GetRaycastHitMonsters(float range)
        {
            var playerCollider = GetComponent<BoxCollider2D>();
            int layerMask = 1 << LayerMask.NameToLayer("Monster");
        
            // var rayPosition = gameObject.transform.position + (Vector3.down * GetComponent<BoxCollider2D>().size.y / 2);
            var rayPosition = transform.position + (Vector3.up * GetComponent<BoxCollider2D>().size.y / 2);
            var hits = Physics2D.RaycastAll
                (rayPosition, Vector2.right, (playerCollider.size.x / 2) + range, layerMask);
            Debug.DrawRay(rayPosition, Vector3.right * (playerCollider.size.x / 2 + range), Color.red);
            return hits;
        }
    }    
}


