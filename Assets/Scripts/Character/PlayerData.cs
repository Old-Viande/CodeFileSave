using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterData
{   /// <summary>
    /// 该数据只读
    /// </summary>
    [SerializeField]
    private Player player;
    private List<PathNode> aroundList;
    private int x2, z2;
    private int nodeListCount;
    ///private string name;
    void Start()
    {
        Character tampSave;
        DataSave.Instance.tempPlayerSave.TryGetValue(this.name, out unit);      
        //GridManager.Instance.characterData.playerSave.TryGetValue(this.name, out unit);
        unit.actionPoint = unit.maxActionPoint;
        unit.hp = unit.maxHp;
        if (unit != null)
        {
            //DataSave.Instance.players.Add(this.gameObject);
            CreateManager.Instance.AddObj(this.name, this);
            DataSave.Instance.currentPlayers.Add(this.name, this);
        }
        player = (Player)unit;
    }
    private void OnDestroy()//物品被销毁时调用
    {
        if (unit != null)
        {
            //DataSave.Instance.players.Remove(this.gameObject);
            CreateManager.Instance.RemoveObj(this.name);
            DataSave.Instance.currentPlayers.Remove(this.name);
        }
    }

}
