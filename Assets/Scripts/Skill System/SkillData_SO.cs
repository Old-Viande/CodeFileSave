using UnityEngine.Playables;
using UnityEngine;
[System.Serializable]
public class SkillData_SO
{
    public string skillName;
    public float attackRange;//������Χ
    public int skillLeve;//���ܵȼ�
    public float skillboots;//���ܱ���
    public float orginAttack;//��ʼ�˺�

    public PlayableAsset timelineAsset;

    public skillType currentSkillType;
    public enum skillType { hill, attack, control, enhanch }
    public skillProperty currentSkillProperty;
    public enum skillProperty { colddown, countdown }
    public skillChoseType currentSkillChoseType;
    public enum skillChoseType { multi,single}
    public int maxUnitChose;
    public int coldDownTime;
    public int maxcoldDownTime;
    public int countDown;
    public int maxcountDown;
}
