using UnityEngine;
using Framework;
using System.Collections.Generic;
using System;

namespace Game.System
{
    /// <summary>
    /// 游戏定时器系统
    /// </summary>
    public interface ITimerSystem : ISystem
    {

        public interface ITimer
        { 
            public bool IsInvalid { get; }

            public void Kill();

            public void Pause();

            public void Resume();
        }

        public float CurrTime { get; }

        /// <summary>
        /// 延迟调用
        /// </summary>
        /// <param name="interval">延迟时间</param>
        /// <param name="onDelayCallback">回调</param>
        /// <returns>定时器</returns>
        public ITimer AddDelayTask(float interval, Action<float> onDelayCallback);

        /// <summary>
        /// 定时回调
        /// </summary>
        /// <param name="interval">间隔</param>
        /// <param name="onDelayCallback">回调</param>
        /// <returns>定时器</returns>
        public ITimer AddTask(float interval, Action<float> onDelayCallback);

        public bool IsPause { get; }

        /// <summary>
        /// 暂停定时器
        /// </summary>
        /// <param name="key"></param>
        public void Pause(string key);

        /// <summary>
        /// 恢复定时器
        /// </summary>
        /// <param name="key"></param>
        public void Resume(string key);
    }

    public class TimerSystem : AbstractSystem, ITimerSystem
    {
        enum TimerStatus
        {
            Running, Pause, Killed
        }

        class Timer : ITimerSystem.ITimer
        {
            public readonly float interval;

            public TimerStatus status = TimerStatus.Running;
            public Action<float> onTimerCallback;
            public float checkTime;

            public Timer(float interval, Action<float> onTimerCallback)
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

            public void Kill()
            {
                status = TimerStatus.Killed;
                onTimerCallback = null;
            }
        }

        protected override void OnInitSystem()
        {
            var gameObject = new GameObject("TimeSystem");
            UnityEngine.Object.DontDestroyOnLoad(gameObject);

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
                    timer.onTimerCallback?.Invoke(delta);
                    timer.Kill();
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
                    timer.onTimerCallback?.Invoke(delta);
                }
                if (timer.status == TimerStatus.Killed)
                {
                    m_Timers.RemoveAt(index);
                }
            }
        }

        public ITimerSystem.ITimer AddDelayTask(float interval, Action<float> onDelayCallback)
        {
            var timer = new Timer(interval, onDelayCallback);
            timer.checkTime = CurrTime + interval;
            m_DelayTimers.AddLast(timer);
            return timer;
        }

        public ITimerSystem.ITimer AddTask(float interval, Action<float> onDelayCallback)
        {
            var timer = new Timer(interval, onDelayCallback);
            timer.checkTime = CurrTime + interval;
            m_Timers.Add(timer);
            return timer;
        }

        private HashSet<string> m_PauseKeys = new HashSet<string>();
        public bool IsPause => m_PauseKeys.Count > 0;

        public void Pause(string key)
        {
            if (m_PauseKeys.Add(key))
            {
                Debug.Log(string.Format("TimerSystem pause same key \"{0}\"", key));
            }
        }

        public void Resume(string key)
        {
            if (m_PauseKeys.Remove(key))
            {
                Debug.Log(string.Format("TimerSystem resume non-existent key \"{0}\"", key));
            }
        }
    }
}