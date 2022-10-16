using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseFSM : Singleton<TurnBaseFSM>
{
    /// <summary>
    /// 这里存入的数据可以作为所有FSM的共享数据
    /// </summary>
    public Dictionary<StateType, IState> state = new Dictionary<StateType, IState>();
    public StateType currentStateType;
    public IState currentState;
    public GameObject currentCharacter;
    public Camera mainCamera; 
    public List<CharacterData> characters = new List<CharacterData>();//储存回合中角色的地方


    protected virtual void Start()
    {
        state.Add(StateType.Start, new StartState(this));
        state.Add(StateType.Running, new RuningState(this));
        state.Add(StateType.End, new EndState(this));
        state.Add(StateType.Check, new CheckState(this));
        state.Add(StateType.Move, new MoveState(this));
        state.Add(StateType.Attack, new AttackState(this));
        state.Add(StateType.Idel, new IdelState(this));
        state.Add(StateType.EnemyAttack, new EnemyAttack(this));
        state.Add(StateType.EnemyIdel, new EnemyIdel(this));
        state.Add(StateType.EnemyMove, new EnemyMove(this));
        state.Add(StateType.Skill, new SkillState(this));
        mainCamera = GridManager.Instance.camera;
        TranState(StateType.Start);
    }

    private void Update()
    {
        currentState.OnUpdata();
    }

    public void TranState(StateType type)
    {
        if (currentStateType == type || type == StateType.Unknown)
        {
            return;
        }
        if (currentStateType != StateType.Unknown)
            currentState.OnExit();
        currentStateType = type;
        currentState = state[type];
        currentState.OnEnter();
    }
    public bool CheckTargetInAttackRange(GameObject currentobj,GameObject target,int attackRange)//此代码需要更改，玩家可以手动控制移动
    {
        int nodeListCount;
        List<PathNode> aroundList = new List<PathNode>();
       
        GridManager.Instance.stepGrid.GetGridXZ(currentobj.transform.position, out int x1, out int z1);
        GridManager.Instance.stepGrid.GetGridXZ(target.transform.position, out int x2, out int z2);

        var pathNode = GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2);
        pathNode.canWalk = true;
        nodeListCount = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2, true).Count;
        if (nodeListCount <= attackRange)
        {
            pathNode.canWalk = false;
            return true;
        }
        //aroundList = GridManager.Instance.pathFinder.CheckAroundNodes(GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2));
        //foreach (var a in aroundList)
        //{
        //    if (a.canWalk)
        //    {
        //        x2 = a.x;
        //        z2 = a.z;
        //        nodeListCount = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count;
        //        if (nodeListCount<=attackRange+1)//如果在这个点位周围有任何一个点是满足条件的
        //        {
        //            // 两者距离大于玩家攻击距离则还需先进行移动
        //           // move = nodeListCount > unit.attackRange;

        //            return true;
        //        }
        //    }
        //}        
        // 判定两者间寻路距离是否小于等于玩家的攻击距离与可走格数之和      
        pathNode.canWalk = false;
        return false;
    }
}
