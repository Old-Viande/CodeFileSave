using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomEventSO))]
public class RoomEventSOInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.FloatField("事件数量", ((RoomEventSO)serializedObject.targetObject).GetListCount());
        EditorGUI.EndDisabledGroup();
        if (GUILayout.Button("Open Editor"))
        {
            if (EditorWindow.HasOpenInstances<RoomEventEditor>())
            {
                EditorWindow.FocusWindowIfItsOpen<RoomEventEditor>();
            }
            else
            {
                EditorApplication.ExecuteMenuItem("Room Event/Editor");
            }
        }
    }
}
