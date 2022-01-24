namespace FSM
{
    public interface IState<Entity>
    {
        void EnterState(Entity entity, IStateMachine<Entity> stateMachine);

        void ExitState(Entity entity, IStateMachine<Entity> stateMachine);

        void UpdateState(Entity entity, IStateMachine<Entity> stateMachine);
    }
}