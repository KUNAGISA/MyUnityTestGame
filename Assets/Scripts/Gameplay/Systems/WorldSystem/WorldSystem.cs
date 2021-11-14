using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 游戏世界系统
    /// </summary>
    public interface IWorldSystem : ISystem
    {
        /// <summary>
        /// 世界物理帧定时器
        /// </summary>
        event Action<float> onFixedTick;

        /// <summary>
        /// 世界帧更新
        /// </summary>
        event Action<float> onFrameTick;

        /// <summary>
        /// 优先执行的帧更新
        /// </summary>
        event Action<float> onPriorFrameTick;

        /// <summary>
        /// 是否暂停
        /// </summary>
        bool IsPause { get; }

        /// <summary>
        /// 暂停游戏
        /// </summary>
        /// <param name="key">暂停标记，与恢复对应一组</param>
        void Pause(string key);

        /// <summary>
        /// 恢复游戏
        /// </summary>
        /// <param name="key">恢复标记，与暂停对应一组</param>
        void Resume(string key);
    }

    public class WorldSystem : AbstractSystem, IWorldSystem
    {
        public event Action<float> onFixedTick;
        public event Action<float> onFrameTick;
        public event Action<float> onPriorFrameTick;

        private WorldComponent m_WorldObj;

        public bool IsPause => m_PauseSet.Count > 0;

        protected override void OnInitSystem()
        {
            var worldObj = new GameObject("WorldSystem");
            UnityEngine.Object.DontDestroyOnLoad(worldObj);

            m_WorldObj = worldObj.AddComponent<WorldComponent>();
            m_WorldObj.onFrameTick += Tick;
            m_WorldObj.onFixedTick += FixedTick;
        }

        private void Tick()
        {
            if (!IsPause)
            {
                var delta = Time.deltaTime;
                onPriorFrameTick?.Invoke(delta);
                onFrameTick?.Invoke(delta);
            }
        }

        private void FixedTick()
        {
            if (!IsPause)
            {
                var delta = Time.fixedDeltaTime;
                onFixedTick?.Invoke(delta);
            }
        }

        private HashSet<string> m_PauseSet = new HashSet<string>();

        public void Pause(string key)
        {
            if (!m_PauseSet.Add(key))
            {
                Debug.LogWarningFormat("World Pause Same Key '{0}'", key);
            }
            else
            {
                ResetRunStatus();
            }
        }

        public void Resume(string key)
        {
            if (!m_PauseSet.Remove(key))
            {
                Debug.LogWarningFormat("World Resume Non-exist Key '{0}'", key);
            }
            else
            {
                ResetRunStatus();
            }
        }

        /// <summary>
        /// 重置运行状态
        /// </summary>
        private void ResetRunStatus()
        {
            var isPause = IsPause;
            Time.timeScale = isPause ? 0 : 1;
        }
    }
}
