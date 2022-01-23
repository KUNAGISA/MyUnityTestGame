using Framework;
using UnityEngine;

namespace Game
{
    public class BaseMonoController : MonoBehaviour, IController
    {
        private bool m_IsEnabledTick = false;

        /// <summary>
        /// 是否开启帧定时器
        /// </summary>
        public bool IsEnabledTick 
        { 
            get => m_IsEnabledTick; 
            protected set
            {
                if (m_IsEnabledTick != value)
                {
                    m_IsEnabledTick = value;
                    var worldSystem = this.GetSystem<System.IWorldSystem>();
                    if (m_IsEnabledTick)
                    {
                        worldSystem.onFrameTick += Tick;
                    }
                    else
                    {
                        worldSystem.onFrameTick -= Tick;
                    }
                }
            }
        }

        private bool m_IsEnabledFixedTick = false;

        /// <summary>
        /// 是否开启物理定时器
        /// </summary>
        public bool IsEnabledFixedTick 
        {
            get => m_IsEnabledFixedTick;
            protected set
            {
                if (m_IsEnabledFixedTick != value)
                {
                    m_IsEnabledFixedTick = value;
                    var worldSystem = this.GetSystem<System.IWorldSystem>();
                    if (m_IsEnabledFixedTick)
                    {
                        worldSystem.onFixedTick += FixedTick;
                    }
                    else
                    {
                        worldSystem.onFixedTick -= FixedTick;
                    }
                }
            }
        }

        /// <summary>
        /// 开启帧定时器后每帧回调
        /// </summary>
        /// <param name="dt"></param>
        protected virtual void Tick(float dt) { }

        protected virtual void FixedTick(float dt) { }

        /// <summary>
        /// 销毁时回调
        /// </summary>
        protected virtual void OnDestroy()
        {
            IsEnabledFixedTick = false;
            IsEnabledTick = false;
        }

        IArchitecture IBelongArchiecture.GetArchitecture()
        {
            return Game2D.Instance;
        }
    }
}
