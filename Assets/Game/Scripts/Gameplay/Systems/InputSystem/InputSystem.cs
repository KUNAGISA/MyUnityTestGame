using Framework;
using UnityEngine.InputSystem;

namespace Game.System
{
    public interface IInputSystem : ISystem
    {
        void SetPlayerCallbacks(CustomInputAction.IPlayerActions input);
    }

    public partial class InputSystem : BaseSystem, IInputSystem
    {
        private readonly CustomInputAction m_InputActions = new CustomInputAction();

        protected override void OnInitSystem()
        {
            var worldSystem = this.GetSystem<IWorldSystem>();
            SetPlayerActionEnable(worldSystem.IsPause);

            this.RegisterEvent((in Event.WorldRunChangeEvent @event) => SetPlayerActionEnable(@event.isPause));

            m_InputActions.Enable();
            m_InputActions.HotKey.SetCallbacks(CreateInputAction<HotKeyAction>());
        }

        private TInputAction CreateInputAction<TInputAction>() where TInputAction : class, IInputAction, new()
        {
            var inputAction = new TInputAction();
            inputAction.SetInputSystem(this);
            inputAction.InitAction();
            return inputAction;
        }

        public void SetPlayerCallbacks(CustomInputAction.IPlayerActions input)
        {
            m_InputActions.Player.SetCallbacks(input);
        }

        private void SetPlayerActionEnable(bool IsEnabled)
        {
            if (IsEnabled)
            {
                m_InputActions.Player.Enable();
            }
            else
            {
                m_InputActions.Player.Disable();
            }
        }
    }
}
