using UnityEngine;

namespace FSM2
{
    public interface IStateMachine
    {
        void ChangeState(IState state);
        void FrameTick();
    }

    public abstract class AbstractStateMachine : MonoBehaviour, IStateMachine
    {
        private IState m_CurrState;

        void IStateMachine.ChangeState(IState state)
        {
            m_CurrState?.ExitState();
            m_CurrState = state;
            m_CurrState?.EnterState();
        }

        void IStateMachine.FrameTick()
        {
            m_CurrState?.TickState();
        }
    }
}
