namespace FSM2
{
    public interface IState
    {
        void EnterState();

        void ExitState();

        void TickState();
    }

    public abstract class AbstractState<Entity, StateMachine> : IState
    {
        public readonly Entity CurrEntity;
        public readonly StateMachine CurrStateMachine;

        public AbstractState(Entity entity, StateMachine stateMachine)
        {
            CurrEntity = entity;
            CurrStateMachine = stateMachine;
        }

        void IState.EnterState() => OnEnterState();

        void IState.ExitState() => OnExitState();

        void IState.TickState() => OnTickState();

        protected virtual void OnEnterState() { }
        protected virtual void OnExitState() { }
        protected virtual void OnTickState() { }
    }
}