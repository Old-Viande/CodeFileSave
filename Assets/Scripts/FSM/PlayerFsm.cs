using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdelState : IState
{
    private TurnBaseFSM manager;
    //private Camera camera;
    public IdelState(TurnBaseFSM manager)
    {
        this.manager = manager;
        //camera = GridManager.Instance.camera;
    }
    public void OnEnter()
    {
        if (DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.actionPoint > 0)//�ж�ֵ����ʣ��
        {

        }
        else
        {
            manager.TranState(StateType.Running);
        }
    }
    public void OnUpdata()
    {
        //Ray ray = manager.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        Ray ray = manager.mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, GridManager.Instance.layerMask))
            {
                if (hit.collider.tag == "enemy")
                {
                    DataSave.Instance.targetObj = hit.collider.gameObject;
                    //if (DataSave.Instance.currentObj.GetComponent<PlayerData>().CheckTargetInAttackRange(DataSave.Instance.targetObj, out canMove))
                    //{
                    //    DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.currentState = Character.State.Attack;
                    //    if (canMove)
                    //    {
                    //        manager.TranState(StateType.Move);
                    //    }
                    //    else
                    //    {
                    //        manager.TranState(StateType.Attack);
                    //    }

                    //}
                    if (manager.CheckTargetInAttackRange(DataSave.Instance.currentObj, DataSave.Instance.targetObj, manager.characters[0].unit.attackRange))//判断是否在攻击范围内
                    {
                        manager.TranState(StateType.Attack);
                    }
                    else
                    {
                        manager.TranState(StateType.Move);
                    }
                }
                else if (hit.collider.tag == "floor" || hit.collider.tag == "door")
                {
                    DataSave.Instance.targetObj = hit.collider.gameObject;
                    // if (DataSave.Instance.currentObj.GetComponent<PlayerData>().CheckMove(DataSave.Instance.targetObj, out canMove))
                    // {
                    DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.currentState = Character.State.Move;
                    manager.TranState(StateType.Move);

                    // }
                }
            }
        }
    }
    public void OnExit() { }

}
public class AttackState : IState
{
    private TurnBaseFSM manager;
    private Player player;
    private Enemy enemy;
    public AttackState(TurnBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {

        //if (DataSave.Instance.currentObj.tag != "enemy")
        //{
        player = (Player)manager.characters[0].unit;
        player.currentState = Character.State.Attack;
        float attack, defence;
        attack = DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.attack;
        defence = DataSave.Instance.targetObj.GetComponent<EnemyData>().unit.defense;

        DataSave.Instance.targetObj.GetComponent<EnemyData>().unit.hp -= (attack - defence);
        DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.actionPoint--;
        manager.TranState(StateType.Running);
        //}
        //else
        //{
        //    enemy = (Enemy)DataSave.Instance.currentObj.GetComponent<EnemyData>().unit;
        //    //��    Player -> ��Character   �������player���������Ҳ���player����ֵ��������Character���治�ᶪʧ����
        //    //��������˲�֪��Character�� player ����enemy�������    if��character  is Player���ж��ǲ��� ���� is Enemy 
        //    float attack, defence;
        //    defence = DataSave.Instance.targetObj.GetComponent<PlayerData>().unit.defense;
        //    attack = DataSave.Instance.currentObj.GetComponent<EnemyData>().unit.attack;

        //    DataSave.Instance.targetObj.GetComponent<PlayerData>().unit.hp -= (attack - defence);
        //    DataSave.Instance.currentObj.GetComponent<EnemyData>().unit.actionPoint--;
        //    manager.TranState(StateType.Running);
        //}
        //if(DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.attackRange)
    }
    public void OnUpdata()
    {

    }
    public void OnExit() { }

}
public class MoveState : IState
{
    private TurnBaseFSM manager;
    private List<PathNode> aroundList;
    private int nodeListCount;
    private Player player;
    private int x2, z2;
    public MoveState(TurnBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {
        manager.characters[0].unit.actionPoint--;//只要进入移动，先减少一点行动点
        GridManager.Instance.stepGrid.GetGridXZ(DataSave.Instance.currentObj.transform.position, out int x1, out int z1);
        GridManager.Instance.stepGrid.GetGridXZ(DataSave.Instance.targetObj.transform.position, out x2, out z2);

        if (GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2).canWalk)//如果选中格可以行走
        {
            MoveVelocity.Instance.SetMoveData(DataSave.Instance.currentObj, GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2));
        }
        else
        {
            //����Ѱ·�в����ڲ���ȥ�ĵ㣬�����Ѱ·ֻ�ܱ������ҳ�ͨ������ǰ��������·�����д�����
            aroundList = GridManager.Instance.pathFinder.CheckAroundNodes(GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2));
            foreach (var a in aroundList)
            {
                if (a.canWalk)
                {
                    x2 = a.x;
                    MoveVelocity.Instance.SetMoveData(DataSave.Instance.currentObj, GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2));
                    break;
                    //这里是制作失败的代码，原本的功能是按照走的距离进行行动点扣除，在DEMO版本这个功能暂时取消
                    //if (DataSave.Instance.currentObj.tag != "enemy")
                    //{
                    //    DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.actionPoint -= Mathf.FloorToInt(GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count / DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.moveSpeed);
                    //}
                    //else
                    //{
                    //    if (GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count < DataSave.Instance.currentObj.GetComponent<EnemyData>().unit.moveSpeed + 1)//������˵��ƶ��ľ������ڵ��˵ĵ����ƶ���Χ���򸽴�һ�ι����������ж�ֵ�۹�
                    //    {
                    //        manager.TranState(StateType.Attack);
                    //    }
                    //    else
                    //    {
                    //        DataSave.Instance.currentObj.GetComponent<EnemyData>().unit.actionPoint--;
                    //    }

                    //}
                }
            }

        }
    }
    public void OnUpdata()
    {
        if (MoveVelocity.Instance.moveFinish)
        {
            if (DataSave.Instance.currentObj.tag != "enemy")
            {
                if (DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.currentState == Character.State.Attack)
                {
                    manager.TranState(StateType.Attack);
                }
                else
                {
                    manager.TranState(StateType.Running);
                }
            }
        }


    }
    public void OnExit() { }

}
