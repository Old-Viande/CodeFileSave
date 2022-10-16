using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystemMono : Singleton<DamageSystemMono>
{
    //�߼��ж����Ƿ���Ա��˺�
    //Ӱ�������о�.
    //
    protected override void Awake()
    {
        base.Awake();
    }
    public void DamageCheck(float skillBoot,SkillData skill,List<CharacterData> charactors)
    {
       float tempDamage= AttackMath(skill.skillLeve, skillBoot, skill.orginAttack);
        //����UI�������������ֲ����ݿ�Ѫ
        foreach (var character in charactors)
        {
            character.unit.hp -= tempDamage;
            character.GetComponentInChildren<MiniCanvas>()?.OnHpChange();
        }
    }
    private float AttackMath(int skillLevel, float skillboots, float origin)
    {
        return (skillLevel+1) * origin * (-1 * Mathf.Pow((skillboots - 10), 2) + 100) / 20;
    }
}
