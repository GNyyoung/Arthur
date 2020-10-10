﻿﻿﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public enum MonsterStatus
    {
        Skill,
        Move,
        Idle
    }
    
    public class Monster : MonoBehaviour, ICombatant
    {
        private PlayerSkill _currentSkill = null;
        private readonly Dictionary<MonsterStatus, IMonsterAction> _actions = new Dictionary<MonsterStatus, IMonsterAction>();
        private float _maxHP;
        private float _currentHP;
        private GameObject _monsterSprite;
        
        public AttackDirection DefenceDirection { get; private set; }
        public MonsterSkill[] Skills { get; private set; }
        public ICombatant Player { get; private set; }
        public List<MonsterSkill> SkillStandbyList { get; private set; } = new List<MonsterSkill>();
        public IMonsterAction CurrentAction { get; set; }
        public bool IsPush { get; private set; }
        public string MonsterType { get; private set; }
        
        
        

        #region Debug
        
        private MonsterStatus currentStatus;

        #endregion

        private void Awake()
        {
            var monsterActions = GetComponents<IMonsterAction>();
            foreach (var action in monsterActions)
            {
                _actions.Add(action.GetStatus(), action);
                (action as MonsterAction).enabled = false;
            }
        }
        
        public void InitializeStat(JsonMonster monsterData, ICombatant player)
        {
            this.Player = player;
            _maxHP = monsterData.HP;
            _currentHP = _maxHP;
            IsPush = monsterData.IsPush;
            MonsterType = monsterData.Type;

            _monsterSprite = Resources.Load<GameObject>(monsterData.ImagePath);
            Instantiate(_monsterSprite, this.transform).name = _monsterSprite.name;

            GetComponent<BoxCollider2D>().enabled = true;
            
            // 몬스터 스킬 추가
            foreach (var skill in monsterData.Skills)
            {
                if (skill == null)
                {
                    break;
                }
                else
                {
                    var skillClassType = Type.GetType($"DefaultNamespace.{skill}");
                    var newSkill = this.gameObject.AddComponent(skillClassType) as MonsterSkill;
                    var skillData = Data.Instance.GetMonsterSkill(skill);
                    if (skillData != null)
                    {
                        Debug.Log(skillData.Cooldown);
                        newSkill.Cooldown = skillData.Cooldown;
                        newSkill.PreDelay = skillData.PreDelay;
                        newSkill.PostDelay = skillData.PostDelay;
                        newSkill.ActiveType =
                            (MonsterSkill.SkillActiveType) Enum.Parse(typeof(MonsterSkill.SkillActiveType), skillData.ActiveType);
                    }
                    else
                    {
                        Debug.LogWarning($"몬스터 스킬이 제대로 할당되지 않았습니다. : {skill}");
                    }
                }
            }
            transform.position = new Vector3(Camera.main.transform.position.x + 9, transform.position.y);

            ChangeDefenceDirection();
            
            // 스킬 쿨다운 시작
            Skills = GetComponents<MonsterSkill>();
            for (int i = 0; i < Skills.Length; i++)
            {
                Skills[i].SkillCastAction = _actions[MonsterStatus.Skill] as MonsterSkillCast;
                Skills[i].StartCooldown();
            }
            
            // 액션 시작
            CurrentAction = _actions[MonsterStatus.Move];
            currentStatus = CurrentAction.GetStatus();
            CurrentAction.StartAction();
        }

        private void FixedUpdate()
        {
            if (IsSkillActiveCondition() == true)
            {
                foreach (var standbySkill in SkillStandbyList)
                {
                    if ((standbySkill.ActiveType == MonsterSkill.SkillActiveType.ShortDist && 
                         MonsterApproach.Instance.IsFirstApproached(this.gameObject) == true) ||
                        standbySkill.ActiveType == MonsterSkill.SkillActiveType.LongDist)
                    {
                        (_actions[MonsterStatus.Skill] as MonsterSkillCast).Skill = standbySkill;
                        SkillStandbyList.Remove(standbySkill);
                        Debug.Log($"{gameObject.GetInstanceID()}");
                        ChangeStatus(_actions[MonsterStatus.Skill]);
                        break;
                    }
                }
            }
        }

        // 방어하지 않은 부분을 공격하면 return true.
        public bool TakeDamage(ICombatant player, int damage, AttackDirection direction)
        {
            if (direction == DefenceDirection)
            {
                Debug.Log($"{gameObject.name}({gameObject.GetInstanceID()}) 방어");
                _currentHP -= damage / 2.0f;
                return false;
            }
            else
            {
                Debug.Log($"몬스터 {gameObject.name}({gameObject.GetInstanceID()}) {damage}데미지 받음.");
                _currentHP -= damage;
                if (_currentHP <= 0)
                {
                    Die();
                }
                else
                {
                    ChangeDefenceDirection();   
                }
                return true;
            }
        }
        
        /// <summary>
        /// 매개변수로 받은 액션의 우선순위가 더 높은 경우 몬스터의 액션 전환
        /// </summary><param name="newAction">전환할 액션</param>
        public void ChangeStatus(IMonsterAction newAction)
        {
            if (CurrentAction.GetStatus() > newAction.GetStatus())
            {
                CurrentAction.StopAction();
                CurrentAction = newAction;
                #if UNITY_EDITOR
                currentStatus = CurrentAction.GetStatus();
                #endif
                CurrentAction.StartAction();
            }
        }
        
        /// <summary>
        /// 현재 액션을 종료하고 몬스터 액션을 대기 상태로 바꿈
        /// </summary>
        public void StopCurrentStatus()
        {
            CurrentAction.StopAction();
            if (CurrentAction.GetStatus() == MonsterStatus.Idle)
            {
                CurrentAction = _actions[MonsterStatus.Move];
            }
            else
            {
                CurrentAction = _actions[MonsterStatus.Idle];    
            }
            #if UNITY_EDITOR
            currentStatus = CurrentAction.GetStatus();
            #endif
            CurrentAction.StartAction();
        }

        public bool IsSkillActiveCondition()
        {
            // 추후 플레이어 있을 때만 스킬 사용하게 한다면 조건 추가 바람.
            if (CurrentAction.GetStatus() == MonsterStatus.Idle &&
                SkillStandbyList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Die()
        {
            foreach (var action in _actions)
            {
                (action.Value as MonsterAction)?.TerminateAction();
            }
            foreach (var skill in Skills)
            {
                skill.Terminate();
            }
            
            // 스킬 제거
            foreach (var skill in Skills)
            {
                Destroy(skill);
            }
            Skills = null;
            
            SkillStandbyList = new List<MonsterSkill>();

            GetComponent<BoxCollider2D>().enabled = false;
            MonsterApproach.Instance.UpdateApproachStatus(this.gameObject);
            Destroy(transform.Find(_monsterSprite.name).gameObject);
            gameObject.SetActive(false);
        }

        public bool IsCastSkill(AttackDirection direction)
        {
            if (CurrentAction.GetStatus() == MonsterStatus.Skill)
            {
                var action = CurrentAction as MonsterSkillCast;
                if (action.Skill.SkillDirection == direction &&
                    action.Skill.ActiveProgress < action.Skill.PreDelay)
                {
                    return true;
                }
            }

            return false;
        }

        public void ChangeDefenceDirection()
        {
            int directionNum = Enum.GetValues(typeof(AttackDirection)).Length;
            int defenceDirection = UnityEngine.Random.Range(1, directionNum);
            ChangeDefenceDirection((AttackDirection) defenceDirection);
        }

        public void ChangeDefenceDirection(AttackDirection direction)
        {
            DefenceDirection = direction;
            transform.Find("Canvas").GetComponent<MonsterDebugUI>().DefenceDirectionText.text = $"Def:{DefenceDirection.ToString()}";
            // 애니메이션 변경
        }
    }
}