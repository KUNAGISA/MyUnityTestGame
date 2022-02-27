using FSM;

namespace Test
{
    public abstract class PlayerBaseState : BaseState<Player>
    {
    }

    public interface IPlayerReceive<TMessage> : IReceiveMsg<TMessage, Player>
    {

    }
}