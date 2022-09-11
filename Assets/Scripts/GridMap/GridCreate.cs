//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GridCreate : MonoBehaviour
//{
//    public Gridmap<GameObject> gridmap;
//    public Gridmap<GameObject> smap;
//    public PathFinder pathFinder;
//    public int width;
//    public int height;
//    public int celllong;
//    private int x, z;
//    public int pathFinderlong;
//    public Vector3 origenPoint;
//    public GameObject cube;
//    public GameObject plan;
//    public Character_SO characterData;
//    public Enemy_SO enemyData;
//    private int minX, minY, roomcell;
//    public Camera camera;
//    public GameObject character;
//    public LayerMask layerMask;
//    private int pX, pZ;
//    private bool canClick = false;
//    private void Start()
//    {
//        gridmap = new Gridmap<GameObject>( maxwidth,  maxheight,  minwidth. minheight, celllong, origenPoint, Outdo);

//        minX = width * celllong;
//        minY = height * celllong;
//        roomcell = 1;
//        pathFinder = new PathFinder(minX, minY, roomcell);
//        smap = new Gridmap<GameObject>(minX, minY, roomcell, origenPoint, Out);

//    }
//    public GameObject Outdo(Gridmap<GameObject> grid, int x, int z)
//    {
//        return Instantiate(cube, grid.GetGridCenter(x, z) + Vector3.down * 0.55f, Quaternion.identity);
//    }
//    public GameObject Out(Gridmap<GameObject> grid, int x, int z)
//    {
//        return Instantiate(plan, grid.GetGridCenter(x, z), Quaternion.identity);
//    }
//    public void Update()
//    {
//        Vector3 save;

//        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
//        if (Input.GetMouseButtonDown(0))
//        {
//            Debug.DrawLine(ray.origin, ray.direction * 100 + ray.origin, Color.red, 100f);

//            if (Input.GetMouseButtonDown(0))
//            {
//                Debug.DrawLine(ray.origin, ray.direction * 100 + ray.origin, Color.red, 100f);
//                RaycastHit hit;

//                //smap.GetGridXZ(character.transform.position, out int pX, out int Pz);
//                if (canClick)
//                {
//                    if (smap.GetValue(pX, pZ) != null)
//                    {
//                        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
//                        {
//                            smap.GetGridXZ(hit.collider.transform.position, out int hitX, out int hitZ);

//                            Debug.Log("hit " + hitX + " ," + hitZ + " !");
//                            Vector3 cell = hit.collider.transform.position;
//                            smap.GetGridXZ(cell, out int endX, out int endZ);
//                            List<PathNode> path = pathFinder.FindPath(pX, pZ, endX, endZ);
//                            TimeCountDown.Instance.AddClock(10f, false, "tset");
//                            //MoveVelocity.Instance.SetMoveData(character, path, endX, endZ);
//                            canClick = false;
//                        }
//                    }
//                }

//                if (Physics.Raycast(ray, out hit, Mathf.Infinity, (int)layerMask))
//                {
//                    if (hit.collider.tag == "Player")
//                    {
//                        character = hit.collider.gameObject;
//                        save = hit.collider.transform.position;
//                        smap.GetGridXZ(save, out pX, out pZ);
//                        canClick = true;
//                    }

//                }

//            }
//        }
//        if (TimeCountDown.Instance.clocks.ToArray().Length != 0)
//        {
//            Debug.Log(Mathf.CeilToInt(TimeCountDown.Instance.clocks[0]) + " this is list and in Grid ");
//        }
//    }


//}


