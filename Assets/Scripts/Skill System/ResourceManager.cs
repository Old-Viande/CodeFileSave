using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��¼���ж�·����string
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
            Debug.Log("δ�ҵ���Ƶ");
        }
        return audiotemp;
    }
    #region//���˼��ܱ��������Դ
    public Dictionary<string, SkillPlayableAboutResource> allSkillPlayableAboutResource;
    public class SkillPlayableAboutResource
    {
        public string AnimDefeat;
        public string AnimHit;//����״̬����ı�Ĳ�����
        public string AudioPath;//Resource��·��
        public SkillPlayableAboutResource(string  animDefeat,string animHit,string audioPath)
        {
            AnimDefeat = animDefeat;
            AnimHit = animHit;
            AudioPath=audioPath;
        }
    }
    #endregion






}