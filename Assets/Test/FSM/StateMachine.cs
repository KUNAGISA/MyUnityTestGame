using UnityEngine;

namespace FSM
{
    public interface IStateMachine<Entity>
    {
        /// <summary>
        /// 初始化状态机
        /// </summary>
        /// <param name="entity">控制的实体</param>
        /// <param name="state">初始化的状态</param>
        void InitStateMachine(Entity entity, IState<Entity> state);

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="state">切换的状态</param>
        void ChangeState(IState<Entity> state);

        /// <summary>
        /// 状态机定时器
        /// </summary>
        void FrameTick();

        /// <summary>
        /// 检查当前状态是否目标状态
        /// </summary>
        /// <typeparam name="State">目标状态</typeparam>
        /// <returns></returns>
        bool CheckState<State>() where State : class, IState<Entity>;
    }

    public abstract class AbstractStateMachine<Entity> : MonoBehaviour, IStateMachine<Entity>
        where Entity : class
    {   
        private Entity m_Entity = null;

        [SerializeField]
        private IState<Entity> m_CurrState;

        protected virtual void OnInitStateMachine() { }

        protected virtual void OnFrameTick() { }

        void IStateMachine<Entity>.InitStateMachine(Entity entity, IState<Entity> state)
        {
            m_Entity = entity;
            ChangeState(state);
            OnInitStateMachine();
        }

        void IStateMachine<Entity>.FrameTick()
        {
            OnFrameTick();
            m_CurrState?.UpdateState(m_Entity, this);
        }

        public bool CheckState<State>() where State : class, IState<Entity>
        {
            return m_CurrState is State;
        }

        public void ChangeState(IState<Entity> state)
        {
            m_CurrState?.ExitState(m_Entity, this);
            m_CurrState = state;
            m_CurrState?.EnterState(m_Entity, this);
        }
    }
}
