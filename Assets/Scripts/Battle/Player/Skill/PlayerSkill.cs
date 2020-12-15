using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    // delegate로 EndSkill()만 가져오는 것도 괜찮을듯.
    // PlayerSkill에서 따로 빼올거 없으면 바꿔버리자.
    public abstract class PlayerSkill : MonoBehaviour
    {
        protected Player player;
        protected PlayerSkillCast skillCastAction;
        protected Coroutine cooldownCoroutine;
        protected AnimationClip skillAnim;
        
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
        public string Type { get; set; }
        public float AnimationTime { get; set; }
        public float Cooldown { get; set; }
        public float BaseBonus { get; set; }
        public float LevelBonus { get; set; }
        public float RangeBonus { get; set; }
        public float RangeLevelBonus { get; set; }
        public float Duration { get; set; }
        public float DurationLevelBonus { get; set; }
        public int DurabilityCost { get; set; }
        public int Level { get; set; } = 1;

        protected abstract void SetSkillAnimation();

        private void Start()
        {
            SetSkillAnimation();
        }

        public void Initialize(string skillName, Player player)
        {
            this.player = player;
            var skillJson = Data.Instance.GetSwordSkill(skillName);
            
            if (skillJson != null)
            {
                Type = skillJson.Type;
                Cooldown = skillJson.Cooldown;
                BaseBonus = skillJson.BaseBonus;
                LevelBonus = skillJson.LevelBonus;
                RangeBonus = skillJson.RangeBonus * 0.01f;
                RangeLevelBonus = skillJson.RangeLevelBonus * 0.01f;
                Duration = skillJson.Duration;
                DurationLevelBonus = skillJson.DurationLevelBonus;
                DurabilityCost = skillJson.DurabilityCost;
            }
            else
            {
                Debug.LogWarning($"몬스터 스킬이 제대로 할당되지 않았습니다. : {skillName}");
            }
        }
        
        /// <summary>
        /// 스킬을 발동시킨다.
        /// </summary>
        public virtual void Active(PlayerSkillCast skillCastAction)
        {
            this.skillCastAction = skillCastAction;
            ActiveProgress = 0;
            
            SkillCoroutine = StartCoroutine(Skill());
        }

        public virtual void Inactive()
        {
            if (SkillCoroutine != null)
            {
                StopCoroutine(SkillCoroutine);   
            }
            player.CurrentSword.DecreaseDurability(DurabilityCost);
        }
        
        public void StartCooldown()
        {
            if (cooldownCoroutine == null)
            {
                Debug.Log($"{gameObject.name}.{this.GetType().Name}.Cooldown");
                StartCoroutine(CooldownSkill());
            }
        }
        
        private IEnumerator CooldownSkill()
        {
            Debug.Log("쿨타임중");
            IsUsable = false;
            CooldownRest = Cooldown;
            var waitForFixedUpdate = new WaitForFixedUpdate();
            while (CooldownRest > 0)
            {
                if (player.CharacterEffect.CurrentEffect != Effect.Stun)
                {
                    CooldownRest -= Time.fixedDeltaTime;    
                }
                
                yield return waitForFixedUpdate;
            }

            IsUsable = true;
            Debug.Log($"{gameObject.name}.{this.GetType().Name} 쿨다운 완료");
            cooldownCoroutine = null;
        }
        
        /// <summary>
        /// 스킬 완료 후 호출
        /// </summary>
        protected void EndSkill()
        {
            skillCastAction.EndSkill();
        }

        public void ApplySkillAnimation()
        {
            var animatorOverrideController = player.Animator.runtimeAnimatorController as AnimatorOverrideController;
            animatorOverrideController[Type] = skillAnim;
        }

        public int GetTotalDamage()
        {
            // 곱, 합연산 혼용 코드
            // if (BaseBonus > 10)
            // {
            //     damage = (1 + ((BaseBonus + (LevelBonus * Level)) * 0.01f)) * Player.CurrentSword.Damage;
            // }
            // else
            // {
            //     damage = (BaseBonus + (LevelBonus * Level)) * Player.CurrentSword.Damage;
            // }
            
            return Mathf.FloorToInt((BaseBonus + (LevelBonus * (Level - 1))) * player.CurrentSword.Damage);
        }

        public float GetTotalRange()
        {
            return player.CurrentSword.Length + RangeBonus + (RangeLevelBonus * (Level - 1));
        }

        public float GetTotalDuration()
        {
            return Duration + (Duration * (Level - 1));
        }

        public void PushPosition(GameObject character, float distance)
        {
            var nextPosition = character.transform.position + Vector3.right * distance;

            if (Mathf.Abs(nextPosition.x) > Mathf.Abs(CameraMove.Instance.RightBoundaryTransform.position.x))
            {
                if (distance > 0)
                {
                    character.transform.position = new Vector3(
                        CameraMove.Instance.RightBoundaryTransform.position.x,
                        nextPosition.y, 
                        nextPosition.z);
                }
                else
                {
                    character.transform.position = new Vector3(
                        CameraMove.Instance.LeftBoundaryTransform.position.x,
                        nextPosition.y, 
                        nextPosition.z);
                }
            }
            else
            {
                character.transform.position = nextPosition;
            }
        }

        public abstract IEnumerator Skill();
    }
}