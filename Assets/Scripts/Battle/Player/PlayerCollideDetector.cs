using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerCollideDetector : MonoBehaviour
    {
        private Player player;

        private void Awake()
        {
            player = transform.parent.GetComponent<Player>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name.Equals("PlayerDeadLine"))
            {
                StartCoroutine(player.GameOver());
            }
        }
    }
}