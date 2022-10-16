using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomEvent
{
    public string ID = "0";
    public string Name = "New Room Event";
    [TextArea]
    public List<string> Description = new();

    #region �¼�����
    public bool RoomEventTriggerFoldout = true;
    [System.Serializable]
    public struct RoomEventTriggerData
    {
        public RoomEventEffectTrigger RoomEventEffectTrigger;
        public int RoomEventEffectTriggerArg;
        public EventTriggerPre EventTriggerPre;
        public float EventTriggerPreArg1;
        [Range(0, 100)]
        public float EventTriggerPreArg2;
        public EventValueFilter EventTriggerPreArg3;
        public int EventTriggerPreArg4;
    }
    public RoomEventTriggerData RoomEventTrigger; // TODO: make a list
    #endregion // �¼�����

    #region �¼�ִ��
    public bool EventEffectFoldout = true;
    [System.Serializable]
    public struct RoomEventEffectData
    {
        public bool HasEventEffect;
        public EventTarget EventTarget;
        public EventTargetFilter EventTargetFilter;
        public EventValueType EventTargetCheckValueType;
        public EventValueFilter EventTargetCheckValueFilter;
        public int EventTargetFilterArg1;
        public float EventTargetFilterArg2;
        public bool EventTargetFilterArg3;
        public EventEffectWithTarget EventEffectWithTarget;
        public EventEffectWithoutTarget EventEffectWithoutTarget;
        public EventValueType EventEffectChangeValueType;
        public ValueChangeType EventEffectChangeValueChangeType;
        public float EventEffectChangeValue1;
        public int EventEffectChangeValue2;
    }
    public RoomEventEffectData RoomEventEffect; // TODO: make a list
    #endregion // �¼�ִ��

    #region �������
    [System.Serializable]
    public struct SkillValueData
    {
        public bool LockState;
        public string Name;
        public int Property;
        public int MaxProperty;
        public float AttackRange;
        public float SkillEffectRange;
        public float OriginAttack;
        public SkillData.skillChoseType ChooseType;
        public SkillData.skillShape Shape;
        public int MaxUnitsChoose;
        public ValueChangeType ValueChangeType;
    }
    public SkillChangeType SkillChangeType;
    public SkillCountChangeType SkillCountChangeType;
    public string SkillName;
    public List<string> SkillNameList;
    public SkillValueType SkillValueType;
    public SkillValueData SkillValue;
    #endregion // �������

    #region �������
    [System.Serializable]
    public struct EnemyCreateData
    {
        public string Name;
        public int Count;
    }
    public List<EnemyCreateData> EnemyCreateDataList = new();
    #endregion // �������

    #region UI��ť
    [System.Serializable]
    public struct ButtonData
    {
        public string Text;
        public EventButtonCondition ButtonCondition;
        public EventValueType ButtonConditionValueType;
        public EventValueFilter ButtonConditionValueFilter;
        public float ButtonConditionArg1;
        public float ButtonConditionArg2;
        public UnityEngine.UI.Button.ButtonClickedEvent ButtonEvent;
    }
    public List<ButtonData> ButtonList = new();
    #endregion // UI��ť

    #region .ctor
    public RoomEvent()
    {
    }

    public RoomEvent(RoomEvent other)
    {
        // ��������
        ID = other.ID;
        Name = other.Name;
        Description = other.Description;

        // �¼�����
        RoomEventTrigger = other.RoomEventTrigger;

        // �¼�ִ��
        RoomEventEffect = other.RoomEventEffect;

        // �������
        SkillChangeType = other.SkillChangeType;
        SkillCountChangeType = other.SkillCountChangeType;
        SkillName = other.SkillName;
        SkillNameList = other.SkillNameList;
        SkillValueType = other.SkillValueType;
        SkillValue = other.SkillValue;

        // �������
        EnemyCreateDataList = other.EnemyCreateDataList;

        // UI��ť
        ButtonList = other.ButtonList;
        for (int i = 0; i < ButtonList.Count; i++)
        {
            ButtonList[i].ButtonEvent.RemoveListener(UIManager.Instance.PopPanel);
            ButtonList[i].ButtonEvent.AddListener(UIManager.Instance.PopPanel);
        }
    }
    #endregion // .ctor

    #region Editor Methods
    public void UpdateSkillValue(string skillName)
    {
        SkillData skill = SkillManager.Instance.GetSkillFromName(skillName);
        if (skill == null)
            return;

        SkillValue.LockState = skill.locked;
        SkillValue.Name = skill.skillName;
        SkillValue.AttackRange = skill.attackRange;
        SkillValue.SkillEffectRange = skill.skillEffectRange;
        SkillValue.OriginAttack = skill.orginAttack;
        SkillValue.ChooseType = skill.currentSkillChoseType;
        SkillValue.Shape = skill.currentSkillShape;
        SkillValue.MaxUnitsChoose = skill.maxUnitChose;
        SkillValue.Shape = skill.currentSkillShape;

        switch (skill.currentSkillProperty)
        {
            case SkillData.skillProperty.colddown:
                SkillValue.Property = skill.coldDownTime;
                SkillValue.MaxProperty = skill.maxcoldDownTime;
                break;
            case SkillData.skillProperty.countdown:
                SkillValue.Property = skill.countDown;
                SkillValue.MaxProperty = skill.maxcountDown;
                break;
            default:
                break;
        }
    }
    #endregion // Editor Methods

    #region Runtime Methods
    #endregion // Runtime Methods
}
