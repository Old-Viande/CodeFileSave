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
        foreach (var item in GridManager.Instance.roomGridmap.gridDictionary)//ѭ���������еķ���
        {
            directionCheck((int)item.Key.x, (int)item.Key.y);//ȡ�÷������������
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
        WallHide(x, z);//�Ȱ�����ǽ��������
        CheckWall(wallType, x, z);//�ٰ�ǽ��ʾ����
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
        //if (z + 1 < GridManager.Instance.roomGrid.gridDictionary.GetLength(1))//�������ĸ񣬲��Ҳ���Խ������ı߽磨���ֵ��
        //{
        //    if (GridManager.Instance.roomGrid.gridDictionary[x, z + 1] != null)
        //        gameObject = GridManager.Instance.roomGrid.gridDictionary[x, z + 1];//����ܶ���ֵ����������ڷ���������Ǹ�����
        //    else
        //        gameObjectSave.TryGetValue(new Vector2(x, z + 1), out gameObject);//������ϸ��汾�Ĺ��ܴ��������ô���
        //    gameObject.transform.Find("back").gameObject.SetActive(false);//�ص���ǰ���ӵ���һ���������ǽ
        //}
        if (GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(x, z + 1), out gameObject))//���Ի�õ�ǰ����Ϸ��������ȡ��
        {
            gameObject.transform.Find("back").gameObject.SetActive(false);//�ص��Ϸ���ĵײ�ǽ
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
        //if (GridManager.Instance.roomGrid.gridDictionary[x, z] != null)//�����ǰ���Ӳ�Ϊ��
        //{
        //    GameObject it = GridManager.Instance.roomGrid.gridDictionary[x, z];//ȡ�õ�ǰ��ķ������
        //    if (wallType.HasFlag(WallType.forward))
        //    {
        //        if (z + 1 >= GridManager.Instance.roomGrid.gridDictionary.GetLength(1))//����Ϸ�һ�񳬹��߽�
        //        {
        //            it.transform.Find("forward").gameObject.SetActive(true);//�Ͱѵ�ǰ��Ŀ��õ���ǽ��ʾ����
        //        }
        //        else if (GridManager.Instance.roomGrid.gridDictionary[x, z + 1] == null)//���û�г����߽�
        //        {
        //            gameObjectSave.TryGetValue(new Vector2(x, z + 1), out GameObject gameObject);
        //            if (gameObject != null)
        //                gameObject.transform.Find("back").gameObject.SetActive(true);//��ʾ�Ϸ�һ��ı���
        //            it.transform.Find("forward").gameObject.SetActive(true);//�Լ���ǰ���ӵ�ǰ��
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
                if (GridManager.Instance.roomGridmap.gridDictionary.TryGetValue(new Vector2(x, z + 1), out otherRoom))//û��Խ���߽�
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
