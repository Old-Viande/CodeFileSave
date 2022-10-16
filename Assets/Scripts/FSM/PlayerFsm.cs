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
        SkillManager.Instance.Init();
        if (DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.actionPoint > 0)//行动点大于等于0就结束回合
        {

        }
        else
        {
            manager.TranState(StateType.Running);
        }
    }
    public void OnUpdata()
    {        
        Ray ray = manager.mainCamera.ScreenPointToRay(Input.mousePosition);

        if (UpdataManager.Instance.moveButtonPushed)//点击移动按钮后才可以开始移动
        {
             LineRendererScript.Instance.StartPosSet();//移动前先显示线指示器
            RaycastHit hit;
           if( Physics.Raycast(ray, out hit, Mathf.Infinity, GridManager.Instance.layerMask))
            {
            LineRendererScript.Instance.EndPosSet(hit.point);//只要射线集中了碰撞体就有导航，后期需要改成范围约束
            }
            if (Input.GetMouseButtonDown(0))
            {
               
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, GridManager.Instance.layerMask))
                {
                    DataSave.Instance.targetObj = hit.collider.gameObject;
                    LineRendererScript.Instance.EndLine();
                     manager.TranState(StateType.Move);
                }
             

            }
        }else if (UpdataManager.Instance.skillUseButton)
        {
            manager.TranState(StateType.Skill);//按下确切的技能时，进入技能状态
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
        manager.TranState(StateType.Idel);
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
/// <summary>
/// 使用技能时的状态
/// </summary>
public class SkillState : IState
{
    private TurnBaseFSM manager;
    private Player player;
    private Enemy enemy;
    private SkillData tempSkill;
    public SkillState(TurnBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {
     
    }
    public void OnUpdata()
    {
        if (GridManager.Instance.skillReadyToUse)//节能无法使用则退出状态
        {
            //manager.TranState(StateType.Idel);
        }
        else
        {
            manager.TranState(StateType.Idel);
        }
    }
    public void OnExit() 
    {
        DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.actionPoint--;
        UpdataManager.Instance.skillUseButton = false;//退出技能时，将技能使用状态切换为关闭
        UIManager.Instance.PopPanel();//UI层关闭目前的UI
    }

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

    private bool IsTargetObjectCanInteract( GameObject obj )
    {
        return obj.CompareTag("enemy");
    }

    public void OnEnter()
    {
        manager.characters[0].unit.actionPoint--;//只要进入移动，先减少一点行动点
        GridManager.Instance.stepGrid.GetGridXZ(DataSave.Instance.currentObj.transform.position, out int x1, out int z1);
        GridManager.Instance.stepGrid.GetGridXZ(DataSave.Instance.targetObj.transform.position, out x2, out z2);

        MoveVelocity.Instance.SetMoveData(DataSave.Instance.currentObj, GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2, IsTargetObjectCanInteract(DataSave.Instance.targetObj)));
        //if (GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2).canWalk)//如果选中格可以行走
        //{
        //    MoveVelocity.Instance.SetMoveData(DataSave.Instance.currentObj, GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2));
        //}
        //else
        //{
        //    //����Ѱ·�в����ڲ���ȥ�ĵ㣬�����Ѱ·ֻ�ܱ������ҳ�ͨ������ǰ��������·�����д�����
        //    aroundList = GridManager.Instance.pathFinder.CheckAroundNodes(GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2));
        //    foreach (var a in aroundList)
        //    {
        //        if (a.canWalk)
        //        {
        //            x2 = a.x;
        //            MoveVelocity.Instance.SetMoveData(DataSave.Instance.currentObj, GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2));
        //            break;
        //            //这里是制作失败的代码，原本的功能是按照走的距离进行行动点扣除，在DEMO版本这个功能暂时取消
        //            //if (DataSave.Instance.currentObj.tag != "enemy")
        //            //{
        //            //    DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.actionPoint -= Mathf.FloorToInt(GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count / DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.moveSpeed);
        //            //}
        //            //else
        //            //{
        //            //    if (GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count < DataSave.Instance.currentObj.GetComponent<EnemyData>().unit.moveSpeed + 1)//������˵��ƶ��ľ������ڵ��˵ĵ����ƶ���Χ���򸽴�һ�ι����������ж�ֵ�۹�
        //            //    {
        //            //        manager.TranState(StateType.Attack);
        //            //    }
        //            //    else
        //            //    {
        //            //        DataSave.Instance.currentObj.GetComponent<EnemyData>().unit.actionPoint--;
        //            //    }

        //            //}
        //        }
        //    }

        //}
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
    public void OnExit() 
    {
        UpdataManager.Instance.moveButtonPushed = false;
    }

}
