using FSM4;

namespace TestFSM4
{
    public interface ITestStateMachine : IStateMachine<TestPlayer>
    {

    }

    public class TestStateMachine : AbstractStateMachine<TestPlayer>, ITestStateMachine
    {

    }
}