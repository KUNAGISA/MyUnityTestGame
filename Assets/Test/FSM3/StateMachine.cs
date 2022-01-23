using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM3
{
    public interface IStateMachine<Entity, StateType> where StateType : Enum
    {
        void InitStateMachine(Entity entity);

        void RegisterState(StateType type, IState<Entity, StateType> state);

        void ChangeState(StateType type);

        void TickStateMachine();
    }

    public class StateMachine<Entity, StateType> : MonoBehaviour, IStateMachine<Entity, StateType> where StateType : Enum
    {
        private Dictionary<StateType, IState<Entity, StateType>> m_States = new Dictionary<StateType, IState<Entity, StateType>>();
        private IState<Entity, StateType> m_CurrState;
        private Entity m_Entity;

        public void RegisterState(StateType type, IState<Entity, StateType> state)
        {
            m_States.Add(type, state);
        }

        public void ChangeState(StateType type)
        {
            m_CurrState?.ExitState(m_Entity, this);
            m_CurrState = m_States[type];
            m_CurrState?.EnterState(m_Entity, this);
        }

        void IStateMachine<Entity, StateType>.InitStateMachine(Entity entity)
        {
            m_Entity = entity;
            OnInitStateMachine();
        }

        void IStateMachine<Entity, StateType>.TickStateMachine()
        {
            OnTickStateMachine();
            m_CurrState?.TickState(m_Entity, this);
        }

        protected virtual void OnInitStateMachine() { }

        protected virtual void OnTickStateMachine() { }
    }
}
