namespace DefaultNamespace
{
    public class MoveSpeedController
    {
        public float MaxSpeed { get; private set; } = 1.5f;
        public float RetreatRate { get; private set; } = 0.2f;
        public float PlayerSpeed { get; private set; }
        public float MonsterSpeed { get; private set; }

        public void UpdatePlayerSpeed(float speed)
        {
            PlayerSpeed = speed;
            MonsterSpeed = MaxSpeed - speed;
        }

        public float GetRetreatSpeed()
        {
            return MaxSpeed * RetreatRate;
        }
    }
}