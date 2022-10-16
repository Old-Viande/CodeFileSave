using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AudioHitOrDeadPlayableBehaviour : PlayableBehaviour
{

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {

        //就 一件事，target的受击动画，全播放
        //if (true)//TODO死亡动画没加，因为现在是直接删除
        //{
        //}
        foreach (var target in SkillManagerMono.Instance.tempTargets)
        {
            if (!target.gameObject.activeInHierarchy)//保证活着
            {
                continue;
            }
            target.audioSour.clip = ResourceManager.Instance.LoadingAudioClip(ResourceManager.Instance.allSkillPlayableAboutResource.NewTryGetValue(target.unit.name).AudioPath);
            if (target.audioSour.clip==null)
            {
                Debug.Log(target.unit.name + "缺失音频");
            }
            else
            {
                target.audioSour.Play();
            }    
        }

    }
}

