using System;
using UnityEngine;
using Framework;
using Game.Manager.View;

namespace Game.System
{
    public interface IViewSystem : ISystem
    {
        /// <summary>
        /// 绑定场景对象
        /// </summary>
        /// <param name="viewRoot"></param>
        void BindViewRoot(IViewManager viewRoot);

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="viewName">界面名字</param>
        void Pop(ViewDefine.ViewName viewName);

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="viewName">界面名字</param>
        void Push(ViewDefine.ViewName viewName);

        /// <summary>
        /// 界面是否在显示
        /// </summary>
        /// <param name="viewName">界面名字</param>
        /// <returns></returns>
        bool IsShowView(ViewDefine.ViewName viewName);
    }

    public class ViewSystem : BaseSystem, IViewSystem
    {
        private WeakReference<IViewManager> m_ViewManager = new WeakReference<IViewManager>(null);

        public IViewManager ViewRoot
        {
            get
            {
                if (!m_ViewManager.TryGetTarget(out IViewManager viewRoot))
                {
                    Debug.LogWarningFormat("View System Not Bind View Root");
                }
                return viewRoot;
            }
        }

        public void BindViewRoot(IViewManager viewRoot) => m_ViewManager.SetTarget(viewRoot);

        public bool IsShowView(ViewDefine.ViewName viewName)
        {
            var root = ViewRoot;
            return root != null ? root.IsShowView(viewName) : false ;
        }

        public void Pop(ViewDefine.ViewName viewName)
        {
            ViewRoot?.Pop(viewName);
        }

        public void Push(ViewDefine.ViewName viewName)
        {
            ViewRoot?.Push(viewName);
        }

        protected override void OnInitSystem()
        {
        }
    }
}
