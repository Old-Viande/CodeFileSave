using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Buff
{
    //��Ѫ��buff
    //ѣ�ε�buff,��⵽������Ϳչ��غ�
    //ǿ����buff,���������ƶ��ľ���
    //buff���ơ������غ�����������ĳЩbuff��Ҫ����
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
