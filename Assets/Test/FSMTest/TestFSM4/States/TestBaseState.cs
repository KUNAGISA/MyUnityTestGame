using FSM4;

namespace TestFSM4
{
    public interface ITestBaseState : IState<TestPlayer>
    { 
    }

    public abstract class TestBaseState : ITestBaseState
    {
        void IState<TestPlayer>.EnterState(TestPlayer entity, ICanChangeState<TestPlayer> stateMachine)
        {
            OnEnterState(entity, stateMachine);
        }

        void IState<TestPlayer>.ExitState(TestPlayer entity, ICanChangeState<TestPlayer> stateMachine)
        {
            OnExitState(entity, stateMachine);
        }

        void IState<TestPlayer>.TickState(TestPlayer entity, ICanChangeState<TestPlayer> stateMachine)
        {
            OnTickState(entity, stateMachine);
        }

        protected virtual void OnEnterState(TestPlayer entity, ICanChangeState<TestPlayer> stateMachine) { }

        protected virtual void OnExitState(TestPlayer entity, ICanChangeState<TestPlayer> stateMachine) { }

        protected virtual void OnTickState(TestPlayer entity, ICanChangeState<TestPlayer> stateMachine) { }
    }
}