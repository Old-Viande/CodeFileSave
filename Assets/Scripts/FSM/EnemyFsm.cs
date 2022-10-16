using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdel : IState
{/// <summary>
/// 这个状态需要判断敌人是否还有剩余的行动指
/// </summary>
    private TurnBaseFSM manager;
    public EnemyIdel(TurnBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {
        if (manager.characters[0].unit.actionPoint <= 0)
        {
            manager.TranState(StateType.Running);
        }
    }
    public void OnUpdata()
    {
        if (manager.CheckTargetInAttackRange(DataSave.Instance.currentObj, DataSave.Instance.targetObj, manager.characters[0].unit.attackRange))
        {
            manager.characters[0].unit.actionPoint--;
            manager.TranState(StateType.EnemyAttack);
        }
        else
        {
            manager.TranState(StateType.EnemyMove);
        }
    }
    public void OnExit()
    {

    }
}
public class EnemyAttack : IState
{
    private TurnBaseFSM manager;
    private Enemy enemy;
    public EnemyAttack(TurnBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {
        enemy = (Enemy)manager.characters[0].unit;
        float attack, defence;
        defence = DataSave.Instance.targetObj.GetComponent<PlayerData>().unit.defense;
        attack = manager.characters[0].unit.attack;
        manager.characters[0].gameObject.transform.LookAt(DataSave.Instance.targetObj.transform);
        DataSave.Instance.targetObj.GetComponent<PlayerData>().unit.hp -= (attack - defence);
        //DataSave.Instance.currentObj.GetComponent<EnemyData>().unit.actionPoint--;

        manager.TranState(StateType.EnemyIdel);
    }
    public void OnUpdata()
    {
       // manager.TranState(StateType.EnemyIdel);
    }
    public void OnExit()
    {

    }
}
public class EnemyMove : IState
{
    private TurnBaseFSM manager;
    private List<PathNode> moveDistance = new List<PathNode>();
    private List<PathNode> Distance = new List<PathNode>();
    private List<PathNode> aroundList = new List<PathNode>();
    private int x2, z2;
    private Enemy enemy;
    public EnemyMove(TurnBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {
        DataSave.Instance.currentObj.GetComponent<EnemyData>().unit.actionPoint--;
        GridManager.Instance.stepGrid.GetGridXZ(DataSave.Instance.currentObj.transform.position, out int x1, out int z1);
        GridManager.Instance.stepGrid.GetGridXZ(DataSave.Instance.targetObj.transform.position, out x2, out z2);
        enemy = (Enemy)manager.characters[0].unit;
        aroundList = GridManager.Instance.pathFinder.CheckAroundNodes(GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2));
        foreach (var a in aroundList)
        {
            if (a.canWalk)
            {
                x2 = a.x;
                moveDistance = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2);
                //MoveVelocity.Instance.SetMoveData(DataSave.Instance.currentObj, moveDistance);
                break;
                
            }
        }
        //for (int i = 0; i<=manager.characters[0].unit.moveSpeed; i++)//只让敌人每次走路走他能走的步数
        //{
        //   Distance.Add(moveDistance[i-1]);
        //}
        if (moveDistance.Count - manager.characters[0].unit.moveSpeed > 1)
        {
            moveDistance.RemoveRange(manager.characters[0].unit.moveSpeed + 1, moveDistance.Count - manager.characters[0].unit.moveSpeed - 1);//范围移除List内部的索引，其中的两个数值分别是开始移除的索引数值，以及移除的总体数量
        }

        MoveVelocity.Instance.SetMoveData(DataSave.Instance.currentObj, moveDistance);
    }
    public void OnUpdata()
    {
        if (MoveVelocity.Instance.moveFinish)//当移动结束时
        {
            if (manager.CheckTargetInAttackRange(DataSave.Instance.currentObj, DataSave.Instance.targetObj, manager.characters[0].unit.attackRange))//玩家在敌人的攻击范围内
            {
                manager.TranState(StateType.EnemyAttack);//进行攻击
            }
            else
            {
                manager.TranState(StateType.EnemyIdel);//回到默认状态
            }
        }
    }
    public void OnExit()
    {

    }
}
