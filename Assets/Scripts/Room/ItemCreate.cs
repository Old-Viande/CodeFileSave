using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreate : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        int numbs = Random.Range(0, items.Count - 1);//���        
     // RoomBuild.Instance.roomCell.SetValue( this.transform.position,Instantiate(items[numbs], this.transform.Find("Point").gameObject.transform.position, Quaternion.identity));//��������ɵĶ���������뷿��������
        Instantiate(items[numbs], this.transform.Find("Point").gameObject.transform.position, Quaternion.identity);
    }

    // Update is called once per frame

}
