using System;
using Framework;
using UnityEngine;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 时间系统
    /// </summary>
    public interface ITimeSystem : ISystem
    {
        /// <summary>
        /// 当前时间更变
        /// </summary>
        event Action OnTimeChange;

        /// <summary>
        /// 当前时间
        /// </summary>
        float CurrTime { get; }

        /// <summary>
        /// 延迟调用
        /// </summary>
        /// <param name="interval">延迟时间</param>
        /// <param name="onDelayTask">回调</param>
        /// <returns>定时器</returns>
        ITimer AddDelayTask(float interval, Action<float> onDelayTask);

        /// <summary>
        /// 定时调用
        /// </summary>
        /// <param name="interval">间隔</param>
        /// <param name="onTickTask">回调</param>
        /// <returns>定时器</returns>
        ITimer AddTickTask(Action<float> onTickTask, float interval = 0.0f);
    }

    public class TimeSystem : BaseSystem, ITimeSystem, ICanGetSystem
    {
        enum TimerStatus
        {
            Running, Pause, Killed
        }

        class Timer : ITimer
        {
            public readonly float interval;
            public readonly bool loop;

            public float checkTime;
            public TimerStatus status = TimerStatus.Running;
            public Action<float> onTick;
            
            public Timer(float interval, bool loop, Action<float> onTick)
            {
                this.interval = interval;
                this.onTick = onTick;
                this.loop = loop;
            }

            public bool IsInvalid => status == TimerStatus.Killed;

            public void Pause()
            {
                if (!IsInvalid)
                {
                    status = TimerStatus.Pause;
                }
            }

            public void Resume()
            {
                if (!IsInvalid)
                {
                    status = TimerStatus.Running;
                }
            }

            public void Kill()
            {
                status = TimerStatus.Killed;
                onTick = null;
            }
        }

        protected override void OnInitSystem()
        {
            var worldSystem = this.GetSystem<IWorldSystem>();

            ///时间跟定时器分开处理，更新时间需要更优先处理
            worldSystem.onPriorFrameTick += TickTime;
            worldSystem.onFrameTick += TickTimerList;
        }

        public float CurrTime { get; private set; } = 0.0f;

        private LinkedList<Timer> m_TimerList = new LinkedList<Timer>();

        public event Action OnTimeChange;

        private void TickTime(float delta)
        {
            CurrTime += delta;
            OnTimeChange?.Invoke();
        }

        private void TickTimerList(float _)
        {
            if (m_TimerList.Count <= 0)
            {
                return;
            }

            var currTime = CurrTime;
            var itor = m_TimerList.Last;
            while (itor != null)
            {
                var timer = itor.Value;
                if (timer.status == TimerStatus.Running && timer.checkTime <= currTime)
                {
                    var delta = timer.interval + CurrTime - timer.checkTime;
                    timer.onTick(delta);
                    timer.checkTime = currTime + timer.interval;

                    if (!timer.loop)
                    {
                        timer.Kill();
                    }
                }

                itor = itor.Previous;
                if (timer.status == TimerStatus.Killed)
                {
                    m_TimerList.Remove(itor.Next);
                }
            }
        }

        public ITimer AddDelayTask(float interval, Action<float> onDelayCallback) => CreateTimerTask(interval, false, onDelayCallback);

        public ITimer AddTickTask(Action<float> onTickTask, float interval = 0.0f) => CreateTimerTask(interval, true, onTickTask);

        private ITimer CreateTimerTask(float interval, bool loop, Action<float> onTick)
        {
            var timer = new Timer(interval, loop, onTick);
            timer.checkTime = CurrTime + interval;
            m_TimerList.AddLast(timer);
            return timer;
        }
    }
}