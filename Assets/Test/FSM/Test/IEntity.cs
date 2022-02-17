namespace FSM.Test
{
    public enum EntityStateTransition
    {
        WaitFinish,
        MoveFinish,
    }

    public interface IEntity
    {
        public float WaitEndTime { get; set; }

        void SetAnimatorBool(string tag, bool isTag);
    }
}