using Framework;
using Game.System;
using System;

namespace Game
{
    /// <summary>
    /// 跟随游戏世界帧更新
    /// </summary>
    public interface ICanFollowWorldFrameTick : ICanGetSystem
    {
    }

    /// <summary>
    /// 跟随游戏世界优先帧更新
    /// </summary>
    public interface ICanFollowWorldPriorFrameTick : ICanGetSystem
    { 
    }

    /// <summary>
    /// 跟随游戏世界后帧更新
    /// </summary>
    public interface ICanFollowWorldLateFrameTick : ICanGetSystem
    {
    }

    /// <summary>
    /// 跟随游戏世界物理帧更新
    /// </summary>
    public interface ICanFollowWorldFixedTick : ICanGetSystem
    {
    }

    public static class FollowWorldTickExtension
    {
        /// <summary>
        /// 设置世界帧更新
        /// </summary>
        /// <param name="self"></param>
        /// <param name="onWorldFrameTick">世界帧更新回调</param>
        /// <param name="IsEnable">是否生效</param>
        public static void SetWorldFrameTick(this ICanFollowWorldFrameTick self, Action<float> onWorldFrameTick, bool IsEnable = true)
        {
            if (IsEnable)
            {
                self.GetSystem<IWorldSystem>().onFrameTick += onWorldFrameTick;
            }
            else
            {
                self.GetSystem<IWorldSystem>().onFrameTick -= onWorldFrameTick;
            }
        }

        /// <summary>
        /// 设置世界优先帧更新
        /// </summary>
        /// <param name="self"></param>
        /// <param name="onWorldPriorFrameTick">优先帧更新回调</param>
        /// <param name="IsEnable">是否生效</param>
        public static void SetWorldPriorFrameTick(this ICanFollowWorldPriorFrameTick self, Action<float> onWorldPriorFrameTick, bool IsEnable = true)
        {
            if (IsEnable)
            {
                self.GetSystem<IWorldSystem>().onPriorFrameTick += onWorldPriorFrameTick;
            }
            else
            {
                self.GetSystem<IWorldSystem>().onPriorFrameTick -= onWorldPriorFrameTick;
            }
        }

        /// <summary>
        /// 设置世界后帧更新
        /// </summary>
        /// <param name="self"></param>
        /// <param name="onWorldLateFrameTick">后帧更新回调</param>
        /// <param name="IsEnable">是否生效</param>
        public static void SetWorldLateFrameTick(this ICanFollowWorldLateFrameTick self, Action<float> onWorldLateFrameTick, bool IsEnable = true)
        {
            if (IsEnable)
            {
                self.GetSystem<IWorldSystem>().onLateFrameTick += onWorldLateFrameTick;
            }
            else
            {
                self.GetSystem<IWorldSystem>().onLateFrameTick -= onWorldLateFrameTick;
            }
        }

        /// <summary>
        /// 设置世界物理帧更新
        /// </summary>
        /// <param name="self"></param>
        /// <param name="onWorldFrameTick">物理帧更新回调</param>
        /// <param name="IsEnable">是否生效</param>
        public static void SetWorldFixedFrameTick(this ICanFollowWorldFixedTick self, Action<float> onWorldFrameTick, bool IsEnable = true)
        {
            if (IsEnable)
            {
                self.GetSystem<IWorldSystem>().onFrameTick += onWorldFrameTick;
            }
            else
            {
                self.GetSystem<IWorldSystem>().onFrameTick -= onWorldFrameTick;
            }
        }
    }
}