using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public Gridmap<GameObject> roomGridmap;//����������������ɵķ���
    public Gridmap<GameObject> stepGrid;//���߸�Ӧ������Ҿ��Լ��������ߵĵ�λ
    public Gridmap<GameObject> roomCell;//�������������ɷ����ڼҾߵ�����
    public PathFinder pathFinder;
    public PathFinder pathFinderTest;
    public int maxWidth;
    public int minWidth;
    public int maxHeight;
    public int minHeight;
    public int celllong;
    public float roomdepth;
    //private int x, z;
    public int pathFinderlong;
    public Vector3 origenPoint;
    public GameObject room;
    public GameObject floor;
    public GameObject door;
    public Character_SO characterData;
    public Enemy_SO enemyData;
    private int maxX, minX, maxY, minY, roomcell;
    public Camera camera;
    public GameObject character;
    public LayerMask layerMask;
    private int pX, pZ;
    public Dictionary<string, Vector2> doorSave = new Dictionary<string, Vector2>();
    //private bool canClick =false;
    private void Start()
    {
        roomGridmap = new Gridmap<GameObject>(maxWidth, maxHeight, minWidth, minHeight, celllong, origenPoint, Outdo);

        minX = minWidth * celllong;
        minY = minHeight * celllong;
        maxX = maxWidth * celllong;
        maxY = maxHeight * celllong;
        roomcell = 1;
        pathFinder = new PathFinder(maxX, maxY, minX, minY, roomcell, origenPoint);
        pathFinderTest = new PathFinder(maxX, maxY, minX, minY, roomcell, origenPoint);
        stepGrid = new Gridmap<GameObject>(maxX, maxY, minX, minY, roomcell, origenPoint, Out);
       /* for (int i = 0; i < 20; i++)
        {
            int randomnumber = Random.RandomRange(0, 100);
            Debug.Log(randomnumber+" ");
        }*/
       

    }
    public GameObject Outdo(Gridmap<GameObject> grid, int x, int z)
    {
        return Instantiate(room, grid.GetGridCenter(x, z), Quaternion.identity);
    }
    public GameObject Out(Gridmap<GameObject> grid, int x, int z)
    {
        return Instantiate(floor, grid.GetGridCenter(x, z), Quaternion.identity);
    }
    public void Update()
    {
        //Vector3 save;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Debug.DrawLine(ray.origin, ray.direction * 100 + ray.origin, Color.red, 100f);

            if (Input.GetMouseButtonDown(0))
            {
                Debug.DrawLine(ray.origin, ray.direction * 100 + ray.origin, Color.red, 100f);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    Debug.Log(hit.collider.name);
            }
        }
        if (TimeCountDown.Instance.clocks.ToArray().Length != 0)
        {
            Debug.Log(Mathf.CeilToInt(TimeCountDown.Instance.clocks[0]) + " this is list and in Grid ");
        }

    }

    private void FixedUpdate()
    {
        foreach (var item in DataSave.Instance.currentPlayers)
        {
            roomGridmap.GetGridXZ(item.Value.gameObject.transform.position, out int x, out int z);//ȡ�ý�ɫ���ڵķ����
            CheckDoor(x, z);
            foreach (var a in doorSave)
            {
                int tx, tz;//�洢��ǰ��ɫ��վλ
                stepGrid.GetGridXZ(item.Value.gameObject.transform.position, out tx, out tz);
                if (tx == a.Value.x && tz == a.Value.y)
                {

                    switch (a.Key)//��������Ÿ���ʲô�����
                    {
                        case "forward":
                            if (roomGridmap.GetValue(x, z + 1) == null)
                            {
                                roomGridmap.SetValue(x, z + 1, Instantiate(room, roomGridmap.GetGridCenter(x, z + 1) , Quaternion.identity));//Ҫ�����ɵ��������������
                                RoomToStep(x, z + 1);
                                RoomBuild.Instance.CreateGrid(roomGridmap.GetWorldPosition(x, z + 1));
                            }
                            break;
                        case "left":
                            if (roomGridmap.GetValue(x - 1, z) == null)
                            {
                                roomGridmap.SetValue(x - 1, z, Instantiate(room, roomGridmap.GetGridCenter(x - 1, z) , Quaternion.identity));
                                RoomToStep(x - 1, z);
                                RoomBuild.Instance.CreateGrid(roomGridmap.GetWorldPosition(x - 1, z));
                            }
                            break;
                        case "right":
                            if (roomGridmap.GetValue(x + 1, z) == null)
                            {
                                roomGridmap.SetValue(x + 1, z, Instantiate(room, roomGridmap.GetGridCenter(x + 1, z), Quaternion.identity));
                                RoomToStep(x + 1, z);
                                RoomBuild.Instance.CreateGrid(roomGridmap.GetWorldPosition(x + 1, z));
                            }
                            break;
                        case "back":
                            if (roomGridmap.GetValue(x, z - 1) == null)
                            {
                                //roomGrid.SetValue(x, z - 1, Instantiate(room, roomGrid.GetGridCenter(x, z - 1) + Vector3.down * 0.55f, Quaternion.identity));
                                roomGridmap.SetValue(x, z - 1, Instantiate(room, roomGridmap.GetGridCenter(x, z - 1), Quaternion.identity));
                                RoomToStep(x, z - 1);
                                RoomBuild.Instance.CreateGrid(roomGridmap.GetWorldPosition(x, z - 1));
                            }
                            break;
                    }
                }
            }


        }
    }
    public void RoomToStep(int gx, int gz)
    {
        roomGridmap.GetGridRangePoints(gx, gz, out int maxX, out int minX, out int maxZ, out int minZ);
        GridTranslateToSmall(maxX, maxZ, out int tXx, out int tXz);
        //sx1--;
        // sz1--
        GridTranslateToSmall(minX, minZ, out int tMx, out int tMz);
        for (int x = tMx; x < tXx; x++)
        {
            for (int z = tMz; z < tXz; z++)
            {
                stepGrid.SetValue(x, z, Instantiate(floor, stepGrid.GetGridCenter(x, z), Quaternion.identity));
                pathFinder.GetGrid().SetValue(x, z, new PathNode(pathFinder.GetGrid(), x, z));
                pathFinder.GetGrid().GetValue(x, z).canWalk = true;
            }
        }
    }
    public void CheckDoor(int x, int z)//����ֻ��һ�ַ������ͣ��������ݷ������͵�������Ҫ�����޸�
    {
        doorSave.Clear();
        int maxX, minX, maxZ, minZ;
        int tampXx, tampMx, tampXz, tampMz;
        int midX, midZ;
        roomGridmap.GetGridRangePoints(x, z, out maxX, out minX, out maxZ, out minZ);//�Ѿ�ȡ������������ĸ��ӵ��ĸ����޵�
        GridTranslateToSmall(maxX, maxZ, out tampXx, out tampXz);//����������ת����С��������
        tampXz--;
        tampXx--;
        GridTranslateToSmall(minX, minZ, out tampMx, out tampMz);
        midX = (tampXx - tampMx) / 2 + tampMx;//ȡ�ñ߽�������е�
        midZ = (tampXz - tampMz) / 2 + tampMz;
        //�õ�Ԫ����ĸ��ŵ㶼��ʶ��
        doorSave.Add("forward", new Vector2(midX, tampXz));//�����ŵĵ�λ
        if (stepGrid.GetValue(midX, tampXz).tag != "door")
        {
            // Destroy(stepGrid.GetValue(midX, tampXz));
            stepGrid.SetValue(midX, tampXz, Instantiate(door, stepGrid.GetGridCenter(midX, tampXz), Quaternion.identity));
        }
        doorSave.Add("left", new Vector2(tampMx, midZ));
        if (stepGrid.GetValue(tampMx, midZ).tag != "door")
        {
            //Destroy(stepGrid.GetValue(tampMx, midZ));
            stepGrid.SetValue(tampMx, midZ, Instantiate(door, stepGrid.GetGridCenter(tampMx, midZ), Quaternion.identity));
        }
        doorSave.Add("right", new Vector2(tampXx, midZ));
        if (stepGrid.GetValue(tampXx, midZ).tag != "door")
        {
            //Destroy(stepGrid.GetValue(tampXx, midZ));
            stepGrid.SetValue(tampXx, midZ, Instantiate(door, stepGrid.GetGridCenter(tampXx, midZ), Quaternion.identity));
        }
        doorSave.Add("back", new Vector2(midX, tampMz));
        if (stepGrid.GetValue(midX, tampMz).tag != "door")
        {
            //Destroy(stepGrid.GetValue(midX, tampMz));
            stepGrid.SetValue(midX, tampMz, Instantiate(door, stepGrid.GetGridCenter(midX, tampMz), Quaternion.identity));
        }
    }
    public void GridTranslateToSmall(int x, int z, out int sx, out int sz)
    {
        stepGrid.GetGridXZ(roomGridmap.GetWorldPosition(x, z), out sx, out sz);

    }
}
