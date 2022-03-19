using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Test
{
    class AttackDebugGizoms : MonoBehaviour
    {
        public event Action onDrawGizmos;

        private void OnDrawGizmos()
        {
            onDrawGizmos?.Invoke();
        }
    }

    public interface IAttackClipAsset : IPlayableAsset
    { 
    }

    [Serializable]
    public abstract class BaseAttackClip<TBehaviour> : PlayableAsset, IAttackClipAsset where TBehaviour : BaseAttackBehaviour, new()
    {
        static Type[] types = { typeof(GameObject) };

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            object[] param = { owner };
            var behaviour = typeof(TBehaviour).GetConstructor(types).Invoke(param) as TBehaviour;
            OnInitBehaviour(behaviour);

            return ScriptPlayable<TBehaviour>.Create(graph, behaviour);
        }

        protected virtual void OnInitBehaviour(TBehaviour behaviour) { }
    }

    public abstract class BaseAttackBehaviour : PlayableBehaviour
    {
        private AttackDebugGizoms m_AttackDebugGizoms;
        public readonly GameObject owner;

        public BaseAttackBehaviour() { }

        public BaseAttackBehaviour(GameObject owner)
        {
            this.owner = owner;
            m_AttackDebugGizoms = owner.GetOrAddComponent<AttackDebugGizoms>();
            m_AttackDebugGizoms.hideFlags = HideFlags.DontSave;
        }

        protected abstract void OnDrawGizoms();

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
            m_AttackDebugGizoms.onDrawGizmos += OnDrawGizoms;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (playable.GetPlayState() == PlayState.Paused)
            {
                m_AttackDebugGizoms.onDrawGizmos -= OnDrawGizoms;
            }
            base.OnBehaviourPause(playable, info);
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            m_AttackDebugGizoms.onDrawGizmos -= OnDrawGizoms;
            base.OnPlayableDestroy(playable);
        }
    }
}