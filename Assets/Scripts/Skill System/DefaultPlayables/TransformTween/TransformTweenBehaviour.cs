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
        //targets,//Ҫ���Ƕ���������⣬�Ȳ���
        mouseClickPosition,
        relayPoint,//�м̵�
    }
    public float relayhight;
    public AimPosition startAim;
    public AimPosition endAim;
    public float speed;
    public PlayableDirector pd;
    //��start��end����伴�ɣ�
    //����ǻ��򡣾��ǹҸ�����Ȼ����Լ�-�������ָ
    //�������������ǹҸ���ʸ��Ȼ����Լ�-������
    //����ǳ�棬���ǹҸ�������Լ�-������
    //����������-���Լ������ߵ���-�������ָ
    //���Ծ����������ֻ��Ҫָ������ȥ�����ָ�����Լ����ǵ��ˣ�˭ȥ�������ˡ�
    //���ܿ�ʼʱ������ö�٣�ѡ��ӣ�������������˭������
    //
    //    track������һ������ö��(�������¸�����������),
    //   Ŀ��tempTargets    ,�Լ�DataSave.Instance.currentObj,Ŀ������trans,�Լ�����trans����ѡ������ȥ,--Ŀǰ����������������track���ư���

    //    Ȼ��clip���뵽ʲôλ�õ�ö��(Ŀ��, �Լ�, �����ָ�����ĵ�+һ���߶�)
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
        //Ϊ��ʵ��ʱ�����ţ���Ҫ�ֳɣ�������track�͸���track��
        //����Ķ�λ��ǣһ����ȫ������Ĳ����ٶȣ�
        //  playableDirector.RebuildGraph(); ��δ��ʼ����ʱ���ٶ�Ҫ������ȣ���������
        //playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        //�ٶȱ仯�����������transtween��track

        //��ʼʱȷ����̣����٣����durationʱ�䣨Ҳ����˵duration�������������������ʱ�䣩��
        //Ȼ�����ʵ������������durationӦ�������ı��� �����̣�ʵ����� = ���ʱ�䣺ʵ��ʱ��
        //���� ʵ��Duration=��ʵ�����*MaxDuration��/�����̣�
        //���SkillManagerMono.Instance.tempSkill.
        //ʵ��GridManager.Instance.currentDistance
        //���ʱ��clip��duration
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