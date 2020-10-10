using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    // 나중에도 PlayerSkill과 다른 점이 없으면 두 클래스 합치기
    public abstract class MonsterSkill : MonoBehaviour
    {
        protected Monster Monster { get; private set; }
        
        protected Coroutine CooldownCoroutine;
        
        public MonsterSkillCast SkillCastAction { get; set; }
        public Coroutine SkillCoroutine { get; protected set; }
        public float ActiveProgress { get; protected set; }
        public float CooldownRest { get; protected set; }
        public AttackDirection SkillDirection { get; set; }
        public bool IsUsable { get; set; } = false;
        public float Cooldown { get; set; }
        public float PreDelay { get; set; }
        public float PostDelay { get; set; }
        public SkillActiveType ActiveType { get; set; }
        public ParticleSystem directionEffect { get; set; }
        
        public enum SkillActiveType
        {
            Prompt, ShortDist, LongDist
        }

        protected void Awake()
        {
            Monster = GetComponent<Monster>();
        }

        protected void Start()
        {
            directionEffect = SkillCastAction.effectObject;
        }

        public virtual void Active()
        {
            ActiveProgress = 0;
            if (IsUsable == true)
            {
                SkillCoroutine = StartCoroutine(Skill());
            }
        }

        public void Inactive()
        {
            if (SkillCoroutine != null)
            {
                StopSkill();
                StartCooldown();
            }
        }

        public void StopSkill()
        {
            if (SkillCoroutine != null)
            {
                StopCoroutine(SkillCoroutine);
                SkillCoroutine = null;
            }
        }

        public void StartCooldown()
        {
            if (CooldownCoroutine == null)
            {
                Debug.Log($"{gameObject.name}.{this.GetType().Name}.Cooldown");
                CooldownCoroutine = StartCoroutine(CooldownSkill());
            }
        }

        public void StopCooldown()
        {
            if (CooldownCoroutine != null)
            {
                StopCoroutine(CooldownCoroutine);
                CooldownCoroutine = null;
            }
        }

        private IEnumerator CooldownSkill()
        {
            IsUsable = false;
            CooldownRest = Cooldown;
            var waitForFixedUpdate = new WaitForFixedUpdate();
            
            while (CooldownRest > 0)
            {
                if (IsPauseCooldown() == false)
                {
                    CooldownRest -= Time.fixedDeltaTime;    
                }
                yield return waitForFixedUpdate;
            }

            IsUsable = true;
            CooldownCoroutine = null;
            Debug.Log($"{gameObject.name}.{this.GetType().Name} 쿨다운 완료");
            ProcessSkill();
        }

        private void ProcessSkill()
        {
            switch (ActiveType)
            {
                case SkillActiveType.Prompt:
                    SkillCastAction.Skill = this;
                    SkillCastAction.ActiveCurrentSkill();
                    break;
                case SkillActiveType.ShortDist:
                case SkillActiveType.LongDist:
                    SkillCastAction.AddStandbySkill(this);
                    break;
                default:
                    Debug.LogError(
                        $"적용되지 않은 몬스터 스킬 타입.\n({gameObject.GetInstanceID()}){gameObject.name}\n{ActiveType.ToString()}");
                    break;
            }
        }

        public void Terminate()
        {
            StopSkill();
            StopCooldown();
        }

        protected void EndSkill()
        {
            SkillCastAction.EndSkill();
            StartCooldown();
        }
        
        public void ShowDirectionEffect(AttackDirection direction)
        {
            Color effectColor = Color.white;
            switch (direction)
            {
                case AttackDirection.Slash:
                    effectColor = Color.green;
                    break;
                case AttackDirection.UpperSlash:
                    effectColor = Color.red;
                    break;
                case AttackDirection.Stab:
                    effectColor = Color.blue;
                    break;
            }

            var gradient = new ParticleSystem.MinMaxGradient(effectColor);
            var mainModule = directionEffect.main;
            mainModule.startColor = gradient;
            directionEffect.gameObject.transform.position = 
                Monster.transform.position + (Vector3.up * GetComponent<BoxCollider2D>().size.y / 2.0f);
            directionEffect.gameObject.SetActive(true);
            directionEffect.Play();
        }
        
        protected abstract IEnumerator Skill();
        protected abstract bool IsPauseCooldown();
    }
}