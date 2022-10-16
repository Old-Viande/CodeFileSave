using UnityEngine.Playables;
using UnityEngine;
[System.Serializable]
public class SkillData
{
    public bool locked;
    public string skillName;
    public float attackRange;//������Χ
    public float playSpeed;
    public int skillLeve;//���ܵȼ�
   // public float skillboots;//���ܱ���
    public float orginAttack;//��ʼ�˺�
    public float skillEffectRange;//Ⱥ�弼�ܵ�Ӱ�췶Χ
    public PlayableAsset timelineAsset;

    public enum skillType
    {
        [EnumName("��������")]
        heal = 0,
        [EnumName("��������")]
        attack,
        [EnumName("���Ƽ���")]
        control,
        [EnumName("���漼��")]
        enhanch
    }
    public skillType currentSkillType;
    public enum skillProperty
    {
        [EnumName("��ȴʱ��")]
        colddown = 0,
        [EnumName("ʹ�ô���")]
        countdown
    }
    public skillProperty currentSkillProperty;
    public enum skillChoseType
    {
        [EnumName("���")]
        multi = 0,
        [EnumName("����")]
        single
    }
    public skillChoseType currentSkillChoseType;
    public enum skillShape
    {
        [EnumName("Բ��")]
        circle = 0,
        [EnumName("����")]
        square,
        [EnumName("����")]
        fan
    }
    public skillShape currentSkillShape;
    public int maxUnitChose;
    public int coldDownTime;
    public int maxcoldDownTime;
    public int countDown;
    public int maxcountDown;
}
