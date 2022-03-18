namespace FSM
{
    public interface IReceiveMsg<TState, TMessage> where TState : IState<TState>
    {
        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <returns>转换状态</returns>
        internal ITransition<TState> ReceiveMsg(in TMessage message);
    }
}
