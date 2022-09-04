using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState : IState
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
    private List<Character> players = new List<Character>();
    private Camera camera;
    private bool clicked;
    private Player player;
    private bool move;
    public RuningState(TurnBaseFSM manager)
    {
        this.manager = manager;
        clicked = false;
    }
    public void OnEnter()
    {
        index = 0;
        players = DataSave.Instance.characters;
        camera = GridManager.Instance.camera;
    }
    public void OnUpdata()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (players != null)
        {
            if (players[index].isPlayer)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.name == players[index].name)
                        {
                            //这里要判断玩家的选择，并且传相应的参数到对应的状态
                            //队列里的可操控单位执行过行为后，先移除，然后再重新加入队列
                            GridManager.Instance.characterData.playerSave.TryGetValue(players[index].name, out DataSave.Instance.currentPlayer);
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
    
    public AttackState(TurnBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter() 
    {
        //DataSave.Instance.characters[0].isPlayer()
    }
    public void OnUpdata() { }
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
    public MoveState(TurnBaseFSM manager)
    {
        this.manager = manager;
    }
    public void OnEnter() 
    {
        GridManager.Instance.smap.GetGridXZ(DataSave.Instance.currentObj.transform.position, out int x1, out int z1);
        GridManager.Instance.smap.GetGridXZ(DataSave.Instance.targetObj.transform.position, out int x2, out int z2);
        
               //GridManager.Instance.pathFinder.FindPath()
        
       
    }
    public void OnUpdata() { }
    public void OnExit() { }

}