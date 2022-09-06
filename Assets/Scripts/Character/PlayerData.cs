using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterData
{   /// <summary>
    /// ������ֻ��
    /// </summary>
    [SerializeField]
    private Player player;
    private List<PathNode> aroundList;
    private int x2, z2;
    private int nodeListCount;
    ///private string name;
    void Start()
    {

        GridManager.Instance.characterData.playerSave.TryGetValue(this.name, out unit);
        unit.actionPoint = unit.maxActionPoint;
        unit.hp = unit.maxHp;
        if (unit != null)
        {
            //DataSave.Instance.players.Add(this.gameObject);
            CreateManager.Instance.AddObj(this.name, this);
        }
        player = (Player)unit;
    }
    private void OnDestroy()//��Ʒ������ʱ����
    {
        if (unit != null)
        {
            //DataSave.Instance.players.Remove(this.gameObject);
            CreateManager.Instance.RemoveObj(this.name);

        }
    }

    public bool CheckTargetInAttackRange(GameObject target, out bool move)//�˴�����Ҫ���ģ���ҿ����ֶ������ƶ�
    {
        move = false;
        if (unit.attackRange == 0)
        {

            return false;
        }

        GridManager.Instance.smap.GetGridXZ(this.transform.position, out int x1, out int z1);
        GridManager.Instance.smap.GetGridXZ(target.transform.position, out int x2, out int z2);
        if (GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2).canWalk)
        {
            nodeListCount = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count;
            if (nodeListCount <= unit.attackRange + (unit.actionPoint > 0 ? unit.actionPoint - 1 : unit.actionPoint) * unit.moveSpeed)//ֱ���ƶ�
            {
                // ���߾��������ҹ������������Ƚ����ƶ�
                move = nodeListCount > unit.attackRange;
                return true;
            }
        }
        else
        {
            aroundList = GridManager.Instance.pathFinder.CheckAroundNodes(GridManager.Instance.pathFinder.GetGrid().GetValue(x2, z2));
            foreach (var a in aroundList)
            {
                if (a.canWalk)
                {
                    x2 = a.x;
                    z2 = a.z;
                    nodeListCount = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count;
                    if (nodeListCount <= unit.attackRange + (unit.actionPoint > 0 ? unit.actionPoint - 1 : unit.actionPoint) * unit.moveSpeed)//����������λ��Χ���κ�һ����������������
                    {
                        // ���߾��������ҹ������������Ƚ����ƶ�
                        move = nodeListCount > unit.attackRange;

                        return true;
                    }
                }
            }
        }
        // �ж����߼�Ѱ·�����Ƿ�С�ڵ�����ҵĹ�����������߸���֮��      
        return false;
    }
    public bool CheckMove(GameObject target, out bool canMove)//�����������������ܲ����ߵ�ѡ���λ�õ�,Ѱ·�ĸ��ӻ�����ʼ��һ����㣬����+1�ȽϺ���
    {
        canMove = false;
        if (unit.actionPoint == 0)
        {
            return false;
        }
        GridManager.Instance.smap.GetGridXZ(this.transform.position, out int x1, out int z1);
        GridManager.Instance.smap.GetGridXZ(target.transform.position, out int x2, out int z2);
        int nodeListCount = GridManager.Instance.pathFinderTest.FindPath(x1, z1, x2, z2).Count;//�ò����������·������
        if (nodeListCount <= unit.actionPoint * unit.moveSpeed + 1)
        {
            return true;
        }
        return false;
    }
}
