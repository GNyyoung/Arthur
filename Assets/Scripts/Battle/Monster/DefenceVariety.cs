using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public static class DefenceVariety
    {
        private static readonly Dictionary<AttackDirection, int> defenceTriggerSet = new Dictionary<AttackDirection, int>()
        {
            {AttackDirection.None, Animator.StringToHash("Idle")},
            {AttackDirection.Slash, Animator.StringToHash("DefSlash")},
            {AttackDirection.Stab, Animator.StringToHash("DefStab")},
            {AttackDirection.UpperSlash, Animator.StringToHash("DefUpperSlash")}
        };

        public static readonly Dictionary<AttackDirection, Color32> defenceColorSet =
            new Dictionary<AttackDirection, Color32>()
            {
                {AttackDirection.None, new Color32(91, 91, 91, 255)},
                {AttackDirection.Slash, new Color32(233, 41, 41, 255)},
                {AttackDirection.Stab, new Color32(63, 34, 229, 255)},
                {AttackDirection.UpperSlash, new Color32(36, 229, 42, 255)}
            };
        
        private static readonly Dictionary<AttackDirection, Sprite> gemSpriteSet = new Dictionary<AttackDirection, Sprite>();
        /// <summary>
        /// 몬스터에게 사용할 보석 스프라이트를 할당함
        /// </summary>
        public static void InitializeGemSprite()
        {
            if (gemSpriteSet == null)
            {
                gemSpriteSet.Add(AttackDirection.None, Resources.Load<Sprite>("Sprites/BlackGem"));
                gemSpriteSet.Add(AttackDirection.Slash, Resources.Load<Sprite>("Sprites/GreenGem"));
                gemSpriteSet.Add(AttackDirection.Stab, Resources.Load<Sprite>("Sprites/BlueGem"));
                gemSpriteSet.Add(AttackDirection.UpperSlash, Resources.Load<Sprite>("Sprites/RedGem"));   
            }
        }
        
        public static Delegate SetDefenceVariety(string defenceType)
        {
            var type = typeof(DefenceVariety);
            var methodInfo = type.GetMethod(defenceType, 
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            Debug.Log(methodInfo);
            var defenceDelegate = Delegate.CreateDelegate(typeof(ChangeDefendDirection),null, methodInfo);
            return defenceDelegate;
        }
        
        public static AttackDirection RandomDirection(Monster monster)
        {
            var nextDirection = GetRandomDirection();
            UpdateDefenceInformation(monster, nextDirection);
            
            return nextDirection;
        }

        public static AttackDirection DamagedDirection(Monster monster)
        {
            AttackDirection nextDirection;
            
            if (monster.MonsterStat.DamagedDirection == AttackDirection.None)
            {
                nextDirection = GetRandomDirection();
                UpdateDefenceInformation(monster, nextDirection);
            }
            else
            {
                UpdateDefenceInformation(monster, monster.MonsterStat.DamagedDirection);
                nextDirection = monster.MonsterStat.DamagedDirection;
            }

            return nextDirection;
        }

        public static AttackDirection NotDefence(Monster monster)
        {
            UpdateDefenceInformation(monster, AttackDirection.None);
            return AttackDirection.None;
        }

        public static AttackDirection NextDirection(Monster monster)
        {
            var directions = Enum.GetValues(typeof(AttackDirection));
            AttackDirection direction;
            
            if ((int) monster.MonsterStat.CurrentDefenceDirection + 1 < directions.Length)
            {
                direction = monster.MonsterStat.CurrentDefenceDirection + 1;
            }
            else
            {
                direction = (AttackDirection)directions.GetValue(1);
            }

            UpdateDefenceInformation(monster, direction);
            return direction;
        }

        public static AttackDirection KeepCurrentDirection(Monster monster)
        {
            var nextDirection = monster.DefenceDirection;
            UpdateDefenceInformation(monster, nextDirection);
            return nextDirection;
        }

        public static void UpdateDefenceInformation(Monster monster, AttackDirection direction)
        {
            monster.MonsterStat.CurrentDefenceDirection = direction;
            monster.DefenceDirection = direction;
            monster.Animator.SetTrigger(defenceTriggerSet[direction]);
            monster.ToolRenderer.color = defenceColorSet[direction];
        }

        public static AttackDirection GetRandomDirection()
        {
            var directions = Enum.GetValues(typeof(AttackDirection));
            int num = Random.Range(1, directions.Length);
            return (AttackDirection) directions.GetValue(num);
        }
    }
}