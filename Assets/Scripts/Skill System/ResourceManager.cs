using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 记录所有读路径的string
/// </summary>
public class ResourceManager
{
    private static ResourceManager instance;
    public static ResourceManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ResourceManager();
                
            }
            return instance;
        }
    }
    public ResourceManager()
    {
        SkillInit();
    }

    private  void SkillInit()
    {
        allSkillPlayableAboutResource = new Dictionary<string, SkillPlayableAboutResource>();
        allSkillPlayableAboutResource.Add("Eyeball", new("Defeat", "Hit", ""));
        allSkillPlayableAboutResource.Add("Death", new("Defeat", "Hit", ""));
        allSkillPlayableAboutResource.Add("Z-Man", new("Defeat", "Hit", ""));

    }
    public AudioClip LoadingAudioClip(string clipPath)
    {
        AudioClip audiotemp = Resources.Load<AudioClip>(clipPath);
        if (audiotemp==null)
        {
            Debug.Log("未找到音频");
        }
        return audiotemp;
    }
    #region//敌人技能表现相关资源
    public Dictionary<string, SkillPlayableAboutResource> allSkillPlayableAboutResource;
    public class SkillPlayableAboutResource
    {
        public string AnimDefeat;
        public string AnimHit;//动画状态机里改变的参数名
        public string AudioPath;//Resource的路径
        public SkillPlayableAboutResource(string  animDefeat,string animHit,string audioPath)
        {
            AnimDefeat = animDefeat;
            AnimHit = animHit;
            AudioPath=audioPath;
        }
    }
    #endregion






}