using UnityEngine;
using Framework;
using UnityEngine.InputSystem;

namespace Game
{
    [DisallowMultipleComponent]
    public class PlayerController : AbstractsController, CustomInputAction.IPlayerActions
    {
        private void Start()
        {
            this.GetSystem<System.IInputSystem>().SetPlayerCallbacks(this);

            var worldSystem = this.GetSystem<System.IWorldSystem>();
            worldSystem.onFixedTick += FixedTick;
        }

        private void OnDestroy()
        {
            this.GetSystem<System.IInputSystem>().SetPlayerCallbacks(null);
            var worldSystem = this.GetSystem<System.IWorldSystem>();
            worldSystem.onFixedTick -= FixedTick;
        }

        private void FixedTick(float _)
        {
            GetComponent<Rigidbody2D>().velocity = m_MoveFace;
        }

        private Vector2 m_MoveFace = Vector2.zero;

        void CustomInputAction.IPlayerActions.OnMove(InputAction.CallbackContext context)
        {
            m_MoveFace = context.ReadValue<Vector2>();
        }

        void CustomInputAction.IPlayerActions.OnFire(InputAction.CallbackContext context)
        {
        }
    }
}