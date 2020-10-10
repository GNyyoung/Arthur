using System;

namespace DefaultNamespace
{
    public interface ICharacterAction
    {
        void StartAction();
        void StopAction();
    }

    public interface IPlayerAction : ICharacterAction
    {
        PlayerStatus GetStatus();
        void InitializeAction(Player player);
    }

    public interface IMonsterAction : ICharacterAction
    {
        MonsterStatus GetStatus();
    }
}