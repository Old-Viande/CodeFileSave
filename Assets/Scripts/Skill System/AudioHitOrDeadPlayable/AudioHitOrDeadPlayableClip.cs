using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AudioHitOrDeadPlayableClip : PlayableAsset, ITimelineClipAsset
{
    public AudioHitOrDeadPlayableBehaviour template = new AudioHitOrDeadPlayableBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AudioHitOrDeadPlayableBehaviour>.Create (graph, template);
        AudioHitOrDeadPlayableBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
