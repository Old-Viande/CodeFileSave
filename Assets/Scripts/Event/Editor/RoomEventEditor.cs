using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class RoomEventEditor : EditorWindow
{
    private static RoomEventEditor m_Window = null;
    private RoomEventSO m_RoomEventSO;
    private int m_CurrentIndex = -1;

    private SerializedObject m_SerializedObject;
    private SerializedProperty m_CurrentRoomEventProperty;

    private ReorderableList m_EventButtonRL;
    private ReorderableList m_EnemyCreateRL;
    private ReorderableList m_SkillChangeRL;

    private string[] m_AllEnemyName;

    private Vector2 m_ScrollPosition1;
    private Vector2 m_ScrollPosition2;

    [MenuItem("Room Event/Editor")]
    static void OpenWindow()
    {
        m_Window = GetWindow<RoomEventEditor>();
        m_Window.titleContent.text = "Room Event Editor";
        m_Window.minSize = new Vector2(820, 360);
        m_Window.Show();
    }

    private void OnEnable()
    {
        m_RoomEventSO = RoomEventManager.Instance.GetRoomEventSO();
        if (m_RoomEventSO != null)
        {
            m_RoomEventSO.hideFlags = HideFlags.DontSave;
            m_SerializedObject = new SerializedObject(m_RoomEventSO);
            var allEnemies = AssetDatabase.LoadAssetAtPath<Enemy_SO>("Assets/Data/Enemy.asset");
            m_AllEnemyName = allEnemies.enemySave.Keys.ToArray();
        }
        else
        {
            Debug.LogError("未找到文件或者文件出错！Assets/Scripts/Event/RoomEventList.asset");
            if (m_Window != null)
                m_Window.Close();
        }
    }

    private void OnGUI()
    {
        // Menu bar
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("新 建", GUILayout.Width(100)))
        {
            m_RoomEventSO.AddNewRoomEvent(out m_CurrentIndex);
        }
        if (GUILayout.Button("删 除", GUILayout.Width(100)))
        {
            m_RoomEventSO.RemoveRoomEvent(ref m_CurrentIndex);
        }
        if (GUILayout.Button("保 存", GUILayout.Width(100)))
        {

        }
        EditorGUILayout.EndHorizontal();

        m_SerializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        {
            // Evnet List
            EditorGUILayout.BeginVertical(GUILayout.Width(210), GUILayout.ExpandHeight(true));
            m_ScrollPosition1 = EditorGUILayout.BeginScrollView(m_ScrollPosition1);
            for (int i = 0; i < m_RoomEventSO.GetListCount(); i++)
            {
                GUIStyle style = GUI.skin.button;
                var originColor = style.normal.textColor;
                if (m_CurrentIndex == i)
                    style.normal.textColor = Color.blue;
                if (GUILayout.Button(m_RoomEventSO.GetRoomEvent(i).Name, style, GUILayout.Width(190)))
                {
                    m_CurrentIndex = i;
                }
                style.normal.textColor = originColor;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            Handles.DrawLine(new Vector3(210, 30), new Vector3(210, 360));

            if (m_CurrentIndex != -1)
                m_CurrentRoomEventProperty = m_SerializedObject.FindProperty("RoomEventList").GetArrayElementAtIndex(m_CurrentIndex);
            else
                m_CurrentRoomEventProperty = null;
            UpdateReorderableList();

            // Event Inspector
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            m_ScrollPosition2 = EditorGUILayout.BeginScrollView(m_ScrollPosition2, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            var currentRoomEvent = m_RoomEventSO.GetRoomEvent(m_CurrentIndex);
            if (currentRoomEvent != null && m_CurrentRoomEventProperty != null)
            {
                currentRoomEvent.Name = EditorGUILayout.TextField("名称", currentRoomEvent.Name);
                currentRoomEvent.ID = EditorGUILayout.TextField("ID", currentRoomEvent.ID);
                EditorGUILayout.PropertyField(m_CurrentRoomEventProperty.FindPropertyRelative("Description"), new GUIContent("描述"));

                var originColor = GUI.backgroundColor;
                GUI.backgroundColor = Color.cyan;
                currentRoomEvent.RoomEventTriggerFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(currentRoomEvent.RoomEventTriggerFoldout, "事件触发");
                GUI.backgroundColor = originColor;
                if (currentRoomEvent.RoomEventTriggerFoldout)
                {
                    EditorGUI.indentLevel++;

                    currentRoomEvent.RoomEventTrigger.RoomEventEffectTrigger = (RoomEventEffectTrigger)EditorGUILayout.Popup("触发类型", (int)currentRoomEvent.RoomEventTrigger.RoomEventEffectTrigger, GetNames(typeof(RoomEventEffectTrigger)));
                    if (currentRoomEvent.RoomEventTrigger.RoomEventEffectTrigger == RoomEventEffectTrigger.OnStay)
                    {
                        EditorGUI.indentLevel++;
                        currentRoomEvent.RoomEventTrigger.RoomEventEffectTriggerArg = EditorGUILayout.IntField("停留的回合数", currentRoomEvent.RoomEventTrigger.RoomEventEffectTriggerArg);
                        EditorGUI.indentLevel--;
                    }
                    currentRoomEvent.RoomEventTrigger.EventTriggerPre = (EventTriggerPre)EditorGUILayout.Popup("前置条件", (int)currentRoomEvent.RoomEventTrigger.EventTriggerPre, GetNames(typeof(EventTriggerPre)));
                    EditorGUI.indentLevel++;
                    switch (currentRoomEvent.RoomEventTrigger.EventTriggerPre)
                    {
                        case EventTriggerPre.TimeDuration:
                            currentRoomEvent.RoomEventTrigger.EventTriggerPreArg1 = EditorGUILayout.FloatField("时间间隔", currentRoomEvent.RoomEventTrigger.EventTriggerPreArg1);
                            break;
                        case EventTriggerPre.Random:
                            currentRoomEvent.RoomEventTrigger.EventTriggerPreArg2 = EditorGUILayout.Slider("小于等于", currentRoomEvent.RoomEventTrigger.EventTriggerPreArg2, 0.0f, 100.0f);
                            break;
                        case EventTriggerPre.EnemyCount:
                        case EventTriggerPre.RoomCount:
                        case EventTriggerPre.RoundSpent:
                            currentRoomEvent.RoomEventTrigger.EventTriggerPreArg3 = (EventValueFilter)EditorGUILayout.Popup("筛选方式", (int)currentRoomEvent.RoomEventTrigger.EventTriggerPreArg3, GetNames(typeof(EventValueFilter)));
                            if (currentRoomEvent.RoomEventTrigger.EventTriggerPreArg3 != EventValueFilter.MinMax)
                                currentRoomEvent.RoomEventTrigger.EventTriggerPreArg4 = EditorGUILayout.IntField("比较值", currentRoomEvent.RoomEventTrigger.EventTriggerPreArg4);
                            else
                                EditorGUILayout.HelpBox("区间还未现实", MessageType.Warning);
                            break;
                        case EventTriggerPre.None:
                        default:
                            break;
                    }
                    EditorGUI.indentLevel -= 2;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();

                GUI.backgroundColor = new Color32(255, 192, 203, 255);
                currentRoomEvent.EventEffectFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(currentRoomEvent.EventEffectFoldout, "事件执行效果");
                GUI.backgroundColor = originColor;
                if (currentRoomEvent.EventEffectFoldout)
                {
                    EditorGUI.indentLevel++;
                    currentRoomEvent.RoomEventEffect.HasEventEffect = EditorGUILayout.Toggle("是否执行效果", currentRoomEvent.RoomEventEffect.HasEventEffect);
                    if (currentRoomEvent.RoomEventEffect.HasEventEffect)
                    {
                        currentRoomEvent.RoomEventEffect.EventTarget = (EventTarget)EditorGUILayout.Popup("执行效果对象", (int)currentRoomEvent.RoomEventEffect.EventTarget, GetNames(typeof(EventTarget)));
                        if (currentRoomEvent.RoomEventEffect.EventTarget == EventTarget.None)
                        {
                            currentRoomEvent.RoomEventEffect.EventEffectWithoutTarget = (EventEffectWithoutTarget)EditorGUILayout.Popup("执行的效果", (int)currentRoomEvent.RoomEventEffect.EventEffectWithoutTarget, GetNames(typeof(EventEffectWithoutTarget)));
                            EditorGUI.indentLevel++;
                            switch (currentRoomEvent.RoomEventEffect.EventEffectWithoutTarget)
                            {
                                case EventEffectWithoutTarget.AddPlayer:
                                case EventEffectWithoutTarget.RemovePlayer:
                                    EditorGUILayout.HelpBox("增删玩家角色还未实现", MessageType.Warning);
                                    break;
                                case EventEffectWithoutTarget.CreateEnemy:
                                    RenderEnemyCreateReorderableList();
                                    break;
                                default:
                                    break;
                            }
                            EditorGUI.indentLevel--;
                        }
                        else
                        {
                            currentRoomEvent.RoomEventEffect.EventTargetFilter = (EventTargetFilter)EditorGUILayout.Popup("对象筛选条件", (int)currentRoomEvent.RoomEventEffect.EventTargetFilter, GetNames(typeof(EventTargetFilter)));
                            EditorGUI.indentLevel++;
                            switch (currentRoomEvent.RoomEventEffect.EventTargetFilter)
                            {
                                case EventTargetFilter.CheckValue:
                                    currentRoomEvent.RoomEventEffect.EventTargetCheckValueType = (EventValueType)EditorGUILayout.Popup("检查的数据", (int)currentRoomEvent.RoomEventEffect.EventTargetCheckValueType, GetNames(typeof(EventValueType)));
                                    EditorGUI.indentLevel++;
                                    switch (currentRoomEvent.RoomEventEffect.EventTargetCheckValueType)
                                    {
                                        case EventValueType.HealthPoint:
                                        case EventValueType.MaxHealthPoint:
                                        case EventValueType.Attack:
                                        case EventValueType.Defense:
                                        case EventValueType.Speed:
                                            currentRoomEvent.RoomEventEffect.EventTargetCheckValueFilter = (EventValueFilter)EditorGUILayout.Popup("筛选方式", (int)currentRoomEvent.RoomEventEffect.EventTargetCheckValueFilter, GetNames(typeof(EventValueFilter)));
                                            if (currentRoomEvent.RoomEventEffect.EventTargetCheckValueFilter != EventValueFilter.MinMax)
                                                currentRoomEvent.RoomEventEffect.EventTargetFilterArg2 = EditorGUILayout.FloatField("比较值", currentRoomEvent.RoomEventEffect.EventTargetFilterArg2);
                                            else
                                                EditorGUILayout.HelpBox("区间还未现实", MessageType.Warning);
                                            break;
                                        case EventValueType.ActionPoint:
                                        case EventValueType.MaxActionPoint:
                                        case EventValueType.MoveSpeed:
                                            currentRoomEvent.RoomEventEffect.EventTargetCheckValueFilter = (EventValueFilter)EditorGUILayout.Popup("筛选方式", (int)currentRoomEvent.RoomEventEffect.EventTargetCheckValueFilter, GetNames(typeof(EventValueFilter)));
                                            if (currentRoomEvent.RoomEventEffect.EventTargetCheckValueFilter != EventValueFilter.MinMax)
                                                currentRoomEvent.RoomEventEffect.EventTargetFilterArg1 = EditorGUILayout.IntField("比较值", currentRoomEvent.RoomEventEffect.EventTargetFilterArg1);
                                            else
                                                EditorGUILayout.HelpBox("区间还未现实", MessageType.Warning);
                                            break;
                                        case EventValueType.SkillColdDown:
                                            EditorGUILayout.HelpBox("技能关联目前未实现！", MessageType.Warning);
                                            break;
                                        default:
                                            break;
                                    }
                                    EditorGUI.indentLevel--;
                                    break;
                                case EventTargetFilter.IsCurrentPlayer:
                                    currentRoomEvent.RoomEventEffect.EventTargetFilterArg3 = EditorGUILayout.Toggle("是当前玩家", currentRoomEvent.RoomEventEffect.EventTargetFilterArg3);
                                    break;
                                case EventTargetFilter.None:
                                default:
                                    break;
                            }
                            EditorGUI.indentLevel--;
                            currentRoomEvent.RoomEventEffect.EventEffectWithTarget = (EventEffectWithTarget)EditorGUILayout.Popup("执行的效果", (int)currentRoomEvent.RoomEventEffect.EventEffectWithTarget, GetNames(typeof(EventEffectWithTarget)));
                            EditorGUI.indentLevel++;
                            switch (currentRoomEvent.RoomEventEffect.EventEffectWithTarget)
                            {
                                case EventEffectWithTarget.ChangeValue:
                                    currentRoomEvent.RoomEventEffect.EventEffectChangeValueType = (EventValueType)EditorGUILayout.Popup("修改的数据", (int)currentRoomEvent.RoomEventEffect.EventEffectChangeValueType, GetNames(typeof(EventValueType)));
                                    EditorGUI.indentLevel++;
                                    switch (currentRoomEvent.RoomEventEffect.EventEffectChangeValueType)
                                    {
                                        case EventValueType.HealthPoint:
                                        case EventValueType.MaxHealthPoint:
                                        case EventValueType.Attack:
                                        case EventValueType.Defense:
                                        case EventValueType.Speed:
                                            DrawValueChange(ref currentRoomEvent.RoomEventEffect.EventEffectChangeValueChangeType, ref currentRoomEvent.RoomEventEffect.EventEffectChangeValue1);
                                            break;
                                        case EventValueType.ActionPoint:
                                        case EventValueType.MaxActionPoint:
                                        case EventValueType.MoveSpeed:
                                            DrawValueChange(ref currentRoomEvent.RoomEventEffect.EventEffectChangeValueChangeType, ref currentRoomEvent.RoomEventEffect.EventEffectChangeValue2);
                                            break;
                                        case EventValueType.SkillColdDown:
                                            EditorGUILayout.HelpBox("技能关联目前未实现！", MessageType.Warning);
                                            break;
                                        default:
                                            break;
                                    }
                                    EditorGUI.indentLevel--;
                                    break;
                                case EventEffectWithTarget.ChangeBuff:
                                    EditorGUILayout.HelpBox("改变Buff目前未实现！", MessageType.Warning);
                                    break;
                                case EventEffectWithTarget.ChangeSkill:
                                    DrawSkillChange(ref currentRoomEvent);
                                    break;
                                case EventEffectWithTarget.MoveObject:
                                    EditorGUILayout.HelpBox("移动对象目前未实现！", MessageType.Warning);
                                    break;
                                default:
                                    break;
                            }
                            EditorGUI.indentLevel--;
                        }
                    }
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndFoldoutHeaderGroup();

                EditorGUILayout.Space();
                EditorGUILayout.Separator();
                RenderEventButtonReorderableList();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        m_SerializedObject.ApplyModifiedProperties();
    }

    private string[] GetNames(Type type)
    {
        Array values = Enum.GetValues(type);
        List<string> names = Enum.GetNames(type).ToList();

        for (int i = 0; i < names.Count; i++)
        {
            var enumVal = (Enum)values.GetValue(i);
            MemberInfo[] member = enumVal.GetType().GetMember(enumVal.ToString());
            object[] customAttributes = member[0].GetCustomAttributes(typeof(EnumNameAttribute), false);
            EnumNameAttribute enumName = (customAttributes.Length != 0) ? ((EnumNameAttribute)customAttributes[0]) : default;
            if (enumName != null && !string.IsNullOrEmpty(enumName.Name))
                names[i] = enumName.Name;
            else
                names[i] = ObjectNames.NicifyVariableName(names[i]);
        }

        return names.ToArray();
    }

    private void DrawValueChange(ref ValueChangeType changeType, ref float value)
    {
        changeType = (ValueChangeType)EditorGUILayout.Popup("改变方式", (int)changeType, GetNames(typeof(ValueChangeType)));
        EditorGUI.indentLevel++;
        switch (changeType)
        {
            case ValueChangeType.Set:
                value = EditorGUILayout.FloatField("设置为", value);
                break;
            case ValueChangeType.Add:
                value = EditorGUILayout.FloatField("增加", value);
                break;
            case ValueChangeType.AddPercent:
                value = EditorGUILayout.FloatField("增加的百分比", value);
                break;
            case ValueChangeType.Minus:
                value = EditorGUILayout.FloatField("减少", value);
                break;
            case ValueChangeType.MinusPercent:
                value = EditorGUILayout.FloatField("减少的百分比", value);
                break;
            case ValueChangeType.Multiply:
                value = EditorGUILayout.FloatField("倍数", value);
                break;
            default:
                break;
        }
        EditorGUI.indentLevel--;
    }

    private void DrawValueChange(ref ValueChangeType changeType, ref int value)
    {
        changeType = (ValueChangeType)EditorGUILayout.Popup("改变方式", (int)changeType, GetNames(typeof(ValueChangeType)));
        EditorGUI.indentLevel++;
        switch (changeType)
        {
            case ValueChangeType.Set:
                value = EditorGUILayout.IntField("设置为", value);
                break;
            case ValueChangeType.Add:
                value = EditorGUILayout.IntField("增加", value);
                break;
            case ValueChangeType.AddPercent:
                value = EditorGUILayout.IntField("增加的百分比", value);
                break;
            case ValueChangeType.Minus:
                value = EditorGUILayout.IntField("减少", value);
                break;
            case ValueChangeType.MinusPercent:
                value = EditorGUILayout.IntField("减少的百分比", value);
                break;
            case ValueChangeType.Multiply:
                value = EditorGUILayout.IntField("倍数", value);
                break;
            default:
                break;
        }
        EditorGUI.indentLevel--;
    }

    private void DrawSkillChange(ref RoomEvent roomEvent)
    {
        roomEvent.SkillChangeType = (SkillChangeType)EditorGUILayout.Popup("改变方式", (int)roomEvent.SkillChangeType, GetNames(typeof(SkillChangeType)));
        EditorGUI.indentLevel++;
        switch (roomEvent.SkillChangeType)
        {
            case SkillChangeType.Add:
            case SkillChangeType.Remove:
                roomEvent.SkillCountChangeType = (SkillCountChangeType)EditorGUILayout.Popup("增删方式", (int)roomEvent.SkillCountChangeType, GetNames(typeof(SkillCountChangeType)));
                switch (roomEvent.SkillCountChangeType)
                {
                    case SkillCountChangeType.SelectAll:
                        RenderSkillChangeReorderableList();
                        break;
                    case SkillCountChangeType.SelectOne:
                        EditorGUILayout.HelpBox("多选一还未实现", MessageType.Warning);
                        break;
                    default:
                        break;
                }
                break;
            case SkillChangeType.ChangeValue:
                string[] skillNames = SkillManager.Instance.GetAllSkillName();
                int skillID = Array.IndexOf(skillNames, roomEvent.SkillName);
                EditorGUI.BeginChangeCheck();
                skillID = EditorGUILayout.Popup("技能名称", skillID, skillNames);
                if (EditorGUI.EndChangeCheck())
                {
                    roomEvent.SkillName = skillNames[skillID];
                    roomEvent.UpdateSkillValue(roomEvent.SkillName);
                }
                else if (string.IsNullOrEmpty(roomEvent.SkillValue.Name))
                {
                    roomEvent.SkillName = skillNames[0];
                    roomEvent.UpdateSkillValue(skillNames[0]);
                }
                roomEvent.SkillValueType = (SkillValueType)EditorGUILayout.Popup("修改的数值", (int)roomEvent.SkillValueType, GetNames(typeof(SkillValueType)));
                EditorGUI.indentLevel++;
                switch (roomEvent.SkillValueType)
                {
                    case SkillValueType.LockState:
                        roomEvent.SkillValue.LockState = EditorGUILayout.Toggle("锁定状态", roomEvent.SkillValue.LockState);
                        break;
                    case SkillValueType.Name:
                        roomEvent.SkillValue.Name = EditorGUILayout.TextField("技能名称", roomEvent.SkillValue.Name);
                        break;
                    case SkillValueType.Property:
                        DrawValueChange(ref roomEvent.SkillValue.ValueChangeType, ref roomEvent.SkillValue.Property);
                        break;
                    case SkillValueType.MaxProperty:
                        DrawValueChange(ref roomEvent.SkillValue.ValueChangeType, ref roomEvent.SkillValue.MaxProperty);
                        break;
                    case SkillValueType.AttackRange:
                        DrawValueChange(ref roomEvent.SkillValue.ValueChangeType, ref roomEvent.SkillValue.AttackRange);
                        break;
                    case SkillValueType.SkillEffectRange:
                        DrawValueChange(ref roomEvent.SkillValue.ValueChangeType, ref roomEvent.SkillValue.SkillEffectRange);
                        break;
                    case SkillValueType.OriginAttack:
                        DrawValueChange(ref roomEvent.SkillValue.ValueChangeType, ref roomEvent.SkillValue.OriginAttack);
                        break;
                    case SkillValueType.ChooseType:
                        roomEvent.SkillValue.ChooseType = (SkillData.skillChoseType)EditorGUILayout.Popup("选择类型", (int)roomEvent.SkillValue.ChooseType, GetNames(typeof(SkillData.skillChoseType)));
                        break;
                    case SkillValueType.Shape:
                        roomEvent.SkillValue.Shape = (SkillData.skillShape)EditorGUILayout.Popup("技能形状", (int)roomEvent.SkillValue.Shape, GetNames(typeof(SkillData.skillShape)));
                        break;
                    case SkillValueType.MaxUnitsChoose:
                        DrawValueChange(ref roomEvent.SkillValue.ValueChangeType, ref roomEvent.SkillValue.MaxUnitsChoose);
                        break;
                    default:
                        break;
                }
                EditorGUI.indentLevel--;
                break;
            case SkillChangeType.Refresh:
                EditorGUILayout.HelpBox("刷新技能目前未实现！", MessageType.Warning);
                break;
            default:
                break;
        }
        EditorGUI.indentLevel--;
    }

    private void UpdateReorderableList()
    {
        if (m_CurrentRoomEventProperty == null)
        {
            m_EventButtonRL = null;
            m_EnemyCreateRL = null;
            m_SkillChangeRL = null;
            return;
        }

        SerializedProperty buttonList = m_CurrentRoomEventProperty.FindPropertyRelative("ButtonList");
        m_EventButtonRL = new ReorderableList(m_SerializedObject, buttonList, true, true, true, true)
        {
            drawHeaderCallback = (rect) =>
            {
                rect.height = 18.0f;
                GUI.Label(rect, "UI按钮");
            },
            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty buttonData = buttonList.GetArrayElementAtIndex(index);

                SerializedProperty text = buttonData.FindPropertyRelative("Text");
                SerializedProperty condition = buttonData.FindPropertyRelative("ButtonCondition");
                SerializedProperty buttonEvent = buttonData.FindPropertyRelative("ButtonEvent");

                float conditionHeight = GetConditionHeight(index);
                Rect textRect = new(rect.x, rect.y, rect.width, 18.0f);
                Rect conditionRect = new(rect.x, rect.y + 20.0f, rect.width, 18.0f);
                Rect eventRect = new(rect.x, rect.y + conditionHeight + 22.0f, rect.width, 44.0f + 49.0f * GetEventCount(index));

                EditorGUI.PropertyField(textRect, text, new GUIContent("描述"));
                condition.enumValueIndex = EditorGUI.Popup(conditionRect, "判定条件", condition.enumValueIndex, GetNames(typeof(EventButtonCondition)));
                if (condition.enumValueIndex == 1)
                {
                    SerializedProperty conditionValueType = buttonData.FindPropertyRelative("ButtonConditionValueType");
                    SerializedProperty conditionValueFilter = buttonData.FindPropertyRelative("ButtonConditionValueFilter");
                    SerializedProperty conditionArg1 = buttonData.FindPropertyRelative("ButtonConditionArg1");

                    Rect conditionArgRect1 = new(rect.x, rect.y + 40.0f, rect.width, 18.0f);
                    Rect conditionArgRect2 = new(rect.x, rect.y + 60.0f, rect.width, 18.0f);
                    Rect conditionArgRect3 = new(rect.x, rect.y + 80.0f, rect.width, 18.0f);

                    EditorGUI.indentLevel++;
                    conditionValueType.enumValueIndex = EditorGUI.Popup(conditionArgRect1, string.Format($"判定的数据 {index}"), conditionValueType.enumValueIndex, GetNames(typeof(EventValueType)));
                    conditionValueFilter.enumValueIndex = EditorGUI.Popup(conditionArgRect2, "筛选方式", conditionValueFilter.enumValueIndex, GetNames(typeof(EventValueFilter)));
                    if (conditionValueFilter.enumValueIndex != 5)
                    {
                        conditionArg1.floatValue = EditorGUI.FloatField(conditionArgRect3, "比较值", conditionArg1.floatValue);
                    }
                    else
                    {
                        SerializedProperty conditionArg2 = buttonData.FindPropertyRelative("ButtonConditionArg2");
                        float minValue = conditionArg1.floatValue;
                        float maxValue = conditionArg2.floatValue;

                        Rect valueRect = EditorGUI.PrefixLabel(conditionArgRect3, new GUIContent("区间"));

                        GUIStyle textStyle = new(GUI.skin.textField);
                        textStyle.alignment = TextAnchor.MiddleCenter;

                        EditorGUI.BeginChangeCheck();
                        {
                            valueRect.xMin -= 15.0f;
                            Rect minValueRect = new(valueRect);
                            minValueRect.width = 83.0f;
                            minValue = EditorGUI.FloatField(minValueRect, minValue, textStyle);
                            valueRect.xMin += 83.0f;

                            Rect maxValueRect = new(valueRect);
                            maxValueRect.xMin = maxValueRect.xMax - 79.0f;
                            maxValue = EditorGUI.FloatField(maxValueRect, maxValue, textStyle);
                            valueRect.xMax -= 79.0f;

                            EditorGUI.MinMaxSlider(valueRect, ref minValue, ref maxValue, -100.0f, 100.0f);
                        }
                        if (EditorGUI.EndChangeCheck())
                        {
                            conditionArg1.floatValue = minValue;
                            conditionArg2.floatValue = maxValue;
                        }
                    }
                    EditorGUI.indentLevel--;
                }
                EditorGUI.PropertyField(eventRect, buttonEvent, new GUIContent("按钮事件"));
            },
            elementHeightCallback = (index) =>
            {
                return 66.0f + GetConditionHeight(index) + 49.0f * GetEventCount(index);
            }
        };

        SerializedProperty enemyCreateDataList = m_CurrentRoomEventProperty.FindPropertyRelative("EnemyCreateDataList");
        m_EnemyCreateRL = new ReorderableList(m_SerializedObject, enemyCreateDataList, true, true, true, true)
        {
            drawHeaderCallback = (rect) =>
            {
                rect.height = 18.0f;
                GUI.Label(rect, "敌人生成数据");
            },
            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty enemyCreateData = enemyCreateDataList.GetArrayElementAtIndex(index);

                SerializedProperty name = enemyCreateData.FindPropertyRelative("Name");
                SerializedProperty count = enemyCreateData.FindPropertyRelative("Count");

                Rect nameRect = new(rect.x, rect.y, rect.width, 18.0f);
                Rect countRect = new(rect.x, rect.y + 20.0f, rect.width, 18.0f);

                int id = Array.IndexOf(m_AllEnemyName, name.stringValue);
                EditorGUI.BeginChangeCheck();
                id = EditorGUI.Popup(nameRect, string.Format($"敌人对象 {index}"), id, m_AllEnemyName);
                if (EditorGUI.EndChangeCheck())
                    name.stringValue = m_AllEnemyName[id];
                EditorGUI.PropertyField(countRect, count, new GUIContent("数量"));
            },
            elementHeightCallback = (index) =>
            {
                return 38.0f;
            }
        };

        SerializedProperty skillNameList = m_CurrentRoomEventProperty.FindPropertyRelative("SkillNameList");
        m_SkillChangeRL = new ReorderableList(m_SerializedObject, skillNameList, true, true, true, true)
        {
            drawHeaderCallback = (rect) =>
            {
                rect.height = 18.0f;
                GUI.Label(rect, "技能数据");
            },
            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty skillName = skillNameList.GetArrayElementAtIndex(index);
                string[] skillNames = SkillManager.Instance.GetAllSkillName();
                int skillID = Array.IndexOf(skillNames, skillName.stringValue);
                EditorGUI.BeginChangeCheck();
                skillID = EditorGUI.Popup(new(rect.x, rect.y, rect.width, 18.0f), string.Format($"技能 {index}"), skillID, skillNames);
                if (EditorGUI.EndChangeCheck())
                    skillName.stringValue = skillNames[skillID];
            },
            elementHeightCallback = (index) =>
            {
                return 18.0f;
            }
        };
    }

    private void RenderEventButtonReorderableList()
    {
        if (m_EventButtonRL == null)
            return;

        m_EventButtonRL.DoLayoutList();
    }

    private void RenderEnemyCreateReorderableList()
    {
        if (m_EnemyCreateRL == null)
            return;

        m_EnemyCreateRL.DoLayoutList();
    }

    private void RenderSkillChangeReorderableList()
    {
        if (m_SkillChangeRL == null)
            return;

        m_SkillChangeRL.DoLayoutList();
    }

    private float GetConditionHeight(int index)
    {
        return m_RoomEventSO.RoomEventList[m_CurrentIndex].ButtonList[index].ButtonCondition switch
        {
            EventButtonCondition.CheckPlayerValue => 78.0f,
            _ => 18.0f
        };
    }

    private int GetEventCount(int index)
    {
        int eventCount = m_RoomEventSO.RoomEventList[m_CurrentIndex].ButtonList[index].ButtonEvent.GetPersistentEventCount();
        return eventCount == 0 ? 1 : eventCount;
    }

    public static void LocateEvent(int index)
    {
        OpenWindow();
        if (index >= m_Window.m_RoomEventSO.GetListCount() || index < 0)
        {
            m_Window.Close();
            return;
        }

        m_Window.m_CurrentIndex = index;
    }
}