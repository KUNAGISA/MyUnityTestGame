using System;

namespace FSM
{
    /// <summary>
    /// 可转换状态接口
    /// </summary>
    /// <typeparam name="ETransition">转换条件</typeparam>
    public interface ITransition<ETransition>
    {
        /// <summary>
        /// 转换状态
        /// </summary>
        /// <param name="transition">转换条件</param>
        void TransState(ETransition transition);
    }

    /// <summary>
    /// 状态接口
    /// </summary>
    /// <typeparam name="Entity">操作的实体类</typeparam>
    /// <typeparam name="ETransition">转换状态条件</typeparam>
    public interface IState<in Entity, ETransition>
    {
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="transition">状态切换</param>
        void EnterState(Entity entity, ITransition<ETransition> transition);

        /// <summary>
        /// 离开状态
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="transition">状态切换</param>
        void ExitState(Entity entity, ITransition<ETransition> transition);

        /// <summary>
        /// 帧更新状态
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="transition">状态切换</param>
        void TickState(Entity entity, ITransition<ETransition> transition);
    }

    /// <summary>
    /// 状态基类
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    /// <typeparam name="ETransition"></typeparam>
    public class BaseState<Entity, ETransition> : IState<Entity, ETransition>
    {
        void IState<Entity, ETransition>.EnterState(Entity entity, ITransition<ETransition> transition)
        {
            OnEnterState(entity, transition);
        }

        void IState<Entity, ETransition>.ExitState(Entity entity, ITransition<ETransition> transition)
        {
            OnExitState(entity, transition);
        }

        void IState<Entity, ETransition>.TickState(Entity entity, ITransition<ETransition> transition)
        {
            OnTickState(entity, transition);
        }

        protected virtual void OnEnterState(Entity entity, ITransition<ETransition> transition) { }

        protected virtual void OnExitState(Entity entity, ITransition<ETransition> transition) { }

        protected virtual void OnTickState(Entity entity, ITransition<ETransition> transition) { }
    }
}