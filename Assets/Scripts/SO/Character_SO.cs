using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Character",menuName ="SO/Character")]
public class Character_SO :ScriptableObject
{

       public List<Player> characters = new List<Player>();
    public Dictionary<string, Character> playerSave = new Dictionary<string, Character>();

#if UNITY_EDITOR
    void OnValidate()
    {
        playerSave.Clear();
        foreach (var property in characters)
        {
            if (!playerSave.ContainsKey(property.name))

            {
                playerSave.Add(property.name, property);

            }
        }
    }
#else
    void Awake()
    {
        playerSave.Clear();
        foreach (var property in characters)
        {
            if (!playerSave.ContainsKey(property.name))

            {
                playerSave.Add(property.name, property);

            }
        }
    }
#endif
}
