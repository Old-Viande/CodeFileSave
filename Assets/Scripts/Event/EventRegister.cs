using System.Collections.Generic;
using UnityEngine;

public class EventRegister : MonoBehaviour
{
    [System.Serializable]
    public struct RoomEventData
    {
        public string Name;
        public float Weight;

        public RoomEventData(string name, float weight)
        {
            Name = name;
            Weight = weight;
        }
    }
    public List<RoomEventData> m_EventDataList = new();
}
