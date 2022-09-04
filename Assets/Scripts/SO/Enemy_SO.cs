using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Enemy",menuName ="SO/Enemy")]
public class Enemy_SO : ScriptableObject
{
    
       public List<Enemy> enemies = new List<Enemy>();
    public Dictionary<string, Enemy> enemySave = new Dictionary<string, Enemy>();
#if UNITY_EDITOR
    void OnValidate()
    {
        enemySave.Clear();
        foreach (var property in enemies)
        {
            if (!enemySave.ContainsKey(property.name))

            {
                enemySave.Add(property.name, property);

            }
        }
    }
#else
    void Awake()
    {
        enemySave.Clear();
        foreach (var property in enemies)
        {
            if (!enemySave.ContainsKey(property.name))

            {
                enemySave.Add(property.name, property);

            }
        }
    }
#endif
}
