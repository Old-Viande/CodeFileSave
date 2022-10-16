using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AnimTargetPlayHitOrDeadClip : PlayableAsset, ITimelineClipAsset
{
    public AnimTargetPlayHitOrDeadBehaviour template = new AnimTargetPlayHitOrDeadBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AnimTargetPlayHitOrDeadBehaviour>.Create (graph, template);
        AnimTargetPlayHitOrDeadBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
