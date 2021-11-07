using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Manager.View
{
    [RequireComponent(typeof(RectTransform))]
    public class StackViewPanel : BaseViewPanel
    {
        List<IView> m_ViewStack = new List<IView>();

        public override void Pop(IView view)
        {
            var viewIndex = m_ViewStack.FindLastIndex((v) => v.Equals(view));
            PopViewByIndex(viewIndex);
        }

        public override void Pop(ViewDefine.ViewName viewName)
        {
            var viewIndex = m_ViewStack.FindLastIndex((v) => v.ViewName == viewName);
            PopViewByIndex(viewIndex);
        }

        public override void Push(IView view)
        {
            view.SetParent(gameObject.transform);
        }

        private void PopViewByIndex(int viewIndex)
        {
            if (viewIndex < 0 || viewIndex >= m_ViewStack.Count)
            {
                return;
            }

            for (var index = m_ViewStack.Count - 1; index >= viewIndex; --index)
            {
                var view = m_ViewStack[index];
                m_ViewStack.RemoveAt(index);

                view.SetManager(null);
                view.Destroy();
            }
        }
    }
}