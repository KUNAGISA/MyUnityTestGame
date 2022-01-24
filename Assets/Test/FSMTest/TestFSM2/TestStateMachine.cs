using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestFSM2
{
    public interface ITestStateMachine : FSM2.IStateMachine
    {

    }

    public class TestStateMachine : FSM2.AbstractStateMachine, ITestStateMachine
    {

    }
}