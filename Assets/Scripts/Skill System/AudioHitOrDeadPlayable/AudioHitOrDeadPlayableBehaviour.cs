using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AudioHitOrDeadPlayableBehaviour : PlayableBehaviour
{

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {

        //�� һ���£�target���ܻ�������ȫ����
        //if (true)//TODO��������û�ӣ���Ϊ������ֱ��ɾ��
        //{
        //}
        foreach (var target in SkillManagerMono.Instance.tempTargets)
        {
            if (!target.gameObject.activeInHierarchy)//��֤����
            {
                continue;
            }
            target.audioSour.clip = ResourceManager.Instance.LoadingAudioClip(ResourceManager.Instance.allSkillPlayableAboutResource.NewTryGetValue(target.unit.name).AudioPath);
            if (target.audioSour.clip==null)
            {
                Debug.Log(target.unit.name + "ȱʧ��Ƶ");
            }
            else
            {
                target.audioSour.Play();
            }    
        }

    }
}

