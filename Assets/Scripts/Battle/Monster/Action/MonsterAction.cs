using System;
using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// 몬스터의 행동을 생성할 때 사용
    /// </summary>
    public abstract class MonsterAction: MonoBehaviour, IMonsterAction
    {
        protected Monster Monster { get; private set; }

        private void Awake()
        {
            Monster = GetComponent<Monster>();
        }
        
        public GameObject GetRaycastHitPlayer(float range)
        {
            // var monsterSprite = Monster.GetComponent<SpriteRenderer>().sprite;
            var monsterCollider = Monster.GetComponent<BoxCollider2D>();
            int layerMask = 1 << LayerMask.NameToLayer("Player");
            GameObject hitObject = null;

            var hit = Physics2D.Raycast
                (transform.position, Vector2.left, (monsterCollider.size.x / 2) + range, layerMask);
            if (hit.collider != null)
            {
                hitObject = hit.transform.parent.gameObject;
            }
            return hitObject;
        }

        public abstract void StartAction();
        public abstract void StopAction();
        public abstract void TerminateAction();
        public abstract MonsterStatus GetStatus();
    }
}