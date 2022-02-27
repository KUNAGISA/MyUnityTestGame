namespace FSM
{
    /// <summary>
    /// 接收信息接口
    /// </summary>
    /// <typeparam name="TMessage">信息类</typeparam>
    /// <typeparam name="Entity">实体类</typeparam>
    public interface IReceiveMsg<TMessage, in Entity>
    {
        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <returns>转换状态</returns>
        public ITransition<Entity> ReceiveMsg(in TMessage message);
    }

    /// <summary>
    /// 接收任意消息接口
    /// </summary>
    /// <typeparam name="Entity">实体类</typeparam>
    public interface IReceiveAnyMsg<in Entity>
    {
        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <returns>转换状态</returns>
        public ITransition<Entity> ReceiveMsg<TMessage>(in TMessage message);
    }
}
