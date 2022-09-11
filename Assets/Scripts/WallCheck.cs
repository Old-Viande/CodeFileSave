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
        GridManager.Instance.roomGrid.gridDictionary.TryGetValue(new Vector2(0, 0), out gameObject);
        Vector3 save = gameObject.transform.forward;
        Vector3 camerasave = camera.ScreenPointToRay(Vector2.zero).direction;
        check = Vector3.Dot(save, camerasave);
      
    }
    private void FixedUpdate()
    {
        foreach (var item in GridManager.Instance.roomGrid.gridDictionary)
        {
            directionCheck((int)item.Key.x, (int)item.Key.y);
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
       // WallHide(x, z);
       // WallCheck(wallType, x, z);
    }
    //private void CheckAll(string a, string b, string c)
    //private void WallHide(int x, int z)
    //{
    //    GameObject gameObject;
    //    gridmap.gridDictionary.TryGetValue(new Vector2(x, z), out gameObject);
  
    //    gameObject.transform.Find("forward").gameObject.SetActive(false);
    //    gameObject.transform.Find("left").gameObject.SetActive(false);
    //    gameObject.transform.Find("right").gameObject.SetActive(false);
    //    gameObject.transform.Find("back").gameObject.SetActive(false);
    //    if (z + 1 < gridmap.gridDictionary.GetLength(1))
    //    {
    //        if (gridmap.gridDictionary[x, z + 1] != null)
    //            gameObject = gridmap.gridDictionary[x, z + 1];
    //        else
    //            gameObjectSave.TryGetValue(new Vector2(x, z + 1), out gameObject);
    //        gameObject.transform.Find("back").gameObject.SetActive(false);
    //    }
    //    if (x - 1 >= 0)
    //    {
    //        if (gridmap.gridDictionary[x - 1, z] != null)
    //            gameObject = gridmap.gridDictionary[x - 1, z];
    //        else
    //            gameObjectSave.TryGetValue(new Vector2(x - 1, z), out gameObject);
    //        gameObject.transform.Find("right").gameObject.SetActive(false);
    //    }
    //    if (x + 1 < gridmap.gridDictionary.GetLength(0))
    //    {
    //        if (gridmap.gridDictionary[x + 1, z] != null)
    //            gameObject = gridmap.gridDictionary[x + 1, z];
    //        else
    //            gameObjectSave.TryGetValue(new Vector2(x + 1, z), out gameObject);
    //        gameObject.transform.Find("left").gameObject.SetActive(false);
    //    }
    //    if (z - 1 >= 0)
    //    {
    //        if (gridmap.gridDictionary[x, z - 1] != null)
    //            gameObject = gridmap.gridDictionary[x, z - 1];
    //        else
    //            gameObjectSave.TryGetValue(new Vector2(x, z - 1), out gameObject);
    //        gameObject.transform.Find("forward").gameObject.SetActive(false);
    //    }

    //}
    //private void WallCheck(WallType wallType, int x, int z)
    //{
    //    if (gridmap.gridDictionary[x, z] != null)
    //    {
    //        GameObject it = gridmap.gridDictionary[x, z];
    //        if (wallType.HasFlag(WallType.forward))
    //        {
    //            if (z + 1 >= gridmap.gridDictionary.GetLength(1))
    //            {
    //                it.transform.Find("forward").gameObject.SetActive(true);
    //            }
    //            else if (gridmap.gridDictionary[x, z + 1] == null)
    //            {
    //                gameObjectSave.TryGetValue(new Vector2(x, z + 1), out GameObject gameObject);
    //                if (gameObject != null)
    //                    gameObject.transform.Find("back").gameObject.SetActive(true);
    //                it.transform.Find("forward").gameObject.SetActive(true);
    //            }

    //        }
    //        if (wallType.HasFlag(WallType.left))
    //        {
    //            if (x - 1 < 0)
    //            {
    //                it.transform.Find("left").gameObject.SetActive(true);
    //            }
    //            else if (gridmap.gridDictionary[x - 1, z] == null)
    //            {
    //                gameObjectSave.TryGetValue(new Vector2(x - 1, z), out GameObject gameObject);
    //                if (gameObject != null)
    //                    gameObject.transform.Find("right").gameObject.SetActive(true);
    //                it.transform.Find("left").gameObject.SetActive(true);
    //            }
    //        }
    //        if (wallType.HasFlag(WallType.right))
    //        {
    //            if (x + 1 >= gridmap.gridDictionary.GetLength(0))
    //            {
    //                it.transform.Find("right").gameObject.SetActive(true);
    //            }
    //            else if (gridmap.gridDictionary[x + 1, z] == null)
    //            {
    //                gameObjectSave.TryGetValue(new Vector2(x + 1, z), out GameObject gameObject);
    //                if (gameObject != null)
    //                    gameObject.transform.Find("left").gameObject.SetActive(true);
    //                it.transform.Find("right").gameObject.SetActive(true);
    //            }
    //        }
    //        if (wallType.HasFlag(WallType.back))
    //        {
    //            if (z - 1 < 0)
    //            {
    //                it.transform.Find("back").gameObject.SetActive(true);
    //            }
    //            else if (gridmap.gridDictionary[x, z - 1] == null)
    //            {
    //                gameObjectSave.TryGetValue(new Vector2(x, z - 1), out GameObject gameObject);
    //                if (gameObject != null)
    //                    gameObject.transform.Find("forward").gameObject.SetActive(true);
    //                it.transform.Find("back").gameObject.SetActive(true);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        gameObjectSave.TryGetValue(new Vector2(x, z), out GameObject two);

    //        if (wallType.HasFlag(WallType.forward))
    //        {
    //            if (z - 1 >= 0 && gridmap.gridDictionary[x, z - 1] != null)
    //            {
    //                gridmap.gridDictionary[x, z - 1].gameObject.transform.Find("forward").gameObject.SetActive(true);
    //                two.transform.Find("back").gameObject.SetActive(true);
    //            }

    //        }
    //        if (wallType.HasFlag(WallType.left))
    //        {
    //            if (x + 1 < gridmap.gridDictionary.GetLength(0) && gridmap.gridDictionary[x + 1, z] != null)
    //            {
    //                gridmap.gridDictionary[x + 1, z].gameObject.transform.Find("left").gameObject.SetActive(true);
    //                two.transform.Find("right").gameObject.SetActive(true);
    //            }
    //        }
    //        if (wallType.HasFlag(WallType.right))
    //        {
    //            if (x - 1 >= 0 && gridmap.gridDictionary[x - 1, z] != null)
    //            {
    //                gridmap.gridDictionary[x - 1, z].gameObject.transform.Find("right").gameObject.SetActive(true);
    //                two.transform.Find("left").gameObject.SetActive(true);
    //            }
    //        }
    //        if (wallType.HasFlag(WallType.back))
    //        {
    //            if (z + 1 < gridmap.gridDictionary.GetLength(1) && gridmap.gridDictionary[x, z + 1] != null)
    //            {
    //                gridmap.gridDictionary[x, z + 1].gameObject.transform.Find("back").gameObject.SetActive(true);
    //                two.transform.Find("forward").gameObject.SetActive(true);
    //            }
    //        }
    //    }
    //}
}
