using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraMove : MonoBehaviour, IInstanceReceiver
    {
        private Player _player;

        private void Awake()
        {
            InstanceProvider.Add(this);
        }

        private void Start()
        {
            StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            
            yield return new WaitWhile(() => _player == null);

            while (true)
            {
                if (this.transform.position.x < _player.transform.position.x)
                {
                    this.transform.position += Vector3.right * (_player.transform.position.x - this.transform.position.x);   
                }

                yield return waitForFixedUpdate;
            }
        }

        public void SetInstance(object obj)
        {
            if (obj.GetType() == typeof(Player))
            {
                _player = obj as Player;
            }
        }
    }
}