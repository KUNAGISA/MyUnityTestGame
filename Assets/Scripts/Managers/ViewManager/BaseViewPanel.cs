using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Manager.View
{
    public abstract class BaseViewPanel : MonoBehaviour
    {
        public abstract void Push(IView view);

        public abstract void Pop(IView view);

        public abstract void Pop(ViewDefine.ViewName viewName);
    }
}