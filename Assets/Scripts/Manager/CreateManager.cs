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
    

    void Start()
    {
       player = Instantiate(characters.characters[0].Item, Vector3.zero, Quaternion.identity);
       player.name = characters.characters[0].name;
        enemy.Add(Instantiate(enemies.enemies[0].Item, GridManager.Instance.smap.GetGridCenter(12, 12), Quaternion.identity));
        enemy[0].name = enemies.enemies[0].name+"01";
        enemy.Add(Instantiate(enemies.enemies[0].Item, GridManager.Instance.smap.GetGridCenter(11, 11), Quaternion.identity));
        enemy[1].name = enemies.enemies[0].name+"02";
        enemy.Add(Instantiate(enemies.enemies[0].Item, GridManager.Instance.smap.GetGridCenter(10, 10), Quaternion.identity));
        enemy[2].name = enemies.enemies[0].name+"03";

    }

    public void AddObject(GameObject item,Vector3 position)//调用时需要在调用处更改物体的mingcheng
    {
        items.Add(item);
        Instantiate(item, position, Quaternion.identity);        
    }
    public void RemoveObject(GameObject item)
    {
        items.Remove(item);
        Destroy(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
