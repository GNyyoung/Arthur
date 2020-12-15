namespace DefaultNamespace
{
    public static class SkillCooldownCondition
    {
        /// <summary>
        /// Idle 상태나 스턴이 아닐 때 true 반환
        /// </summary>
        /// <param name="monster"></param>
        /// <returns></returns>
        public static bool IsPauseCooldownWhenIdle(Monster monster)
        {
            if (monster.CurrentAction?.GetStatus() == MonsterStatus.Idle || 
                monster.CharacterEffect.CurrentEffect == Effect.Stun)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}