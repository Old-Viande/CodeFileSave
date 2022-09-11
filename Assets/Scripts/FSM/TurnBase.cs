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
        //DataSave.Instance.SaveSoData();
        DataSave.Instance.SaveData();
        manager.TranState(StateType.Running);
    }
    public void OnExit() { }

}
public class RuningState : IState
{
    private TurnBaseFSM manager;
    private int index;
    // private List<Character> characters = new List<Character>();
    private List<CharacterData> characters = new List<CharacterData>();
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
        characters= DataSave.Instance.valueSave;
       
        camera = GridManager.Instance.camera;
        if (characters[0].unit.actionPoint == 0)//�����ǰ��λ���ж�ֵΪ�յ�Ѫ������ʣ�࣬�����Ƴ��������¼ӻض���
        {
            currentCharacter = characters[0].unit;

            //CharacterData tempCharacters = characters[0];
            characters.Remove(characters[0]);
            //������Ŀɲٿص�λִ�й���Ϊ�����Ƴ���Ȼ�������¼������              
            //if (currentCharacter.hp > 0)
            //{
            //    currentCharacter.actionPoint = currentCharacter.maxActionPoint;//�����Ƴ���λ���ж�ֵ�ָ���
            //    characters.Add(tempCharacters);//����߼������ϵ����ȷʵ��Ч
            //}
        }

    }
    public void OnUpdata()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (characters != null)
        {//�����Ϊ��
            DataSave.Instance.currentObj = characters[index].gameObject;
            if (characters[index].unit is Player)//����Ƿ������λ�ǿɿ��Ƶ�λ
            {             
                             //����Ҫ�ж���ҵ�ѡ�񣬲��Ҵ���Ӧ�Ĳ�������Ӧ��״̬
                              
                DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.currentState = Character.State.Idle;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.actionPoint = 0;
                    manager.TranState(StateType.Move);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity,GridManager.Instance.layerMask))
                    {
                        if (hit.collider.tag == "enemy")
                        {
                            DataSave.Instance.targetObj = hit.collider.gameObject;
                            if (DataSave.Instance.currentObj.GetComponent<PlayerData>().CheckTargetInAttackRange(DataSave.Instance.targetObj, out canMove))
                            {
                                DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.currentState = Character.State.Attack;
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
                        else if (hit.collider.tag == "floor"|| hit.collider.tag == "door")
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
            else
            {
                DataSave.Instance.objSave.TryGetValue(characters[index].name, out DataSave.Instance.currentObj);
                DataSave.Instance.targetObj = CreateManager.Instance.player;
               
                if (DataSave.Instance.currentObj.GetComponent<EnemyData>().CheckEnemyMove(DataSave.Instance.targetObj))//���Ϊ������Ҫ�ƶ�
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
            player = (Player)DataSave.Instance.currentObj.GetComponent<PlayerData>().unit;
            float attack, defence;
            attack = DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.attack;
            defence = DataSave.Instance.targetObj.GetComponent<EnemyData>().unit.defense;

            DataSave.Instance.targetObj.GetComponent<EnemyData>().unit.hp -= (attack - defence);
            DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.actionPoint--;
            manager.TranState(StateType.Running);
        }
        else
        {
            enemy = (Enemy)DataSave.Instance.currentObj.GetComponent<EnemyData>().unit;
       //��    Player -> ��Character   �������player���������Ҳ���player����ֵ��������Character���治�ᶪʧ����
       //��������˲�֪��Character�� player ����enemy�������    if��character  is Player���ж��ǲ��� ���� is Enemy 
            float attack, defence;
            defence = DataSave.Instance.targetObj.GetComponent<PlayerData>().unit.defense; 
            attack = DataSave.Instance.currentObj.GetComponent<EnemyData>().unit.attack;

            DataSave.Instance.targetObj.GetComponent<PlayerData>().unit.hp -= (attack - defence);
            DataSave.Instance.currentObj.GetComponent<EnemyData>().unit.actionPoint--;
            manager.TranState(StateType.Running);
        }
    }
    public void OnUpdata()
    {       
        
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
            if (DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.actionPoint == 0)
            {
                manager.TranState(StateType.Running);
            }
        }
       
        GridManager.Instance.stepGrid.GetGridXZ(DataSave.Instance.currentObj.transform.position, out int x1, out int z1);
        GridManager.Instance.stepGrid.GetGridXZ(DataSave.Instance.targetObj.transform.position, out x2, out z2);
   
        if (GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2).canWalk)//�����������������ǲ������ߵģ����ò���Ѱ·�����ҵ�ͨ����������·����Ȼ��ͣ�ڵִ�Ŀ��ĵ�֮ǰ
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
                    if (DataSave.Instance.currentObj.tag != "enemy")
                    {
                        DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.actionPoint -= Mathf.FloorToInt(GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count / DataSave.Instance.currentObj.GetComponent<PlayerData>().unit.moveSpeed);
                    }
                    else
                    {if(GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count< DataSave.Instance.currentObj.GetComponent<EnemyData>().unit.moveSpeed + 1)//������˵��ƶ��ľ������ڵ��˵ĵ����ƶ���Χ���򸽴�һ�ι����������ж�ֵ�۹�
                        {
                            manager.TranState(StateType.Attack);
                        }
                        else
                        {
                            DataSave.Instance.currentObj.GetComponent<EnemyData>().unit.actionPoint--;
                        }
                     
                    }                  
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
