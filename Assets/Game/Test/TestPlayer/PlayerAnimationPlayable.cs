using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Game.Test
{
    [CreateAssetMenu(menuName = "测试/PlayableAsset/PlayerAnimation")]
    public class PlayerAnimationPlayable : PlayableAsset
    {
        public AnimationClip clip;

        public override double duration
        {
            get
            {
                if (clip == null || clip.empty)
                    return base.duration;

                double length = clip.length;
                if (clip.frameRate > 0)
                {
                    double frames = Mathf.Round(clip.length * clip.frameRate);
                    length = frames / clip.frameRate;
                }
                return length;
            }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var animator = owner.GetComponent<Animator>();
            if (animator != null && clip != null)
            {
                AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(graph, clip);
                AnimationPlayableOutput output = AnimationPlayableOutput.Create(graph, "AnimationClip", animator);
                output.SetSourcePlayable(animationClipPlayable);
                return animationClipPlayable;
            }
            return Playable.Null;
        }
    }
}
