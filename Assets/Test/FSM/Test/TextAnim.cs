using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Test
{
    public class TextAnim : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var player = animator.GetComponent<Player>();
            player.IsInBattle = true;
            Debug.Log("进入战斗！！！！");
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var player = animator.GetComponent<Player>();
            player.IsInBattle = false;
            Debug.Log("结束了战斗！！！！");
        }
    }
}