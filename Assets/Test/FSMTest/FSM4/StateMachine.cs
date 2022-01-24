using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace FSM4
{
    public interface ICanChangeState<Entity>
    {
        void ChangeState<State>() where State : class, IState<Entity>;
    }

    public interface IStateMachine<Entity> : ICanChangeState<Entity>
    {
        void InitStateMachine(Entity entity);

        void RegisterState<State>(State state) where State : class, IState<Entity>;

        void TickStateMachin();
    }

    public abstract class AbstractStateMachine<Entity> : MonoBehaviour, IStateMachine<Entity>
    {
        private Entity m_Entity;
        private IOCContainer m_StateContainer = new IOCContainer();
        private IState<Entity> m_CurrState;

        public void RegisterState<State>(State state) where State : class, IState<Entity>
        {
            m_StateContainer.Register(state);
        }

        public void ChangeState<State>() where State : class, IState<Entity>
        {
            var newState = m_StateContainer.Get<State>();
            if (newState == null)
            {
                Debug.LogWarning(typeof(State) + "×´Ì¬Ã»ÓÐ×¢²á");
                return;
            }

            m_CurrState?.ExitState(m_Entity, this);
            m_CurrState = newState;
            m_CurrState.EnterState(m_Entity, this);
        }

        void IStateMachine<Entity>.InitStateMachine(Entity entity)
        {
            m_Entity = entity;
            OnInitStateMachine();
        }

        void IStateMachine<Entity>.TickStateMachin()
        {
            TickStateMachin();
            m_CurrState?.TickState(m_Entity, this);
        }

        protected virtual void OnInitStateMachine() { }

        protected virtual void TickStateMachin() { }
    }
}