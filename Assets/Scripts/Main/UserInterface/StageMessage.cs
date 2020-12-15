using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class StageMessage : MonoBehaviour
    {
        [SerializeField] private Text messageText = null;
        private const float activeTime = 2.0f;
        private Coroutine currentCoroutine;
        
        public enum StageMessageType
        {
            EmptyEquippedSword
        }
        
        public void ShowMessage(StageMessageType messageType)
        {
            gameObject.SetActive(true);
            
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            
            currentCoroutine = StartCoroutine(ShowMessageAnimation(messageType));
        }

        private string GetMessage(StageMessageType messageType)
        {
            string message = null;
            switch (messageType)
            {
                case StageMessageType.EmptyEquippedSword:
                    message = "장착한 무기가 없어 시작할 수 없습니다. 인벤토리에서 무기를 장착해주세요.";
                    break;
            }

            return message;
        }

        private IEnumerator ShowMessageAnimation(StageMessageType messageType)
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            var objectImage = GetComponent<Image>();
            messageText.text = GetMessage(messageType);

            float time = 0;
            var transparentColor = new Color(1, 1, 1, 0);
            var objectColorStart = objectImage.color * transparentColor + Color.black * 0.7f;
            var objectColorEnd = objectImage.color * transparentColor;
            var textColorStart = messageText.color * transparentColor + Color.black * 0.85f;
            var textColorEnd = messageText.color * transparentColor;

            while (time < activeTime)
            {
                objectImage.color = Color.Lerp(objectColorStart, objectColorEnd, time / activeTime);
                messageText.color = Color.Lerp(textColorStart, textColorEnd, time / activeTime);
                
                time += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            
            gameObject.SetActive(false);
            currentCoroutine = null;
        }
    }
}