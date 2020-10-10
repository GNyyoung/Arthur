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
    
    public string Name { get; private set; }
    public float Damage { get; private set; }
    public float Length { get; private set; }
    public int Durability { get; set; }
    /// <summary>
    /// 몬스터를 공격했을 때의 검 내구도 감소량
    /// </summary>
    public int AttackCost { get; private set; }
    /// <summary>
    /// 몬스터에게 피격됐을 때의 검 내구도 감소량
    /// </summary>
    public int HitCost { get; private set; }
    /// <summary>
    /// 공격 모션이 이루어지는 시간
    /// </summary>
    // public float AttackTime { get; private set; }
    /// <summary>
    /// 재공격에 걸리는 시간
    /// </summary>
    public float CooldownTime { get; private set; }
    public float DamageTime { get; private set; }
    public float CooldownRest { get; private set; }
    public PlayerSkill ActiveSkill { get; private set; }
    public PlayerSkill DrawSkill { get; private set; }
    public bool IsUsable { get; private set; }
    public Sprite SwordImage { get; private set; }

    private void Awake()
    {
        ActiveSkill = gameObject.AddComponent<TestSkill>();
        DrawSkill = gameObject.AddComponent<TestDrawSkill2>();
        foreach (var attackButton in BattleUI.Instance.attackButton)
        {
            Debug.Log(attackButton.TryGetComponent<ICooldownObserver>(out var observe));
            if (attackButton.TryGetComponent<ICooldownObserver>(out var observer) == true)
            {
                cooldownDisplay = observer;
                break;
            }
        }
    }

    public void Initialize(SwordInfo swordInfo)
    {
        Name = swordInfo.Name;
        Damage = swordInfo.Damage;
        Length = swordInfo.Length / 100.0f;
        Durability = swordInfo.Durability;
        AttackCost = 1;
        HitCost = 1;
        DamageTime = swordInfo.DamageTime;
        CooldownTime = swordInfo.Cooldown;
        IsUsable = true;
        SwordImage = Resources.Load<Sprite>($"Sprites/Sword/{swordInfo.ImageName}");
    }

    private IEnumerator Cooldown()
    {
        var waitForFixedUpdate = new WaitForFixedUpdate();

        IsUsable = false;
        CooldownRest = CooldownTime;
        while (CooldownRest > 0)
        {
            CooldownRest -= Time.fixedDeltaTime;
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

    public void DamageByAttack(Player player)
    {
        Durability -= AttackCost;
        CheckDurability(player);
    }

    public void DamageByBadAttack(Player player)
    {
        Durability -= AttackCost * 2;
        CheckDurability(player);
    }

    public void DamageByBadDefend(Player player, float damage)
    {
        Debug.Log($"플레이어 {damage}데미지 입음");
        Durability -= Mathf.FloorToInt(damage);
        CheckDurability(player);
    }

    private void CheckDurability(Player player)
    {
        if (Durability <= 0)
        {
            player.RemoveCurrentSword();
        }
    }
}
