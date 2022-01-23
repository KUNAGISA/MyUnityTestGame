using FSM3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestFSM3
{
    [CreateAssetMenu(fileName = "MoveState", menuName = "AI State/Move State")]
    public class AIMoveState : AIBaseState
    {
        [SerializeField]
        private float m_MoveArea = 5.0f;

        protected override void OnTickState(AIEntity entity, IStateMachine<AIEntity, AIState> stateMachine)
        {
            base.OnTickState(entity, stateMachine);

            var movex = entity.face * entity.speeds * Time.deltaTime + entity.transform.position.x;
            entity.transform.position = new Vector3(movex, entity.transform.position.y, entity.transform.position.z);

            if (Mathf.Abs(entity.transform.position.x) >= m_MoveArea)
            {
                entity.face = -entity.face;
                stateMachine.ChangeState(AIState.Idle);
            }
        }
    }
}
