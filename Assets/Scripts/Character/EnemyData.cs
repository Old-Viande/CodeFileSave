using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : CharacterData
{
    /// <summary>
    /// 该数据只读
    /// </summary>
    [SerializeField]
    private Enemy enemy;
    private List<PathNode> aroundList;
    private int x2, z2;
    private int nodeListCount;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var a in GridManager.Instance.enemyData.enemies)
        {
            if (this.name.Contains(a.name))
            {
                DataSave.Instance.tempEnemySave.TryGetValue(a.name, out unit);
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
    private void OnDestroy()//物品被销毁时调用
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

    //     // 判定两者间寻路距离是否小于等于敌人单位的攻击距离
    //     /*if (nodeListCount <= enemy.attackRange)
    //     {
    //         // 两者距离大于敌人攻击距离则还需先进行移动
    //         move = nodeListCount > enemy.attackRange;
    //         return true;
    //     }*/
    //     if (GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2).canWalk)
    //     {
    //         nodeListCount = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count;
    //         if (nodeListCount <= unit.attackRange)//直接攻击
    //         {
    //             // 两者距离大于玩家攻击距离则还需先进行移动
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
    //                 if (nodeListCount <= unit.attackRange)//直接攻击
    //                 {
    //                     // 两者距离大于玩家攻击距离则还需先进行移动
    //                     move = nodeListCount > unit.attackRange;
    //                     return true;
    //                 }
    //             }
    //         }
    //     }
    //     return false;
    //}

}
