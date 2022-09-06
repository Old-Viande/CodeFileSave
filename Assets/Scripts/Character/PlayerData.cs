using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterData
{   /// <summary>
    /// 该数据只读
    /// </summary>
    [SerializeField]
    private Player player;
    private List<PathNode> aroundList;
    private int x2, z2;
    private int nodeListCount;
    ///private string name;
    void Start()
    {

        GridManager.Instance.characterData.playerSave.TryGetValue(this.name, out unit);
        unit.actionPoint = unit.maxActionPoint;
        unit.hp = unit.maxHp;
        if (unit != null)
        {
            //DataSave.Instance.players.Add(this.gameObject);
            CreateManager.Instance.AddObj(this.name, this);
        }
        player = (Player)unit;
    }
    private void OnDestroy()//物品被销毁时调用
    {
        if (unit != null)
        {
            //DataSave.Instance.players.Remove(this.gameObject);
            CreateManager.Instance.RemoveObj(this.name);

        }
    }

    public bool CheckTargetInAttackRange(GameObject target, out bool move)//此代码需要更改，玩家可以手动控制移动
    {
        move = false;
        if (unit.attackRange == 0)
        {

            return false;
        }

        GridManager.Instance.smap.GetGridXZ(this.transform.position, out int x1, out int z1);
        GridManager.Instance.smap.GetGridXZ(target.transform.position, out int x2, out int z2);
        if (GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2).canWalk)
        {
            nodeListCount = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count;
            if (nodeListCount <= unit.attackRange + (unit.actionPoint > 0 ? unit.actionPoint - 1 : unit.actionPoint) * unit.moveSpeed)//直接移动
            {
                // 两者距离大于玩家攻击距离则还需先进行移动
                move = nodeListCount > unit.attackRange;
                return true;
            }
        }
        else
        {
            aroundList = GridManager.Instance.pathFinder.CheckAroundNodes(GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2));
            foreach (var a in aroundList)
            {
                if (a.canWalk)
                {
                    x2 = a.x;
                    z2 = a.z;
                    nodeListCount = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count;
                    if (nodeListCount <= unit.attackRange + (unit.actionPoint > 0 ? unit.actionPoint - 1 : unit.actionPoint) * unit.moveSpeed)//如果在这个点位周围有任何一个点是满足条件的
                    {
                        // 两者距离大于玩家攻击距离则还需先进行移动
                        move = nodeListCount > unit.attackRange;

                        return true;
                    }
                }
            }
        }
        // 判定两者间寻路距离是否小于等于玩家的攻击距离与可走格数之和      
        return false;
    }
    public bool CheckMove(GameObject target, out bool canMove)//这个检测是用来测试能不能走到选择的位置的,寻路的格子会连起始点一起计算，或许+1比较合适
    {
        canMove = false;
        if (unit.actionPoint == 0)
        {
            return false;
        }
        GridManager.Instance.smap.GetGridXZ(this.transform.position, out int x1, out int z1);
        GridManager.Instance.smap.GetGridXZ(target.transform.position, out int x2, out int z2);
        int nodeListCount = GridManager.Instance.pathFinderTest.FindPath(x1, z1, x2, z2).Count;//用测试网格进行路径检索
        if (nodeListCount <= unit.actionPoint * unit.moveSpeed + 1)
        {
            return true;
        }
        return false;
    }
}
