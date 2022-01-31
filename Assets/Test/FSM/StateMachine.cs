using System.Collections.Generic;

namespace FSM
{
    public interface IStateMachine<Entity, ETransition>
    {
        /// <summary>
        /// ע��������״̬����ã���ʼ��״̬��
        /// </summary>
        /// <param name="entity">״̬�����Ƶ�ʵ��</param>
        /// <param name="transition">��ʼ�л�</param>
        void InitStateMachine(Entity entity, ETransition transition = default);

        /// <summary>
        /// ע��״̬
        /// </summary>
        /// <typeparam name="State">״̬</typeparam>
        /// <param name="state">״̬ʵ��</param>
        /// <param name="transitions">�л�����״̬������</param>
        void RegisterState<State>(State state, params ETransition[] transitions) where State : IState<Entity, ETransition>;

        /// <summary>
        /// ״̬��֡��ʱ��
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
                    UnityEngine.Debug.LogWarning("�ظ����ת��" + transition);
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
        /// ��ʼ��״̬��
        /// </summary>
        protected virtual void OnInitStateMachine() { }

        /// <summary>
        /// ״̬��֡����
        /// </summary>
        protected virtual void OnTickStateMachine() { }
    }
}