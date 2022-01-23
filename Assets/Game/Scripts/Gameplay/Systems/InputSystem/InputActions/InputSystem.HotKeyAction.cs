using UnityEngine.InputSystem;
using Framework;

namespace Game.System
{
    public partial class  InputSystem
    {
        private class HotKeyAction : BaseInputAction, CustomInputAction.IHotKeyActions
        {
            public void OnPause(InputAction.CallbackContext context)
            {
                if (!context.canceled)
                {
                    return;
                }

                var viewSystem = this.GetSystem<IViewSystem>();
                if (viewSystem.IsShowView(ViewDefine.ViewName.PauseView))
                {
                    viewSystem.Pop(ViewDefine.ViewName.PauseView);
                }
                else
                {
                    viewSystem.Push(ViewDefine.ViewName.PauseView);
                }
            }
        }
    }
}
