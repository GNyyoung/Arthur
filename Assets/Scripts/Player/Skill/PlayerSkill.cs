using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    // delegate로 EndSkill()만 가져오는 것도 괜찮을듯.
    // PlayerSkill에서 따로 빼올거 없으면 바꿔버리자.
    public abstract class PlayerSkill : MonoBehaviour
    {
        protected PlayerSkillCast SkillCastAction;
        protected Coroutine CooldownCoroutine;
        
        public Coroutine SkillCoroutine { get; protected set; }
        /// <summary>
        /// 스킬 활성화 후 경과한 시간
        /// </summary>
        public float ActiveProgress { get; protected set; }
        /// <summary>
        /// 스킬을 다시 사용하기까지 남은 시간
        /// </summary>
        public float CooldownRest { get; protected set; }
        public bool IsUsable { get; set; } = false;
        public float Cooldown { get; set; }
        public float PreDelay { get; set; }
        public float PostDelay { get; set; }

        /// <summary>
        /// 스킬을 발동시킨다.
        /// </summary>
        public virtual void Active(PlayerSkillCast skillCastAction)
        {
            this.SkillCastAction = skillCastAction;
            ActiveProgress = 0;
        }

        public virtual void Inactive()
        {
            if (SkillCoroutine != null)
            {
                StopCoroutine(SkillCoroutine);   
            }
        }
        
        public void StartCooldown()
        {
            if (CooldownCoroutine == null)
            {
                Debug.Log($"{gameObject.name}.{this.GetType().Name}.Cooldown");
                StartCoroutine(CooldownSkill());
            }
        }
        
        private IEnumerator CooldownSkill()
        {
            IsUsable = false;
            CooldownRest = Cooldown;
            var waitForFixedUpdate = new WaitForFixedUpdate();
            while (CooldownRest > 0)
            {
                CooldownRest -= Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }

            IsUsable = true;
            Debug.Log($"{gameObject.name}.{this.GetType().Name} 쿨다운 완료");
            CooldownCoroutine = null;
        }
    }
}