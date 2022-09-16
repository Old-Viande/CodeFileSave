using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : Singleton<CreateManager>
{
    public Character_SO characters;
    public Enemy_SO enemies;
    public List<GameObject> items = new List<GameObject>();//���е�ǰ�������н�ɫ�͵��˵�λ
    public GameObject player;
    public List<GameObject> enemy= new List<GameObject>();
    public Dictionary<string, CharacterData> unitSave = new Dictionary<string, CharacterData>();//�����������Զ����Լ���������ֵ�
  
    

    void Start()
    {
        player = Instantiate(characters.characters[0].Item, GridManager.Instance.stepGrid.GetGridCenter(3,3), Quaternion.identity);       
        GridManager.Instance.pathFinder.GetGrid().GetValue(3, 3).canWalk = false;
        //player = Instantiate(characters.characters[0].Item, GridManager.Instance.stepGrid.GetGridCenter(1, 1), Quaternion.identity);
        //GridManager.Instance.pathFinder.GetGrid().GetValue(1, 1).canWalk = false;
        player.name = characters.characters[0].name;
        //  DataSave.Instance.AddObject(player.name, player);
        /*enemy.Add(Instantiate(enemies.enemies[0].Item, GridManager.Instance.stepGrid.GetGridCenter(11, 10), Quaternion.identity));
        enemy[0].name = enemies.enemies[0].name;
        DataSave.Instance.AddObject(enemy[0].name, enemy[0]);
        GridManager.Instance.pathFinder.GetGrid().GetValue(11,10).canWalk = false;*/
       

    }
    public void AddObj(string name, CharacterData data)
    {
        unitSave.Add(name, data);
    }
    public void RemoveObj(string name)
    {
        unitSave.Remove(name);
    }

    public void EnenmyCreate(Vector3 position)
    {
        
        enemy.Add(Instantiate(enemies.enemies[0].Item, position, Quaternion.identity));//�������飬���ж�̬�仯
        enemy[enemy.Count - 1].name = enemies.enemies[0].name + "-"+enemy.Count ;        //ÿ����һ�������鳤�ȶ������ӣ����鳤��-1�õ�����Ŀǰ�������´�����Ǹ���λ
        GridManager.Instance.pathFinder.GetGrid().GetValue(position).canWalk = false;   //������λ�õĸ�����Ϊ��������
    }

   public void CreateItem(GameObject item,Vector3 position)//����ʱ��Ҫ�ڵ��ô��������������
    {
        items.Add(item);
        Instantiate(item, position, Quaternion.identity);        
    }
    public void RemoveItem(GameObject item)
    {
        items.Remove(item);
        Destroy(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
