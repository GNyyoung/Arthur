using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class GroundScroll : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _camera;

        private int scrollCount;
        private float groundSize;
        private int standardCameraPixel;
    
        // Start is called before the first frame update
        private void Start()
        {
            var ground = gameObject.transform.GetChild(0);
            standardCameraPixel = Mathf.CeilToInt(540 / _camera.GetComponent<Camera>().orthographicSize);
            groundSize = ground.localScale.x * 
                         ground.GetComponent<SpriteRenderer>().sprite.rect.width * 2 /
                         standardCameraPixel;
            Debug.Log(groundSize);
            StartCoroutine(ScrollGround());
        }

        private IEnumerator ScrollGround()
        {
            float nextPosX = transform.GetChild(0).position.x + groundSize;
            int groundNum = transform.childCount;
        
            while (true)
            {
                if (_camera.transform.position.x > nextPosX)
                {
                    transform.GetChild(scrollCount % groundNum).position += Vector3.right * (groundSize * (groundNum));
                    scrollCount += 1;
                    nextPosX += groundSize;
                }

                yield return null;
            }
        }
    }
}
