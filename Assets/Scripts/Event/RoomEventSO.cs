using System;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
[Serializable]
public class RoomEventSO : ScriptableObject
{
    public List<RoomEvent> RoomEventList = new();

    public int GetListCount()
    {
        return RoomEventList.Count;
    }

    public RoomEvent GetRoomEvent(int index)
    {
        if (index < 0 || index >= RoomEventList.Count)
            return null;

        return RoomEventList[index];
    }

    public void SetRoomEvent(int index, RoomEvent roomEvent)
    {
        if (index < 0 || index >= RoomEventList.Count)
            return;

        RoomEventList[index] = roomEvent;
    }

    private bool AnyHasID(string ID)
    {
        for (int i = 0; i < RoomEventList.Count; i++)
        {
            if (RoomEventList[i].ID == ID)
                return true;
        }
        return false;
    }

    public void AddNewRoomEvent(out int currentIndex)
    {
        var RoomEvent = new RoomEvent();
        int id = 0;
        while (AnyHasID(RoomEvent.ID))
        {
            id = int.Parse(RoomEvent.ID);
            id += 1;
            RoomEvent.ID = id.ToString();
        }
        RoomEvent.Name += string.Format($" {id}");
        RoomEventList.Add(RoomEvent);
        currentIndex = RoomEventList.Count - 1;
    }

    public void RemoveRoomEvent(ref int index)
    {
        if (index < 0 || index >= RoomEventList.Count)
        {
            index = -1;
            return;
        }

        RoomEventList.RemoveAt(index);

        if (RoomEventList.Count == 0)
            index = -1;
        else
            index = index == 0 ? 0 : (index - 1);
    }

    public string[] GetAllRoomEventName()
    {
        List<string> nameList = new();
        for (int i = 0; i < RoomEventList.Count; i++)
            nameList.Add(RoomEventList[i].Name);
        return nameList.ToArray();
    }

    public int GetRoomEventIndexByName(string name)
    {
        for (int i = 0; i < RoomEventList.Count; i++)
            if (RoomEventList[i].Name == name)
                return i;
        return -1;
    }
}
