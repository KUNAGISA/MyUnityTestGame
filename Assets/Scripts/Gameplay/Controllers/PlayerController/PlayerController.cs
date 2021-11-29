using UnityEngine;
using Framework;
using UnityEngine.InputSystem;

namespace Game
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : BaseMonoController, CustomInputAction.IPlayerActions
    {
        private Vector2 m_MoveFace = Vector2.zero;

        [SerializeField]
        private float m_MoveSpeed = 10.0f;

        private void Start()
        {
            this.GetSystem<System.IInputSystem>()
                .SetPlayerCallbacks(this);
            IsEnabledFixedTick = true;
        }

        protected override void OnDestroy()
        {
            this.GetSystem<System.IInputSystem>()
                .SetPlayerCallbacks(null);
            base.OnDestroy();
        }

        protected override void FixedTick(float _)
        {
            var ridgidBody2d = GetComponent<Rigidbody2D>();
            ridgidBody2d.velocity = new Vector2(m_MoveFace.x * m_MoveSpeed, ridgidBody2d.velocity.y);
            Debug.Log(ridgidBody2d.velocity);
        }

        void CustomInputAction.IPlayerActions.OnMove(InputAction.CallbackContext context)
        {
            m_MoveFace = context.ReadValue<Vector2>();
        }

        void CustomInputAction.IPlayerActions.OnFire(InputAction.CallbackContext context)
        {
        }
    }
}