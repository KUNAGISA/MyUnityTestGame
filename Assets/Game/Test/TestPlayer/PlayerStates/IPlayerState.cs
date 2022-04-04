using FSM;

namespace Game.Test
{
    public interface IPlayerState : IState<IPlayerState>
    {

    }

    public interface IPlayerReceiveMsg<TMsg> : IReceiveMsg<IPlayerState, TMsg>
    {

    }

    public interface IPlayerTransition : ITransition<IPlayerState>
    {

    }

    public abstract class BasePlayerState : IPlayerState, IEnterState
    {
        protected readonly BasePlayer player;

        public BasePlayerState(BasePlayer player)
        {
            this.player = player;
        }

        void IEnterState.EnterState() => OnEnterState();

        void IState<IPlayerState>.ExitState() => OnExitState();

        ITransition<IPlayerState> IState<IPlayerState>.TickState() => OnTickState();

        protected virtual void OnEnterState() { }

        protected virtual void OnExitState() { }

        protected virtual ITransition<IPlayerState> OnTickState() { return null; }
    }

    public abstract class BasePlayerState<TData> : IPlayerState, IEnterState<TData>
    {
        protected readonly BasePlayer player;

        public BasePlayerState(BasePlayer player)
        {
            this.player = player;
        }

        void IEnterState<TData>.EnterState(in TData data) => OnEnterState(in data);

        void IState<IPlayerState>.ExitState() => OnExitState();

        ITransition<IPlayerState> IState<IPlayerState>.TickState() => OnTickState();

        protected abstract void OnEnterState(in TData data);

        protected virtual void OnExitState() { }

        protected virtual ITransition<IPlayerState> OnTickState() { return null; }
    }
}