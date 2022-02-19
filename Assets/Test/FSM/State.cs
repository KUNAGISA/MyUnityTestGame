namespace FSM
{
    /// <summary>
    /// 状态接口
    /// </summary>
    /// <typeparam name="Entity">操作的实体类</typeparam>
    public interface IState<in Entity>
    {
        /// <summary>
        /// 初始化状态
        /// </summary>
        /// <param name="entity">操作的实体</param>
        void InitState(Entity entity);

        /// <summary>
        /// 进入状态
        /// </summary>
        void EnterState();

        /// <summary>
        /// 离开状态
        /// </summary>
        void ExitState();

        /// <summary>
        /// 状态定时器
        /// </summary>
        ITransition<Entity> TickState();
    }

    public abstract class BaseState<Entity> : IState<Entity>
    {
        private Entity m_Entity;
        protected Entity entity => m_Entity;

        void IState<Entity>.InitState(Entity entity)
        {
            m_Entity = entity;
            OnInitState();
        }

        void IState<Entity>.EnterState() => OnEnterState();

        void IState<Entity>.ExitState() => OnExitState();

        ITransition<Entity> IState<Entity>.TickState() => OnTickState();

        /// <summary>
        /// 状态初始化时回调
        /// </summary>
        protected virtual void OnInitState() { }

        /// <summary>
        /// 进入状态时回调
        /// </summary>
        protected virtual void OnEnterState() { }

        /// <summary>
        /// 离开状态时回调
        /// </summary>
        protected virtual void OnExitState() { }

        /// <summary>
        /// 状态定时器回调
        /// </summary>
        /// <returns></returns>
        protected virtual ITransition<Entity> OnTickState() => null;
    }
}