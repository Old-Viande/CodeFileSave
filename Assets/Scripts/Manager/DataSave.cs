using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSave : Singleton<DataSave>
{/// <summary>
/// tempPlayers和tempEnemys都是对SO数据的深拷贝
/// </summary>
    public Dictionary<string, Character> tempPlayerSave = new Dictionary<string, Character>();
    public Dictionary<string, Character> tempEnemySave = new Dictionary<string, Character>();

    public List<CharacterData> valueSave = new List<CharacterData>();//存入的是场上实例的每回合排序，这里有目前的场上所有单位
    public List<string> keySave = new List<string>();//存入的是场上实例的名称每回合排序
    /// <summary>
    /// 用于存储场上现有可控单位的字典
    /// </summary>
    public Dictionary<string, PlayerData> currentPlayers = new Dictionary<string, PlayerData>();//存有场上现有的可控制玩家列表
    //public Player currentPlayer;
    public GameObject currentObj, targetObj;
    //public Dictionary<string, GameObject> objSave = new Dictionary<string, GameObject>();
    public GameObject mark;
    public float height;
    /// <summary>
    /// 全局回合数
    /// </summary>
    public int roundCount = 0;
    public List<Buff> bufflist = new List<Buff>();
    public static T DeepCopy<T>(T obj)//从SO文件深拷贝数据
    {
        string json = JsonUtility.ToJson(obj);
        T targetData = JsonUtility.FromJson<T>(json);
        return targetData;
    }
    private void Start()
    {
        SaveSoData();
        bufflist.Add(new HealBuff());
        bufflist.Add(new ContralBuff());
        bufflist.Add(new EnhanceBuff());
    }

    public Buff GetBuffByName(string name)
    {
        for (int i = 0; i < bufflist.Count; i++)
        {
            if (bufflist[i].buffName == name)
            {
                return bufflist[i];
            }
        }
        return null;
    }

    public void SaveSoData()
    {
        Player Psave;
        Enemy Esave;
        for (int i = 0; i < GridManager.Instance.characterData.characters.Count; i++)
        {
            Psave = DeepCopy<Player>(GridManager.Instance.characterData.characters[i]);
            tempPlayerSave.Add(Psave.name, Psave);
        }

        for (int i = 0; i < GridManager.Instance.enemyData.enemies.Count; i++)
        {
            Esave = DeepCopy<Enemy>(GridManager.Instance.enemyData.enemies[i]);
            tempEnemySave.Add(Esave.name, Esave);
        }
    }

    public void SaveData()
    {
        string tempNaame;
        CharacterData tempCharacter;


        foreach (var item in CreateManager.Instance.unitSave)
        {
            valueSave.Add(item.Value);
            keySave.Add(item.Key);

        }

        for (int i = 0; i < valueSave.Count - 1; i++)
        {
            for (int x = 0; x < valueSave.Count - 1 - i; x++)
            {
                if (valueSave[x].unit.speed > valueSave[x + 1].unit.speed)
                {
                    tempCharacter = valueSave[x];
                    valueSave[x] = valueSave[x + 1];
                    valueSave[x + 1] = tempCharacter;

                    tempNaame = keySave[x];
                    keySave[x] = keySave[x + 1];
                    keySave[x + 1] = tempNaame;


                }
            }
        }
        valueSave.Reverse();
        keySave.Reverse();
        roundCount++;
        //说明执行完一次回合了

    }

    public List<Character> GetPlayerDataSave()
    {
        List<Character> result = new();
        for (int i = 0; i < valueSave.Count; i++)
            if (valueSave[i] is PlayerData)
                result.Add(valueSave[i].unit);

        return result;
    }

    public List<Character> GetEnemyDataSave()
    {
        List<Character> result = new();
        for (int i = 0; i < valueSave.Count; i++)
            if (valueSave[i] is EnemyData)
                result.Add(valueSave[i].unit);

        return result;
    }
}
