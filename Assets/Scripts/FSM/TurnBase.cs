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
        //初始化

    }
    public void OnUpdata()
    {
        //DataSave.Instance.SaveSoData();
        DataSave.Instance.SaveData();
        manager.characters = DataSave.Instance.valueSave;
        manager.TranState(StateType.Running);
    }
    public void OnExit() 
    {

    }

}
public class RuningState : IState
{
    private TurnBaseFSM manager;
    private int index;
    // private List<Character> characters = new List<Character>();

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
        if (manager.characters[0].unit.actionPoint <= 0)//如果当前单位的行动值为空但血量仍有剩余，则将其移除，并重新加回队列
        {
            currentCharacter = manager.characters[0].unit;
            currentCharacter.actionPoint = currentCharacter.maxActionPoint;
            //CharacterData tempCharacters = manager.characters[0];
            manager.characters.Remove(manager.characters[0]);
            //队列里的可操控单位执行过行为后，先移除，然后再重新加入队列              
            //if (currentCharacter.hp > 0)
            //{
            //    //将被移除单位的行动值恢复满
            //    manager.characters.Add(tempCharacters);//这段逻辑经过断点测试确实生效
            //}
        }
        if (manager.characters.Count == 0)//队列中没有物体时，就该重新排序了，这意味着已经走完一轮了
        {
            manager.TranState(StateType.Start);
        }

    }
    public void OnUpdata()
    {
        if (UpdataManager.Instance.eventHappen)
            return;

        if (manager.characters.Count>0)
        {//如果不为空
            DataSave.Instance.currentObj = manager.characters[index].gameObject;//当前物体等于回合顺序第一个单位
            //这个位置留给buff判断
            manager.characters[index].buffManager.OnRoundStart(manager.characters[index]);

            if (manager.characters[index].unit is Player)//检测是否这个单位是可控制单位
            {
                //这里要判断玩家的选择，并且传相应的参数到对应的状态

                DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.currentState = Character.State.Idle;
                //if (Input.GetKeyDown(KeyCode.Space))
                //{
                //    DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.actionPoint = 0;
                //    manager.TranState(StateType.Move);
                //}
                manager.TranState(StateType.Idel);
            }
            else
            {
                //Instance.objSave.TryGetValue(manager.characters[index].name, out DataSave.Instance.currentObj);
                //DataSave.Instance.currentObj = 
                DataSave.Instance.targetObj = CreateManager.Instance.player;

                manager.TranState(StateType.EnemyIdel);
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

