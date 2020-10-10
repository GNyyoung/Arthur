namespace DefaultNamespace
{
    // 몬스터 -> 플레이어 데이터 전달을 목적으로 만드려 함.
    public interface IPlayerReceiver
    {
        // 몬스터가 플레이어 공격할 때 호출
        void AttackPlayer();
        void AttackPlayer(int damage);
    }
}