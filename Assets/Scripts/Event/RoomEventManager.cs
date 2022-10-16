using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomEventManager
{
    private static RoomEventManager instance;

    public static RoomEventManager Instance
    {
        get
        {
            if (instance == null)
                instance = new();
            return instance;
        }
    }

    private RoomEventSO m_RoomEventSO;

    public RoomEventManager()
    {
        m_RoomEventSO = Resources.Load<RoomEventSO>("RoomEventList");
    }

    public RoomEventSO GetRoomEventSO()
    {
        return m_RoomEventSO;
    }

    public RoomEvent GetTriggeredRoomEvent(EventRegister register)
    {
        float trigger = Random.Range(0.0f, 100.0f);
        for (int i = 0; i < register.m_EventDataList.Count; i++)
        {
            if (trigger <= register.m_EventDataList[i].Weight)
                return GetRoomEventByName(register.m_EventDataList[i].Name);

            trigger -= register.m_EventDataList[i].Weight;
        }

        return null;
    }

    public RoomEvent GetRoomEventByName(string name)
    {
        return new RoomEvent(m_RoomEventSO.GetRoomEvent(m_RoomEventSO.GetRoomEventIndexByName(name)));
    }

    public bool CheckRoomEventPreCondition(RoomEvent roomEvent)
    {
        var trigger = roomEvent.RoomEventTrigger;
        return trigger.EventTriggerPre switch
        {
            EventTriggerPre.None => true,
            EventTriggerPre.TimeDuration => true,
            EventTriggerPre.Random => trigger.EventTriggerPreArg2 <= Random.Range(0.0f, 100.0f),
            EventTriggerPre.EnemyCount => CheckEventValueFilter(CreateManager.Instance.enemyList.Count, trigger.EventTriggerPreArg4, trigger.EventTriggerPreArg3),
            EventTriggerPre.RoomCount => CheckEventValueFilter(GridManager.Instance.roomGridmap.gridDictionary.Count, trigger.EventTriggerPreArg4, trigger.EventTriggerPreArg3),
            EventTriggerPre.RoundSpent => CheckEventValueFilter(DataSave.Instance.roundCount, trigger.EventTriggerPreArg4, trigger.EventTriggerPreArg3),
            _ => true,
        };
    }

    private bool CheckEventValueFilter(int value, int compare, EventValueFilter filter)
    {
        bool et = value == compare;
        bool lt = value < compare;
        bool gt = value > compare;
        return filter switch
        {
            EventValueFilter.EqualsTo => et,
            EventValueFilter.LessThan => lt,
            EventValueFilter.LessThanOrEqualsTo => lt || et,
            EventValueFilter.GreaterThan => gt,
            EventValueFilter.GreaterThanOrEqualsTo => gt || et,
            _ => false,
        };
    }

    private bool CheckEventValueFilter(float value, float compare, EventValueFilter filter)
    {
        bool et = value == compare;
        bool lt = value < compare;
        bool gt = value > compare;
        return filter switch
        {
            EventValueFilter.EqualsTo => et,
            EventValueFilter.LessThan => lt,
            EventValueFilter.LessThanOrEqualsTo => lt || et,
            EventValueFilter.GreaterThan => gt,
            EventValueFilter.GreaterThanOrEqualsTo => gt || et,
            _ => false,
        };
    }

    public bool CheckRoomEventCondition(RoomEvent roomEvent)
    {
        bool result = true; // RoomEventEffectTrigger
        return result && CheckRoomEventPreCondition(roomEvent);
    }

    public bool CheckButtonCondition(RoomEvent.ButtonData buttonData)
    {
        return true;
    }

    public void ExecuteRoomEventEffect(RoomEvent roomEvent)
    {
        var effectData = roomEvent.RoomEventEffect;

        if (!effectData.HasEventEffect)
            return;

        var target = effectData.EventTarget;
        if (target == EventTarget.None)
        {
            switch (effectData.EventEffectWithoutTarget)
            {
                case EventEffectWithoutTarget.CreateEnemy:
                    List<string> enemyNameList = new();
                    for (int i = 0; i < roomEvent.EnemyCreateDataList.Count; i++)
                        for (int j = 0; j < roomEvent.EnemyCreateDataList[i].Count; j++)
                            enemyNameList.Add(roomEvent.EnemyCreateDataList[i].Name);

                    if (enemyNameList.Count == 0)
                        return;

                    var roomPos = GridManager.Instance.newRoomPos;
                    GridManager.Instance.GetRoomBound(roomPos.x, roomPos.y, out int minX, out int minZ, out int maxX, out int maxZ);
                    List<Vector3> availableList = new();
                    for (int x = minX; x < maxX; x++)
                        for (int z = minZ; z < maxZ; z++)
                            if (GridManager.Instance.pathFinder.GetGrid().GetValue(x, z).canWalk)
                                availableList.Add(GridManager.Instance.stepGrid.GetGridCenter(x, z));

                    for (int i = 0; i < enemyNameList.Count; i++)
                    {
                        if (availableList.Count == 0)
                            break;

                        int index = Random.Range(0, availableList.Count);
                            CreateManager.Instance.EnenmyCreate(enemyNameList[i], availableList[index]);

                        availableList.RemoveAt(index);
                    }
                    break;
                case EventEffectWithoutTarget.AddPlayer:
                case EventEffectWithoutTarget.RemovePlayer:
                default:
                    return;
            }
        }
        else
        {
            Character[] characters = GetEventTargetCharacters(effectData);
            switch (effectData.EventEffectWithTarget)
            {
                case EventEffectWithTarget.ChangeValue:
                    for (int i = 0; i < characters.Length; i++)
                        ChangeCharacterValue(ref characters[i], effectData);
                    break;
                case EventEffectWithTarget.ChangeSkill:
                    for (int i = 0; i < characters.Length; i++)
                        ChangeCharacterSkill(ref characters[i], roomEvent);
                    break;
                case EventEffectWithTarget.ChangeBuff:
                case EventEffectWithTarget.MoveObject:
                default:
                    return;
            }
        }
    }

    private Character[] GetEventTargetCharacters(RoomEvent.RoomEventEffectData effectData)
    {
        EventTarget target = effectData.EventTarget;
        EventTargetFilter filter = effectData.EventTargetFilter;

        List<Character> tempResult = new();
        switch (target)
        {
            case EventTarget.CurrentPlayer:
                if (TurnBaseFSM.Instance.characters[0] is PlayerData)
                    tempResult.Add(TurnBaseFSM.Instance.characters[0].unit);
                break;
            case EventTarget.AllPlayer:
                tempResult = DataSave.Instance.GetPlayerDataSave();
                break;
            case EventTarget.AllEnemy:
                tempResult = DataSave.Instance.GetEnemyDataSave();
                break;
            case EventTarget.None:
            case EventTarget.AllPlayerInRoom:
            case EventTarget.RandomPlayer:
            case EventTarget.AllEnemyInRoom:
            case EventTarget.RandomEnemy:
            default:
                return null;
        }

        if (tempResult == null || tempResult.Count == 0)
            return null;

        List<Character> result = new();

        switch (filter)
        {
            case EventTargetFilter.CheckValue:
                for (int i = 0; i < tempResult.Count; i++)
                    if (CheckEventValueType(effectData, tempResult[i]))
                        result.Add(tempResult[i]);
                break;
            case EventTargetFilter.IsCurrentPlayer:
                for (int i = 0; i < tempResult.Count; i++)
                    if (tempResult[i].Equals(TurnBaseFSM.Instance.characters[0].unit) == effectData.EventTargetFilterArg3)
                        result.Add(tempResult[i]);
                break;
            case EventTargetFilter.None:
            default:
                return tempResult.ToArray();
        }

        return result.ToArray();
    }

    private bool CheckEventValueType(RoomEvent.RoomEventEffectData effectData, Character character)
    {
        return effectData.EventTargetCheckValueType switch
        {
            EventValueType.HealthPoint => CheckEventValueFilter(character.hp, effectData.EventTargetFilterArg2, effectData.EventTargetCheckValueFilter),
            EventValueType.MaxHealthPoint => CheckEventValueFilter(character.maxHp, effectData.EventTargetFilterArg2, effectData.EventTargetCheckValueFilter),
            EventValueType.Attack => CheckEventValueFilter(character.attack, effectData.EventTargetFilterArg2, effectData.EventTargetCheckValueFilter),
            EventValueType.Defense => CheckEventValueFilter(character.defense, effectData.EventTargetFilterArg2, effectData.EventTargetCheckValueFilter),
            EventValueType.Speed => CheckEventValueFilter(character.speed, effectData.EventTargetFilterArg2, effectData.EventTargetCheckValueFilter),
            EventValueType.ActionPoint => CheckEventValueFilter(character.actionPoint, effectData.EventTargetFilterArg1, effectData.EventTargetCheckValueFilter),
            EventValueType.MaxActionPoint => CheckEventValueFilter(character.maxActionPoint, effectData.EventTargetFilterArg1, effectData.EventTargetCheckValueFilter),
            EventValueType.MoveSpeed => CheckEventValueFilter(character.moveSpeed, effectData.EventTargetFilterArg1, effectData.EventTargetCheckValueFilter),
            EventValueType.SkillColdDown => false,
            _ => false
        };
    }

    private void ChangeCharacterValue(ref Character character, RoomEvent.RoomEventEffectData effectData)
    {
        switch (effectData.EventEffectChangeValueType)
        {
            case EventValueType.HealthPoint:
                ChangeCharacterValueByType(ref character.hp, character.maxHp, effectData.EventEffectChangeValue1, effectData.EventEffectChangeValueChangeType);
                break;
            case EventValueType.MaxHealthPoint:
                ChangeCharacterValueByType(ref character.maxHp, effectData.EventEffectChangeValue1, effectData.EventEffectChangeValueChangeType);
                break;
            case EventValueType.Attack:
                ChangeCharacterValueByType(ref character.attack, effectData.EventEffectChangeValue1, effectData.EventEffectChangeValueChangeType);
                break;
            case EventValueType.Defense:
                ChangeCharacterValueByType(ref character.defense, effectData.EventEffectChangeValue1, effectData.EventEffectChangeValueChangeType);
                break;
            case EventValueType.Speed:
                ChangeCharacterValueByType(ref character.speed, effectData.EventEffectChangeValue1, effectData.EventEffectChangeValueChangeType);
                break;
            case EventValueType.ActionPoint:
                ChangeCharacterValueByType(ref character.actionPoint, character.maxActionPoint, effectData.EventEffectChangeValue2, effectData.EventEffectChangeValueChangeType);
                break;
            case EventValueType.MaxActionPoint:
                ChangeCharacterValueByType(ref character.maxActionPoint, effectData.EventEffectChangeValue2, effectData.EventEffectChangeValueChangeType);
                break;
            case EventValueType.MoveSpeed:
                ChangeCharacterValueByType(ref character.moveSpeed, effectData.EventEffectChangeValue2, effectData.EventEffectChangeValueChangeType);
                break;
            case EventValueType.SkillColdDown:
            default:
                break;
        }
    }

    private void ChangeCharacterValueByType(ref int prop, int value, ValueChangeType type)
    {
        switch (type)
        {
            case ValueChangeType.Set:
                prop = value;
                break;
            case ValueChangeType.Add:
                prop += value;
                break;
            case ValueChangeType.AddPercent:
                prop += (int)(prop * value * 0.01f);
                break;
            case ValueChangeType.Minus:
                prop -= value;
                break;
            case ValueChangeType.MinusPercent:
                prop -= (int)(prop * value * 0.01f);
                break;
            case ValueChangeType.Multiply:
                prop *= value;
                break;
            default:
                break;
        }
    }

    private void ChangeCharacterValueByType(ref int prop, int maxProp, int value, ValueChangeType type)
    {
        int result;
        switch (type)
        {
            case ValueChangeType.Set:
                prop = value > maxProp ? maxProp : value;
                break;
            case ValueChangeType.Add:
                result = prop + value;
                prop = result > maxProp ? maxProp : result;
                break;
            case ValueChangeType.AddPercent:
                result = prop + (int)(prop * value * 0.01f);
                prop = result > maxProp ? maxProp : result;
                break;
            case ValueChangeType.Minus:
                result = prop - value;
                prop = result < 0 ? 0 : result;
                break;
            case ValueChangeType.MinusPercent:
                result = prop - (int)(prop * value * 0.01f);
                prop = result < 0 ? 0 : result;
                break;
            case ValueChangeType.Multiply:
                result = prop * value;
                prop = result > maxProp ? maxProp : result;
                break;
            default:
                break;
        }
    }

    private void ChangeCharacterValueByType(ref float prop, float value, ValueChangeType type)
    {
        switch (type)
        {
            case ValueChangeType.Set:
                prop = value;
                break;
            case ValueChangeType.Add:
                prop += value;
                break;
            case ValueChangeType.AddPercent:
                prop += prop * value * 0.01f;
                break;
            case ValueChangeType.Minus:
                prop -= value;
                break;
            case ValueChangeType.MinusPercent:
                prop -= prop * value * 0.01f;
                break;
            case ValueChangeType.Multiply:
                prop *= value;
                break;
            default:
                break;
        }
    }

    private void ChangeCharacterValueByType(ref float prop, float maxProp, float value, ValueChangeType type)
    {
        float result;
        switch (type)
        {
            case ValueChangeType.Set:
                prop = value > maxProp ? maxProp : value;
                break;
            case ValueChangeType.Add:
                result = prop + value;
                prop = result > maxProp ? maxProp : result;
                break;
            case ValueChangeType.AddPercent:
                result = prop + prop * value * 0.01f;
                prop = result > maxProp ? maxProp : result;
                break;
            case ValueChangeType.Minus:
                result = prop - value;
                prop = result < 0 ? 0 : result;
                break;
            case ValueChangeType.MinusPercent:
                result = prop - prop * value * 0.01f;
                prop = result < 0 ? 0 : result;
                break;
            case ValueChangeType.Multiply:
                result = prop * value;
                prop = result > maxProp ? maxProp : result;
                break;
            default:
                break;
        }
    }

    private void ChangeCharacterSkill(ref Character character, RoomEvent roomEvent)
    {
        switch (roomEvent.SkillChangeType)
        {
            case SkillChangeType.Add:
                switch (roomEvent.SkillCountChangeType)
                {
                    case SkillCountChangeType.SelectAll:
                        for (int i = 0; i < roomEvent.SkillNameList.Count; i++)
                            SkillManager.Instance.AddSkill(roomEvent.SkillNameList[i]);
                        break;
                    case SkillCountChangeType.SelectOne:
                        throw new System.NotImplementedException("技能多选一还未实现");
                    default:
                        break;
                }
                break;
            case SkillChangeType.Remove:
                switch (roomEvent.SkillCountChangeType)
                {
                    case SkillCountChangeType.SelectAll:
                        for (int i = 0; i < roomEvent.SkillNameList.Count; i++)
                            SkillManager.Instance.RemoveSkill(roomEvent.SkillNameList[i]);
                        break;
                    case SkillCountChangeType.SelectOne:
                        throw new System.NotImplementedException("技能多选一还未实现");
                    default:
                        break;
                }
                break;
            case SkillChangeType.ChangeValue:
                var skillData = character.GetSkillData(roomEvent.SkillName);
                if (skillData == null)
                    return;

                var skillValueData = roomEvent.SkillValue;
                switch (roomEvent.SkillValueType)
                {
                    case SkillValueType.LockState:
                        skillData.locked = skillValueData.LockState;
                        break;
                    case SkillValueType.Name:
                        skillData.skillName = skillValueData.Name;
                        break;
                    case SkillValueType.Property:
                        switch (skillData.currentSkillProperty)
                        {
                            case SkillData.skillProperty.colddown:
                                ChangeCharacterValueByType(ref skillData.coldDownTime, skillData.maxcoldDownTime, skillValueData.Property, skillValueData.ValueChangeType);
                                break;
                            case SkillData.skillProperty.countdown:
                                ChangeCharacterValueByType(ref skillData.countDown, skillData.maxcountDown, skillValueData.Property, skillValueData.ValueChangeType);
                                break;
                            default:
                                break;
                        }
                        break;
                    case SkillValueType.MaxProperty:
                        switch (skillData.currentSkillProperty)
                        {
                            case SkillData.skillProperty.colddown:
                                ChangeCharacterValueByType(ref skillData.maxcoldDownTime, skillValueData.MaxProperty, skillValueData.ValueChangeType);
                                break;
                            case SkillData.skillProperty.countdown:
                                ChangeCharacterValueByType(ref skillData.maxcountDown, skillValueData.MaxProperty, skillValueData.ValueChangeType);
                                break;
                            default:
                                break;
                        }
                        break;
                    case SkillValueType.AttackRange:
                        ChangeCharacterValueByType(ref skillData.attackRange, skillValueData.AttackRange, skillValueData.ValueChangeType);
                        break;
                    case SkillValueType.SkillEffectRange:
                        ChangeCharacterValueByType(ref skillData.skillEffectRange, skillValueData.SkillEffectRange, skillValueData.ValueChangeType);
                        break;
                    case SkillValueType.OriginAttack:
                        ChangeCharacterValueByType(ref skillData.orginAttack, skillValueData.OriginAttack, skillValueData.ValueChangeType);
                        break;
                    case SkillValueType.ChooseType:
                        skillData.currentSkillChoseType = skillValueData.ChooseType;
                        break;
                    case SkillValueType.Shape:
                        skillData.currentSkillShape = skillValueData.Shape;
                        break;
                    case SkillValueType.MaxUnitsChoose:
                        ChangeCharacterValueByType(ref skillData.maxUnitChose, skillValueData.MaxUnitsChoose, skillValueData.ValueChangeType);
                        break;
                    default:
                        break;
                }
                break;
            case SkillChangeType.Refresh:
            default:
                break;
        }
    }
}
