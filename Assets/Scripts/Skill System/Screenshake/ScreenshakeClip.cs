using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ScreenshakeClip : PlayableAsset, ITimelineClipAsset
{
    public ScreenshakeBehaviour template = new ScreenshakeBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ScreenshakeBehaviour>.Create (graph, template);
        ScreenshakeBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
