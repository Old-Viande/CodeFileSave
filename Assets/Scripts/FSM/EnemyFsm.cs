using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdel : IState
{/// <summary>
/// ���״̬��Ҫ�жϵ����Ƿ���ʣ����ж�ָ
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
        //for (int i = 0; i<=manager.characters[0].unit.moveSpeed; i++)//ֻ�õ���ÿ����·�������ߵĲ���
        //{
        //   Distance.Add(moveDistance[i-1]);
        //}
        if(moveDistance.Count> moveDistance.Count - manager.characters[0].unit.moveSpeed) 
        { 
        moveDistance.RemoveRange(manager.characters[0].unit.moveSpeed, moveDistance.Count-manager.characters[0].unit.moveSpeed);//��Χ�Ƴ�List�ڲ������������е�������ֵ�ֱ��ǿ�ʼ�Ƴ���������ֵ���Լ��Ƴ�����������
        }

        MoveVelocity.Instance.SetMoveData(DataSave.Instance.currentObj, moveDistance);
    }
    public void OnUpdata()
    {
        if (MoveVelocity.Instance.moveFinish)//���ƶ�����ʱ
        {
            if (manager.CheckTargetInAttackRange(DataSave.Instance.currentObj, DataSave.Instance.targetObj, manager.characters[0].unit.attackRange))//����ڵ��˵Ĺ�����Χ��
            {
                manager.TranState(StateType.EnemyAttack);//���й���
            }
            else
            {
                manager.TranState(StateType.EnemyIdel);//�ص�Ĭ��״̬
            }
        }
    }
    public void OnExit()
    {

    }
}
