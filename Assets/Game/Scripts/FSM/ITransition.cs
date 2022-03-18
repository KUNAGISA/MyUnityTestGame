namespace FSM
{
    public interface IChangeState<TState> where TState : IState<TState>
    {
        void ChangeState<State>() where State : class, TState, IEnterState;

        void ChangeState<State, TData>(in TData data) where State : class, TState, IEnterState<TData>;
    }

    /// <summary>
    /// 状态转换接口
    /// </summary>
    public interface ITransition<TState> where TState : IState<TState>
    {
        void Execute(IChangeState<TState> changeState);
    }
}
