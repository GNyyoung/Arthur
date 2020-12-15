using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CharacterModel : MonoBehaviour
    {
        public GameObject toolObject;
        public Vector3 SpriteSize { get; private set; }

        private void Awake()
        {
            var bound = GetComponent<Renderer>().bounds;
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<Renderer>(out var childRenderer) == true)
                {
                    var childBound = childRenderer.bounds;
                    bound.Encapsulate(childBound);   
                }
            }

            SpriteSize = bound.size;
        }
    }
}