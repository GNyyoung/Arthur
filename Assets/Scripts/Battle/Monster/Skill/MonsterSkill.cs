using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    // 나중에도 PlayerSkill과 다른 점이 없으면 두 클래스 합치기
    public abstract class MonsterSkill : MonoBehaviour
    {
        private int _cooldownFinishCount = 0;
        private AttackDirection skillDirection = AttackDirection.None;
        
        protected Monster Monster { get; private set; }
        protected Coroutine cooldownCoroutine;
        protected AnimationClip skillAnim = null;
        protected AnimationClip skillProgressAnim = null;
        protected AnimationClip skillFinishAnim = null;
        protected Coroutine attackCoroutine;
        // protected Coroutine postDelayCoroutine;
        
        public MonsterSkillCast SkillCastAction { get; set; }
        public Coroutine SkillCoroutine { get; protected set; }
        public float ActiveProgress { get; protected set; }
        public float CooldownRest { get; protected set; }
        public static readonly Dictionary<AttackDirection, Sprite> AlarmImageSet = new Dictionary<AttackDirection, Sprite>();
        protected Queue<GameObject> alarmObjectQueue = new Queue<GameObject>();

        public AttackDirection SkillDirection
        {
            get => skillDirection;
            protected set
            {
                skillDirection = value;
                Monster.MonsterStat.CurrentSkillDirection = value;
            }
        }
        public bool IsUsable { get; set; } = false;
        public float FirstCooldown { get; set; }
        public float Cooldown { get; set; }
        public float PreDelay { get; set; }
        public float PostDelay { get; set; }
        public SkillActiveType ActiveType { get; set; }
        public ParticleSystem directionEffect { get; set; }
        
        public enum SkillActiveType
        {
            Prompt, ShortDist, LongDist, NotCollide
        }

        protected struct SkillAttackInfo
        {
            public SkillAttackInfo(AttackDirection direction, float delay)
            {
                this.direction = direction;
                this.delay = delay;
            }
            
            public AttackDirection direction;
            public float delay;
        }

        protected virtual void Awake()
        {
            Monster = GetComponent<Monster>();
            if (AlarmImageSet.Count == 0)
            {
                var alarmImagePath = "Sprites/UI/Battle";
                AlarmImageSet.Add(AttackDirection.Slash, Resources.Load<Sprite>(Path.Combine(alarmImagePath, "AlarmSlash")));
                AlarmImageSet.Add(AttackDirection.UpperSlash, Resources.Load<Sprite>(Path.Combine(alarmImagePath, "AlarmUpperSlash")));
                AlarmImageSet.Add(AttackDirection.Stab, Resources.Load<Sprite>(Path.Combine(alarmImagePath, "AlarmStab")));
                AlarmImageSet.Add(AttackDirection.None, Resources.Load<Sprite>(Path.Combine(alarmImagePath, "AlarmOff")));
            }
            SetSkillAnimation();
        }

        public void Initialize(string skillName)
        {
            var skillJson = Data.Instance.GetMonsterSkill(skillName);
            if (skillJson != null)
            {
                FirstCooldown = skillJson.FirstCooldown;
                Cooldown = skillJson.Cooldown;
                PreDelay = skillJson.PreDelay;
                PostDelay = skillJson.PostDelay;
                ActiveType =
                    (MonsterSkill.SkillActiveType) Enum.Parse(typeof(MonsterSkill.SkillActiveType), skillJson.ActiveType);
            }
            else
            {
                Debug.LogWarning($"몬스터 스킬이 제대로 할당되지 않았습니다. : {skillName}");
                FirstCooldown = int.MaxValue;
            }
        }

        public virtual void Active()
        {
            ActiveProgress = 0;
            if (IsUsable == true)
            {
                Monster.ToolRenderer.color = DefenceVariety.defenceColorSet[AttackDirection.None];
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
                Debug.Log($"스킬 종료 {SkillCoroutine}");
                StopCoroutine(SkillCoroutine);
                RemoveAllAlarm();
                SkillCoroutine = null;
            }

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
        }

        public void StartCooldown()
        {
            if (cooldownCoroutine == null)
            {
                Debug.Log($"{gameObject.name}.{this.GetType().Name}.Cooldown");
                cooldownCoroutine = StartCoroutine(CooldownSkill());
            }
        }

        public void StopCooldown()
        {
            if (cooldownCoroutine != null)
            {
                StopCoroutine(cooldownCoroutine);
                cooldownCoroutine = null;
            }
        }

        private IEnumerator CooldownSkill()
        {
            IsUsable = false;
            if (_cooldownFinishCount == 0)
            {
                CooldownRest = FirstCooldown;
            }
            else
            {
                CooldownRest = Cooldown;    
            }
            
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
            cooldownCoroutine = null;
            _cooldownFinishCount += 1;
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
                case SkillActiveType.NotCollide:
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

        /// <summary>
        /// 스킬 종료 시 호출
        /// </summary>
        protected void EndSkill()
        {
            RemoveAllAlarm();
            SkillDirection = AttackDirection.None;
            SkillCastAction.EndSkill();
            var nextDefenceDirection = Monster.ChangeDefendDirection(Monster);
            Monster.ToolRenderer.color = DefenceVariety.defenceColorSet[nextDefenceDirection];
            StartCooldown();
        }
        
        // public void ShowDirectionEffect(AttackDirection direction)
        // {
        //     Color effectColor = Color.white;
        //     switch (direction)
        //     {
        //         case AttackDirection.Slash:
        //             effectColor = Color.green;
        //             break;
        //         case AttackDirection.UpperSlash:
        //             effectColor = Color.red;
        //             break;
        //         case AttackDirection.Stab:
        //             effectColor = Color.blue;
        //             break;
        //     }
        //
        //     var gradient = new ParticleSystem.MinMaxGradient(effectColor);
        //     var mainModule = directionEffect.main;
        //     mainModule.startColor = gradient;
        //     // 몬스터 프리팹 크기 구하는 법을 모르겠어서 일단 주석처리
        //     //directionEffect.gameObject.transform.position = 
        //     //    transform.position + (Vector3.up * transform.lossyScale.y / 2.0f);
        //     directionEffect.gameObject.SetActive(true);
        //     directionEffect.Play();
        // }
        
        public void PlaySkillAnimation()
        {
            var animatorOverrideController = Monster.Animator.runtimeAnimatorController as AnimatorOverrideController;
            Debug.Log(skillAnim);
            animatorOverrideController["Skill"] = skillAnim;
            if (skillProgressAnim != null)
            {
                animatorOverrideController["SkillProgress"] = skillProgressAnim;
                Monster.Animator.SetTrigger("SkillSeperate");
            }

            if (skillFinishAnim != null)
            {
                animatorOverrideController["SkillFinish"] = skillFinishAnim;
            }
            Monster.Animator.SetTrigger("Skill");
        }

        /// <summary>
        /// 플레이어에게 지정된 방향으로 데미지를 준 후 모든 공격 알림을 제거합니다.
        /// </summary>
        /// <param name="skillDirectionList">공격할 방향들과 횟수</param>
        /// <param name="damage">플레이어에게 입힐 데미지</param>
        /// <returns></returns>
        protected IEnumerator AttackPlayer(IEnumerable<AttackDirection> skillDirectionList, int damage)
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();

            foreach (var skillDirection in skillDirectionList)
            {
                SkillDirection = skillDirection;
                
                Monster.ToolRenderer.color = DefenceVariety.defenceColorSet[skillDirection];
                ActiveProgress = 0;
                while (ActiveProgress < PreDelay)
                {
                    ActiveProgress += Time.fixedDeltaTime;
                    yield return waitForFixedUpdate;
                }
                
                SkillCastAction.AttackPlayer(damage, SkillDirection);
                Monster.ToolRenderer.color = DefenceVariety.defenceColorSet[AttackDirection.None];
                TurnOffFrontAlarm();
            }
        }
        
        protected IEnumerator AttackPlayer(IEnumerable<SkillAttackInfo> attackInfoList, int damage)
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();

            foreach (var attackInfo in attackInfoList)
            {
                SkillDirection = attackInfo.direction;
                
                Monster.ToolRenderer.color = DefenceVariety.defenceColorSet[attackInfo.direction];
                ActiveProgress = 0;
                while (ActiveProgress < attackInfo.delay)
                {
                    ActiveProgress += Time.fixedDeltaTime;
                    yield return waitForFixedUpdate;
                }
                
                SkillCastAction.AttackPlayer(damage, SkillDirection);
                Monster.ToolRenderer.color = DefenceVariety.defenceColorSet[AttackDirection.None];
                TurnOffFrontAlarm();
            }
        }

        /// <summary>
        /// 몬스터 위에 공격방향을 알려주는 인터페이스를 띄웁니다.
        /// </summary>
        /// <param name="directions"></param>
        /// <returns></returns>
        protected IEnumerator ShowSkillDirectionAlarm(IEnumerable<AttackDirection> directions)
        {
            const float alarmInterval = 0.15f;
            var waitForAlarmInterval = new WaitForSeconds(alarmInterval);
            foreach (var direction in directions)
            {
                var alarmObject = Monster.characterCanvas.AlarmPool.GetFrontObject();
                alarmObject.GetComponent<Image>().sprite = AlarmImageSet[direction];
                alarmObjectQueue.Enqueue(alarmObject);
                alarmObject.SetActive(true);
                Monster.SoundSet.OutputSound(MonsterSound.SoundType.SkillAlarm);
                yield return waitForAlarmInterval;
            }

            yield return waitForAlarmInterval;
            Debug.Log("알림 종료");
        }

        protected IEnumerator WaitForPostDelay()
        {
            float delay = ActiveProgress + PostDelay;
            while (ActiveProgress < delay)
            {
                ActiveProgress += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        private void TurnOffFrontAlarm()
        {
            if (alarmObjectQueue.Count > 0)
            {
                var removedObject = alarmObjectQueue.Dequeue();
                removedObject.GetComponent<Image>().sprite = AlarmImageSet[AttackDirection.None];
                alarmObjectQueue.Enqueue(removedObject);
            }
            else
            {
                Debug.LogWarning("스킬 알람이 존재하지 않으나 제거 명령이 호출됐습니다.");
            }
        }

        protected void RemoveAllAlarm()
        {
            foreach (var alarmObject in alarmObjectQueue)
            {
                alarmObject.SetActive(false);
            }
            
            alarmObjectQueue.Clear();
        }

        /// <summary>
        /// 공격 명중 시 효과를 발동합니다.
        /// </summary>
        public virtual void UseHitEffect()
        {
            return;
        }
        
        // 방향 알림 -> 애니메이션 -> Predelay 대기 -> 효과 발동 -> PostDelay 대기 -> 스킬 종료
        protected abstract IEnumerator Skill();
        protected abstract bool IsPauseCooldown();
        protected abstract void SetSkillAnimation();
    }
}