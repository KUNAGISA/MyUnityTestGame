using System.Collections.Generic;

namespace FSM
{
    public interface IStateMachine<Entity, ETransition>
    {
        /// <summary>
        /// 注册完所有状态后调用，初始化状态机
        /// </summary>
        /// <param name="entity">状态机控制的实体</param>
        /// <param name="transition">初始切换</param>
        void InitStateMachine(Entity entity, ETransition transition = default);

        /// <summary>
        /// 注册状态
        /// </summary>
        /// <typeparam name="State">状态</typeparam>
        /// <param name="state">状态实例</param>
        /// <param name="transitions">切换到该状态的条件</param>
        void RegisterState<State>(State state, params ETransition[] transitions) where State : IState<Entity, ETransition>;

        /// <summary>
        /// 状态机帧定时器
        /// </summary>
        void TickStateMachine();
    }

    public class StateMachine<Entity, ETransition> : IStateMachine<Entity, ETransition>, ITransition<ETransition>
    {
        private readonly List<IState<Entity, ETransition>> m_States = new List<IState<Entity, ETransition>>();
        private readonly Dictionary<ETransition, int> m_TransitionMap = new Dictionary<ETransition, int>();

        private Entity m_Entity;
        private IState<Entity, ETransition> m_CurrState = null;

        public void RegisterState<State>(State state, params ETransition[] transitions) where State : IState<Entity, ETransition>
        {
            m_States.Add(state);

            var index = m_States.Count - 1;
            foreach (var transition in transitions)
            {
                if (m_TransitionMap.ContainsKey(transition))
                {
                    UnityEngine.Debug.LogWarning("重复添加转换" + transition);
                }
                else
                {
                    m_TransitionMap.Add(transition, index);
                }
            }
        }

        void IStateMachine<Entity, ETransition>.InitStateMachine(Entity entity, ETransition transition)
        {
            m_Entity = entity;
            OnInitStateMachine();
            (this as ITransition<ETransition>).TransState(transition);
        }

        void IStateMachine<Entity, ETransition>.TickStateMachine()
        {
            OnTickStateMachine();
            m_CurrState?.TickState(m_Entity, this);
        }

        void ITransition<ETransition>.TransState(ETransition transition)
        {
            if (m_TransitionMap.TryGetValue(transition, out var index))
            {
                m_CurrState?.ExitState(m_Entity, this);
                m_CurrState = m_States[index];
                m_CurrState?.EnterState(m_Entity, this);
            }
        }

        /// <summary>
        /// 初始化状态机
        /// </summary>
        protected virtual void OnInitStateMachine() { }

        /// <summary>
        /// 状态机帧更新
        /// </summary>
        protected virtual void OnTickStateMachine() { }
    }
}