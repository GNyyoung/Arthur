﻿﻿﻿using System;
   using System.Collections;
   using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public enum MonsterStatus
    {
        None,
        Skill,
        Move,
        Idle
    }

    public enum MonsterType
    {
        Normal,
        Passage,
        Boss
    }
    
    public delegate AttackDirection ChangeDefendDirection(Monster monster);
    
    public class Monster : MonoBehaviour, IEffectReceiver
    {
        private PlayerSkill _currentSkill = null;
        private readonly Dictionary<MonsterStatus, IMonsterAction> _actions = new Dictionary<MonsterStatus, IMonsterAction>();
        private string monsterName;
        private GameObject monsterSprite;
        private PlayerReward playerReward;
        [SerializeField] 
        private CharacterEffect characterEffect;

        private ObjectShake objectShake;

        public float MaxHP { get; set; }
        public float CurrentHP { get; set; }
        public bool IsCheckCollision { get; set; }
        public ChangeDefendDirection ChangeDefendDirection { get; private set; }
        public AttackDirection DefenceDirection { get; set; }
        public MonsterSkill[] Skills { get; private set; }
        public Player Player { get; private set; }
        public List<MonsterSkill> SkillStandbyList { get; private set; } = new List<MonsterSkill>();
        public IMonsterAction CurrentAction { get; set; }
        public bool IsPush { get; private set; }
        public MonsterType MonsterType { get; private set; }
        public MonsterStat MonsterStat { get; } = new MonsterStat();
        public Animator Animator { get; private set; }
        public float DamageMultiple { get; private set; }
        public float HPMultiple { get; private set; }
        public float CooldownMultiple { get; private set; }
        public SpriteRenderer ToolRenderer { get; private set; }
        public float SwordDamageRate { get; private set; }
        public MonsterSound SoundSet { get; private set; }
        public CharacterEffect CharacterEffect
        {
            get => characterEffect;
            private set => characterEffect = value;
        }
        
        // 몬스터가 다른 오브젝트와 충돌한 상태인지.
        public bool isCollided = false;
        public CharacterCanvas characterCanvas;

        private void Awake()
        {
            var monsterActions = GetComponents<IMonsterAction>();
            SoundSet = GetComponent<MonsterSound>();
            foreach (var action in monsterActions)
            {
                _actions.Add(action.GetStatus(), action);
                (action as MonsterAction).enabled = false;
            }

            CharacterEffect.effectReceiver = this;
        }
        
        private void FixedUpdate()
        {
            if (IsSkillActiveCondition() == true)
            {
                foreach (var standbySkill in SkillStandbyList)
                {
                    if (IsAbleSkillUsage(standbySkill) == true)
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
        
        /// <summary>
        /// 몬스터의 모든 정보를 초기화함.
        /// </summary>
        /// <param name="monsterJsonData">초기화에 사용할 데이터</param>
        /// <param name="player"></param>
        public void Initialize(JsonMonster monsterJsonData, Player player, PlayerReward playerReward, JsonStageInfo stageInfo)
        {
            SetStat(monsterJsonData, player, stageInfo);
            this.playerReward = playerReward;

            SetImage(monsterJsonData);
            GetComponent<BoxCollider2D>().enabled = true;
            SetSkill(monsterJsonData);
            transform.position = new Vector3(Camera.main.transform.position.x + 7, transform.position.y);
            InitializeDefence(monsterJsonData);
            characterCanvas.Initialize(this.gameObject, monsterSprite);
            
            // 스킬 쿨다운 시작
            Skills = GetComponents<MonsterSkill>();
            for (int i = 0; i < Skills.Length; i++)
            {
                Skills[i].SkillCastAction = _actions[MonsterStatus.Skill] as MonsterSkillCast;
                Skills[i].StartCooldown();
            }
            
            // 액션 시작
            CurrentAction = _actions[MonsterStatus.Move];
            CurrentAction.StartAction();
        }

        public void SetStat(JsonMonster monsterJsonData, Player player, JsonStageInfo stageInfo)
        {
            DamageMultiple = stageInfo.DamageMultiple;
            HPMultiple = stageInfo.HPMultiple;
            CooldownMultiple = stageInfo.CooldownMultiple;
            
            monsterName = monsterJsonData.Name;
            this.Player = player;
            MaxHP = monsterJsonData.HP * HPMultiple;
            CurrentHP = MaxHP;
            IsPush = monsterJsonData.IsPush;
            SwordDamageRate = monsterJsonData.SwordDamageRate;
            MonsterType = (MonsterType)Enum.Parse(typeof(MonsterType), monsterJsonData.Type);
            if (MonsterType == MonsterType.Passage)
            {
                IsCheckCollision = false;
            }
            else
            {
                IsCheckCollision = true;
            }
            
            MonsterStat.Name = monsterName;
            MonsterStat.MaxHP = MaxHP;
            MonsterStat.CurrentHP = CurrentHP;
        }

        // 방어하지 않은 부분을 공격하면 return true.
        public bool TakeDamage(Player player, int damage, AttackDirection direction)
        {
            MonsterStat.DamagedDirection = direction;
            Debug.Log(DefenceDirection);
            if (direction == DefenceDirection)
            {
                Debug.Log($"{gameObject.name}({gameObject.GetInstanceID()}) 방어");
                CurrentHP -= damage / 2.0f;
                if (CurrentHP <= 0)
                {
                    SoundSet.OutputSound(MonsterSound.SoundType.Die);
                    StartCoroutine(Die());
                }
                else
                {
                    SoundSet.OutputSound(MonsterSound.SoundType.Defence);
                }
                return false;
            }
            else
            {
                Debug.Log($"몬스터 {gameObject.name}({gameObject.GetInstanceID()}) {damage}데미지 받음.");
                CurrentHP -= damage;
                if (CurrentHP <= 0)
                {
                    SoundSet.OutputSound(MonsterSound.SoundType.Die);
                    // 지나가는 몬스터일 경우 플레이어 검 내구도 회복
                    if (MonsterType == MonsterType.Passage)
                    {
                        float DurabilityRecover = Player.CurrentSword.AttackCost + Player.CurrentSword.MaxDurability * 0.01f;
                        Player.CurrentSword.IncreaseDurability(DurabilityRecover);
                    }
                    
                    StartCoroutine(Die());
                }
                else
                {
                    SoundSet.OutputSound(MonsterSound.SoundType.Damage);
                    if (CurrentAction.GetStatus() != MonsterStatus.Skill &&
                        CharacterEffect.CurrentEffect != Effect.Stun)
                    {
                        Debug.Log($"방어방향 변경 {CurrentAction.GetStatus()}");
                        DefenceDirection = ChangeDefendDirection(this);   
                    }
                    objectShake.ShakeOnDamage(direction);
                }
                return true;
            }
        }

        /// <summary>
        /// 몬스터 이미지 및 도구 이미지를 설정.
        /// </summary>
        private void SetImage(JsonMonster monsterJsonData)
        {
            // 몬스터 이미지 및 도구 이미지 설정
            var spritePrefab = Resources.Load<GameObject>(monsterJsonData.ImagePath);
            monsterSprite = Instantiate(spritePrefab, this.transform);
            monsterSprite.name = spritePrefab.name;
            MonsterStat.ImageName = spritePrefab.name;
            
            Animator = monsterSprite.GetComponent<Animator>();
            var toolSprite = Resources.Load<Sprite>(monsterJsonData.ToolPath);
            var monsterModel = monsterSprite.GetComponent<CharacterModel>();
            var monsterToolObject = monsterModel.toolObject; 
            ToolRenderer = monsterToolObject.GetComponent<SpriteRenderer>(); 
            if (toolSprite == null)
            {
                monsterToolObject.SetActive(false);
            }
            else
            {
                monsterToolObject.SetActive(true);
                ToolRenderer.sprite = toolSprite;
            }

            objectShake = monsterSprite.AddComponent<ObjectShake>();
            objectShake.shakeAngle = 45;
            objectShake.shakeDegree = 0.25f;
            objectShake.shakeElastic = 0.6f;
            objectShake.ShakeTime = 0.1f;
            objectShake.shakeDirection = 1;
        }

        /// <summary>
        /// 몬스터에게 스킬 컴포넌트를 추가한다.
        /// </summary>
        private void SetSkill(JsonMonster monsterJsonData)
        {
            foreach (var skillName in monsterJsonData.Skills)
            {
                if (skillName == null)
                {
                    break;
                }
                else
                {
                    var skillClassType = Type.GetType($"DefaultNamespace.{skillName}");
                    var newSkill = this.gameObject.AddComponent(skillClassType) as MonsterSkill;
                    if (newSkill != null)
                    {
                        newSkill.Initialize(skillName);    
                    }
                    else
                    {
                        Debug.LogWarning(
                            $"{gameObject.GetInstanceID()}에게 {skillName}이 추가되지 않았습니다.\nskillClassType : {skillClassType}\nnewSkill : {newSkill}");
                    }
                }
            }
        }

        private void InitializeDefence(JsonMonster monsterJsonData)
        {
            ChangeDefendDirection =
                DefenceVariety.SetDefenceVariety(monsterJsonData.DefenceType) as ChangeDefendDirection;
            DefenceDirection = DefenceVariety.RandomDirection(this);
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
                MonsterStat.CurrentStatus = CurrentAction.GetStatus();
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

            MonsterStat.CurrentStatus = CurrentAction.GetStatus();
            CurrentAction.StartAction();
        }

        public bool IsSkillActiveCondition()
        {
            // 추후 플레이어 있을 때만 스킬 사용하게 한다면 조건 추가 바람.
            if ((CurrentAction.GetStatus() == MonsterStatus.Idle || CurrentAction.GetStatus() == MonsterStatus.Move) &&
                SkillStandbyList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 몬스터 체력이 0 이하일 때 호출.
        /// </summary>
        public IEnumerator Die()
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
            
            playerReward.RaiseReward(new Reward(5, (string)null));

            // MonsterApproach.Instance.RemoveApproach(this.gameObject);
            GetComponent<BoxCollider2D>().enabled = false;
            
            Animator.SetTrigger("Die");
            yield return new WaitForSeconds(2);
            Destroy(monsterSprite);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 몬스터가 매개변수로 받은 방향으로 스킬을 쓰려 하는지를 반환함.
        /// </summary>
        /// <param name="direction">스킬 발동을 확인할 방향</param>
        /// <returns></returns>
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


        public void ApplyEffect(Effect effect)
        {
            switch (effect)
            {
                case Effect.Stun:
                    ChangeStatus(_actions[MonsterStatus.None]);
                    DefenceVariety.UpdateDefenceInformation(this, AttackDirection.None);
                    Animator.speed = 0;
                    break;
            }
        }

        public void DisapplyEffect(Effect effect)
        {
            switch (effect)
            {
                case Effect.Stun:
                    StopCurrentStatus();
                    ChangeDefendDirection(this);
                    Animator.speed = 1;
                    break;
            }
        }

        public bool IsAbleSkillUsage(MonsterSkill skill)
        {
            var isAble = false;
            switch (skill.ActiveType)
            {
                case MonsterSkill.SkillActiveType.Prompt:
                    isAble = true;
                    break;
                case MonsterSkill.SkillActiveType.LongDist:
                    if (isCollided == true)
                        isAble = true;
                    break;
                case MonsterSkill.SkillActiveType.ShortDist:
                    if (MonsterApproach.Instance.IsFirstApproached(this.gameObject) == true)
                        isAble = true;
                    break;
                case MonsterSkill.SkillActiveType.NotCollide:
                    if (isCollided == false)
                        isAble = true;
                    break;
            }

            return isAble;
            // if ((skill.ActiveType == MonsterSkill.SkillActiveType.ShortDist && 
            //      MonsterApproach.Instance.IsFirstApproached(this.gameObject) == true))
            // {
            //     return true;
            // }
            // else if(skill.ActiveType == MonsterSkill.SkillActiveType.LongDist)
            // {
            //     return true;
            // }
            // else if (skill.ActiveType == MonsterSkill.SkillActiveType.NotCollide)
            // {
            //     if (isCollided == false)
            //     {
            //         return true;
            //     }
            //     Debug.Log($"조건 확인 : {isCollided}");
            // }
            // else if (skill.ActiveType == MonsterSkill.SkillActiveType.Prompt)
            // {
            //     return true;
            // }
            //
            // return false;
        }
    }

    public class MonsterStat
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public float MaxHP { get; set; }
        public float CurrentHP { get; set; }
        public MonsterStatus CurrentStatus { get; set; }
        public AttackDirection CurrentSkillDirection { get; set; }
        public AttackDirection CurrentDefenceDirection { get; set; }
        public bool IsPush { get; set; }
        public bool IsDefend { get; set; }
        public bool IsCollide { get; set; }
        public AttackDirection DamagedDirection { get; set; } = AttackDirection.None;
        public string CurrentSkillName { get; set; }
    }
}