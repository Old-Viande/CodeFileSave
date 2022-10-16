using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBuff : Buff
{
    public int buffLayer=10;
    public HealBuff()
    {
        buffName = "MiniHeal";
    }

    public override void OnRoundStart(CharacterData character)
    {
        if (roundCount > 0)
        {

            if (character.unit.hp + buffLayer <= character.unit.maxHp)
            {
                character.unit.hp += buffLayer;
            }
            else
            {
                character.unit.hp = character.unit.maxHp;
            }
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
