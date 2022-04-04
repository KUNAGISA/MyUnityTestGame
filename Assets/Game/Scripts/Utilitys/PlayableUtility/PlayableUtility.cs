using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public static class PlayableUtility
{

    public static void PlayAnima(PlayableGraph playableGraph, string name, AnimationClip clip, Animator animator)
    {
        var playableOutput = AnimationPlayableOutput.Create(playableGraph, name, animator);
        var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
        playableOutput.SetSourcePlayable(clipPlayable);
        playableGraph.Play();
    }
}
