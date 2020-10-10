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

        protected void Start()
        {
            
        }
        
        public RaycastHit2D GetRaycastHitPlayer(float range)
        {
            var monsterSprite = Monster.GetComponent<SpriteRenderer>().sprite;
            var monsterCollider = Monster.GetComponent<BoxCollider2D>();
            int layerMask = 1 << LayerMask.NameToLayer("Player");

            var rayPosition =
                Monster.transform.position - new Vector3(Monster.GetComponent<SpriteRenderer>().size.x / 2 + 0.1f,
                    (monsterSprite.rect.height / monsterSprite.pixelsPerUnit / 2));
            var hit = Physics2D.Raycast
                (rayPosition, Vector2.left, monsterCollider.size.x / 2, layerMask);
            Debug.DrawRay(rayPosition, Vector3.left * 0.1f, Color.red);
            return hit;
        }

        public abstract void StartAction();
        public abstract void StopAction();
        public abstract void TerminateAction();
        public abstract MonsterStatus GetStatus();
    }
}