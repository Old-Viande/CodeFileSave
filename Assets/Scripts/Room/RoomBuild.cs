
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBuild : Singleton<RoomBuild>
{
    public RoomType roomType;
    public List<RoomType> saveType = new List<RoomType>();
    public List<GameObject> liveroom = new List<GameObject>();
    // public List<GameObject> bedroom = new List<GameObject>();
    public List<GameObject> studyroom = new List<GameObject>();
    public GameObject[,] objArrey = new GameObject[7, 7];
    public List<Vector2> pointlist = new List<Vector2>();

    private void Start()
    {  //将房屋类型传入list
       // saveType.Add(RoomType.bedRoom);
       //saveType.Add(RoomType.gym);
       //saveType.Add(RoomType.kitchen);
        saveType.Add(RoomType.livingRoom);
        saveType.Add(RoomType.studyRoom);
        //随机类型     
    }
    public void CreateGrid(Vector3 origenPoint)//将当前格的位置导入进来，基于房间格的点进行
    {
        int numbs = Random.Range(0, saveType.Count - 1);
        GridManager.Instance.roomCell = new Gridmap<GameObject>(7, 7, 0, 0, 1, origenPoint, Outdo);//初始化网格
        RoomCreate(saveType[numbs]);//随机房间类型
    }

    public GameObject Outdo(Gridmap<GameObject> grid, int x, int z)
    {
        return null;
    }

    private bool CheckAroundNode(float x, float y)
    {
        return pointlist.Contains(new Vector2(x, y)) && GridManager.Instance.roomCell.GetValue((int)x, (int)y) == null;
    }

    private void RoomCreate(RoomType type)
    {
        pointlist.Clear();
        foreach (var item in GridManager.Instance.roomCell.gridDictionary)//将网格内的所有物品全部存入数组
        {
            if (item.Key.x != 3 && item.Key.y != 3)
                pointlist.Add(new Vector2(item.Key.x, item.Key.y));
        }
        int createNumb = Random.Range(3, 15);
        if (type == RoomType.livingRoom)
        {
            for (int i = 0; i < createNumb; i++)
            {
                int rNumb = Random.Range(0, pointlist.Count - 1);
                GameObject save = GridManager.Instance.roomCell.GetValue((int)pointlist[rNumb].x, (int)pointlist[rNumb].y);
                if (save != null)
                    continue;

                int objRandom = Random.Range(0, liveroom.Count - 1);
                if (liveroom[objRandom].GetComponent<FurnitureData>().Type == FurnitureData.FurnitureType.OneByOne)
                {
                    GridManager.Instance.roomCell.SetValue((int)pointlist[rNumb].x, (int)pointlist[rNumb].y, Instantiate(liveroom[objRandom], GridManager.Instance.roomCell.GetGridCenter((int)pointlist[rNumb].x, (int)pointlist[rNumb].y), Quaternion.identity));
                    //如果为空则生成一个家具
                    //Instantiate(liveroom[objRandom], GridManager.Instance.roomCell.GetGridCenter((int)pointlist[rNumb].x, (int)pointlist[rNumb].y), Quaternion.identity);
                    //这里出了BUG,不知道为什么，这个字典里面存入的值是被生成的物品的坐标，这导致了网格内存入的物体的网格坐标超出了其他网格的边界！！！！(已修复)
                }
                else
                {
                    bool found = false;
                    float itemX = 0.0f, itemY = 0.0f;
                    if (CheckAroundNode(pointlist[rNumb].x - 1, pointlist[rNumb].y))
                    {
                        found = true;
                        itemX = pointlist[rNumb].x - 1;
                        itemY = pointlist[rNumb].y;
                    }
                    else if (!found && CheckAroundNode(pointlist[rNumb].x + 1, pointlist[rNumb].y))
                    {
                        found = true;
                        itemX = pointlist[rNumb].x + 1;
                        itemY = pointlist[rNumb].y;
                    }
                    else if (!found && CheckAroundNode(pointlist[rNumb].x, pointlist[rNumb].y - 1))
                    {
                        found = true;
                        itemX = pointlist[rNumb].x;
                        itemY = pointlist[rNumb].y - 1;
                    }
                    else if (!found && CheckAroundNode(pointlist[rNumb].x, pointlist[rNumb].y + 1))
                    {
                        found = true;
                        itemX = pointlist[rNumb].x;
                        itemY = pointlist[rNumb].y + 1;
                    }
                    if (found)
                    {
                        Vector3 itemPos1 = GridManager.Instance.roomCell.GetGridCenter((int)pointlist[rNumb].x, (int)pointlist[rNumb].y);
                        Vector3 itemPos2 = GridManager.Instance.roomCell.GetGridCenter((int)itemX, (int)itemY);
                        GameObject furniture = Instantiate(liveroom[objRandom], (itemPos1 + itemPos2) / 2.0f, Quaternion.Euler(new Vector3(0, (pointlist[rNumb].y == itemY) ? 0 : 90, 0)));
                        GridManager.Instance.roomCell.SetValue((int)pointlist[rNumb].x, (int)pointlist[rNumb].y, furniture);
                        GridManager.Instance.roomCell.SetValue((int)itemX, (int)itemY, furniture);
                    }
                }
            }
        }
        else if (type == RoomType.studyRoom)
        {
            for (int i = 0; i < createNumb; i++)
            {
                GameObject save;
                int rNumb = Random.Range(0, pointlist.Count - 1);
                int objRandom = Random.Range(0, studyroom.Count - 1);
                save = GridManager.Instance.roomCell.GetValue((int)pointlist[rNumb].x, (int)pointlist[rNumb].y);
                if (save == null)
                {
                    GridManager.Instance.roomCell.SetValue((int)pointlist[rNumb].x, (int)pointlist[rNumb].y, Instantiate(studyroom[objRandom], GridManager.Instance.roomCell.GetGridCenter((int)pointlist[rNumb].x, (int)pointlist[rNumb].y), Quaternion.identity));
                }
                //Instantiate(studyroom[objRandom], GridManager.Instance.roomCell.GetGridCenter((int)pointlist[rNumb].x, (int)pointlist[rNumb].y), Quaternion.identity);
            }
        }
        foreach (var a in pointlist)
        {
            GameObject save;
            save = GridManager.Instance.roomCell.GetValue((int)a.x, (int)a.y);
            if (save == null)
            {
                CreateManager.Instance.EnenmyCreate(GridManager.Instance.roomCell.GetGridCenter((int)a.x, (int)a.y));
                break;
            }

        }

        foreach (var item in GridManager.Instance.roomCell.gridDictionary)//将网格内的所有物品全部存入数组
        {
            if (item.Value != null)
            {
                GridManager.Instance.pathFinder.GetGrid().GetValue(GridManager.Instance.roomCell.GetGridCenter((int)item.Key.x, (int)item.Key.y)).canWalk = false;
            }

        }
    }
}
