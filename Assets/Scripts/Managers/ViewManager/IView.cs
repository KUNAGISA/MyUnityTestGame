using UnityEngine;

namespace Game.Manager.View
{
    /// <summary>
    /// 受ViewManager管理的界面接口
    /// </summary>
    public interface IView
    {
        public ViewDefine.ViewName ViewName { get; }

        void SetViewName(ViewDefine.ViewName viewName);

        void SetManager(IViewManager manager);

        void SetParent(Transform transform);

        /// <summary>
        /// ViewManager调用，外部一般别调
        /// </summary>
        void Destroy();
    }
}