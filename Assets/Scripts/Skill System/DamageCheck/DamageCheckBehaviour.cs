using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class DamageCheckBehaviour : PlayableBehaviour
{
    public float SkillBoot;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        //һ���ǰѱ��ʴ���ȥ��һ���������˺�ϵͳ֪����ʲôskill����skillManagerMonoҪ��,֪������Щ�����ˣ���skillManagerMonoҪ����
        if (SkillManagerMono.Instance.tempTargets.Count>0)
        {
            DamageSystemMono.Instance.DamageCheck(SkillBoot, SkillManagerMono.Instance.tempSkill, SkillManagerMono.Instance.tempTargets);
        }
    }
}
