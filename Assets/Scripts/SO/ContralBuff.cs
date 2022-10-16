using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContralBuff : Buff
{
   
    public ContralBuff()
    {
        buffName = "Dizzy";
    }

    public override void OnRoundStart(CharacterData character)
    {
        if (roundCount > 0)
        {
            character.unit.actionPoint = 0;
        }
        if (roundCount > 1)
        {
            roundCount--;
        }
        else
        {
            character.buffManager.m_RemoveBuffList.Add(this);
        }

    }
}
