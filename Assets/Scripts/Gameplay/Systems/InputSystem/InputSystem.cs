using Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.System
{
    public interface IInputSystem : ISystem
    {
        void SetPlayerCallbacks(CustomInputAction.IPlayerActions input);
    }

    public class InputSystem : BaseSystem, IInputSystem, ICanGetSystem, CustomInputAction.IHotKeyActions
    {
        private readonly CustomInputAction m_InputActions = new CustomInputAction();

        protected override void OnInitSystem()
        {
            m_InputActions.Enable();

            var worldSystem = this.GetSystem<IWorldSystem>();
            worldSystem.onRunStatusChange += OnWorldRunChange;
            OnWorldRunChange(worldSystem.IsPause);

            m_InputActions.HotKey.SetCallbacks(this);
        }

        public void SetPlayerCallbacks(CustomInputAction.IPlayerActions input)
        {
            m_InputActions.Player.SetCallbacks(input);
        }

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

        private void OnWorldRunChange(bool isPause)
        {
            if (isPause)
            {
                m_InputActions.Player.Disable();
            }
            else
            {
                m_InputActions.Player.Enable();
            }
        }
    }
}
