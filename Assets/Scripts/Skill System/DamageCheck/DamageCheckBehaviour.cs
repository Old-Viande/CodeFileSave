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
        //一个是把倍率传过去，一个是能让伤害系统知道是什么skill（问skillManagerMono要）,知道有哪些人受伤（问skillManagerMono要）。
        if (SkillManagerMono.Instance.tempTargets.Count>0)
        {
            DamageSystemMono.Instance.DamageCheck(SkillBoot, SkillManagerMono.Instance.tempSkill, SkillManagerMono.Instance.tempTargets);
        }
    }
}
