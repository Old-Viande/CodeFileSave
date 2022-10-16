using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public partial class SkillManager
{
    private SkillDatas_SO allSkill;

    private static SkillManager instance;
    public static SkillManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SkillManager();
            }
            return instance;
        }
    }

    


    public SkillManager()
    {
        allSkill = Resources.Load<SkillDatas_SO>("AllSkill");
    }
    public void Init()
    {
        //AddSkill("fireball");//暂时增加的技能
    }
    #region//判定是否能够使用
    /// <summary>
    /// 返0是冷却，1是使用次数
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int ColdorCount(SkillData data)
    {
        //1.判定是冷却还是使用次数
        return (int)data.currentSkillProperty;
    }
    public bool SkillCheckColdDown(SkillData data)
    {
        //2.根据判定结果，再判定是否可以被使用
        if (data.coldDownTime>0)
        {
            return false;
        }
        else if(data.coldDownTime==0)
        {
            return true;
        }
        else
        {
            Debug.Log("冷却时间为负");
            return false;
        }
    }
    public bool SkillCheckCountDown(SkillData data)
    {
        //2.根据判定结果，再判定是否可以被使用
        if (data.countDown>0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 自动判定技能是否可以使用
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool SkillCheckAll(SkillData data)
    {
        if( ColdorCount(data)==0)
        {
           return SkillCheckColdDown(data);
        }
        else if(ColdorCount(data)==1)
        {
            return SkillCheckCountDown(data);
        }
        else
        {
            Debug.Log("技能冷却类型错误");
            return false;
        }
    }
    #endregion

  
    /// <summary>
    /// 给人物增加技能
    /// </summary>
    /// <param name="skillname"></param>
    #region//给人物增加技能
    public void AddSkill(string skillname)
    {
        bool canAdd = true;
        GridManager.Instance.characterData.playerSave.TryGetValue(DataSave.Instance.currentObj.name, out Character player);//读出数据中的指定单位
        //技能系统还要有一个从allskill根据名字分配技能给characterSo的api.
        foreach (var skilldata in player.skillSave)
        {
            if( skilldata.skillName==skillname)
            {
                canAdd = false ;
                break;
            }
        }
        if (canAdd)
        {
            allSkill.datasSave.TryGetValue(skillname, out SkillData skill);//根据名字读到技能
            DataSave.Instance.currentObj.GetComponent<CharacterData>().unit.skillSave.Add(skill);//把技能存入当前行动单位的技能表中
            player.skillSave.Add(skill);
        }
    }

    #endregion
    public void RemoveSkill(string skillname)
    {
        bool canRemove = false;
        GridManager.Instance.characterData.playerSave.TryGetValue(DataSave.Instance.currentObj.name, out Character player);//读出数据中的指定单位
        //技能系统还要有一个从allskill根据名字分配技能给characterSo的api.
        foreach (var skilldata in player.skillSave)
        {
            if (skilldata.skillName == skillname)
            {
                canRemove = true;
                break;
            }
        }
        if (canRemove)
        {
            allSkill.datasSave.TryGetValue(skillname, out SkillData skill);//根据名字读到技能
            for (int i = 0; i < DataSave.Instance.currentObj.GetComponent<CharacterData>().unit.skillSave.Count; i++)
            {
                if (DataSave.Instance.currentObj.GetComponent<CharacterData>().unit.skillSave[i].skillName == skillname)
                    DataSave.Instance.currentObj.GetComponent<CharacterData>().unit.skillSave.RemoveAt(i);
            }
            for (int i = 0; i < player.skillSave.Count; i++)
            {
                if (player.skillSave[i].skillName == skillname)
                    player.skillSave.RemoveAt(i);
            }
        }
    }
    //开始调用
    //选择目标UI+人物朝向+取消返回+群体/单体选中目标时ui变化+可选择范围(射程)提示
    public void SkillStart(SkillData skill)
    {       
        //string skillname = "";
        //allSkill.datasSave.TryGetValue(skillname, out SkillData_SO skill);//根据名字读到技能
        //if(skill.currentSkillChoseType == SkillData_SO.skillChoseType.multi)//检测读到的技能是单体还是群体
        //{
        //    if (skill.attackRange >= (DataSave.Instance.currentObj.transform.position - DataSave.Instance.targetObj.transform.position).magnitude)
        //    {
        //        //如果技能的释放点在范围内
        //        //并且技能的范围内存在至少一个单位
        //        //把目标头上的标记显示出来
        //        //群体射线要使用Physics.OverlapSphere来获得多个单位的碰撞体，把它们都存入收到技能影响的范围
        //        //这个检测需要实时检测鼠标点击
        //    }
        //}
        //else//是单体
        //{
        //    if (skill.attackRange >= (DataSave.Instance.currentObj.transform.position - DataSave.Instance.targetObj.transform.position).magnitude)//选择的单位在技能的范围内
        //    {
        //        //把目标头上的标记显示出来
        //        //进行一个弧度的画 使用LineRenderer
        //        //曲线 用 curve和line进行做
        //    }
        //}
        GridManager.Instance.skillset(skill);
        UpdataManager.Instance.skillUseButton = true;
    }
    
    public string[] GetAllSkillName()
    {
        return allSkill.datasSave.Keys.ToArray();
    }

    public SkillData GetSkillFromName(string skillName)
    {
        allSkill.datasSave.TryGetValue(skillName, out SkillData result);
        return result;
    }
    
    //显示技能射程，鼠标放出屏幕射线，
    //单体技能射线碰到人物，判定成功后，就 头上显示标记，
    //同时开启点击感应，检测不到时点击感应不开。
    //群体技能射线碰到地面就显示范围，范围内人物，显示标记，至少有一人时开启感应。
    //1.显示范围用什么做
    //2.标记用什么

    //点击后，符合条件的话。把身上挂animator的物体传给timeline。


}
