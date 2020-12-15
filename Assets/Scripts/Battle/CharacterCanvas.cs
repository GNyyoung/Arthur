using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class CharacterCanvas : MonoBehaviour
    {
        public GameObject skillAlarmFolder;
        public GameObject effectFolder;
        public ObjectPool AlarmPool { get; private set; }
        public ObjectPool EffectPool { get; private set; }
        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
            
            var imageObject = new GameObject("Alarm");
            imageObject.AddComponent<Image>();
            AlarmPool = skillAlarmFolder.GetComponent<ObjectPool>(); 
            AlarmPool.InitializePool(imageObject, skillAlarmFolder, 5);

            imageObject.name = "Effect";
            EffectPool = effectFolder.GetComponent<ObjectPool>();
            EffectPool.InitializePool(imageObject, effectFolder, 4);
            
            Destroy(imageObject);
        }
        
        public void Initialize(GameObject characterObj, GameObject spriteObj)
        {
            var rectTransform = GetComponent<RectTransform>(); 
            var imageSize = spriteObj.GetComponent<CharacterModel>().SpriteSize;
            rectTransform.sizeDelta = new Vector2(
                (imageSize.x / rectTransform.localScale.x), 
                (imageSize.y / rectTransform.localScale.y));
            transform.position = characterObj.transform.position;
            
            var skillAlarmRectTransform = skillAlarmFolder.GetComponent<RectTransform>();
            skillAlarmRectTransform.anchoredPosition = new Vector2(
                skillAlarmRectTransform.anchoredPosition.x,
                skillAlarmRectTransform.sizeDelta.y / 2.0f);
            
            var effectRectTransform = effectFolder.GetComponent<RectTransform>();
            effectRectTransform.anchoredPosition = new Vector2(
                effectRectTransform.anchoredPosition.x,
                -(effectRectTransform.sizeDelta.y / 2.0f));
        }
    }
}