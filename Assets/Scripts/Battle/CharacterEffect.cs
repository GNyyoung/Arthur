using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    [Flags] 
    public enum Effect
    {
        None = 0,
        Immortality = 1 << 1,
        Slow = 1 << 2,
        Stun = 1 << 3,
        DamageUp = 1 << 4,
        Counter = 1 << 5,
        Knockback = 1 << 6
    }
    
    /// <summary>
    /// 스킬 등으로 부여되는 상태효과를 관리한다.
    /// </summary>
    public class CharacterEffect : MonoBehaviour
    {
        private delegate IEnumerator EffectIEnumerator(EffectInfo effectInfo);
        public struct EffectInfo
        {
            public int ID;
            public Effect effect;
            public Coroutine effectCoroutine;
            public float duration;
            public float bonusRate;
            public GameObject effectIconObj;
            
            public EffectInfo(Effect effect, float duration, float bonusRate, GameObject effectIconObj, int id)
            {
                this.effect = effect;
                this.duration = duration;
                this.effectIconObj = effectIconObj;
                this.effectCoroutine = null;
                this.bonusRate = bonusRate;
                ID = id;
            }
        }

        [SerializeField] private CharacterCanvas characterCanvas = null;
        private string effectIconPath = "Sprites/UI/Battle/Effect";
        private int createNum = 0;
        
        public Effect CurrentEffect { get; private set; }
        public IEffectReceiver effectReceiver { get; set; }
        /// <summary>
        /// 적용 중인 모든 효과 코루틴을 저장하는 딕셔너리.
        /// </summary>
        public List<EffectInfo> EffectInfoList = new List<EffectInfo>();

        public float attackCooldownBonus { get; private set; } = 1;
        public float damageBonus { get; private set; } = 1;
        public float attackSpeedBonus { get; private set; } = 1;

        public void AddEffect(Effect effect, float duration, float bonusRate = 0)
        {
            Debug.Log($"효과 추가 : {effect}");
            CurrentEffect |= effect;
            Debug.Log(CurrentEffect);
            
            var effectIEnumerator = GetEffectCoroutine(effect);
            var effectObj = ShowEffectIcon(effect);
            EffectInfo effectInfo = new EffectInfo(effect, duration, bonusRate, effectObj, createNum++);
            var newEffectCoroutine = StartCoroutine(effectIEnumerator(effectInfo));
            effectInfo.effectCoroutine = newEffectCoroutine;
            EffectInfoList.Add(effectInfo);
        }

        public void RemoveEffect(Effect effect)
        {
            foreach (var effectInfo in EffectInfoList)
            {
                if (effectInfo.effect == effect)
                {
                    RemoveEffect(effectInfo);
                    return;
                }
            }
        }

        public void RemoveEffect(EffectInfo effectInfo)
        {
            Debug.Log($"효과 제거 : {effectInfo.effect}");
            foreach (var effect in EffectInfoList)
            {
                if (effect.ID == effectInfo.ID)
                {
                    EffectInfoList.Remove(effect);
                    break;
                }
            }
            Debug.Log($"이펙트 제거 후 남은 수 : {EffectInfoList.Count}");
            if (effectInfo.effectCoroutine != null)
            {
                StopCoroutine(effectInfo.effectCoroutine);   
            }
            VaryStatBonus(effectInfo.effect, -effectInfo.bonusRate);
            HideEffectIcon(effectInfo.effectIconObj);
            
            foreach (var effect in EffectInfoList)
            {
                if (effect.effect == effectInfo.effect)
                {
                    Debug.Log($"{effect.effect}와 동일한 이펙트 있음");
                    return;
                }
            }
            CurrentEffect ^= effectInfo.effect;
        }

        private EffectIEnumerator GetEffectCoroutine(Effect effect)
        {
            EffectIEnumerator effectIEnumerator;
            switch (effect)
            {
                case Effect.Immortality:
                    effectIEnumerator = ActiveEffect;
                    break;
                case Effect.Slow:
                case Effect.DamageUp:
                case Effect.Counter:
                    effectIEnumerator = ActiveStatVariationEffect;
                    break;
                case Effect.Stun:
                case Effect.Knockback:
                    effectIEnumerator = ActivePlayerCC;
                    break;
                default:
                    effectIEnumerator = null;
                    break;
            }

            return effectIEnumerator;
        }

        private IEnumerator ActiveEffect(EffectInfo effectInfo)
        {
            yield return new WaitForSeconds(effectInfo.duration);
            RemoveEffect(effectInfo);
        }

        IEnumerator ActiveStatVariationEffect(EffectInfo effectInfo)
        {
            VaryStatBonus(effectInfo.effect, effectInfo.bonusRate);
            yield return new WaitForSeconds(effectInfo.duration);

            RemoveEffect(effectInfo);
        }

        IEnumerator ActivePlayerCC(EffectInfo effectInfo)
        {
            effectReceiver.ApplyEffect(effectInfo.effect);
            
            yield return new WaitForSeconds(effectInfo.duration);
            
            effectReceiver.DisapplyEffect(effectInfo.effect);
            RemoveEffect(effectInfo);
        }

        // IEnumerator ActiveCounterEffect(EffectInfo effectInfo)
        // {
        //     var effectObj = ShowEffectIcon(effectInfo.effect);
        //     yield return new WaitForSeconds(effectInfo.duration);
        //     RemoveEffect(effectInfo);
        // }

        public GameObject ShowEffectIcon(Effect effect)
        {
            var effectObj = characterCanvas.EffectPool.GetObject(); 
            var effectSprite = Resources.Load<Sprite>(Path.Combine(effectIconPath, effect.ToString()));
            if (effectSprite == null)
            {
                effectObj = null;
            }
            else
            {
                effectObj.GetComponent<Image>().sprite = effectSprite;
                effectObj.SetActive(true);    
            }
            
            return effectObj;
        }

        public void HideEffectIcon(GameObject effectObj)
        {
            if (effectObj != null)
            {
                effectObj.SetActive(false);   
            }
        }

        private void VaryStatBonus(Effect effect, float rate)
        {
            switch (effect)
            {
                case Effect.Slow:
                    attackCooldownBonus += rate;
                    break;
                case Effect.DamageUp:
                    damageBonus += rate;
                    break;
                default:
                    break;
            }
        }
    }
}