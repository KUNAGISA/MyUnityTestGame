using Game.Manager.View;
using System;
using UnityEngine;

namespace Game
{
    public interface IView
    {
        public ViewDefine.ViewName ViewName { get; }

        void SetViewName(ViewDefine.ViewName viewName);

        void SetManager(IViewManager manager);

        void SetParent(Transform transform);

        void Destroy();
    }

    [DisallowMultipleComponent]
    public abstract class BaseView : AbstractsController, IView
    {
        private WeakReference<IViewManager> m_ViewManager = new WeakReference<IViewManager>(null);

        private ViewDefine.ViewName m_ViewName;
        ViewDefine.ViewName IView.ViewName => m_ViewName;

        void IView.Destroy()
        {
            Destroy(gameObject);
        }

        void IView.SetManager(IViewManager manager)
        {
            m_ViewManager.SetTarget(manager);
        }

        protected void PopSelf()
        {
            if (m_ViewManager.TryGetTarget(out var manager))
            {
                manager.Pop(this);
            }
        }

        void IView.SetParent(Transform transform)
        {
            gameObject.transform.SetParent(transform, true);
        }

        void IView.SetViewName(ViewDefine.ViewName viewName)
        {
            m_ViewName = viewName;
        }
    }
}