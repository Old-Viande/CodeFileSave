using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AnimTargetPlayHitOrDeadBehaviour : PlayableBehaviour
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
            target.anim.SetTrigger(ResourceManager.Instance.allSkillPlayableAboutResource[target.unit.name].AnimHit);
        }

    }
}
