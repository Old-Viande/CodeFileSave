using UnityEngine;

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

    #region//判定是否能够使用
    /// <summary>
    /// 返0是冷却，1是使用次数
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int ColdorCount(SkillData_SO data)
    {
        //1.判定是冷却还是使用次数
        return (int)data.currentSkillProperty;
    }
    public bool SkillCheckColdDown(SkillData_SO data)
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
    public bool SkillCheckCountDown(SkillData_SO data)
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
    public bool SkillCheckAll(SkillData_SO data)
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

    public float AttackMath(int skillLevel,float skillboots,float origin )
    {
        return skillLevel* origin*(-1 *Mathf.Pow((skillboots - 10),2)  + 100) / 20;
    }
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
            allSkill.datasSave.TryGetValue(skillname, out SkillData_SO skill);//根据名字读到技能
            DataSave.Instance.currentObj.GetComponent<CharacterData>().unit.skillSave.Add(skill);//把技能存入当前行动单位的技能表中
            player.skillSave.Add(skill);
        }
    }
    #endregion
    //开始调用
    //选择目标UI+人物朝向+取消返回+群体/单体选中目标时ui变化+可选择范围(射程)提示
    public void skillStart()
    {

    }
    //显示技能射程，鼠标放出屏幕射线，
    //单体技能射线碰到人物，判定成功后，就 头上显示标记，
    //同时开启点击感应，检测不到时点击感应不开。
    //群体技能射线碰到地面就显示范围，范围内人物，显示标记，至少有一人时开启感应。
    //1.显示范围用什么做
    //2.标记用什么

}
