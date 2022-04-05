using DG.Tweening;
using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Test
{
    public class EnemyGoRoundState : BasePlayerState
    {
        private PlayableGraph m_playableGraph;
        private Animator m_animator;
        private AnimationClip m_WalkClip;
        private AnimationClip m_IdelClip;


        public EnemyGoRoundState(EnemyPlayer player) : base(player)
        {
            m_WalkClip = player.m_WalkClip;
            m_IdelClip = player.m_IdleClip;
            m_animator = player.m_Animator;
            m_playableGraph = PlayableGraph.Create("EnemyWalkAnimation");
            m_playableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);


            m_MoveSceond = kTurnAroundSeconds - kStayIdelSeconds;
            if (m_MoveSceond < 0)
            {
                Debug.LogError("错误的配置 m_MoveSceond：" + m_MoveSceond);
            }

        }
        protected override void OnEnterState()
        {
            base.OnEnterState();
            m_originalPos = player.transform.position;
            m_TargetMoveDis = new Vector3(player.transform.position.x + goAroundDistance, player.transform.position.y, 0);

            time = 0;
            m_curDir = 0;
            m_isStop = true;
            if (m_isStop)
            {
                playAnima(m_IdelClip);
            }
            else
            {
                playAnima(m_WalkClip);
            }

        }



        private void playAnima(AnimationClip clip)
        {
            PlayableUtility.PlayAnima(m_playableGraph, clip.name, clip, m_animator);
        }

        /// <summary>
        /// 停下來的时间
        /// </summary>
        private const int kStayIdelSeconds = 1;
        /// <summary>
        /// 巡逻的时间
        /// </summary>
        private const int kTurnAroundSeconds = 3;
        /// <summary>
        /// 巡逻的距离
        /// </summary>
        private const int goAroundDistance = 2;
        /// <summary>
        /// 每一趟的移动速度
        /// </summary>
        private readonly int m_MoveSceond;
        private float time = 0;
        private readonly Vector3 m_rightDir = new Vector3(1, 1, 1);
        private readonly Vector3 m_leftDir = new Vector3(-1, 1, 1);
        private Vector3 m_TargetMoveDis;
        private Vector3 m_originalPos;
        /// <summary>
        /// 当前方向  0 右 1 左 
        /// </summary>
        private int m_curDir = 0;

        private bool m_isStop = false;
        protected override ITransition<IPlayerState> OnTickState()
        {
            m_playableGraph.Evaluate(Time.deltaTime);

            if (m_curDir == 1)
            {
                time -= Time.deltaTime;
                if (time < 0)
                {
                    if (time < -kStayIdelSeconds)
                    {
                        m_curDir = 0;
                        m_isStop = false;
                        m_TargetMoveDis = player.transform.position + new Vector3(goAroundDistance, 0, 0);
                        playAnima(m_WalkClip);

                    }
                    else
                    {
                        if (!m_isStop)
                        {
                            m_isStop = true;
                            playAnima(m_IdelClip);
                        }

                    }
                }

            }
            else
            {
                time += Time.deltaTime;
                if (time > kTurnAroundSeconds)
                {
                    if (time > kTurnAroundSeconds + kStayIdelSeconds)
                    {
                        m_curDir = 1;
                        m_isStop = false;
                        m_TargetMoveDis = player.transform.position - new Vector3(goAroundDistance, 0, 0);
                        playAnima(m_WalkClip);
                    }
                    else
                    {
                        if (!m_isStop)
                        {
                            m_isStop = true;
                            playAnima(m_IdelClip);
                        }
                    }
                }
            }
            turnAroundFunc();


            return base.OnTickState();
        }


        private void turnAroundFunc()
        {
            if (m_isStop)
            {
                return;
            }
            if (m_curDir == 1)
            {
                //右边
                player.transform.localScale = m_rightDir;
                player.transform.Translate(Vector3.right * 0.01f, Space.World);
            }
            else
            {
                //左边
                player.transform.localScale = m_leftDir;
                player.transform.Translate(Vector3.left * 0.01f, Space.World);
            }
        }

        private void onAroundFinish()
        {
            playAnima(m_IdelClip);
        }

        protected override void OnExitState()
        {
            m_playableGraph.Stop();
            m_playableGraph.GetRootPlayable(0).SetTime(0);
            base.OnExitState();
        }
    }



}
