namespace FSM
{
    /// <summary>
    /// 可转换状态接口
    /// </summary>
    /// <typeparam name="Entity">实体</typeparam>
    public interface IChangeState<out Entity>
    {
        void ChangeState<State>() where State : IState<Entity>;
    }

    public interface ITransition<in Entity>
    {
        void Execute(Entity entity, IChangeState<Entity> transition);
    }
}
