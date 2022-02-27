using FSM;

namespace Test
{
    public struct PlayerTextAnimFinish : ITransition<Player>
    {
        public void Execute(Player entity, IChangeState<Player> transition)
        {
            transition.ChangeState<PlayerIdleState>();
        }
    }
}