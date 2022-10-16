using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ParticlePlayableClip : PlayableAsset, ITimelineClipAsset
{
    public ParticlePlayableBehaviour template = new ParticlePlayableBehaviour ();
    public ExposedReference<ParticleSystem> particle;
    public ExposedReference<Transform> trans;
    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ParticlePlayableBehaviour>.Create (graph, template);
        ParticlePlayableBehaviour clone = playable.GetBehaviour ();
        clone.particle = particle.Resolve (graph.GetResolver ());
        clone.trans = trans.Resolve(graph.GetResolver());
        particle.exposedName= System.Guid.NewGuid().ToString();
        return playable;
    }
}
