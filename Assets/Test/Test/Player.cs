using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using FSM;

namespace Test
{
    public struct TestMsg
    {

    }

    public class Player : MonoBehaviour
    {
        public int face = 1;

        public float speed = 1.0f;

        public bool IsInBattle = false;

        [SerializeField]
        private float m_WaitEndTime = 0.0f;
        public float WaitEndTime { get => m_WaitEndTime; set => m_WaitEndTime = value; }

        private StateMachine<Player> m_StateMachine = null;

        public TimelineAsset a;
        public PlayableGraph m_timeLinePlayer;
        public Playable m_timeline;

        public PlayableGraph m_AnimationClipPlayer;
        public AnimationClip idleAnimation;

        private void Awake()
        {


            m_StateMachine = new StateMachine<Player>(this);
            m_StateMachine.RegisterState(new PlayerIdleState());
            m_StateMachine.RegisterState(new PlayerTextState());
            m_StateMachine.ChangeState<PlayerIdleState>();

            m_timeLinePlayer = PlayableGraph.Create("TimeLine");


            //m_timeline = a.CreatePlayable(m_timeLinePlayer, gameObject);

            //for (var index = 0; index < m_timeLinePlayer.GetOutputCount(); ++index)
            //{
            //    var output = m_timeLinePlayer.GetOutput(index);
            //    if (output.GetPlayableOutputType() == typeof(AnimationPlayableOutput))
            //    {
            //        ((AnimationPlayableOutput)output).SetTarget(GetComponent<Animator>());
            //    }
            //}

            //m_timeLinePlayer.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            //m_timeLinePlayer.Play();

            m_AnimationClipPlayer = PlayableGraph.Create("Animation");
            var animOutput = AnimationPlayableOutput.Create(m_AnimationClipPlayer, "Idle", GetComponent<Animator>());
            animOutput.SetSourcePlayable(AnimationClipPlayable.Create(m_AnimationClipPlayer, idleAnimation));
            m_AnimationClipPlayer.Play();
        }

        private void OnDestroy()
        {
            m_timeLinePlayer.Destroy();
            m_AnimationClipPlayer.Destroy();
        }

        private void Update()
        {
            m_StateMachine.TickStateMachine();
            //m_timeLinePlayer.Evaluate();
            //m_AnimationClipPlayer.Evaluate();
        }

        public void SetAnimatorBool(string tag, bool isTag)
        {
        }

        public void OnInput()
        {
            m_StateMachine.SendMessage(new TestMsg());
        }
    }
}