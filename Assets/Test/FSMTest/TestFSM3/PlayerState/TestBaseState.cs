using FSM3;

namespace TestFSM3
{
    public class TestBaseState : IState<TestPlayer, PlayerState>
    {
        void IState<TestPlayer, PlayerState>.EnterState(TestPlayer entity, IStateMachine<TestPlayer, PlayerState> stateMachine)
        {
            OnEnterState(entity, stateMachine);
        }

        void IState<TestPlayer, PlayerState>.ExitState(TestPlayer entity, IStateMachine<TestPlayer, PlayerState> stateMachine)
        {
            OnExitState(entity, stateMachine);
        }

        void IState<TestPlayer, PlayerState>.TickState(TestPlayer entity, IStateMachine<TestPlayer, PlayerState> stateMachine)
        {
            OnTickState(entity, stateMachine);
        }

        protected virtual void OnEnterState(TestPlayer entity, IStateMachine<TestPlayer, PlayerState> stateMachine) { }

        protected virtual void OnExitState(TestPlayer entity, IStateMachine<TestPlayer, PlayerState> stateMachine) { }

        protected virtual void OnTickState(TestPlayer entity, IStateMachine<TestPlayer, PlayerState> stateMachine) { }
    }
}
