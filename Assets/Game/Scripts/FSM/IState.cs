namespace FSM
{
    /// <summary>
    /// 进入状态接口
    /// </summary>
    public interface IEnterState
    {
        void EnterState();
    }

    /// <summary>
    /// 带参数进入状态接口
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IEnterState<TData>
    {
        void EnterState(in TData data);
    }

    /// <summary>
    /// 状态接口
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public interface IState<TState> where TState : IState<TState>
    {
        ITransition<TState> TickState();

        void ExitState();
    }
}