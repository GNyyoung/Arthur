namespace DefaultNamespace
{
    public interface ICombatant
    {
        bool TakeDamage(ICombatant enemy, int damage, AttackDirection direction);
    }
}