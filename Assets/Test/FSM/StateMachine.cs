using System.Collections.Generic;

namespace FSM
{
    public interface ITransitionDef<ETransition>
    {
        /// <summary>
        /// 添加转换到当前类型的条件
        /// </summary>
        /// <param name="transition">转换条件</param>
        /// <returns>自身</returns>
        ITransitionDef<ETransition> Transition(ETransition transition);
    }

    public interface IStateMachine<Entity, ETransition> : ITransition<ETransition>
    {
        /// <summary>
        /// 注册状态
        /// </summary>
        /// <typeparam name="State">状态类</typeparam>
        /// <param name="state">状态</param>
        /// <returns>转换定义器</returns>
        ITransitionDef<ETransition> RegisterState<State>(State state) where State : IState<Entity, ETransition>;

        /// <summary>
        /// 状态机帧定时器
        /// </summary>
        void TickStateMachine();
    }

    public class StateMachine<Entity, ETransition> : IStateMachine<Entity, ETransition>
    {
        private readonly List<IState<Entity, ETransition>> m_States = new List<IState<Entity, ETransition>>();
        private readonly Dictionary<ETransition, int> m_TransitionMap = new Dictionary<ETransition, int>();

        private readonly Entity m_Entity;
        private IState<Entity, ETransition> m_CurrState = null;

        private class TransitionDef : ITransitionDef<ETransition>
        {
            private readonly StateMachine<Entity, ETransition> stateMachine;
            private readonly int index;

            public TransitionDef(StateMachine<Entity, ETransition> stateMachine, int index)
            {
                this.stateMachine = stateMachine;
                this.index = index;
            }

            public ITransitionDef<ETransition> Transition(ETransition transition)
            {
                if (stateMachine.m_TransitionMap.ContainsKey(transition))
                {
                    UnityEngine.Debug.LogWarning("重复添加转换" + transition);
                }
                else
                {
                    stateMachine.m_TransitionMap.Add(transition, index);
                }
                return this;
            }
        }

        public void ChangeState(ETransition transition)
        {
            if (m_TransitionMap.TryGetValue(transition, out var index))
            {
                ChangeState(m_States[index]);
            }
        }

        public StateMachine(Entity entity)
        {
            m_Entity = entity;
        }

        public ITransitionDef<ETransition> RegisterState<State>(State state) where State : IState<Entity, ETransition>
        {
            m_States.Add(state);
            return new TransitionDef(this, m_States.Count - 1);
        }

        void IStateMachine<Entity, ETransition>.TickStateMachine()
        {
            OnTickStateMachine();
            m_CurrState?.TickState(m_Entity, this);
        }

        private void ChangeState(IState<Entity, ETransition> state)
        {
            m_CurrState?.ExitState(m_Entity, this);
            m_CurrState = state;
            m_CurrState?.EnterState(m_Entity, this);
        }

        protected virtual void OnTickStateMachine() { }
    }
}