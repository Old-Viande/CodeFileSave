using UnityEngine.Playables;
using UnityEngine;
[System.Serializable]
public class SkillData
{
    public bool locked;
    public string skillName;
    public float attackRange;//攻击范围
    public float playSpeed;
    public int skillLeve;//技能等级
   // public float skillboots;//技能倍率
    public float orginAttack;//初始伤害
    public float skillEffectRange;//群体技能的影响范围
    public PlayableAsset timelineAsset;

    public enum skillType
    {
        [EnumName("治愈技能")]
        heal = 0,
        [EnumName("攻击技能")]
        attack,
        [EnumName("控制技能")]
        control,
        [EnumName("增益技能")]
        enhanch
    }
    public skillType currentSkillType;
    public enum skillProperty
    {
        [EnumName("冷却时间")]
        colddown = 0,
        [EnumName("使用次数")]
        countdown
    }
    public skillProperty currentSkillProperty;
    public enum skillChoseType
    {
        [EnumName("多个")]
        multi = 0,
        [EnumName("单体")]
        single
    }
    public skillChoseType currentSkillChoseType;
    public enum skillShape
    {
        [EnumName("圆形")]
        circle = 0,
        [EnumName("方形")]
        square,
        [EnumName("扇形")]
        fan
    }
    public skillShape currentSkillShape;
    public int maxUnitChose;
    public int coldDownTime;
    public int maxcoldDownTime;
    public int countDown;
    public int maxcountDown;
}
