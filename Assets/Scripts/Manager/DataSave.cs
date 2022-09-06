using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSave : Singleton<DataSave>
{
    //public List<GameObject> players = new List<GameObject>();
    //public List<GameObject> enemies = new List<GameObject>();
    //public List<GameObject> characters = new List<GameObject>();
    //public List<Player> players = new List<Player>();
    //public List<Enemy> enemies = new List<Enemy>();
    //public List<Character> characters = new List<Character>();
    public List<CharacterData> valueSave = new List<CharacterData>();//存入的是场上实例
    public List<string> keySave = new List<string>();//
    public Dictionary<string, Player> currentPlayers = new Dictionary<string, Player>();
    //public Player currentPlayer;
    public GameObject currentObj, targetObj;
    public Dictionary<string, GameObject> objSave = new Dictionary<string, GameObject>();
    public GameObject mark;
    public float height;
    public static T InDeepCopy<T>(T obj)//从SO文件深拷贝数据
    {
        string json = JsonUtility.ToJson(obj);
        T targetData = JsonUtility.FromJson<T>(json);
        return targetData;
    }
    public void AddObject(string name,GameObject obj)
    {
        objSave.Add(name, obj);
    }
    public void RemovObject(string name, GameObject obj)
    {
        objSave.Remove(name);
    }
    public void SaveData()
    {
        string tempNaame;
        CharacterData tempCharacter;
      /*  characters.Clear();
        for (int i = 0; i < players.Count; i++)
        {
            for (int f = 0; f < characters.Count; f++)
            {
                if (characters[f].tag != "enemy")
                {
                    if (players[i].GetComponent<PlayerData>().player.speed > characters[f].GetComponent<PlayerData>().player.speed)
                    {
                        if (f == characters.Count - 1)
                        {
                            characters.Add(players[i]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        characters.Insert(f, players[i]);
                        break;
                    }
                }
                else
                {
                    if (players[i].GetComponent<PlayerData>().player.speed > characters[f].GetComponent<EnemyData>().enemy.speed)
                    {
                        if (f == characters.Count - 1)
                        {
                            characters.Add(players[i]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        characters.Insert(f, players[i]);
                        break;
                    }
                }

            }
            if (characters.Count == 0)
            {
                characters.Add(players[i]);
            }
        }
        for (int i = 0; i < enemies.Count; i++)
        {

            for (int f = 0; f < characters.Count; f++)
            {
                if (characters[f].tag != "enemy")
                {
                    if (enemies[i].GetComponent<PlayerData>().player.speed > characters[f].GetComponent<PlayerData>().player.speed)
                    {
                        if (f == characters.Count - 1)
                        {
                            characters.Add(players[i]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        characters.Insert(f, players[i]);
                        break;
                    }
                }
                else
                {
                    if (enemies[i].GetComponent<PlayerData>().player.speed > characters[f].GetComponent<EnemyData>().enemy.speed)
                    {
                        if (f == characters.Count - 1)
                        {
                            characters.Add(players[i]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        characters.Insert(f, enemies[i]);
                        break;
                    }
                }
            }
            if (characters.Count == 0)
            {
                characters.Add(enemies[i]);
            }
        }
        characters.Reverse();*/

        foreach (var item in CreateManager.Instance.unitSave)
        {          
                valueSave.Add(item.Value);
                keySave.Add(item.Key);
               
        }

        for (int i = 0; i < valueSave.Count-1; i++)
        {
            for (int x = 0;x< valueSave.Count - 1 - i; x++)
            {
                if (valueSave[x].unit.speed > valueSave[x+1].unit.speed)
                {
                    tempCharacter = valueSave[x];
                    valueSave[x] = valueSave[x+1];
                    valueSave[x+1] = tempCharacter;

                    tempNaame = keySave[x];
                    keySave[x] = keySave[x + 1];
                    keySave[x + 1] = tempNaame;
                        

                }
            }
        }
        
    }
    private void Update()
    {
        if(currentObj!=null)
        mark.transform.position = currentObj.transform.position + Vector3.up * height;
    }

    
}
