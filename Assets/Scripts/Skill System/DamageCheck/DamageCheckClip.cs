using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class DamageCheckClip : PlayableAsset, ITimelineClipAsset
{
    public DamageCheckBehaviour template = new DamageCheckBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DamageCheckBehaviour>.Create (graph, template);
        DamageCheckBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
