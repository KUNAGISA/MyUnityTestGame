using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Test
{
    [DisplayName("矩形攻击盒")]
    public class BoxAttackClip : BaseAttackClip<BoxAttackBehaviour>
    {
        [Serializable]
        public struct BoxArea
        {
            public Vector2 size;
            public Vector2 offset;
        }

        public BoxArea area;
        public LayerMask layer;

        protected override void OnInitBehaviour(BoxAttackBehaviour behaviour)
        {
            behaviour.area = area;
            behaviour.layer = layer;
        }
    }

    public class BoxAttackBehaviour : BaseAttackBehaviour
    {
        public BoxAttackClip.BoxArea area;
        public LayerMask layer;

        public BoxAttackBehaviour()
        {

        }

        public BoxAttackBehaviour(GameObject owner) : base(owner)
        {

        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            if (!Application.isPlaying)
            {
                return;
            }

            var position = (Vector2)owner.transform.position;
            List<RaycastHit2D> hitResult = new List<RaycastHit2D>();

            var hitResults = Physics2D.BoxCastAll(position + area.offset, area.size, 0, Vector2.zero, 0, layer);
            foreach(var result in hitResults)
            {
                if (result.rigidbody.gameObject != owner)
                {
                    UnityEngine.Object.Destroy(result.rigidbody.gameObject);
                }
            }
        }

        protected override void OnDrawGizoms()
        {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
            Gizmos.DrawCube(owner.transform.position + (Vector3)area.offset, area.size);
        }
    }
}
