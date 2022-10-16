using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager
{
    public List<Buff> m_BuffList = new();
    public List<Buff> m_RemoveBuffList = new();

    public void OnRoundStart(CharacterData character)
    {
        for (int i = 0; i < m_BuffList.Count; i++)
        {
            m_BuffList[i].OnRoundStart(character);
        }
        for (int i = 0; i < m_RemoveBuffList.Count; i++)
        {
            m_RemoveBuffList[i].OnBuffRemove(character);
            m_BuffList.Remove(m_RemoveBuffList[i]);
        }
        m_RemoveBuffList.Clear();
    }
}
