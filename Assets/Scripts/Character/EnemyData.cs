using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : CharacterData
{
    /// <summary>
    /// ������ֻ��
    /// </summary>
    [SerializeField]
    private Enemy enemy;
    private List<PathNode> aroundList;
    private int x2, z2;
    private int nodeListCount;

   //public Transform heardMark;
    // Start is called before the first frame update
    public static T DeepCopy<T>(T obj)//��SO�ļ��������
    {
        string json = JsonUtility.ToJson(obj);
        T targetData = JsonUtility.FromJson<T>(json);
        return targetData;
    }
    void Start()
    {
        //var topCanvas = transform.Find("TopCanvas");
        //if (topCanvas != null)
           // heardMark = topCanvas.Find("Mark");
        foreach (var a in GridManager.Instance.enemyData.enemies)
        {
            if (this.name.Contains(a.name))
            {
                Character DataUnit;
                DataSave.Instance.tempEnemySave.TryGetValue(a.name, out DataUnit);
                //����ֱ�Ӹ�unit�������õ��� ����
                //����json����� json
                unit = DeepCopy<Enemy>((Enemy)DataUnit);

                //unit.hp = DataUnit.hp;
                //unit.attack = DataUnit.attack;

                //....
                //GridManager.Instance.enemyData.enemySave.TryGetValue(a.name, out unit);
                unit.actionPoint = unit.maxActionPoint;
                unit.hp = unit.maxHp;
            }
        }

        if (unit != null)
        {
            CreateManager.Instance.AddObj(this.name, this);
        }
        enemy = (Enemy)unit;
    }
    private void OnDestroy()//��Ʒ������ʱ����
    {
        if (unit != null)
        {
            /* DataSave.Instance.enemies.Remove(enemy);
             DataSave.Instance.objSave.Remove(this.name);*/
            CreateManager.Instance.RemoveObj(this.name);
        }
    }
    //public bool CheckTargetInAttackRange(GameObject target, out bool move)
    // {
    //     move = false;
    //     if (unit.attackRange == 0)
    //     {
    //         return false;
    //     }

    //     GridManager.Instance.stepGrid.GetGridXZ(this.transform.position, out int x1, out int z1);
    //     GridManager.Instance.stepGrid.GetGridXZ(target.transform.position, out int x2, out int z2);
    //     int nodeListCount = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count;

    //     // �ж����߼�Ѱ·�����Ƿ�С�ڵ��ڵ��˵�λ�Ĺ�������
    //     /*if (nodeListCount <= enemy.attackRange)
    //     {
    //         // ���߾�����ڵ��˹������������Ƚ����ƶ�
    //         move = nodeListCount > enemy.attackRange;
    //         return true;
    //     }*/
    //     if (GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2).canWalk)
    //     {
    //         nodeListCount = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count;
    //         if (nodeListCount <= unit.attackRange)//ֱ�ӹ���
    //         {
    //             // ���߾��������ҹ������������Ƚ����ƶ�
    //             move = nodeListCount > unit.attackRange;
    //             return true;
    //         }
    //         else
    //         {
    //             move = nodeListCount > unit.attackRange;
    //             return false;
    //         }
    //     }
    //     else
    //     {
    //         aroundList = GridManager.Instance.pathFinder.CheckAroundNodes(GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2));
    //         foreach (var a in aroundList)
    //         {
    //             if (a.canWalk)
    //             {
    //                 x2 = a.x;
    //                 z2 = a.z;
    //                 nodeListCount = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count;
    //                 if (nodeListCount <= unit.attackRange)//ֱ�ӹ���
    //                 {
    //                     // ���߾��������ҹ������������Ƚ����ƶ�
    //                     move = nodeListCount > unit.attackRange;
    //                     return true;
    //                 }
    //             }
    //         }
    //     }
    //     return false;
    //}

}
