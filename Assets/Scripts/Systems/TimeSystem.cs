using UnityEngine;
using Framework;
using System.Collections.Generic;

namespace Game.System
{
    public interface ITimeSystem : ISystem
    {

        public interface ITimer
        { 
            public bool IsInvalid { get; }

            public void Stop();

            public void Pause();

            public void Resume();
        }

        public delegate void ActionTimer(in float dt);

        public float CurrTime { get; }

        /// <summary>
        /// 延迟调用
        /// </summary>
        /// <param name="interval">延迟时间</param>
        /// <param name="onDelayCallback">回调</param>
        /// <returns>定时器</returns>
        public ITimer AddDelayTask(float interval, ActionTimer onDelayCallback);

        /// <summary>
        /// 定时回调
        /// </summary>
        /// <param name="interval">间隔</param>
        /// <param name="onDelayCallback">回调</param>
        /// <returns>定时器</returns>
        public ITimer AddTask(float interval, ActionTimer onDelayCallback);
    }

    public class TimeSystem : AbstractSystem, ITimeSystem
    {
        enum TimerStatus
        {
            Running, Pause, Killed
        }

        class Timer : ITimeSystem.ITimer
        {

            public TimerStatus status = TimerStatus.Running;
            public ITimeSystem.ActionTimer onTimerCallback;
            public readonly float interval;
            public float checkTime;

            public Timer(float interval, ITimeSystem.ActionTimer onTimerCallback)
            {
                this.interval = interval;
                this.onTimerCallback = onTimerCallback;
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

            public void Stop()
            {
                status = TimerStatus.Killed;
                onTimerCallback = null;
            }
        }

        protected override void OnInitSystem()
        {
            var gameObject = new GameObject("TimeSystem");
            Object.DontDestroyOnLoad(gameObject);

            gameObject.AddComponent<TimerComponent>()
                .OnUpdateCallback += OnUpdate;
        }

        private LinkedList<Timer> m_DelayTimers = new LinkedList<Timer>();
        private List<Timer> m_Timers = new List<Timer>();

        private float m_CurrTime = 0.0f;
        public float CurrTime => m_CurrTime;

        private void OnUpdate()
        {
            m_CurrTime += Time.deltaTime;
            OnUpdateTask();
            OnUpdateDelayTask();
        }

        private void OnUpdateDelayTask()
        {
            if (m_DelayTimers.Count <= 0)
            {
                return;
            }

            var currTime = m_CurrTime;
            var itor = m_DelayTimers.First;
            var next = itor.Next;

            while(itor != null)
            {
                var timer = itor.Value;
                if (timer.status == TimerStatus.Running && timer.checkTime <= currTime)
                {
                    var delta = timer.interval + (currTime - timer.checkTime);
                    timer.onTimerCallback?.Invoke(in delta);
                    timer.Stop();
                }
                if (timer.status == TimerStatus.Killed)
                {
                    m_DelayTimers.Remove(itor);
                }

                itor = next;
                next = itor?.Next;
            }
        }

        private void OnUpdateTask()
        {
            if (m_Timers.Count <= 0)
            {
                return;
            }

            var currTime = m_CurrTime;
            for(var index = m_Timers.Count - 1; index >= 0; --index)
            {
                var timer = m_Timers[index];
                if (timer.status == TimerStatus.Running && timer.checkTime <= currTime)
                {
                    var delta = timer.interval + (currTime - timer.checkTime);
                    timer.checkTime = currTime + timer.interval;
                    timer.onTimerCallback?.Invoke(in delta);
                }
                if (timer.status == TimerStatus.Killed)
                {
                    m_Timers.RemoveAt(index);
                }
            }
        }

        public ITimeSystem.ITimer AddDelayTask(float interval, ITimeSystem.ActionTimer onDelayCallback)
        {
            var timer = new Timer(interval, onDelayCallback);
            timer.checkTime = CurrTime + interval;
            m_DelayTimers.AddLast(timer);
            return timer;
        }

        public ITimeSystem.ITimer AddTask(float interval, ITimeSystem.ActionTimer onDelayCallback)
        {
            var timer = new Timer(interval, onDelayCallback);
            timer.checkTime = CurrTime + interval;
            m_Timers.Add(timer);
            return timer;
        }
    }
}