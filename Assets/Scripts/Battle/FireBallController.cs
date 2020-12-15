using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class FireBallController : MonoBehaviour
    {
        private Player player;
        private float range;
        private float damage;
        private float fireBallSpeed = 16;

        private void Start()
        {
            StartCoroutine(Move());
        }

        public void Initialize(Player player, PlayerSkill skill)
        {
            this.player = player;
            range = skill.GetTotalRange();
            damage = player.CurrentSword.GetFinalDamage() + skill.GetTotalDamage();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("파이어볼 충돌");
            if (other.transform.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                other.GetComponent<Monster>().TakeDamage(player, Mathf.FloorToInt(damage), AttackDirection.None);
                Destroy(this.gameObject);
            }
        }

        private IEnumerator Move()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            float movedDist = 0;
            var moveDistPerFrame = Vector3.right * fireBallSpeed * Time.fixedDeltaTime;
            
            while (movedDist < range)
            {
                transform.position += moveDistPerFrame;
                movedDist += moveDistPerFrame.x;
                yield return waitForFixedUpdate;
            }
            
            Destroy(this.gameObject);
        }
    }
}