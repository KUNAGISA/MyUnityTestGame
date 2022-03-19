using UnityEngine;

namespace Game.Test
{
    public class Monster : MonoBehaviour
    {
        public Vector2 from = Vector2.zero;
        public Vector2 to = Vector2.zero;
        public float time = 5.0f;

        private float totalTime = 0.0f;
        private int face = 1;

        private void Awake()
        {
            transform.position = from;
        }

        private void FixedUpdate()
        {
            totalTime += face * Time.fixedDeltaTime;
            if (totalTime >= time)
            {
                totalTime = 2 * time - totalTime;
                face = -1;
            }
            else if (totalTime <= 0.0f)
            {
                totalTime = -totalTime;
                face = 1;
            }

            var position = Vector2.Lerp(from, to, totalTime / time);
            transform.position = position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f);
            Gizmos.DrawLine(from, to);
        }
    }
}
