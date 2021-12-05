using Framework;

namespace Game.System
{
    public partial class InputSystem
    {
        private interface IInputAction : ICanGetSystem, ICanGetModel, ICanSendCommand, ICanSendQuery, ICanSendEvent
        {
            public void SetInputSystem(IInputSystem inputSystem);

            public void InitAction();
        }

        private class BaseInputAction : IInputAction
        {
            private IInputSystem m_InputSystem;

            IArchitecture IBelongArchiecture.GetArchitecture()
            {
                return m_InputSystem.GetArchitecture();
            }

            void IInputAction.SetInputSystem(IInputSystem inputSystem)
            {
                m_InputSystem = inputSystem;
            }

            void IInputAction.InitAction() => OnInitAction();

            protected virtual void OnInitAction() { }
        }
    }
}
