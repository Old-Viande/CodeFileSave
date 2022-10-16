using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TransformTweenBehaviour : PlayableBehaviour
{
    public enum TweenType
    {
        Linear,
        Deceleration,
        Harmonic,
        Custom,
    }
    public enum AimPosition
    {
        currentObj,
        target,
        //targets,//要考虑多轴分配问题，先不做
        mouseClickPosition,
        relayPoint,//中继点
    }
    public float relayhight;
    public AimPosition startAim;
    public AimPosition endAim;
    public float speed;
    public PlayableDirector pd;
    //对start和end做填充即可，
    //如果是火球。就是挂给火球，然后从自己-》鼠标所指
    //如果是射击，就是挂给箭矢，然后从自己-》敌人
    //如果是冲锋，就是挂给人物，从自己-》敌人
    //吸扯，敌人-》自己，或者敌人-》鼠标所指
    //所以绝大多数技能只需要指明，是去鼠标所指还是自己还是敌人，谁去，就行了。
    //技能开始时，根据枚举，选择从？到？，主体是谁，即可
    //
    //    track主体做一个塞入枚举(最差情况下根据名字塞入),
    //   目标tempTargets    ,自己DataSave.Instance.currentObj,目标粒子trans,自己粒子trans根据选择塞进去,--目前采用最差情况，根据track名称包含

    //    然后clip加入到什么位置的枚举(目标, 自己, 鼠标所指，中心点+一定高度)
    public Transform startLocation;
    public Transform endLocation;
    public bool tweenPosition = true;
    public bool tweenRotation = true;
    public TweenType tweenType;
    public AnimationCurve customCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    public Vector3 startingPosition;
    public Quaternion startingRotation = Quaternion.identity;

    AnimationCurve m_LinearCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    AnimationCurve m_DecelerationCurve = new AnimationCurve
    (
        new Keyframe(0f, 0f, -k_RightAngleInRads, k_RightAngleInRads),
        new Keyframe(1f, 1f, 0f, 0f)
    );
    AnimationCurve m_HarmonicCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    const float k_RightAngleInRads = Mathf.PI * 0.5f;


    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        //为了实现时间缩放，需要分成，可缩放track和跟进track，
        //与其改动位置牵一发动全身，不如改播放速度，
        //  playableDirector.RebuildGraph(); 当未开始播放时调速度要调这句先，开播不用
        //playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        //速度变化的区间仅仅是transtween的track

        //开始时确定射程，射速，算出duration时间（也就是说duration代表最大射程下射击所花时间），
        //然后根据实际射击距离给出duration应该缩减的比例 最大射程：实际射程 = 最大时间：实际时间
        //所以 实际Duration=（实际射程*MaxDuration）/最大射程；
        //最大SkillManagerMono.Instance.tempSkill.
        //实际GridManager.Instance.currentDistance
        //最大时间clip。duration
        if (pd == null)
        {
            pd = (PlayableDirector)playable.GetGraph().GetResolver();
        }
        float maxDuration = (float)playable.GetDuration();
        float realDuration = (GridManager.Instance.currentDistance * maxDuration) / SkillManagerMono.Instance.tempSkill.attackRange;
        speed = maxDuration / realDuration;
        pd.playableGraph.GetRootPlayable(0).SetSpeed(speed);

    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (startLocation)
        {
            startingPosition = startLocation.position;
            startingRotation = startLocation.rotation;
        }
        if (pd != null)
        {
            if (pd.playableGraph.GetRootPlayable(0).GetSpeed() != speed)
            {
                pd.playableGraph.GetRootPlayable(0).SetSpeed(speed);
            }
        }
    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (pd == null)
        {
            pd = (PlayableDirector)playable.GetGraph().GetResolver();
        }

        pd.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
    public float EvaluateCurrentCurve(float time)
    {
        if (tweenType == TweenType.Custom && !IsCustomCurveNormalised())
        {
            Debug.LogError("Custom Curve is not normalised.  Curve must start at 0,0 and end at 1,1.");
            return 0f;
        }

        switch (tweenType)
        {
            case TweenType.Linear:
                return m_LinearCurve.Evaluate(time);
            case TweenType.Deceleration:
                return m_DecelerationCurve.Evaluate(time);
            case TweenType.Harmonic:
                return m_HarmonicCurve.Evaluate(time);
            default:
                return customCurve.Evaluate(time);
        }
    }

    bool IsCustomCurveNormalised()
    {
        if (!Mathf.Approximately(customCurve[0].time, 0f))
            return false;

        if (!Mathf.Approximately(customCurve[0].value, 0f))
            return false;

        if (!Mathf.Approximately(customCurve[customCurve.length - 1].time, 1f))
            return false;

        return Mathf.Approximately(customCurve[customCurve.length - 1].value, 1f);
    }
}