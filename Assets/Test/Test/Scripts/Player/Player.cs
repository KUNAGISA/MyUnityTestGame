using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using FSM;
using UnityEngine.Audio;
using UnityEngine.VFX;

namespace Test
{
    public struct TestMsg
    {

    }

    public class Player : MonoBehaviour, IExposedPropertyTable, INotificationReceiver
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

        public AnimationPlayableAsset m_track;

        public PlayableGraph m_AnimationClipPlayer;
        public AnimationClip idleAnimation;

        private void Awake()
        {
            GetComponent<VisualEffect>().Simulate(0);

            m_StateMachine = new StateMachine<Player>(this);
            m_StateMachine.RegisterState(new PlayerIdleState());
            m_StateMachine.RegisterState(new PlayerTextState());
            m_StateMachine.ChangeState<PlayerIdleState>();

            var director = GetComponent<PlayableDirector>();
            director?.Play(a, DirectorWrapMode.Loop);
            
            //foreach (var track in a.GetOutputTracks())
            //{
            //    if (track.name == "Player")
            //    {
            //        director.SetGenericBinding(track, GetComponent<Animator>());
            //    }
            // }

            //m_timeLinePlayer = PlayableGraph.Create("TimeLine");
            //m_timeLinePlayer.SetResolver(this);

            //m_timeline = a.CreatePlayable(m_timeLinePlayer, gameObject);

            //var notificationOutput = m_timeLinePlayer.GetOutputByType<ScriptPlayableOutput>(0);
            //notificationOutput.AddNotificationReceiver(this);

            //for (var index = 0; index < m_timeLinePlayer.GetOutputCount(); ++index)
            //{
            //    var output = m_timeLinePlayer.GetOutput(index);
            //    if (output.GetPlayableOutputType() == typeof(AnimationPlayableOutput))
            //    {
            //        ((AnimationPlayableOutput)output).SetTarget(GetComponent<Animator>());
            //    }
            //    else if (output.GetPlayableOutputType() == typeof(AudioPlayableOutput))
            //    {
            //        ((AudioPlayableOutput)output).SetEvaluateOnSeek(true);
            //        ((AudioPlayableOutput)output).SetTarget(GetComponent<AudioSource>());
            //    }
            //}

            //m_timeLinePlayer.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            //m_timeLinePlayer.Play();

            //m_AnimationClipPlayer = PlayableGraph.Create("Animation");
            //var animOutput = AnimationPlayableOutput.Create(m_AnimationClipPlayer, "Idle", GetComponent<Animator>());
            //animOutput.SetSourcePlayable(AnimationClipPlayable.Create(m_AnimationClipPlayer, idleAnimation));
            //m_AnimationClipPlayer.Play();
        }

        private void OnDestroy()
        {
            //m_timeLinePlayer.Destroy();
            //m_AnimationClipPlayer.Destroy();
        }

        private void Update()
        {
            m_StateMachine.TickStateMachine();

            //var director = GetComponent<PlayableDirector>();
            //director.time = GetComponent<PlayableDirector>().time + Time.deltaTime;
            //GetComponent<PlayableDirector>().Evaluate();

            //if (director.extrapolationMode == DirectorWrapMode.Loop && director.time > director.duration)
            //{
            //    director.time %= director.duration;
            //}

            
            //m_timeLinePlayer.Evaluate(Time.deltaTime);
            //m_AnimationClipPlayer.Evaluate();
        }

        public void SetAnimatorBool(string tag, bool isTag)
        {
        }

        public void OnInput()
        {
            m_StateMachine.SendMessage(new TestMsg());
        }

        private Dictionary<PropertyName, Object> m_propertyObjs = new Dictionary<PropertyName, Object>();

        void IExposedPropertyTable.SetReferenceValue(PropertyName id, Object value)
        {
            m_propertyObjs[id] = value;
        }

        Object IExposedPropertyTable.GetReferenceValue(PropertyName id, out bool idValid)
        {
            idValid = m_propertyObjs.TryGetValue(id, out var obj);
            return obj;
        }

        void IExposedPropertyTable.ClearReferenceValue(PropertyName id)
        {
            m_propertyObjs.Remove(id);
        }

        void INotificationReceiver.OnNotify(Playable origin, INotification notification, object context)
        {
            Debug.Log(notification);
            Debug.Log(context);
        }
    }
}