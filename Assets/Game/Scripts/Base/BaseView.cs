using Game.Manager.View;
using System;
using UnityEngine;

namespace Game {
    [DisallowMultipleComponent]
    public abstract class BaseView : BaseMonoController, IView {
        private readonly WeakReference<IViewManager> m_ViewManager = new WeakReference<IViewManager>(null);

        private ViewDefine.ViewName m_ViewName;
        ViewDefine.ViewName IView.ViewName => m_ViewName;

        void IView.Destroy() {
            Destroy(gameObject);
        }

        void IView.SetManager(IViewManager manager) {
            m_ViewManager.SetTarget(manager);
        }

        void IView.SetParent(Transform transform) {
            this.transform.SetParent(transform, false);
        }

        void IView.SetViewName(ViewDefine.ViewName viewName) {
            m_ViewName = viewName;
        }

        protected void PopSelf() {
            if (m_ViewManager.TryGetTarget(out var manager)) {
                manager.Pop(this);
            }
        }

        void IAnalysisUI.InitAnalysis() {

        }
    }
}