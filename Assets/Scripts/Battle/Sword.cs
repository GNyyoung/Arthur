using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Main;
using UnityEngine;

// 플레이어 오브젝트 말고 검 오브젝트에 붙여서 사용한다.
public class Sword : MonoBehaviour
{
    private ICooldownObserver cooldownDisplay;
    private PlayerSkill _activeSkill;
    private PlayerSkill _drawSkill;
    private CharacterEffect characterEffect;
    private Player player;
    
    public string Name { get; private set; }
    public float Damage { get; private set; }
    public float Length { get; private set; }
    public int MaxDurability { get; set; }
    public float Durability { get; set; }
    /// <summary>
    /// 몬스터를 공격했을 때의 검 내구도 감소량
    /// </summary>
    public int AttackCost { get; private set; }
    /// <summary>
    /// 몬스터에게 피격됐을 때의 검 내구도 감소량
    /// </summary>
    public int HitCost { get; private set; }
    /// <summary>
    /// 재공격에 걸리는 시간
    /// </summary>
    public float AttackCooldown { get; private set; }
    /// <summary>
    /// 공격 애니메이션 중 데미지가 들어가는 시간
    /// </summary>
    public float DamageTime { get; private set; }
    public float CooldownRest { get; private set; }

    public PlayerSkill ActiveSkill
    {
        get => _activeSkill;
        private set => _activeSkill = value;
    }

    public PlayerSkill DrawSkill
    {
        get => _drawSkill;
        private set => _drawSkill = value;
    }
    public bool IsUsable { get; private set; }
    public Sprite SwordImage { get; private set; }

    private void Awake()
    {
        foreach (var attackButton in BattleUI.Instance.attackButtons)
        {
            Debug.Log(attackButton.TryGetComponent<ICooldownObserver>(out var observe));
            if (attackButton.TryGetComponent<ICooldownObserver>(out var observer) == true)
            {
                cooldownDisplay = observer;
                break;
            }
        }
    }

    public void Initialize(SwordInfo swordInfo, Player player)
    {
        this.player = player;
        
        Name = swordInfo.Name;
        Damage = swordInfo.Damage;
        Length = swordInfo.Length * 0.01f;
        MaxDurability = swordInfo.Durability;
        Durability = MaxDurability;
        AttackCost = 1;
        HitCost = 1;
        DamageTime = swordInfo.DamageTime;
        AttackCooldown = swordInfo.AttackCooldown;
        
        AddSkillComponent(ref _activeSkill, swordInfo.ActiveSkill);
        AddSkillComponent(ref _drawSkill, swordInfo.DrawSkill);
        characterEffect = player.CharacterEffect;
        
        IsUsable = true;
        SwordImage = Resources.Load<Sprite>($"Sprites/Sword/{swordInfo.ImageName}");
    }

    private IEnumerator Cooldown()
    {
        var waitForFixedUpdate = new WaitForFixedUpdate();

        IsUsable = false;
        CooldownRest = GetFinalAttackCooldown();
        while (CooldownRest > 0)
        {
            if (player.CharacterEffect.CurrentEffect != Effect.Stun)
            {
                CooldownRest -= Time.fixedDeltaTime;   
            }
            yield return waitForFixedUpdate;
        }
        
        IsUsable = true;
    }

    // 공격 즉시 쿨타임이 돌고, Predelay가 끝나면 Attack을 실행한다.
    public void StartCooldown()
    {
        StartCoroutine(Cooldown());
        cooldownDisplay.DisplayCooldown(this);
    }

    /// <summary>
    /// 적 방어 방향과 다르게 공격 시 내구도 소모.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="damageRate"></param>
    public void DamageByAttack(Player player, float damageRate)
    {
        DecreaseDurability(Mathf.CeilToInt(AttackCost * damageRate));
    }

    /// <summary>
    /// 몬스터로부터 데미지를 받음.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="damage"></param>
    public void DamageByBadDefend(float damage)
    {
        Debug.Log($"플레이어 {damage}데미지 입음");
        DecreaseDurability(Mathf.FloorToInt(damage));
    }

    private void CheckDurability()
    {
        if (Durability <= 0)
        {
            player.RemoveCurrentSword();
        }
    }

    public void DecreaseDurability(int decrease)
    {
        Durability -= decrease;
        if (Durability <= 0)
        {
            player.RemoveCurrentSword();
        }
        BattleUI.Instance.durabilityRemainDisplay.ChangeRemain(Durability, MaxDurability);
    }

    public void IncreaseDurability(float increase)
    {
        Debug.Log(increase);
        if (Durability + increase > MaxDurability)
        {
            Durability = MaxDurability;
        }
        else
        {
            Durability += increase;
        }
        BattleUI.Instance.durabilityRemainDisplay.ChangeRemain(Durability, MaxDurability);
    }

    private void AddSkillComponent(ref PlayerSkill skill, string skillName)
    {
        if (skillName != null)
        {
            var skillClassType = Type.GetType($"DefaultNamespace.{skillName}");
            skill = gameObject.AddComponent(skillClassType) as PlayerSkill;
            
            if (skill != null)
            {
                skill.Initialize(skillName, player);
            }
            else
            {
                Debug.LogWarning(
                    $"{gameObject.GetInstanceID()}에게 {skillName}이 추가되지 않았습니다.\nskillClassType : {skillClassType}\nnewSkill : {skill}");
            }
        }
    }

    public float GetFinalAttackCooldown()
    {
        return AttackCooldown * characterEffect.attackCooldownBonus;
    }

    public float GetFinalDamage()
    {
        return Damage * characterEffect.damageBonus;
    }
}
