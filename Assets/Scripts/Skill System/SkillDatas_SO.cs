using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="New Skill",menuName ="Skill")]
public class SkillDatas_SO : ScriptableObject
{
    public List<SkillData> datas;
    public Dictionary<string, SkillData> datasSave = new Dictionary<string, SkillData>();

#if UNITY_EDITOR
    void OnValidate()
    {
        datasSave.Clear();
        foreach (var property in datas)
        {
            if (!datasSave.ContainsKey(property.skillName))

            {
                datasSave.Add(property.skillName, property);

            }
        }
    }
#else
    void Awake()
    {
        datasSave.Clear();
        foreach (var property in datas)
        {
            if (!datasSave.ContainsKey(property.skillName))

            {
                datasSave.Add(property.skillName, property);

            }
        }
    }
#endif
}
