using Game.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IView
    {
        void SetManager(IViewManager manager);

        void Destroy();
    }

    [DisallowMultipleComponent]
    public abstract class BaseView : AbstractsController, IView
    {
        private WeakReference<IViewManager> m_ViewManager = new WeakReference<IViewManager>(null);

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
    }
}