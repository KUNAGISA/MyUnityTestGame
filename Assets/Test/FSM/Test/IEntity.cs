namespace FSM.Test
{
    public enum EntityStateTransition
    {
        StateInit,
        WaitFinish,
        MoveFinish,
    }

    public interface IEntity
    {
        public float WaitEndTime { get; set; }
    }
}