using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : Singleton<CreateManager>
{
    public Character_SO characters;
    public Enemy_SO enemies;
    public List<GameObject> items = new List<GameObject>();//存有当前场上所有角色和敌人单位
    public GameObject player;
    public List<GameObject> enemy= new List<GameObject>();
    public Dictionary<string, CharacterData> unitSave = new Dictionary<string, CharacterData>();
  
    

    void Start()
    {
       player = Instantiate(characters.characters[0].Item, Vector3.zero, Quaternion.identity);
       player.name = characters.characters[0].name;
        DataSave.Instance.AddObject(player.name, player);
        enemy.Add(Instantiate(enemies.enemies[0].Item, GridManager.Instance.smap.GetGridCenter(11, 10), Quaternion.identity));
        enemy[0].name = enemies.enemies[0].name;
        DataSave.Instance.AddObject(enemy[0].name, enemy[0]);
        GridManager.Instance.pathFinder.GetGrid().GetValue(11,10).canWalk = false;
       

    }
    public void AddObj(string name, CharacterData data)
    {
        unitSave.Add(name, data);
    }
    public void RemoveObj(string name)
    {
        unitSave.Remove(name);
    }


   /* public void AddObject(GameObject item,Vector3 position)//调用时需要在调用处更改物体的名称
    {
        items.Add(item);
        Instantiate(item, position, Quaternion.identity);        
    }
    public void RemoveObject(GameObject item)
    {
        items.Remove(item);
        Destroy(item);
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
