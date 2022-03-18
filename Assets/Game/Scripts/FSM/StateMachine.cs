using System;
using System.Collections.Generic;

namespace FSM
{
    /// <summary>
    /// 状态机
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public class StateMachine<TState> : IChangeState<TState> where TState : class, IState<TState>
    {
        private readonly Dictionary<Type, TState> m_States = new Dictionary<Type, TState>();

        private IState<TState> m_CurrState = null;

        /// <summary>
        /// 注册状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <exception cref="Exception">重复注册</exception>
        public void RegisterState<State>(State state) where State : class, TState
        {
            if (m_States.ContainsKey(typeof(State)))
            {
                throw new Exception("重复注册状态" + typeof(State));
            }
            m_States.Add(typeof(State), state);
        }

        /// <summary>
        /// 发送信息到状态
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="msg">信息</param>
        public void SendMessage<TMsg>(in TMsg msg)
        {
            var receive = m_CurrState as IReceiveMsg<TState, TMsg>;
            receive?.ReceiveMsg(in msg)?.Execute(this);
        }

        /// <summary>
        /// 帧更新
        /// </summary>
        public void TickStateMachine()
        {
            m_CurrState?.TickState()?.Execute(this);
        }
        
        void IChangeState<TState>.ChangeState<State>()
        {
            if (!m_States.TryGetValue(typeof(State), out var newState))
            {
                throw new Exception("没有注册状态" + typeof(State));
            }

            m_CurrState?.ExitState();
            m_CurrState = newState;
            (m_CurrState as State).EnterState();
        }

        void IChangeState<TState>.ChangeState<State, TData>(in TData data)
        {
            if (!m_States.TryGetValue(typeof(State), out var newState))
            {
                throw new Exception("没有注册状态" + typeof(State));
            }

            m_CurrState?.ExitState();
            m_CurrState = newState;
            (m_CurrState as State).EnterState(in data);
        }
    }
}