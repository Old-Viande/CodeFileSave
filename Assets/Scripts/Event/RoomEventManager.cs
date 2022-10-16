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
        return true;
    }

    public bool CheckRoomEventCondition(RoomEvent roomEvent)
    {
        return true;
    }

    public bool CheckButtonCondition(RoomEvent.ButtonData buttonData)
    {
        return true;
    }

    public void ExecuteRoomEventEffect(RoomEvent roomEvent)
    {

    }
}
