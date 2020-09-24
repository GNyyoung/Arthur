using System;
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
        private AttackDirection _defenceDirection;
        private PlayerSkill _currentSkill = null;
        private Dictionary<MonsterStatus, IMonsterAction> _actions = new Dictionary<MonsterStatus, IMonsterAction>();
        private float _maxHP;
        private float _currentHP;
        public Queue<MonsterSkill> SkillStandbyQueue { get; } = new Queue<MonsterSkill>();
        public IMonsterAction CurrentAction { get; set; }
        public MonsterSkill[] Skills { get; private set; }
        public ICombatant Player { get; private set; }
        
        

        #region Debug
        
        private MonsterStatus currentStatus;

        #endregion

        private void Awake()
        {
            var monsterActions = GetComponents<IMonsterAction>();
            foreach (var action in monsterActions)
            {
                _actions.Add(action.GetStatus(), action);
            }

            CurrentAction = _actions[MonsterStatus.Idle];
            currentStatus = CurrentAction.GetStatus();
        }
        
        public void InitializeStat(JsonMonster monsterData, ICombatant player)
        {
            this.Player = player;
            _maxHP = monsterData.HP;
            _currentHP = _maxHP;

            var skillClassType = Type.GetType($"DefaultNamespace.{monsterData.Skill}");
            Debug.Log($"{monsterData.Skill}, {skillClassType}");
            var newSkill = this.gameObject.AddComponent(skillClassType) as MonsterSkill;
            var skillData = Data.Instance.GetMonsterSkill(monsterData.Skill);
            if (skillData != null)
            {
                Debug.Log(skillData.Cooldown);
                newSkill.Cooldown = skillData.Cooldown;
                newSkill.PreDelay = skillData.PreDelay;
                newSkill.PostDelay = skillData.PostDelay;
            }
            else
            {
                Debug.LogWarning($"몬스터 스킬이 제대로 할당되지 않았습니다. : {skillData.SkillName}");
            }
            transform.position = new Vector3(Camera.main.transform.position.x + 9, 4);

            // 추후 별도 코드로 분리
            int defenceDirection = UnityEngine.Random.Range(1, 4);
            _defenceDirection = (AttackDirection) defenceDirection;
            transform.Find("Canvas").GetComponent<MonsterDebugUI>().DefenceDirectionText.text = _defenceDirection.ToString();
            // 코드 끝
        }

        private void Start()
        {
            Skills = GetComponents<MonsterSkill>();
            for (int i = 0; i < Skills.Length; i++)
            {
                Skills[i].SkillCastAction = _actions[MonsterStatus.Skill] as MonsterSkillCast;
                Skills[i].StartCooldown();
            }
            CurrentAction.StartAction();
        }

        private void FixedUpdate()
        {
            if (CheckSkillActiveCondition() == true)
            {
                (_actions[MonsterStatus.Skill] as MonsterSkillCast).Skill = SkillStandbyQueue.Dequeue(); 
                ChangeStatus(_actions[MonsterStatus.Skill]);
            }
        }

        // 방어하지 않은 부분을 공격하면 return true.
        public bool TakeDamage(ICombatant player, int damage, AttackDirection direction)
        {
            if (direction == _defenceDirection)
            {
                Debug.Log("방어");
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
                    int defDirection = UnityEngine.Random.Range(1, 4);
                    _defenceDirection = (AttackDirection) defDirection;
                    transform.Find("Canvas").GetComponent<MonsterDebugUI>().DefenceDirectionText.text = _defenceDirection.ToString();   
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
            CurrentAction = GetComponent<MonsterIdle>();
            #if UNITY_EDITOR
            currentStatus = CurrentAction.GetStatus();
            #endif
            CurrentAction.StartAction();
        }

        public bool CheckSkillActiveCondition()
        {
            // 추후 플레이어 있을 때만 스킬 사용하게 한다면 조건 추가 바람.
            if (CurrentAction.GetStatus() == MonsterStatus.Idle &&
                SkillStandbyQueue.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IMonsterAction GetAction(MonsterStatus status)
        {
            return _actions[status];
        }

        private void Die()
        {
            Destroy(this.gameObject);
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
    }
}