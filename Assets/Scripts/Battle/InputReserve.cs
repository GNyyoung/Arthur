using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public enum InputActionType
    {
        None,
        Draw,
        Skill,
        Slash,
        UpperSlash,
        Stab
    }
    
    public class InputReserve : MonoBehaviour
    {
        private Coroutine _currentCoroutine;

        public InputActionType InputActionType { get; private set; }

        public const float ReserveTime = 0.2f;

        public void ReserveKey(InputActionType type)
        {
            InputActionType = type;
            if (_currentCoroutine == null)
            {
                Debug.Log("키 저장");
                _currentCoroutine = StartCoroutine(WaitInputProcess());
            }
        }

        public void CancelReserve()
        {
            InputActionType = InputActionType.None;
            Debug.Log("입력취소");
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
        }
        
        private IEnumerator WaitInputProcess()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            var initialType = InputActionType;
            float time = 0;

            while (time < ReserveTime)
            {
                if (initialType.Equals(InputActionType) == false)
                {
                    initialType = InputActionType;
                    time = 0;
                }

                time += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }

            _currentCoroutine = null;
            InputActionType = InputActionType.None;
            Debug.Log($"예약 제거 : {InputActionType}");
        }
    }
}