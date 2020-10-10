﻿﻿﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
 using DefaultNamespace.Main;
 using UnityEngine;

namespace DefaultNamespace
{
    public enum PlayerStatus
    {
        None,
        DrawSkill,
        SkillActive,
        Attack,
        Defend,
        Move,
        Idle
    }
    
    public enum AttackDirection
    {
        None,
        Slash,
        UpperSlash,
        Stab
    }
    
    public class Player : MonoBehaviour, ICombatant
    {
        private LinkedList<Sword> _equippedSwords = new LinkedList<Sword>();
        private Dictionary<PlayerStatus, IPlayerAction> _actions = new Dictionary<PlayerStatus, IPlayerAction>();
        [SerializeField]
        private Animator animator;
        [SerializeField] 
        private SpriteRenderer swordRenderer;
        
        public IPlayerAction CurrentAction { get; private set; }
        public Sword CurrentSword { get; private set; }
        public static MoveSpeedController SpeedController { get; private set; } = new MoveSpeedController();
        public Animator Animator => animator;
        public InputReserve InputReserve { get; private set; }

        #region DEBUG

        // 디버그 외로 사용하는 코드들 나중에 정리 바람.
        public PlayerStatus CurrentStatus { get; set; }

        #endregion

        private void Awake()
        {
            var playerActions = GetComponents<IPlayerAction>();
            foreach (var action in playerActions)
            {
                _actions.Add(action.GetStatus(), action);
                (action as PlayerAction).enabled = false;
            }
            CurrentAction = _actions[PlayerStatus.Move];
            CurrentStatus = CurrentAction.GetStatus();
            
            // 나중에 GameManager로부터 데이터 받아 검 생성
            var swordPrefab = Resources.Load<GameObject>("Prefabs/Sword");
            foreach (var swordInfo in InformationReceiver.Instance.InformationDictionary["Sword"] as SwordInfo[])
            {
                var sword = Instantiate(swordPrefab, this.transform).GetComponent<Sword>();
                sword.Initialize(swordInfo);
                _equippedSwords.AddLast(sword);
            }

            InputReserve = GetComponent<InputReserve>();
        }

        public void Initialize()
        {
            InstanceProvider.SendInstance(this);
            InitializeAllActions();
            ChangeCurrentSword();
            
            // DrawSkill 쿨다운 시작
            LinkedListNode<Sword> currentSwordNode = _equippedSwords.Find(CurrentSword);
            (_actions[PlayerStatus.DrawSkill] as PlayerDrawSkillCast)?.CooldownReserveSword(currentSwordNode);
            // ActiveSkill 쿨다운 시작
            (_actions[PlayerStatus.SkillActive] as PlayerActiveSkillCast)?.CooldownAllSword(_equippedSwords);
        }

        /// <summary>
        /// 새 액션의 우선순위가 현재 액션보다 높은 경우 액션을 전환함
        /// </summary>
        /// <param name="newAction">전환할 액션</param>
        public void ChangeStatus(IPlayerAction newAction)
        {
            Debug.Log("현재 액션 변경");
            if (CurrentAction.GetStatus() >= newAction.GetStatus())
            {
                InputReserve.CancelReserve();
                CurrentAction.StopAction();
                CurrentAction = newAction;
                CurrentStatus = newAction.GetStatus();
                CurrentAction.StartAction();
            }
        }

        /// <summary>
        /// 현재 액션을 종료하고 대기 상태로 전환함.
        /// </summary>
        public void StopCurrentStatus()
        {
            Debug.Log("현재 액션 종료");
            CurrentAction.StopAction();
            if (InputReserve.InputActionType == InputActionType.None)
            {
                if (CurrentAction.GetStatus() == PlayerStatus.Idle)
                {
                    CurrentAction = _actions[PlayerStatus.Move];
                }
                else
                {
                    CurrentAction = _actions[PlayerStatus.Idle];
                }
                
                CurrentStatus = CurrentAction.GetStatus();
                CurrentAction.StartAction();
            }
            else
            {
                DoAction(InputReserve.InputActionType);
            }
        }

        /// <summary>
        /// 입력받은 방향으로 공격 또는 방어 행동을 취함
        /// </summary>
        /// <param name="direction">칼을 휘두를 방향</param>
        private void DoSwordAction(AttackDirection direction)
        {
            Debug.Log(CurrentSword.IsUsable);
            if (CurrentSword.IsUsable == true)
            {
                var playerDefend = _actions[PlayerStatus.Defend] as PlayerDefend;
                if (playerDefend == null)
                {
                    Debug.LogError("Player 오브젝트에 PlayerDefend 컴포넌트를 추가해야 합니다.");
                }
                // 공격범위 내의 몬스터 중 스킬을 사용하는 적이 있으면 방어함.
                else if (playerDefend.IsDefendCondition(direction) == true)
                {
                    Debug.Log("방어");
                    playerDefend.defendDirection = direction;
                    ChangeStatus(playerDefend);
                }
                else
                {
                    // 방어할 상황이 아니면 공격함.
                    var playerAttack = _actions[PlayerStatus.Attack] as PlayerAttack;
                    if (playerAttack == null)
                    {
                        Debug.LogError("Player 오브젝트에 PlayerAttack 컴포넌트를 추가해야 합니다.");
                    }
                    else
                    {
                        Debug.Log("공격");
                        playerAttack.AttackDirection = direction;
                        ChangeStatus(playerAttack);
                    }    
                }
            }
        }

        /// <summary>
        /// 현재 사용 중인 검의 스킬이 사용 가능한 상태이면 스킬을 사용함
        /// </summary>
        private void ActiveSwordSkill()
        {
            _actions.TryGetValue(PlayerStatus.SkillActive, out var playerSkill);
            var skill = (playerSkill as PlayerActiveSkillCast)?.Skill;
            Debug.Log(skill);
            Debug.Log(skill.IsUsable);
            if (skill != null && skill.IsUsable == true)
            {
                Debug.Log("스킬");
                ChangeStatus(_actions[PlayerStatus.SkillActive]);
            }
        }

        public void RemoveCurrentSword()
        {
            _equippedSwords.Remove(CurrentSword);
            if (_equippedSwords.Count == 0)
            {
                Debug.Log("게임오버");
                // 게임오버 코드 작성
            }
        }

        /// <summary>
        /// 사용할 무기를 다음 예비무기로 바꿈
        /// </summary>
        private void ChangeCurrentSword()
        {
            if (CurrentSword == null)
            {
                CurrentSword = _equippedSwords.First.Value;
                if (CurrentSword.ActiveSkill != null)
                {
                    var activeSkillCast = _actions[PlayerStatus.SkillActive] as PlayerActiveSkillCast;
                    if (activeSkillCast != null)
                    {
                        activeSkillCast.Skill = CurrentSword.ActiveSkill;
                        activeSkillCast.UpdateSkillCooldown();   
                    }
                }
                
                if (CurrentSword.DrawSkill != null)
                {
                    var drawSkillCast = _actions[PlayerStatus.DrawSkill] as PlayerDrawSkillCast;
                    if (drawSkillCast != null)
                    {
                        drawSkillCast.Skill = CurrentSword.DrawSkill;
                    }
                }
            }
            else
            {
                var nextSword = _equippedSwords.Find(CurrentSword)?.Next ?? _equippedSwords.First;;
                Debug.Log(nextSword.Value.name + ", " + nextSword.Value.DrawSkill.IsUsable);
            
                if (nextSword.Value.Equals(CurrentSword) == false && 
                    nextSword.Value.DrawSkill.IsUsable == true)
                {
                    Debug.Log($"무기변경 : {nextSword.Value.Name}");
                    CurrentSword = nextSword.Value;
                    if (CurrentSword.ActiveSkill != null)
                    {
                        var activeSkillCast = _actions[PlayerStatus.SkillActive] as PlayerActiveSkillCast;
                        if (activeSkillCast != null)
                        {
                            activeSkillCast.Skill = CurrentSword.ActiveSkill;
                            activeSkillCast.UpdateSkillCooldown();   
                        }
                    }
                
                    var drawSkillCast = _actions[PlayerStatus.DrawSkill] as PlayerDrawSkillCast;
                    if (CurrentSword.DrawSkill != null)
                    {
                        if (drawSkillCast != null)
                        {
                            drawSkillCast.Skill = CurrentSword.DrawSkill;
                            drawSkillCast.UpdateSkillCooldown(_equippedSwords.Find(CurrentSword));
                            ChangeStatus(drawSkillCast);
                        }
                    }
                    else
                    {
                        if (drawSkillCast != null)
                        {
                            drawSkillCast.Skill = null;
                            drawSkillCast.UpdateSkillCooldown(_equippedSwords.Find(CurrentSword));    
                        }
                        StopCurrentStatus();
                    }
                }
            }
            
            swordRenderer.sprite = CurrentSword.SwordImage;
        }

        public void InitializeAllActions()
        {
            foreach (var actionPair in _actions)
            {
                actionPair.Value.InitializeAction(this);
            }
        }

        /// <summary>
        /// 플레이어가 사용 중인 검에 데미지를 입힌다.
        /// </summary>
        public bool TakeDamage(ICombatant monster, int damage, AttackDirection direction)
        {
            var playerDefend = CurrentAction as PlayerDefend; 
            if (playerDefend != null &&
                playerDefend.defendDirection == direction)
            {
                playerDefend.SucceedDefence();
                return false;
            }
            else
            {
                CurrentSword.DamageByBadDefend(this, damage);
                return true;
            }
        }

        public void DoAction(InputActionType type)
        {
            InputReserve.ReserveKey(type);
            
            switch (type)
            {
                case InputActionType.Slash:
                    DoSwordAction(AttackDirection.Slash);
                    break;
                case InputActionType.UpperSlash:
                    DoSwordAction(AttackDirection.UpperSlash);
                    break;
                case InputActionType.Stab:
                    DoSwordAction(AttackDirection.Stab);
                    break;
                case InputActionType.Skill:
                    ActiveSwordSkill();
                    break;
                case InputActionType.Draw:
                    ChangeCurrentSword();
                    break;
                default:
                    Debug.LogWarning($"적용되지 않은 InputActionType : {type}");
                    break;
            }
        }

        public void Retreat()
        {
            var playerMove = _actions[PlayerStatus.Move] as PlayerMove;
            if (playerMove != null)
            {
                playerMove.StartRetreat();
            }
        }
    }
}
