using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ParticlePlayableBehaviour : PlayableBehaviour
{
    public ParticleSystem particle;
    public Transform trans;
    private GameObject tempGo;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        particle.transform.transform.position = trans.position;
        particle.transform.transform.rotation = trans.rotation;
        tempGo=GameObject.Instantiate(particle.gameObject);
        particle.Play();
        
    }
    public override void OnGraphStop(Playable playable)
    {
        tempGo.SetActive(false);
    }
}
