namespace DefaultNamespace
{
    public class PlayerNonAction : PlayerAction
    {
        public override PlayerStatus GetStatus()
        {
            return PlayerStatus.None;
        }
    }
}