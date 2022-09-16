using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    private Camera camera;
    public float check;
    public CameraMove cameraMove;
    public Dictionary<Vector2, GameObject> gameObjectSave = new Dictionary<Vector2, GameObject>();
    [Flags]
    public enum WallType
    {
        forward = 1,
        back = 2,
        left = 4,
        right = 8,

    }

    public WallType wallType;
    // Start is called before the first frame update
    void Start()
    {

        camera = GridManager.Instance.camera;

    }
    // Update is called once per frame
    void Update()
    {
        GameObject gameObject;
        GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(0, 0), out gameObject);
        Vector3 save = gameObject.transform.forward;
        Vector3 camerasave = camera.ScreenPointToRay(Vector2.zero).direction;
        check = Vector3.Dot(save, camerasave);

    }
    private void FixedUpdate()
    {
        foreach (var item in GridManager.Instance.roomGridmap.gridDictionary)//循环场上所有的房间
        {
            directionCheck((int)item.Key.x, (int)item.Key.y);//取得房间的网格坐标
        }
    }

    private void directionCheck(int x, int z)
    {

        if (cameraMove.direction > 240 && cameraMove.direction <= 300)
        {
            wallType = WallType.forward | WallType.left | WallType.right;
        }
        else if (cameraMove.direction > 300 && cameraMove.direction <= 330)
        {
            wallType = WallType.forward | WallType.left;
        }
        else if (cameraMove.direction > -30 && cameraMove.direction <= 30)
        {
            wallType = WallType.forward | WallType.left | WallType.back;
        }
        else if (cameraMove.direction > 30 && cameraMove.direction <= 60)
        {
            wallType = WallType.left | WallType.back;
        }
        else if (cameraMove.direction > 60 && cameraMove.direction <= 120)
        {
            wallType = WallType.right | WallType.left | WallType.back;
        }
        else if (cameraMove.direction > 120 && cameraMove.direction <= 150)
        {
            wallType = WallType.right | WallType.back;
        }
        else if (cameraMove.direction > 150 && cameraMove.direction <= 210)
        {
            wallType = WallType.forward | WallType.right | WallType.back;
        }
        else if (cameraMove.direction > 210 && cameraMove.direction <= 240)
        {
            wallType = WallType.forward | WallType.right;
        }
        WallHide(x, z);//先把所有墙都藏起来
        CheckWall(wallType, x, z);//再把墙显示出来
    }
    //private void CheckAll(string a, string b, string c)
    private void WallHide(int x, int z)
    {
        GameObject gameObject;
        GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(x, z), out gameObject);

        gameObject.transform.Find("forward").gameObject.SetActive(false);
        gameObject.transform.Find("left").gameObject.SetActive(false);
        gameObject.transform.Find("right").gameObject.SetActive(false);
        gameObject.transform.Find("back").gameObject.SetActive(false);
        //if (z + 1 < GridManager.Instance.roomGrid.gridDictionary.GetLength(1))//检测上面的格，并且不能越过网格的边界（最大值）
        //{
        //    if (GridManager.Instance.roomGrid.gridDictionary[x, z + 1] != null)
        //        gameObject = GridManager.Instance.roomGrid.gridDictionary[x, z + 1];//如果能读到值，则变量等于房间上面的那个房间
        //    else
        //        gameObjectSave.TryGetValue(new Vector2(x, z + 1), out gameObject);//这个是上个版本的功能带来的无用代码
        //    gameObject.transform.Find("back").gameObject.SetActive(false);//关掉当前格子的上一个的下面的墙
        //}
        if (GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(x, z + 1), out gameObject))//尝试获得当前格的上方格，如果能取得
        {
            gameObject.transform.Find("back").gameObject.SetActive(false);//关掉上方格的底部墙
        }
        //if (x - 1 >= 0)
        //{
        //    if (GridManager.Instance.roomGrid.gridDictionary[x - 1, z] != null)
        //        gameObject = GridManager.Instance.roomGrid.gridDictionary[x - 1, z];
        //    else
        //        gameObjectSave.TryGetValue(new Vector2(x - 1, z), out gameObject);
        //    gameObject.transform.Find("right").gameObject.SetActive(false);
        //}
        if (GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(x - 1, z), out gameObject))
        {
            gameObject.transform.Find("right").gameObject.SetActive(false);
        }
        //if (x + 1 < GridManager.Instance.roomGrid.gridDictionary.GetLength(0))
        //{
        //    if (GridManager.Instance.roomGrid.gridDictionary[x + 1, z] != null)
        //        gameObject = GridManager.Instance.roomGrid.gridDictionary[x + 1, z];
        //    else
        //        gameObjectSave.TryGetValue(new Vector2(x + 1, z), out gameObject);
        //    gameObject.transform.Find("left").gameObject.SetActive(false);
        //}
        if (GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(x + 1, z), out gameObject))
        {
            gameObject.transform.Find("left").gameObject.SetActive(false);
        }
        //if (z - 1 >= 0)
        //{
        //    if (GridManager.Instance.roomGrid.gridDictionary[x, z - 1] != null)
        //        gameObject = GridManager.Instance.roomGrid.gridDictionary[x, z - 1];
        //    else
        //        gameObjectSave.TryGetValue(new Vector2(x, z - 1), out gameObject);
        //    gameObject.transform.Find("forward").gameObject.SetActive(false);
        //}
        if (GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(x, z - 1), out gameObject))
        {
            gameObject.transform.Find("forward").gameObject.SetActive(false);
        }
    }
    private void CheckWall(WallType wallType, int x, int z)
    {
        GameObject room;
        GameObject otherRoom;
        //if (GridManager.Instance.roomGrid.gridDictionary[x, z] != null)//如果当前格子不为空
        //{
        //    GameObject it = GridManager.Instance.roomGrid.gridDictionary[x, z];//取得当前格的房间对象
        //    if (wallType.HasFlag(WallType.forward))
        //    {
        //        if (z + 1 >= GridManager.Instance.roomGrid.gridDictionary.GetLength(1))//如果上方一格超过边界
        //        {
        //            it.transform.Find("forward").gameObject.SetActive(true);//就把当前格的看得到的墙显示出来
        //        }
        //        else if (GridManager.Instance.roomGrid.gridDictionary[x, z + 1] == null)//如果没有超过边界
        //        {
        //            gameObjectSave.TryGetValue(new Vector2(x, z + 1), out GameObject gameObject);
        //            if (gameObject != null)
        //                gameObject.transform.Find("back").gameObject.SetActive(true);//显示上方一格的背面
        //            it.transform.Find("forward").gameObject.SetActive(true);//以及当前格子的前面
        //        }

        //    }
        //    if (wallType.HasFlag(WallType.left))
        //    {
        //        if (x - 1 < 0)
        //        {
        //            it.transform.Find("left").gameObject.SetActive(true);
        //        }
        //        else if (GridManager.Instance.roomGrid.gridDictionary[x - 1, z] == null)
        //        {
        //            gameObjectSave.TryGetValue(new Vector2(x - 1, z), out GameObject gameObject);
        //            if (gameObject != null)
        //                gameObject.transform.Find("right").gameObject.SetActive(true);
        //            it.transform.Find("left").gameObject.SetActive(true);
        //        }
        //    }
        //    if (wallType.HasFlag(WallType.right))
        //    {
        //        if (x + 1 >= GridManager.Instance.roomGrid.gridDictionary.GetLength(0))
        //        {
        //            it.transform.Find("right").gameObject.SetActive(true);
        //        }
        //        else if (GridManager.Instance.roomGrid.gridDictionary[x + 1, z] == null)
        //        {
        //            gameObjectSave.TryGetValue(new Vector2(x + 1, z), out GameObject gameObject);
        //            if (gameObject != null)
        //                gameObject.transform.Find("left").gameObject.SetActive(true);
        //            it.transform.Find("right").gameObject.SetActive(true);
        //        }
        //    }
        //    if (wallType.HasFlag(WallType.back))
        //    {
        //        if (z - 1 < 0)
        //        {
        //            it.transform.Find("back").gameObject.SetActive(true);
        //        }
        //        else if (GridManager.Instance.roomGrid.gridDictionary[x, z - 1] == null)
        //        {
        //            gameObjectSave.TryGetValue(new Vector2(x, z - 1), out GameObject gameObject);
        //            if (gameObject != null)
        //                gameObject.transform.Find("forward").gameObject.SetActive(true);
        //            it.transform.Find("back").gameObject.SetActive(true);
        //        }
        //    }
        //}
        //else
        //{
        //    gameObjectSave.TryGetValue(new Vector2(x, z), out GameObject two);

        //    if (wallType.HasFlag(WallType.forward))
        //    {
        //        if (z - 1 >= 0 && GridManager.Instance.roomGrid.gridDictionary[x, z - 1] != null)
        //        {
        //            GridManager.Instance.roomGrid.gridDictionary[x, z - 1].gameObject.transform.Find("forward").gameObject.SetActive(true);
        //            two.transform.Find("back").gameObject.SetActive(true);
        //        }

        //    }
        //    if (wallType.HasFlag(WallType.left))
        //    {
        //        if (x + 1 < GridManager.Instance.roomGrid.gridDictionary.GetLength(0) && GridManager.Instance.roomGrid.gridDictionary[x + 1, z] != null)
        //        {
        //            GridManager.Instance.roomGrid.gridDictionary[x + 1, z].gameObject.transform.Find("left").gameObject.SetActive(true);
        //            two.transform.Find("right").gameObject.SetActive(true);
        //        }
        //    }
        //    if (wallType.HasFlag(WallType.right))
        //    {
        //        if (x - 1 >= 0 && GridManager.Instance.roomGrid.gridDictionary[x - 1, z] != null)
        //        {
        //            GridManager.Instance.roomGrid.gridDictionary[x - 1, z].gameObject.transform.Find("right").gameObject.SetActive(true);
        //            two.transform.Find("left").gameObject.SetActive(true);
        //        }
        //    }
        //    if (wallType.HasFlag(WallType.back))
        //    {
        //        if (z + 1 < GridManager.Instance.roomGrid.gridDictionary.GetLength(1) && GridManager.Instance.roomGrid.gridDictionary[x, z + 1] != null)
        //        {
        //            GridManager.Instance.roomGrid.gridDictionary[x, z + 1].gameObject.transform.Find("back").gameObject.SetActive(true);
        //            two.transform.Find("forward").gameObject.SetActive(true);
        //        }
        //    }
        //}
        if (GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(x, z), out room))
        {
            if (wallType.HasFlag(WallType.forward))
            {
                if (GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(x, z + 1), out otherRoom))//没有越过边界
                {
                    //otherRoom.transform.Find("back").gameObject.SetActive(true);
                    //room.transform.Find("forward").gameObject.SetActive(true);
                }
                else
                {
                    room.transform.Find("forward").gameObject.SetActive(true);
                }
            }
            if (wallType.HasFlag(WallType.left))
            {
                if (GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(x - 1, z), out otherRoom))
                {
                   // otherRoom.transform.Find("right").gameObject.SetActive(true);
                    //room.transform.Find("left").gameObject.SetActive(true);
                }
                else
                {
                    room.transform.Find("left").gameObject.SetActive(true);
                }
            }
            if (wallType.HasFlag(WallType.right))
            {
                if (GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(x + 1, z), out otherRoom))
                {
                    //otherRoom.transform.Find("left").gameObject.SetActive(true);
                   // room.transform.Find("right").gameObject.SetActive(true);
                }
                else
                {
                    room.transform.Find("right").gameObject.SetActive(true);
                }
            }
            if (wallType.HasFlag(WallType.back))
            {
                if (GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(x, z - 1), out otherRoom))
                {
                   // otherRoom.transform.Find("forward").gameObject.SetActive(true);
                    //room.transform.Find("back").gameObject.SetActive(true);
                }
                else
                {
                    room.transform.Find("back").gameObject.SetActive(true);
                }
            }
        }
    }
}
