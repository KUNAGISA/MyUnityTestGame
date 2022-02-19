namespace FSM.Test
{
    public abstract class PlayerBaseState : BaseState<Player>
    {
    }

    public interface IPlayerReceive<TMessage> : IReceiveMsg<TMessage, Player>
    {

    }
}