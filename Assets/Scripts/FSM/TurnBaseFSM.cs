using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseFSM : Singleton<TurnBaseFSM>
{
    /// <summary>
    /// �����������ݿ�����Ϊ����FSM�Ĺ�������
    /// </summary>
    public Dictionary<StateType, IState> state = new Dictionary<StateType, IState>();
    public StateType currentStateType;
    public IState currentState;
    public GameObject currentCharacter;
    public Camera mainCamera; 
    public List<CharacterData> characters = new List<CharacterData>();//����غ��н�ɫ�ĵط�


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
    public bool CheckTargetInAttackRange(GameObject currentobj,GameObject target,int attackRange)//�˴�����Ҫ���ģ���ҿ����ֶ������ƶ�
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
        //        if (nodeListCount<=attackRange+1)//����������λ��Χ���κ�һ����������������
        //        {
        //            // ���߾��������ҹ������������Ƚ����ƶ�
        //           // move = nodeListCount > unit.attackRange;

        //            return true;
        //        }
        //    }
        //}        
        // �ж����߼�Ѱ·�����Ƿ�С�ڵ�����ҵĹ�����������߸���֮��      
        pathNode.canWalk = false;
        return false;
    }
}
