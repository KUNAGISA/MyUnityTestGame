using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFSM2
{
    public class TestBaseState : FSM2.AbstractState<TestPlayer, ITestStateMachine>
    {
        public TestBaseState(TestPlayer entity, ITestStateMachine stateMachine) : base(entity, stateMachine)
        {
        }
    }
}
