using System;
using System.Collections.Generic;

using Debug = UnityEngine.Debug;

namespace FSM
{
    public class StateMachine<Entity> : IChangeState<Entity>
    {
        private readonly Dictionary<Type, IState<Entity>> m_States = new Dictionary<Type, IState<Entity>>();

        private readonly Entity m_Entity;
        private IState<Entity> m_CurrState = null;

        public StateMachine(Entity entity)
        {
            m_Entity = entity;
        }

        public void RegisterState<State>(State state) where State : IState<Entity>
        {
            if (m_States.ContainsKey(typeof(State)))
            {
                Debug.LogWarning("重复注册状态" + typeof(State));
                return;
            }
            m_States.Add(typeof(State), state);
            state.InitState(m_Entity);
        }

        public void SendMessage<TMessage>(in TMessage message)
        {
            var receive = m_CurrState as IReceiveMsg<TMessage, Entity>;
            receive?.ReceiveMsg(in message)?.Execute(m_Entity, this);
        }

        public void TickStateMachine()
        {
            m_CurrState?.TickState()?.Execute(m_Entity, this);
        }

        public void ChangeState<State>() where State : IState<Entity>
        {
            Debug.Log("转换状态");
            if (!m_States.TryGetValue(typeof(State), out var state))
            {
                Debug.LogWarning("找不到切换的状态" + typeof(State));
                return;
            }

            m_CurrState?.ExitState();
            m_CurrState = state;
            m_CurrState.EnterState();
        }
    }
}