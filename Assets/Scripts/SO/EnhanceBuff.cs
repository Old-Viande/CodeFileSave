using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhanceBuff : Buff
{
    public EnhanceBuff()
    {
        buffName = "FullEnhance";
    }

    public override void OnRoundStart(CharacterData character)
    {
        if (roundCount > 0)
        {
            character.unit.speedModifier = 5;
            if (character.unit is Player player)
                player.luck = 1;
        }
        if (roundCount > 0)
        {
            roundCount--;
        }
        else
        {
            character.buffManager.m_RemoveBuffList.Add(this);
        }

    }

    public override void OnBuffRemove(CharacterData character)
    {
        character.unit.speedModifier = 0;

    }
}
