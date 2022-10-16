using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SkillManagerMono : Singleton<SkillManagerMono>
{
    public PlayableDirector pd;
    public SkillData tempSkill;//当前要播放的skill
    public List<CharacterData> tempTargets;

    protected override void Awake()
    {
        base.Awake();
    }
    
    private void Start()
    {
        tempTargets = new List<CharacterData>();
    }
    public void SkillPlay(List<Collider> targets,GameObject currentObj,SkillData skill)
    {
        if (pd.state == PlayState.Playing)
        {
            return;
        }
        tempSkill = skill;
        tempTargets.Clear();
        foreach (var target in targets)
        {
            tempTargets.Add(target.GetComponent<CharacterData>());
        }
        switch (skill.currentSkillChoseType)
        {
            case SkillData.skillChoseType.multi:
                MultiSkill(targets, currentObj, skill);
                break;
            case SkillData.skillChoseType.single:
                SingleSkill(targets, currentObj, skill);
                break;
            default:
                break;
        }
      
    }

    private void MultiSkill(List<Collider> targets, GameObject currentObj, SkillData skill)
    {
        pd.playableAsset = Instantiate(skill.timelineAsset);
        TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

        //群攻生track
        //var track1 = timeline.CreateTrack<AnimationTrack>(null, "TrackAnimator");
        //track1.muted = false;
        //

        foreach (var track in timeline.GetOutputTracks())
        {
            if (track.name == "Attacker Animation")
            {
                pd.SetGenericBinding(track, currentObj.GetComponentInChildren<Animator>());
            }
            else if (track.name == "Attacker Audio")
            {
                pd.SetGenericBinding(track, currentObj.GetComponent<AudioSource>());

            }
            else if (track.name == "AttackParticle")
            {  
                pd.SetGenericBinding(track, currentObj.GetComponent<CharacterData>().carryPoint);
                foreach (var clip in track.GetClips())
                {
                    ParticlePlayableClip myclip = (ParticlePlayableClip)clip.asset;

                    ParticlePlayableBehaviour mybehav = myclip.template;
                    myclip.trans.exposedName = System.Guid.NewGuid().ToString(); 
                    pd.SetReferenceValue(myclip.trans.exposedName, (Transform)pd.GetGenericBinding(track));
                }
            }
            //else if (track.name == "AttackParticle")
            //{
            //    pd.SetGenericBinding(track, currentObj.GetComponent<CharacterData>().carryPoint);

            //}
            else if (track.name == "Victim Animation")
            {
                //群攻生track
                //因为使用了暴力timeline，所以啥也不用做了
                //for (int i = 0; i < targets.Count; i++)
                //{
                //    var trackTemp= CopyTrack(track, timeline);
                //    pd.SetGenericBinding(trackTemp, targets[i].GetComponentInChildren<Animator>());
                //}
            }
            else if (track.name == "Victim Audio")
            {
                //因为使用了暴力timeline，所以啥也不用做了
                ////pd.SetGenericBinding(track, currentObj.GetComponent<AudioSource>());
                //for (int i = 0; i < targets.Count; i++)
                //{
                //    var trackTemp = CopyTrack(track, timeline);
                //    pd.SetGenericBinding(trackTemp, targets[i].GetComponent<AudioSource>());
                //}
            }
            else if (track.name == "DamageParticle")
            {
                //pd.SetGenericBinding(track, currentObj.GetComponent<AudioSource>());
                for (int i = 0; i < targets.Count; i++)
                {
                    var trackTemp = CopyTrack(track, timeline);
                    pd.SetGenericBinding(trackTemp, targets[i].GetComponent<CharacterData>().hitPoint);
                    foreach (var clip in trackTemp.GetClips())
                    {
                        ParticlePlayableClip myclip = (ParticlePlayableClip)clip.asset;

                        ParticlePlayableBehaviour mybehav = myclip.template;
                        myclip.trans.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.trans.exposedName, (Transform)pd.GetGenericBinding(trackTemp));
                    }
                }
            }

            if (track is TransformTweenTrack)
            {
                //如果是transformtween的话
                if (track.name.Contains("target"))
                {
                    for (int i = 0; i < targets.Count; i++)
                    {
                        var trackTemp = CopyTrack(track, timeline);

                        pd.SetGenericBinding(track, targets[i].transform);
                    }
                }
                else if (track.name.Contains("tarParticle"))
                {
                    for (int i = 0; i < targets.Count; i++)
                    {
                        var trackTemp = CopyTrack(track, timeline);

                        pd.SetGenericBinding(track, targets[0].GetComponent<CharacterData>().carryPoint.transform);
                    }
                }
                else if (track.name.Contains("currentobj"))
                {
                    pd.SetGenericBinding(track, DataSave.Instance.currentObj.transform);

                }
                else if (track.name.Contains("currentParticle"))
                {
                    pd.SetGenericBinding(track, DataSave.Instance.currentObj.GetComponent<CharacterData>().carryPoint.transform);

                }
                #region//这是绑clip上的值
                foreach (var clip in track.GetClips())
                {
                    TransformTweenClip myclip = (TransformTweenClip)clip.asset;

                    TransformTweenBehaviour mybehav = myclip.template;
                    myclip.startLocation.exposedName = System.Guid.NewGuid().ToString();
                    pd.SetReferenceValue(myclip.startLocation.exposedName, ChangeClipValue(mybehav.startAim, mybehav.relayhight));
                    pd.SetReferenceValue(myclip.endLocation.exposedName, ChangeClipValue(mybehav.endAim, mybehav.relayhight));
                }
                #endregion

            }
            //damageparticle

        }
        pd.Evaluate();//开始前再评估,不要早评估
        pd.Play();
    }

    private void SingleSkill(List<Collider> targets, GameObject currentObj, SkillData skill)
    {
        pd.playableAsset = Instantiate(skill.timelineAsset);
        TimelineAsset timeline = (TimelineAsset)pd.playableAsset;
        foreach (var track in timeline.GetOutputTracks())
        {
          

            if (track.name == "Attacker Animation")
            {
                pd.SetGenericBinding(track, currentObj.GetComponentInChildren<Animator>());
            }

            else if (track.name == "Attacker Audio")
            {
                pd.SetGenericBinding(track, currentObj.GetComponent<AudioSource>());

            }
            else if (track.name == "AttackParticle")
            {
                pd.SetGenericBinding(track, currentObj.GetComponent<CharacterData>().carryPoint);//绑定原始点给大track//绑的没用，其实不用这个

                
                foreach (var clip in track.GetClips())
                {
                    ParticlePlayableClip myclip = (ParticlePlayableClip)clip.asset;

                    ParticlePlayableBehaviour mybehav = myclip.template;
                    myclip.trans.exposedName = System.Guid.NewGuid().ToString();
                    pd.SetReferenceValue(myclip.trans.exposedName, (Transform)pd.GetGenericBinding(track));//
                }
            }
            //else if (track.name == "Victim Animation")
            //{
            //    if (targets.Count>0)
            //    {
            //        pd.SetGenericBinding(track, targets[0].GetComponentInChildren<Animator>());
            //    }
            //}
            //else if (track.name == "Victim Audio")
            //{
            //    pd.SetGenericBinding(track, currentObj.GetComponent<AudioSource>());
            //}
            else if (track.name == "DamageParticle")
            {
                //pd.SetGenericBinding(track, currentObj.GetComponent<AudioSource>());
                if (targets.Count>0)
                {
                    pd.SetGenericBinding(track, targets[0].GetComponent<CharacterData>().hitPoint);
                    foreach (var clip in track.GetClips())
                    {
                        ParticlePlayableClip myclip = (ParticlePlayableClip)clip.asset;

                        ParticlePlayableBehaviour mybehav = myclip.template;
                        myclip.trans.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.trans.exposedName, (Transform)pd.GetGenericBinding(track));
                    }
                }

            }
            //Transform skillTempTrans = new GameObject("skillMoveTrans").transform;//不如直接复位简单，太傻逼了
            if (track is TransformTweenTrack)
            {
                #region//这是绑track
                //如果是transformtween的话
                if (track.name.Contains("target"))
                {
                    //skillTempTrans.position = targets[0].transform.position;
                    //skillTempTrans.rotation = targets[0].transform.rotation;
                    pd.SetGenericBinding(track, targets[0]);
                }
                else if (track.name.Contains("currentobj"))
                {
                    //skillTempTrans.position = DataSave.Instance.currentObj.transform.position;
                    //skillTempTrans.rotation = DataSave.Instance.currentObj.transform.rotation;
                    pd.SetGenericBinding(track, DataSave.Instance.currentObj.transform);
                }
                else if (track.name.Contains("targetParticle"))
                {
                    //skillTempTrans.position = targets[0].GetComponent<CharacterData>().carryPoint.transform.position;
                    //skillTempTrans.rotation = targets[0].GetComponent<CharacterData>().carryPoint.transform.rotation;
                    pd.SetGenericBinding(track, targets[0].GetComponent<CharacterData>().carryPoint.transform);
                }
                else if (track.name.Contains("currentobjParticle"))
                {
                    //skillTempTrans.position = DataSave.Instance.currentObj.GetComponent<CharacterData>().carryPoint.transform.position;
                    //skillTempTrans.rotation = DataSave.Instance.currentObj.GetComponent<CharacterData>().carryPoint.transform.rotation;
                    pd.SetGenericBinding(track, DataSave.Instance.currentObj.GetComponent<CharacterData>().carryPoint.transform);
                }

                #endregion

                #region//这是绑clip上的值
                foreach (var clip in track.GetClips())
                {
                    TransformTweenClip myclip = (TransformTweenClip)clip.asset;

                    TransformTweenBehaviour mybehav = myclip.template;
                    myclip.startLocation.exposedName = System.Guid.NewGuid().ToString();
                    pd.SetReferenceValue(myclip.startLocation.exposedName, ChangeClipValue(mybehav.startAim, mybehav.relayhight));
                    pd.SetReferenceValue(myclip.endLocation.exposedName, ChangeClipValue(mybehav.endAim, mybehav.relayhight));

                }
                #endregion
            }

        }
        //foreach (var track in pd.playableAsset.outputs)
        //{
        //    if (track.streamName == "Attacker Animation")
        //    {
        //        pd.SetGenericBinding(track.sourceObject, attacker.ac.anim);
        //    }
        //    else if (track.streamName == "Victim Animation")
        //    {
        //        pd.SetGenericBinding(track.sourceObject, victim.ac.anim);
        //    }
        //    else if (track.streamName == "Attacker Script")
        //    {
        //        pd.SetGenericBinding(track.sourceObject, attacker);
        //    }
        //    else if (track.streamName == "Victim Script")
        //    {
        //        pd.SetGenericBinding(track.sourceObject, victim);
        //    }
        //}
        //if (track.name == "Attacker Script")
        //{
        //    pd.SetGenericBinding(track, attacker);
        //    foreach (var clip in track.GetClips())
        //    {
        //        MySuperPlayableClip myclip = (MySuperPlayableClip)clip.asset;
        //        MySuperPlayableBehaviour mybehav = myclip.template;
        //        myclip.am.exposedName = System.Guid.NewGuid().ToString();
        //        pd.SetReferenceValue(myclip.am.exposedName, attacker);
        //        Debug.Log(myclip.am.exposedName);
        //    }
        //}
        //else if (track.name == "Victim Script")
        //{
        //    pd.SetGenericBinding(track, victim);
        //    foreach (var clip in track.GetClips())
        //    {
        //        MySuperPlayableClip myclip = (MySuperPlayableClip)clip.asset;
        //        MySuperPlayableBehaviour mybehav = myclip.template;
        //        myclip.am.exposedName = System.Guid.NewGuid().ToString();
        //        pd.SetReferenceValue(myclip.am.exposedName, victim);
        //        Debug.Log(myclip.am.exposedName);
        //    }
        //}

        //var track1 = timeline.CreateTrack<AnimationTrack>(null, "TrackAnimator");
        //track1.muted = false;
        //群体技能。先做一个mute的通用群体受击者动画track，然后在代码里分配里所有受击者这个track，让受击者都播放这个track
        pd.Evaluate();//开始前再评估,不要早评估
        pd.Play();
    }
    //伤害timeline拥有，技能倍率
    //需要知道skill，传给伤害系统。

   
    private TrackAsset CopyTrack(TrackAsset track, TimelineAsset timeline)
    {
        //尝试写反射
        var assembly = Assembly.Load("Unity.Timeline.Editor");
        // AssemblyからTimelineEditorの機能をReflectionで取得してくる

        var trackExtensionsType = assembly.GetType("UnityEditor.Timeline.TrackExtensions");
        if (trackExtensionsType == null) Debug.LogWarning("TrackExtensions not found.");

        //拿复制代码
        var duplicateInfo = trackExtensionsType.GetMethod("Duplicate",
            BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static);
        if (duplicateInfo == null) Debug.LogWarning("TrackExtensions.Duplicate not found.");
        // Duplicateメソッドに渡す引数の配列をあらかじめ作成
        // Duplicateメソッドを呼ぶ際はそれぞれの複製物をindex:0に代入して渡す
        // TrackAsset, ターゲットのPlayableDirector, null, コピー先のTimelineAssetの順


        var duplicateArgs = new[] { (object)null, pd, null, pd.playableAsset };

        // パラメータ配列の先頭をコピーしたいトラックに置き換える
        duplicateArgs[0] = track;
        // TrackをCloneして、作成したグループトラックに紐づける
        var clone = duplicateInfo.Invoke(null, duplicateArgs) as TrackAsset;//到这里成功复制出来Track
        if (clone.muted)
        {
            clone.muted = false;
        }
        return clone;

        // TrackAsset track1 = null;
        // if (track is AnimationTrack)
        // {
        //     track1 = timeline.CreateTrack<AnimationTrack>(null, "TrackAnimator");
        //    
        // }
        // else if(track is AudioTrack)
        // {
        //     track1 = timeline.CreateTrack<AudioTrack>(null, "TrackAudio");
        // }
        // else if (track is ParticlePlayableTrack )
        // {
        //     track1 = timeline.CreateTrack<ParticlePlayableTrack>(null, "TrackParticle");
        // }
        // else if (track is TransformTweenTrack )
        // {
        //     track1 = timeline.CreateTrack<TransformTweenTrack>(null, "TrackTween");
        // }
        // foreach (var cliptemp in clone.GetClips())
        // {
        //     /ar copyClip = track1.CreateClip<AnimationPlayableAsset>();
        //     // TimelineClip clip= DeepCopy<TimelineClip>(cliptemp);
        //     // TimelineClipExtensions.MoveToTrack(clip, track1);
        //     
        // }
        //return clone;
    }

    //public static T DeepCopy<T>(T obj)//从SO文件深拷贝数据
    //{
    //    string json = JsonUtility.ToJson(obj);
    //    T targetData = JsonUtility.FromJson<T>(json);
    //    return targetData;
    //}
    private Transform ChangeClipValue(TransformTweenBehaviour.AimPosition aim,float relayhight)
    {
        Transform changeTrans=null;
        switch (aim)
        {
            case TransformTweenBehaviour. AimPosition.currentObj:
                changeTrans = DataSave.Instance.currentObj.transform;
                break;
            case TransformTweenBehaviour.AimPosition.target:
                changeTrans = SkillManagerMono.Instance.tempTargets[0].transform;
                break;
            case TransformTweenBehaviour.AimPosition.mouseClickPosition:
                changeTrans = GameObject.Instantiate(new GameObject("mouseClikPosition"), GridManager.Instance.skillHitPoint, Quaternion.Euler((GridManager.Instance.skillHitPoint-DataSave.Instance.currentObj.transform.position).normalized)).transform;
                break;
            case TransformTweenBehaviour.AimPosition.relayPoint:
                if (SkillManagerMono.Instance.tempSkill.currentSkillChoseType == SkillData.skillChoseType.multi)
                {
                    changeTrans = GameObject.Instantiate(new GameObject("relayPoint"),
                      (DataSave.Instance.currentObj.transform.position + GridManager.Instance.skillHitPoint) / 2 + Vector3.up * relayhight
                      *(GridManager.Instance.currentDistance/SkillManagerMono.Instance.tempSkill.attackRange)
                      , DataSave.Instance.currentObj.transform.rotation).transform;
                }
                else if (SkillManagerMono.Instance.tempSkill.currentSkillChoseType == SkillData.skillChoseType.single)
                {
                    changeTrans = GameObject.Instantiate(new GameObject("relayPoint"),
                        (DataSave.Instance.currentObj.transform.position + SkillManagerMono.Instance.tempTargets[0].transform.position) / 2 + Vector3.up * relayhight
                        * (GridManager.Instance.currentDistance / SkillManagerMono.Instance.tempSkill.attackRange)
                        , DataSave.Instance.currentObj.transform.rotation).transform;
                }
                break;
            default:
                break;
        }
        //switch (endAim)
        //{
        //    case TransformTweenBehaviour.AimPosition.currentObj:
        //        changeTrans = DataSave.Instance.currentObj.transform;
        //        break;
        //    case TransformTweenBehaviour.AimPosition.target:
        //        changeTrans = SkillManagerMono.Instance.tempTargets[0].transform;
        //        break;
        //    case TransformTweenBehaviour.AimPosition.mouseClickPosition:
        //        changeTrans = GameObject.Instantiate(new GameObject("mouseClikPosition"), GridManager.Instance.skillHitPoint, Quaternion.identity).transform;
        //        break;
        //    case TransformTweenBehaviour.AimPosition.relayPoint:
        //        if (SkillManagerMono.Instance.tempSkill.currentSkillChoseType == SkillData.skillChoseType.multi)
        //        {
        //            changeTrans = GameObject.Instantiate(new GameObject("relayPoint"),
        //              (DataSave.Instance.currentObj.transform.position + GridManager.Instance.skillHitPoint) / 2 + Vector3.up * relayhight
        //              , DataSave.Instance.currentObj.transform.rotation).transform;
        //        }
        //        else if (SkillManagerMono.Instance.tempSkill.currentSkillChoseType == SkillData.skillChoseType.single)
        //        {
        //            changeTrans = GameObject.Instantiate(new GameObject("relayPoint"),
        //                (DataSave.Instance.currentObj.transform.position + SkillManagerMono.Instance.tempTargets[0].transform.position) / 2 + Vector3.up * relayhight
        //                , DataSave.Instance.currentObj.transform.rotation).transform;
        //        }
        //        break;
        //    default:
        //        break;
        //}
        return changeTrans;
    }
}
