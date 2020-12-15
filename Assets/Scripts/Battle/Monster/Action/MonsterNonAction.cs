namespace DefaultNamespace
{
    public class MonsterNonAction : MonsterAction
    {
        public override void StartAction()
        {
            this.enabled = true;
        }

        public override void StopAction()
        {
            this.enabled = false;
        }

        public override void TerminateAction()
        {
            this.enabled = false;
        }

        public override MonsterStatus GetStatus()
        {
            return MonsterStatus.None;
        }
    }
}