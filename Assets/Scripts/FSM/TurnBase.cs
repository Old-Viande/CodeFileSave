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
        manager.characters = DataSave.Instance.valueSave;
        manager.TranState(StateType.Running);
    }
    public void OnExit() { }

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
        if (manager.characters[0].unit.actionPoint <= 0)//�����ǰ��λ���ж�ֵΪ�յ�Ѫ������ʣ�࣬�����Ƴ��������¼ӻض���
        {
            currentCharacter = manager.characters[0].unit;
            currentCharacter.actionPoint = currentCharacter.maxActionPoint;
            //CharacterData tempCharacters = manager.characters[0];
            manager.characters.Remove(manager.characters[0]);
            //������Ŀɲٿص�λִ�й���Ϊ�����Ƴ���Ȼ�������¼������              
            //if (currentCharacter.hp > 0)
            //{
            //    //�����Ƴ���λ���ж�ֵ�ָ���
            //    manager.characters.Add(tempCharacters);//����߼������ϵ����ȷʵ��Ч
            //}
        }
        if (manager.characters.Count == 0)//������û������ʱ���͸����������ˣ�����ζ���Ѿ�����һ����
        {
            manager.TranState(StateType.Start);
        }

    }
    public void OnUpdata()
    {

        if (manager.characters.Count>0)
        {//�����Ϊ��
            DataSave.Instance.currentObj = manager.characters[index].gameObject;//��ǰ������ڻغ�˳���һ����λ
            if (manager.characters[index].unit is Player)//����Ƿ������λ�ǿɿ��Ƶ�λ
            {
                //����Ҫ�ж���ҵ�ѡ�񣬲��Ҵ���Ӧ�Ĳ�������Ӧ��״̬

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

