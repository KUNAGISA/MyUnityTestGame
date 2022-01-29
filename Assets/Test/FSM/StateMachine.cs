using System.Collections.Generic;

namespace FSM
{
    public interface ITransitionDef<ETransition>
    {
        /// <summary>
        /// ���ת������ǰ���͵�����
        /// </summary>
        /// <param name="transition">ת������</param>
        /// <returns>����</returns>
        ITransitionDef<ETransition> Transition(ETransition transition);
    }

    public interface IStateMachine<Entity, ETransition> : ITransition<ETransition>
    {
        /// <summary>
        /// ע��״̬
        /// </summary>
        /// <typeparam name="State">״̬��</typeparam>
        /// <param name="state">״̬</param>
        /// <returns>ת��������</returns>
        ITransitionDef<ETransition> RegisterState<State>(State state) where State : IState<Entity, ETransition>;

        /// <summary>
        /// ״̬��֡��ʱ��
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
                    UnityEngine.Debug.LogWarning("�ظ����ת��" + transition);
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