using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Test
{
    class BoxGrzmos : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 3);
        }
    }

    class BoxBehaviour : PlayableBehaviour
    {
        public Transform owner;
        public GameObject go;

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            GameObject.DestroyImmediate(go);
            base.OnBehaviourPause(playable, info);
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);

            go = new GameObject("BoxGrzmos");
            go.transform.SetParent(owner, true);
            go.transform.localPosition = Vector3.zero;
            go.AddComponent<BoxGrzmos>();
            SetHideFlagsRecursive(go);
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Debug.Log(info.weight);
            base.ProcessFrame(playable, info, playerData);
        }

        static void SetHideFlagsRecursive(GameObject gameObject)
        {
            gameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;

            if (!Application.isPlaying)
                gameObject.hideFlags |= HideFlags.HideInInspector;

            foreach (Transform child in gameObject.transform)
            {
                SetHideFlagsRecursive(child.gameObject);
            }
        }
    }

    [DisplayName("啊啊啊")]
    class BoxAsset : PlayableAsset
    {
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var box = ScriptPlayable<BoxBehaviour>.Create(graph);
            box.GetBehaviour().owner = owner.transform;
            return box;
        }
    }
}