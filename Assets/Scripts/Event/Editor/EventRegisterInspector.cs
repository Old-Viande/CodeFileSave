using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor(typeof(EventRegister))]
public class EventRegisterInspector : Editor
{
    private ReorderableList m_RoomEventRL;

    private void OnEnable()
    {
        var roomEventSO = RoomEventManager.Instance.GetRoomEventSO();
        var register = target as EventRegister;

        SerializedProperty roomEventDataList = serializedObject.FindProperty("m_EventDataList");
        m_RoomEventRL = new ReorderableList(serializedObject, roomEventDataList, true, true, true, true)
        {
            drawHeaderCallback = (rect) =>
            {
                rect.height = 18.0f;
                GUI.Label(rect, "房间事件列表");
            },
            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty roomEventData = roomEventDataList.GetArrayElementAtIndex(index);
                SerializedProperty name = roomEventData.FindPropertyRelative("Name");
                SerializedProperty probability = roomEventData.FindPropertyRelative("Weight");

                Rect eventRect = new(rect.x, rect.y, rect.width, 18.0f);
                Rect probabilityRect = new(rect.x, rect.y + 20.0f, rect.width, 18.0f);

                int eventIndex = roomEventSO.GetRoomEventIndexByName(name.stringValue);
                EditorGUI.BeginChangeCheck();
                eventIndex = EditorGUI.Popup(eventRect, string.Format($"房间事件 {index}"), eventIndex, roomEventSO.GetAllRoomEventName());
                if (EditorGUI.EndChangeCheck())
                    register.m_EventDataList[index] = new EventRegister.RoomEventData(roomEventSO.GetRoomEvent(eventIndex).Name, 0.0f);

                probability.floatValue = EditorGUI.Slider(probabilityRect, "权重（百分比）", probability.floatValue, 0.0f, 100.0f - GetProbabilitySum(register.m_EventDataList, index));
            },
            elementHeightCallback = (index) =>
            {
                return 38.0f;
            },
            onAddCallback = (list) =>
            {
                var roomData = new EventRegister.RoomEventData(roomEventSO.RoomEventList[0].Name, 0.0f);
                register.m_EventDataList.Add(roomData);
            }
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (m_RoomEventRL != null)
            m_RoomEventRL.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private float GetProbabilitySum(List<EventRegister.RoomEventData> list, int index)
    {
        float result = 0.0f;
        for (int i = 0; i < index; i++)
            result += list[i].Weight;
        return result;
    }
}
