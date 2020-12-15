namespace DefaultNamespace
{
    public class MonsterData
    {
        public string Name { get; set; }
        public float MaxHP { get; set; }
        public float CurrentHP { get; set; }
        public MonsterStatus CurrentStatus { get; set; }
        public AttackDirection CurrentAttackDirection { get; set; }
        public AttackDirection CurrentDefenceDirection { get; set; }
        public bool IsPush { get; set; }
        public bool IsDefend { get; set; }
        public bool IsCollide { get; set; }
        public AttackDirection DamagedDirection { get; set; } = AttackDirection.None;
        public string CurrentSkillName { get; set; }
    }
}