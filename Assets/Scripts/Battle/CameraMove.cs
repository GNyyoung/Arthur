using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraMove : MonoBehaviour, IInstanceReceiver
    {
        private CameraMove(){}
        private static CameraMove _instance;

        public static CameraMove Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<CameraMove>();
                    if (instances.Length == 0)
                    {
                        var newInstance = GameObject.Find("Screen").AddComponent<CameraMove>();
                        _instance = newInstance;
                    }
                    else if (instances.Length >= 1)
                    {
                        for (int i = 1; i > instances.Length; i++)
                        {
                            Destroy(instances[i]);
                        }
                    
                        _instance = instances[0];
                    }
                }

                return _instance;
            }
        }
        
        private Player _player;
        [SerializeField]
        private Transform leftBoundaryTransform = null;
        [SerializeField]
        private Transform rightBoundaryTransform = null;
        [SerializeField]
        private Transform moveBoundaryTransform = null;

        private Coroutine moveCoroutine;

        public Transform LeftBoundaryTransform => leftBoundaryTransform;
        public Transform RightBoundaryTransform => rightBoundaryTransform;


        private void Awake()
        {
            InstanceProvider.Add(this);
        }

        private void Start()
        {
            // moveCoroutine = StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            yield return new WaitWhile(() => _player == null);
            bool isMove = true;
            Debug.Log("이동");
            while (true)
            {
                if (MonsterApproach.Instance.IsExistCollide() == true)
                {
                    isMove = false;
                }
                else if (_player.CurrentAction.GetStatus() == PlayerStatus.Move)
                {
                    isMove = true;
                }
                
                if (isMove == true)
                {
                    if (_player.transform.position.x > moveBoundaryTransform.position.x)
                    {
                        float intervalX = _player.transform.position.x - moveBoundaryTransform.position.x;
                        transform.position += Vector3.right * intervalX;
                    }
                    else
                    {
                        this.transform.position += Vector3.right * (Player.SpeedController.PlayerSpeed * Time.fixedDeltaTime);   
                    }
                }

                yield return waitForFixedUpdate;
            }
        }

        private IEnumerator Wait()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            Debug.Log("대기");
            while (true)
            {
                if (_player.transform.position.x > moveBoundaryTransform.position.x)
                {
                    float intervalX = _player.transform.position.x - moveBoundaryTransform.position.x;
                    transform.position += Vector3.right * intervalX;
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

        public void ActiveCameraMove(bool isActive)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);    
            }
            
            if (isActive)
            {
                moveCoroutine = StartCoroutine(Move());
            }
            else
            {
                moveCoroutine = StartCoroutine(Wait());
            }
        }
    }
}