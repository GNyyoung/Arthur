namespace DefaultNamespace
{
    public class MoveSpeedController
    {
        public const float MaxSpeed = 2.5f;
        public const float PlayerMaxSpeed = 1.3f;
        public const float MonsterMaxSpeed = MaxSpeed - PlayerMaxSpeed;
        public const float StandardRetreatRate = 0.2f;
        
        public float CurrentRetreatRate { get; private set; }
        public float PlayerSpeed { get; private set; } = MonsterMaxSpeed;
        public float MonsterSpeed { get; private set; }

        public void UpdatePlayerSpeed(float speed)
        {
            PlayerSpeed = speed;
        }

        public float GetRetreatSpeed()
        {
            return MaxSpeed * CurrentRetreatRate;
        }

        public void ActiveRetreatSpeed(bool isActive)
        {
            if (isActive == true)
            {
                CurrentRetreatRate = StandardRetreatRate;
            }
            else
            {
                CurrentRetreatRate = 0;
            }
        }
    }
}