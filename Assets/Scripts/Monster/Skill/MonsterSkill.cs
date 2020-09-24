using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    // 나중에도 PlayerSkill과 다른 점이 없으면 두 클래스 합치기
    public class MonsterSkill : MonoBehaviour
    {
        public MonsterSkillCast SkillCastAction { get; set; }
        protected Coroutine CooldownCoroutine;
        
        public Coroutine SkillCoroutine { get; protected set; }
        public float ActiveProgress { get; protected set; }
        public float CooldownRest { get; protected set; }
        public AttackDirection SkillDirection { get; set; }
        public bool IsUsable { get; set; } = false;
        public float Cooldown { get; set; }
        public float PreDelay { get; set; }
        public float PostDelay { get; set; }

        public virtual void Active()
        {
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
            SkillCastAction.EnqueueSkill(this);
            CooldownCoroutine = null;
        }
    }
}