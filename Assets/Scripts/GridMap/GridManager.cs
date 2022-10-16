using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GridManager : Singleton<GridManager>
{
    public Gridmap<GameObject> roomGridmap;//房间格，网格内着生成的房间
    public Gridmap<GameObject> stepGrid;//行走格，应当储存家具以及场上行走的单位
    public Gridmap<GameObject> roomCell;//仅用来矫正生成房间内家具的网格
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
    //一下皆为技能相关全局变量
    /// <summary>
    /// 技能是否在运行中
    /// </summary>
    public bool skillReadyToUse;//用来检测是否处于技能选择目标期间
    public List<Collider> skilleffect = new List<Collider>();//存储所有被技能影响的单位,无论技能是单体还是群体
    private SkillData tempSkill;
    public Vector3 skillHitPoint;
    DecalProjector tempRange;

    /// <summary>
    /// 当前技能的释放距离
    /// </summary>
    public float currentDistance;

    public Vector2Int newRoomPos;

    //private bool canClick =false;
    private void Start()
    {

        roomGridmap = new Gridmap<GameObject>(maxWidth, maxHeight, minWidth, minHeight, celllong, origenPoint, Outdo);
        skillReadyToUse = false;
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
        GameObject effect = Resources.Load<GameObject>("attackRange");//读到物体
        tempRange = Instantiate(effect).GetComponent<DecalProjector>();
        UIManager.Instance.PushPanel(UIPanelType.OptionPanel);//生成物体
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

            //if (Input.GetMouseButtonDown(0))
            //{
            Debug.DrawLine(ray.origin, ray.direction * 100 + ray.origin, Color.red, 100f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                Debug.Log(hit.collider.name);
            //}

        }
        if (TimeCountDown.Instance.clocks.ToArray().Length != 0)
        {
            Debug.Log(Mathf.CeilToInt(TimeCountDown.Instance.clocks[0]) + " this is list and in Grid ");
        }
        if (skillReadyToUse)
        {
            UpdataManager.Instance.skillLineOpen = true;
            Debug.Log("skill is ready for using");
            //LineRenderer line;//取得组件
            RaycastHit checkHit;
            Physics.Raycast(ray, out checkHit, Mathf.Infinity);
            // line = DataSave.Instance.currentObj.GetComponent<LineRenderer>();
            // line.SetPosition(0, DataSave.Instance.currentObj.transform.position);//起始点是物体坐标
            //这里往下的代码是箭头指示物
            LineRendererScript.Instance.StartPosSet();
            if (tempSkill.currentSkillChoseType == SkillData.skillChoseType.multi)
            {
                UpdataManager.Instance.skillMarkOpen = true;
                tempRange.size = new Vector3(tempSkill.skillEffectRange * 2, tempSkill.skillEffectRange * 2, 20);
                tempRange.pivot = new Vector3(0, 0, tempRange.size.z / 2);
                //遇到了问题，我找不到投射器组件的调整宽高的方法
                //范围检测，首先找到当前行动单位和目标(已解决)
                if ((checkHit.point - DataSave.Instance.currentObj.transform.position).magnitude > tempSkill.attackRange)//如果鼠标的距离大于技能的攻击距离
                {
                    currentDistance = tempSkill.attackRange;

                    skillHitPoint = (checkHit.point - DataSave.Instance.currentObj.transform.position).normalized * tempSkill.attackRange + DataSave.Instance.currentObj.transform.position;
                    LineRendererScript.Instance.EndPosSet(skillHitPoint);
                    tempRange.transform.position = (checkHit.point - DataSave.Instance.currentObj.transform.position).normalized * tempSkill.attackRange + DataSave.Instance.currentObj.transform.position + Vector3.up;
                }
                else
                {
                    currentDistance = (checkHit.point - DataSave.Instance.currentObj.transform.position).magnitude;
                    skillHitPoint = checkHit.point;
                    LineRendererScript.Instance.EndPosSet(checkHit.point);//结束点是射线碰撞点坐标
                    tempRange.transform.position = checkHit.point + Vector3.up * 0.5f;
                }
            }
            else
            {
                //关闭范围显示技能
                UpdataManager.Instance.skillMarkOpen = false;
                //范围检测，首先找到当前行动单位和目标
                if ((checkHit.point - DataSave.Instance.currentObj.transform.position).magnitude > tempSkill.attackRange)//如果鼠标的距离大于技能的攻击距离
                {
                    skillHitPoint = (checkHit.point - DataSave.Instance.currentObj.transform.position).normalized * tempSkill.attackRange + DataSave.Instance.currentObj.transform.position;
                    LineRendererScript.Instance.EndPosSet(skillHitPoint);
                }
                else
                {
                    skillHitPoint = checkHit.point;
                    LineRendererScript.Instance.EndPosSet(checkHit.point);//结束点是射线碰撞点坐标

                }
            }

            if ((checkHit.point - DataSave.Instance.currentObj.transform.position).magnitude < tempSkill.attackRange)//如果鼠标的距离小于技能的攻击距离
            {
                if (Input.GetMouseButtonDown(0))
                {
                    skilleffect.Clear();//先清空储存
                    if (tempSkill.currentSkillType == SkillData.skillType.attack || tempSkill.currentSkillType == SkillData.skillType.control)//检测传入的技能类型
                    {
                        //攻击和控制类型检测标签必须为敌人
                        if (tempSkill.currentSkillChoseType == SkillData.skillChoseType.multi)
                        {

                            Collider[] hitItem = Physics.OverlapSphere(checkHit.point, tempSkill.skillEffectRange);//如果是多选就用这个球形检测
                            foreach (var item in hitItem)
                            {
                                if (item.tag == "enemy")
                                {
                                    skilleffect.Add(item);//把所有可以受到影响的单位全部存入全局变量

                                }
                            }


                        }
                        else
                        {
                            if (checkHit.collider.tag == "enemy")
                            {
                                skilleffect.Add(checkHit.collider);//单体直接检测


                            }
                        }
                    }
                    else if (tempSkill.currentSkillType == SkillData.skillType.heal || tempSkill.currentSkillType == SkillData.skillType.enhanch)
                    {
                        //治愈和强化类型的技能选择对象必须为友方
                        if (tempSkill.currentSkillChoseType == SkillData.skillChoseType.multi)
                        {

                            Collider[] hitItem = Physics.OverlapSphere(checkHit.point, tempSkill.attackRange);//如果是多选就用这个球形检测
                            foreach (var item in hitItem)
                            {
                                if (item.tag == "player")
                                {
                                    skilleffect.Add(item);//把所有可以受到影响的单位全部存入全局变量

                                }
                            }

                        }
                        else
                        {
                            if (checkHit.collider.tag == "player")
                            {
                                skilleffect.Add(checkHit.collider);//单体直接检测

                            }
                        }
                    }
                    foreach (var item in skilleffect)
                    {
                        Debug.Log("skill effect " + " " + item.name);
                        //item.GetComponent<EnemyData>().heardMark.GetComponent<SpriteRenderer>().enabled = true;//在组件里渲染箭头                
                    }
                    UpdataManager.Instance.skillLineOpen = false;
                    UpdataManager.Instance.skillMarkOpen = false;
                    SkillManagerMono.Instance.SkillPlay(skilleffect, DataSave.Instance.currentObj, tempSkill);
                    skillReadyToUse = false;
                }



            }


        }
    }
    public void skillset(SkillData skill)
    {
        skillReadyToUse = true;
        tempSkill = skill;
    }
    public void skillover()
    {
        skillReadyToUse = false;
    }

    private void CheckEvent(GameObject roomGO)
    {
        var register = roomGO.GetComponent<EventRegister>();
        var roomEvent = RoomEventManager.Instance.GetTriggeredRoomEvent(register);
        if (roomEvent == null)
            return;

        if (RoomEventManager.Instance.CheckRoomEventCondition(roomEvent))
        {
            EventPanel eventPanel = UIManager.Instance.PushPanel(UIPanelType.EventPanel) as EventPanel;
            eventPanel.UpdateRoomEvent(roomEvent);
            RoomEventManager.Instance.ExecuteRoomEventEffect(roomEvent);
        }
    }

    private void CreateRoom(int x, int z)
    {
        if (roomGridmap.GetValue(x, z) != null)
            return;

        GameObject roomGO = Instantiate(room, roomGridmap.GetGridCenter(x, z), Quaternion.identity);
        roomGridmap.SetValue(x, z, roomGO);//要将生成的物体存入网格内
        RoomToStep(x, z);
        RoomBuild.Instance.CreateGrid(roomGridmap.GetWorldPosition(x, z));
        newRoomPos = new Vector2Int(x, z);
        CheckEvent(roomGO);
    }

    private void FixedUpdate()
    {
        foreach (var item in DataSave.Instance.currentPlayers)
        {
            roomGridmap.GetGridXZ(item.Value.gameObject.transform.position, out int x, out int z);//取得角色所在的房间格
            CheckDoor(x, z);
            foreach (var a in doorSave)
            {
                stepGrid.GetGridXZ(item.Value.gameObject.transform.position, out int tx, out int tz);
                if (tx == a.Value.x && tz == a.Value.y)
                {
                    switch (a.Key)//检测踩入的门格是什么方向的
                    {
                        case "forward":
                            CreateRoom(x, z + 1);
                            break;
                        case "left":
                            CreateRoom(x - 1, z);
                            break;
                        case "right":
                            CreateRoom(x + 1, z);
                            break;
                        case "back":
                            CreateRoom(x, z - 1);
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

    public void GetRoomBound(int x, int z, out int minX, out int minZ, out int maxX, out int maxZ)
    {
        roomGridmap.GetGridRangePoints(x, z, out int _maxX, out int _minX, out int _maxZ, out int _minZ);
        GridTranslateToSmall(_minX, _minZ, out minX, out minZ);
        GridTranslateToSmall(_maxX, _maxZ, out maxX, out maxZ);
    }

    public void CheckDoor(int x, int z)//现在只有一种房间类型，后续根据房间类型的增多需要进行修改
    {
        doorSave.Clear();
        int maxX, minX, maxZ, minZ;
        int tampXx, tampMx, tampXz, tampMz;
        int midX, midZ;
        roomGridmap.GetGridRangePoints(x, z, out maxX, out minX, out maxZ, out minZ);//已经取出该网格坐标的格子的四个界限点
        GridTranslateToSmall(maxX, maxZ, out tampXx, out tampXz);//大网格坐标转换成小网格坐标
        tampXz--;
        tampXx--;
        GridTranslateToSmall(minX, minZ, out tampMx, out tampMz);
        midX = (tampXx - tampMx) / 2 + tampMx;//取得边界坐标的中点
        midZ = (tampXz - tampMz) / 2 + tampMz;
        //该单元格的四个门点都标识了
        doorSave.Add("forward", new Vector2(midX, tampXz));//存入门的点位
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
