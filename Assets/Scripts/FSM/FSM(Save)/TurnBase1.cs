using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*public class StartState : IState
{
    private TurnBaseFSM manager;
    public StartState(TurnBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter()
    {
        //DataSave.Instance.SaveData();
    }
    public void OnUpdata()
    {
        DataSave.Instance.SaveData();
        manager.TranState(StateType.Running);
    }
    public void OnExit() { }

}
public class RuningState : IState
{
    private TurnBaseFSM manager;
    private int index;
    private List<Character> characters = new List<Character>();
    private Camera camera;
    private Character currentCharacter;
    private Player player;
    private bool canMove;
    public RuningState(TurnBaseFSM manager)
    {
        this.manager = manager;

    }
    public void OnEnter()
    {
        index = 0;
        characters = DataSave.Instance.characters;
        camera = GridManager.Instance.camera;
        if (characters[0].actionPoint == 0)//如果当前单位的行动值为空但血量仍有剩余，则将其移除，并重新加回队列
        {
            currentCharacter = characters[0];
            characters.Remove(characters[0]);
            if (currentCharacter.hp > 0)
            {
                currentCharacter.actionPoint = currentCharacter.maxActionPoint;//将被移除单位的行动值恢复满
                  characters.Add(currentCharacter);//这段逻辑经过断点测试确实生效
            }

        }
       
    }
    public void OnUpdata()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (characters != null)
        {
            if (characters[index].isPlayer)//检测是否这个单位是可控制单位
            {
                if (Input.GetMouseButtonDown(0))
                 {
                     RaycastHit hit;
                     if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                     {
                         if (hit.collider.name == characters[index].name)
                         {
                             //这里要判断玩家的选择，并且传相应的参数到对应的状态
                             //队列里的可操控单位执行过行为后，先移除，然后再重新加入队列
                             GridManager.Instance.characterData.playerSave.TryGetValue(characters[index].name, out DataSave.Instance.currentPlayer);
                             player = DataSave.Instance.currentPlayer;
                             DataSave.Instance.currentObj = hit.collider.gameObject;
                         }
                         else if (player != null)
                         {
                             if (hit.collider.tag == "enemy")
                             {
                                 DataSave.Instance.targetObj = hit.collider.gameObject;
                                 if(DataSave.Instance.currentObj.GetComponent<PlayerData>().CheckTargetInAttackRange(DataSave.Instance.targetObj,out move ) )
                                 {
                                     DataSave.Instance.currentObj.GetComponent<PlayerData>().player.currentState = Character.State.Attack;
                                     if ( move )
                                     {
                                         manager.TranState( StateType.Move );                                        
                                     }
                                     else
                                     {
                                         manager.TranState( StateType.Attack );
                                     }
                                 }
                             }
                             else
                             if (hit.collider.tag == "floor")
                             {
                                 manager.TranState(StateType.Move);
                             }
                         }
                     }

                 }
                DataSave.Instance.objSave.TryGetValue(characters[index].name, out DataSave.Instance.currentObj);
                DataSave.Instance.currentObj.GetComponent<PlayerData>().player.currentState = Character.State.Idle;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    DataSave.Instance.currentObj.GetComponent<PlayerData>().player.actionPoint = 0;
                    manager.TranState(StateType.Move);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.tag == "enemy")
                        {
                            DataSave.Instance.targetObj = hit.collider.gameObject;
                            if (DataSave.Instance.currentObj.GetComponent<PlayerData>().CheckTargetInAttackRange(DataSave.Instance.targetObj, out canMove))
                            {
                                DataSave.Instance.currentObj.GetComponent<PlayerData>().player.currentState = Character.State.Attack;
                                if (canMove)
                                {
                                    manager.TranState(StateType.Move);
                                }
                                else
                                {
                                    manager.TranState(StateType.Attack);
                                }

                            }

                            //manager.TranState(StateType.Attack);
                        }
                        else if (hit.collider.tag == "floor")
                        {
                            DataSave.Instance.targetObj = hit.collider.gameObject;
                            if (DataSave.Instance.currentObj.GetComponent<PlayerData>().CheckMove(DataSave.Instance.targetObj, out canMove))
                            {
                                DataSave.Instance.currentObj.GetComponent<PlayerData>().player.currentState = Character.State.Move;
                                manager.TranState(StateType.Move);

                            }
                        }
                    }
                }
            }
            else
            {
                DataSave.Instance.objSave.TryGetValue(characters[index].name, out DataSave.Instance.currentObj);
                DataSave.Instance.targetObj = CreateManager.Instance.player;
               
                if (DataSave.Instance.currentObj.GetComponent<EnemyData>().CheckEnemyMove(DataSave.Instance.targetObj))//结果为真则需要移动
                {                 
                        manager.TranState(StateType.Move);                  
                }
                else
                {
                    manager.TranState(StateType.Attack);
                }
            
            }
        }
    }

    public void OnExit() { }

}
public class EndState : IState
{
    private TurnBaseFSM manager;
    public EndState(TurnBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter() { }
    public void OnUpdata() { }
    public void OnExit() { }

}
public class CheckState : IState
{
    private TurnBaseFSM manager;
    public CheckState(TurnBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter() { }
    public void OnUpdata() { }
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
        //DataSave.Instance.characters[0].isPlayer()
        if (DataSave.Instance.currentObj.tag != "enemy")
        {
            player = DataSave.Instance.currentObj.GetComponent<PlayerData>().player;
            float attack, defence;
            attack = DataSave.Instance.currentObj.GetComponent<PlayerData>().player.attack;
            defence = DataSave.Instance.targetObj.GetComponent<EnemyData>().enemy.defense;

            DataSave.Instance.targetObj.GetComponent<EnemyData>().enemy.hp -= (attack - defence);
            DataSave.Instance.currentObj.GetComponent<PlayerData>().player.actionPoint--;
            manager.TranState(StateType.Running);
        }
        else
        {
            enemy = DataSave.Instance.currentObj.GetComponent<EnemyData>().enemy;

            float attack, defence;
            defence = DataSave.Instance.targetObj.GetComponent<PlayerData>().player.defense;
            attack = DataSave.Instance.currentObj.GetComponent<EnemyData>().enemy.attack;

            DataSave.Instance.targetObj.GetComponent<PlayerData>().player.hp -= (attack - defence);
            DataSave.Instance.currentObj.GetComponent<EnemyData>().enemy.actionPoint--;
            manager.TranState(StateType.Running);
        }
    }
    public void OnUpdata()
    {
        if (player != null)
        {
            if (Input.GetMouseButtonDown(0) && player.actionPoint > 0)
            {
                Ray ray = GridManager.Instance.camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject.tag == "enemy")
                    {
                        float attack, defence;
                        attack = DataSave.Instance.currentObj.GetComponent<PlayerData>().player.attack;
                        defence = DataSave.Instance.targetObj.GetComponent<EnemyData>().enemy.defense;

                        DataSave.Instance.targetObj.GetComponent<EnemyData>().enemy.hp -= (attack-defence);
                        DataSave.Instance.currentObj.GetComponent<PlayerData>().player.actionPoint--;
                    }
                }
            }
        }
        else
        {

        }
        
    }
    public void OnExit() { }

}
public class DeffenceState : IState
{
    private TurnBaseFSM manager;
    public DeffenceState(TurnBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter() { }
    public void OnUpdata() { }
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
        if (DataSave.Instance.currentObj.tag != "enemy")
        {
            if (DataSave.Instance.currentObj.GetComponent<PlayerData>().player.actionPoint == 0)
            {
                manager.TranState(StateType.Running);
            }
        }
       
        GridManager.Instance.smap.GetGridXZ(DataSave.Instance.currentObj.transform.position, out int x1, out int z1);
        GridManager.Instance.smap.GetGridXZ(DataSave.Instance.targetObj.transform.position, out x2, out z2);
        //GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2);
        if (GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2).canWalk)//如果输入的最终坐标是不可行走的，则用测试寻路网格找到通往这个坐标的路径，然后停在抵达目标的点之前
        {
            MoveVelocity.Instance.SetMoveData(DataSave.Instance.currentObj, GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2));
        }
        else
        {
            // MoveVelocity.Instance.SetMoveData(DataSave.Instance.currentObj, GridManager.Instance.pathFinderTest.FindPath(x1, z1, x2, z2));//测试寻路中不存在不能去的点，但这个寻路只能被用来找出通往不可前往地区的路径，有待更改
            aroundList = GridManager.Instance.pathFinder.CheckAroundNodes(GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2));
            foreach (var a in aroundList)
            {
                if (a.canWalk)
                {
                    x2 = a.x;
                    z2 = a.z;
                    //int nodeListCount = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count;
                    //if (nodeListCount <= player.attackRange + (player.actionPoint > 0 ? player.actionPoint - 1 : player.actionPoint) * player.moveSpeed)//如果在这个点位周围有任何一个点是满足条件的
                    //{
                    //    MoveVelocity.Instance.SetMoveData(DataSave.Instance.currentObj, GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2));
                    //}
                    MoveVelocity.Instance.SetMoveData(DataSave.Instance.currentObj, GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2));
                    if (DataSave.Instance.currentObj.tag != "enemy")
                    {
                        DataSave.Instance.currentObj.GetComponent<PlayerData>().player.actionPoint -= Mathf.FloorToInt(GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count / DataSave.Instance.currentObj.GetComponent<PlayerData>().player.moveSpeed);
                    }
                    else
                    {if(GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count< DataSave.Instance.currentObj.GetComponent<EnemyData>().enemy.moveSpeed + 1)//如果敌人的移动的距离少于敌人的单次移动范围，则附带一次攻击，否则行动值扣光
                        {
                            manager.TranState(StateType.Attack);
                        }
                        else
                        {
                            DataSave.Instance.currentObj.GetComponent<EnemyData>().enemy.actionPoint--;
                        }
                     
                    }                  
                }
            }

        }
        /*if (DataSave.Instance.currentObj.tag != "enemy")
        {
            player = DataSave.Instance.currentObj.GetComponent<PlayerData>().player;
            if (GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count < player.actionPoint * player.moveSpeed)
            {

            }
        }
        
    }
    public void OnUpdata()
    {
        if (MoveVelocity.Instance.moveFinish)
        {
            if (DataSave.Instance.currentObj.tag != "enemy")
            {
                if (DataSave.Instance.currentObj.GetComponent<PlayerData>().player.currentState == Character.State.Attack)
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
*/
