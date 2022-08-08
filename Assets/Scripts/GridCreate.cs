using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreate : Singleton<GridCreate>
{
    public GameObject cube;

    public Gridmap<GameObject> gridmap;
    public int x;
    public int z;
    public float celllong;
    public Vector3 Position;
    public LayerMask layerMask;
    public Camera camera;
    public float check;
    public CameraMove cameraMove;


    // Start is called before the first frame update
    void Start()
    {
        gridmap = new Gridmap<GameObject>(x, z, celllong, Position, CreateOne);
        
    }
    private GameObject CreateOne(Gridmap<GameObject> grid, int x, int z)//简化的方法写在PthFinder的构造函数里
    {
        return Instantiate(cube, grid.GetGridCenter(x, z), Quaternion.identity);//会在网格中存入一个生成的物体，存入的步骤在Gridmap脚本
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 save = gridmap.gridArray[0, 0].transform.forward;
        Vector3 camerasave = camera.ScreenPointToRay(Vector2.zero).direction;
        check = Vector3.Dot(save, camerasave);


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.direction*100+ray.origin, Color.red,100f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,Mathf.Infinity, (int) layerMask))
            {

                //hit.collider.gameObject.SetActive(false);
                hit.collider.GetComponent<MeshRenderer>().enabled = !hit.collider.GetComponent<MeshRenderer>().enabled;
                gridmap.GetGridXZ(hit.collider.gameObject.transform.position,out int cubex,out int cubez);
                Debug.DrawLine(gridmap.gridArray[cubex, cubez].transform.position, Vector3.up * 5 + gridmap.gridArray[cubex, cubez].transform.position, Color.black, 500f);
                Debug.DrawLine(gridmap.GetGridCenter(cubex, cubez), Vector3.up*2 + gridmap.GetGridCenter(cubex, cubez), Color.cyan, 100f);
                
            }
        }
       
    }
    private void FixedUpdate()
    {
        if(cameraMove.direction>240&& cameraMove.direction <= 300)
        {
            WallCheck("forward", "left","right");
        } else if(cameraMove.direction>300&& cameraMove.direction <= 330)
        {

            WallCheck("forward", "left");
        }
        else if (cameraMove.direction>-30&& cameraMove.direction<=30)
        {

            WallCheck("forward", "left", "back");
        }
        else if (cameraMove.direction>30&& cameraMove.direction<=60)
        {

            WallCheck( "left", "back");
        }
        else if (cameraMove.direction>60&& cameraMove.direction<=120)
        {

            WallCheck("right", "left", "back");
        }
        else if (cameraMove.direction>120&& cameraMove.direction<=150)
        {

            WallCheck("right","back");

        }
        else if(cameraMove.direction>150&& cameraMove.direction <= 210)
        {

            WallCheck("forward", "right", "back");
        }
        else if(cameraMove.direction>210&& cameraMove.direction <= 240)
        {
  
            WallCheck("forward", "right");
        }
    }

    private void WallCheck(string a, string b, string c)
    {
        string[] save = new string []{ a, b, c };
        foreach(var word in save)
        {
            if (word.Contains("forward"))
            {
                if (cameraMove.arreyz + 1 < gridmap.gridArray.GetLength(1))
                {
                    if (gridmap.gridArray[cameraMove.arreyx, cameraMove.arreyz + 1] != null)
                    {
                        gridmap.gridArray[cameraMove.arreyx, cameraMove.arreyz].transform.Find(word).gameObject.SetActive(false);
                    }
                }


            }
            else if (word.Contains("left"))
            {
                if (cameraMove.arreyx - 1 >= 0)
                {
                    if (gridmap.gridArray[cameraMove.arreyx - 1, cameraMove.arreyz] != null)
                    {
                        gridmap.gridArray[cameraMove.arreyx, cameraMove.arreyz].transform.Find(word).gameObject.SetActive(false);
                    }
                }

            }
            else if (word.Contains("right"))
            {
                if (cameraMove.arreyx + 1 < gridmap.gridArray.GetLength(0))
                {
                    if (gridmap.gridArray[cameraMove.arreyx + 1, cameraMove.arreyz] != null)
                    {
                        gridmap.gridArray[cameraMove.arreyx, cameraMove.arreyz].transform.Find(word).gameObject.SetActive(false);
                    }
                }

            }
            else if (word.Contains("back"))
            {
                if (cameraMove.arreyz - 1 >= 0)
                {
                    if (gridmap.gridArray[cameraMove.arreyx, cameraMove.arreyz - 1] != null)
                    {
                        gridmap.gridArray[cameraMove.arreyx, cameraMove.arreyz].transform.Find(word).gameObject.SetActive(false);
                    }
                }

            }
        }
        
       
        
    }
    private void WallCheck(string a, string b)
    {
        string[] save = new string[] { a, b };
        foreach (var word in save)
        {
            if (word.Contains("forward"))
            {
                if (cameraMove.arreyz + 1 < gridmap.gridArray.GetLength(1))
                {
                    if (gridmap.gridArray[cameraMove.arreyx, cameraMove.arreyz + 1] != null)
                    {
                        gridmap.gridArray[cameraMove.arreyx, cameraMove.arreyz].transform.Find(word).gameObject.SetActive(false);
                    }
                }
               

            }
            else if (word.Contains("left"))
            {
                if (cameraMove.arreyx -1 >= 0)
                {
                    if (gridmap.gridArray[cameraMove.arreyx - 1, cameraMove.arreyz] != null)
                    {
                        gridmap.gridArray[cameraMove.arreyx, cameraMove.arreyz].transform.Find(word).gameObject.SetActive(false);
                    }
                }
                    
            }
            else if (word.Contains("right"))
            {
                if (cameraMove.arreyx + 1 < gridmap.gridArray.GetLength(0))
                {
                    if (gridmap.gridArray[cameraMove.arreyx + 1, cameraMove.arreyz] != null)
                    {
                        gridmap.gridArray[cameraMove.arreyx, cameraMove.arreyz].transform.Find(word).gameObject.SetActive(false);
                    }
                }
                    
            }
            else if (word.Contains("back"))
            {
                if (cameraMove.arreyz - 1 >= 0)
                {
                    if (gridmap.gridArray[cameraMove.arreyx, cameraMove.arreyz - 1] != null)
                    {
                        gridmap.gridArray[cameraMove.arreyx, cameraMove.arreyz].transform.Find(word).gameObject.SetActive(false);
                    }
                }
                
            }
        }

    }
}
