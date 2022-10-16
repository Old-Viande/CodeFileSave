using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Character
{

    public string name;

    public string UID;

    //[HideInInspector]
    public float hp;

    public float maxHp;
    /// <summary>
    /// 
    /// </summary>
    public float speed;

    public float speedModifier;

    public float attack;

    public float defense;

    //[HideInInspector]
    public int actionPoint;

    public int maxActionPoint;

    public bool isPlayer;

    public int attackRange;

    public int moveSpeed;

    public GameObject Item;

    public enum State
    {
        Idle,
        Attack,
        Deffence,
        Move
    }

    public State currentState;

    public List<SkillData> skillSave = new List<SkillData>();

    public SkillData GetSkillData(string name)
    {
        for (int i = 0; i < skillSave.Count; i++)
            if (skillSave[i].skillName == name)
                return skillSave[i];

        return null;
    }
}
