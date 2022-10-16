using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Buff
{
    //回血的buff
    //眩晕的buff,检测到有这个就空过回合
    //强化的buff,可以增加移动的距离
    //buff名称、持续回合数、层数（某些buff需要）、
    public string buffName;
    public string description;
    public int roundCount;





    public virtual void OnRoundStart(CharacterData character)
    {

    }

    public virtual void OnBuffRemove(CharacterData character)
    {

    }
}
