using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSave : Singleton<DataSave>
{/// <summary>
/// tempPlayers��tempEnemys���Ƕ�SO���ݵ����
/// </summary>
    public Dictionary<string, Character> tempPlayerSave = new Dictionary<string, Character>();
    public Dictionary<string, Character> tempEnemySave = new Dictionary<string, Character>();

    public List<CharacterData> valueSave = new List<CharacterData>();//������ǳ���ʵ����ÿ�غ�����������Ŀǰ�ĳ������е�λ
    public List<string> keySave = new List<string>();//������ǳ���ʵ��������ÿ�غ�����
    /// <summary>
    /// ���ڴ洢�������пɿص�λ���ֵ�
    /// </summary>
    public Dictionary<string, PlayerData> currentPlayers = new Dictionary<string, PlayerData>();//���г������еĿɿ�������б�
    //public Player currentPlayer;
    public GameObject currentObj, targetObj;
    //public Dictionary<string, GameObject> objSave = new Dictionary<string, GameObject>();
    public GameObject mark;
    public float height;
    /// <summary>
    /// ȫ�ֻغ���
    /// </summary>
    public int roundCount = 0;
    public List<Buff> bufflist = new List<Buff>();
    public static T DeepCopy<T>(T obj)//��SO�ļ��������
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
        //˵��ִ����һ�λغ���

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
