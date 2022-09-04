using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterData
{
    public Player player;   

    ///private string name;
    void Start()
    {
        
        GridManager.Instance.characterData.playerSave.TryGetValue(this.name,out player);
        if (player != null)
        {
            DataSave.Instance.players.Add(player);
        }
      
    }
    private void OnDestroy()//��Ʒ������ʱ����
    {
        if (player != null)
        {
            DataSave.Instance.players.Remove(player);
        }
    }

	public bool CheckTargetInAttackRange( GameObject target, out bool move )
    {
        move = false;
        if ( player.attackRange == 0)
        {
            return false;
        }

        GridManager.Instance.smap.GetGridXZ( this.transform.position, out int x1, out int z1 );
        GridManager.Instance.smap.GetGridXZ( target.transform.position, out int x2, out int z2);
        int nodeListCount = GridManager.Instance.pathFinder.FindPath(x1, z1, x2, z2).Count;

        // �ж����߼�Ѱ·�����Ƿ�С�ڵ�����ҵĹ�����������߸���֮��
        if ( nodeListCount <= player.attackRange + ( player.actionPoint > 0 ? player.actionPoint - 1 : player.actionPoint ) * player.speed )
        {
            // ���߾��������ҹ������������Ƚ����ƶ�
            move = nodeListCount > player.attackRange;
            return true;
        }

		return false;
    }
}
