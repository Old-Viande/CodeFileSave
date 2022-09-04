using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var a in GridManager.Instance.enemyData.enemies)
        {
            if (this.name.Contains(a.name))
            {
                GridManager.Instance.enemyData.enemySave.TryGetValue(a.name, out enemy);
            }
        }
       
        if (enemy != null)
        {
            DataSave.Instance.enemies.Add(enemy);
        }

    }
    private void OnDestroy()//物品被销毁时调用
    {
        if (enemy != null)
        {
            DataSave.Instance.enemies.Remove(enemy);
        }
    }
}
