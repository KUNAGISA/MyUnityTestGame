using FSM;

namespace Test
{
    public struct PlayerIdleFinish : ITransition<Player>
    {
        public void Execute(Player entity, IChangeState<Player> transition)
        {
            transition.ChangeState<PlayerTextState>();

        }
    }
}