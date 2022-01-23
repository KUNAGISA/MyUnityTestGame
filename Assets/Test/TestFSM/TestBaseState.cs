using FSM;

namespace Test
{
    public class TestBaseState : IState<TestPlayer>
    {
        protected virtual void OnEnterState(TestPlayer entity, IStateMachine<TestPlayer> stateMachine) { }

        protected virtual void OnExitState(TestPlayer entity, IStateMachine<TestPlayer> stateMachine) { }

        protected virtual void OnUpdateState(TestPlayer entity, IStateMachine<TestPlayer> stateMachine) { }

        void IState<TestPlayer>.EnterState(TestPlayer entity, IStateMachine<TestPlayer> stateMachine)
        {
            OnEnterState(entity, stateMachine);
        }

        void IState<TestPlayer>.ExitState(TestPlayer entity, IStateMachine<TestPlayer> stateMachine)
        {
            OnExitState(entity, stateMachine);
        }

        void IState<TestPlayer>.UpdateState(TestPlayer entity, IStateMachine<TestPlayer> stateMachine)
        {
            OnUpdateState(entity, stateMachine);
        }
    }
}
