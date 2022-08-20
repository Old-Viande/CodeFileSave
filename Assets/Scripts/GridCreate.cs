using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreate : Singleton<GridCreate>
{
    public GameObject cube;

    public Gridmap<GameObject> gridmap;
    public int weith;
    public int height;
    public float celllong;
    public Vector3 Position;
    public LayerMask layerMask;
    public Camera camera;
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
        gridmap = new Gridmap<GameObject>(weith, height, celllong, Position, CreateOne);

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
            Debug.DrawLine(ray.origin, ray.direction * 100 + ray.origin, Color.red, 100f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, (int)layerMask))
            {
                int x, z;

                //hit.collider.gameObject.SetActive(false);
                hit.collider.GetComponent<MeshRenderer>().enabled = !hit.collider.GetComponent<MeshRenderer>().enabled;
                gridmap.GetGridXZ(hit.transform.position, out x, out z);
                if (hit.collider.GetComponent<MeshRenderer>().enabled)
                {
                    gameObjectSave.TryGetValue(new Vector2(x, z), out gridmap.gridArray[x, z]);
                    gameObjectSave.Remove(new Vector2(x, z));
                }
                else
                {
                    gameObjectSave.Add(new Vector2(x, z), gridmap.gridArray[x, z]);
                    gridmap.gridArray[x, z] = null;
                }
                gridmap.GetGridXZ(hit.collider.gameObject.transform.position, out int cubex, out int cubez);
                //Debug.DrawLine(gridmap.gridArray[cubex, cubez].transform.position, Vector3.up * 5 + gridmap.gridArray[cubex, cubez].transform.position, Color.black, 500f);
                Debug.DrawLine(gridmap.GetGridCenter(cubex, cubez), Vector3.up * 2 + gridmap.GetGridCenter(cubex, cubez), Color.cyan, 100f);

            }
        }

    }
    private void FixedUpdate()
    {
        for (int x = 0; x < gridmap.gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridmap.gridArray.GetLength(1); z++)
            {
                directionCheck(x, z);
            }
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
        WallHide(x, z);
        WallCheck(wallType, x, z);
    }
    //private void CheckAll(string a, string b, string c)
    private void WallHide(int x,int z)
    {
        GameObject gameObject;
        if (gridmap.gridArray[x, z] != null)
            gameObject = gridmap.gridArray[x, z];
        else 
            gameObjectSave.TryGetValue(new Vector2(x, z), out gameObject);

        gameObject.transform.Find("forward").gameObject.SetActive(false);
        gameObject.transform.Find("left").gameObject.SetActive(false);
        gameObject.transform.Find("right").gameObject.SetActive(false);
        gameObject.transform.Find("back").gameObject.SetActive(false);
        if (z + 1 < gridmap.gridArray.GetLength(1)) 
        {
            if (gridmap.gridArray[x, z+1] != null)
                gameObject = gridmap.gridArray[x, z+1];
            else
                gameObjectSave.TryGetValue(new Vector2(x, z+1), out gameObject);
            gameObject.transform.Find("back").gameObject.SetActive(false);
        }
        if (x - 1 >= 0) 
        {
            if (gridmap.gridArray[x-1, z] != null)
                gameObject = gridmap.gridArray[x-1, z];
            else
                gameObjectSave.TryGetValue(new Vector2(x-1, z), out gameObject);
            gameObject.transform.Find("right").gameObject.SetActive(false);
        }
        if (x + 1 < gridmap.gridArray.GetLength(0)) 
        {
            if (gridmap.gridArray[x+1, z] != null)
                gameObject = gridmap.gridArray[x+1, z];
            else
                gameObjectSave.TryGetValue(new Vector2(x+1, z), out gameObject);
            gameObject.transform.Find("left").gameObject.SetActive(false);
        }
        if (z - 1 >= 0)
        {
            if (gridmap.gridArray[x, z-1] != null)
                gameObject = gridmap.gridArray[x, z-1];
            else
                gameObjectSave.TryGetValue(new Vector2(x, z-1), out gameObject);
            gameObject.transform.Find("forward").gameObject.SetActive(false);
        }

    }
    private void WallCheck(WallType wallType, int x, int z)
    {
        if (gridmap.gridArray[x, z] != null)
        {
            GameObject it = gridmap.gridArray[x, z];
            if (wallType.HasFlag(WallType.forward))
            {
                if (z + 1 >= gridmap.gridArray.GetLength(1))
                {
                    it.transform.Find("forward").gameObject.SetActive(true);
                }else if (gridmap.gridArray[x, z + 1] == null)
                {
                    gameObjectSave.TryGetValue(new Vector2(x, z + 1), out GameObject gameObject);
                    if (gameObject != null)
                        gameObject.transform.Find("back").gameObject.SetActive(true);
                    it.transform.Find("forward").gameObject.SetActive(true);
                } 
               
            }
            if (wallType.HasFlag(WallType.left))
            {
                if (x - 1 < 0)
                {
                    it.transform.Find("left").gameObject.SetActive(true);
                }
                else if (gridmap.gridArray[x-1, z] == null)
                {
                    gameObjectSave.TryGetValue(new Vector2(x-1, z), out GameObject gameObject);
                    if (gameObject != null)
                        gameObject.transform.Find("right").gameObject.SetActive(true);
                    it.transform.Find("left").gameObject.SetActive(true);
                }
            }
            if (wallType.HasFlag(WallType.right))
            {
                if (x + 1 >= gridmap.gridArray.GetLength(0))
                {
                    it.transform.Find("right").gameObject.SetActive(true);
                }
                else if (gridmap.gridArray[x+1, z] == null)
                {
                    gameObjectSave.TryGetValue(new Vector2(x+1, z), out GameObject gameObject);
                    if (gameObject != null)
                        gameObject.transform.Find("left").gameObject.SetActive(true);
                    it.transform.Find("right").gameObject.SetActive(true);
                }
            }
            if (wallType.HasFlag(WallType.back))
            {
                if (z - 1 <0)
                {
                    it.transform.Find("back").gameObject.SetActive(true);
                }
                else if (gridmap.gridArray[x, z - 1] == null)
                {
                    gameObjectSave.TryGetValue(new Vector2(x, z - 1), out GameObject gameObject);
                    if (gameObject != null)
                        gameObject.transform.Find("forward").gameObject.SetActive(true);
                    it.transform.Find("back").gameObject.SetActive(true);
                }
            }
        }
        else
        {
            gameObjectSave.TryGetValue(new Vector2(x, z), out GameObject two);
           
            if (wallType.HasFlag(WallType.forward))
            {
              if (z - 1 >=0&&gridmap.gridArray[x, z - 1] != null)
                {
                    gridmap.gridArray[x,z-1].gameObject.transform.Find("forward").gameObject.SetActive(true);                   
                    two.transform.Find("back").gameObject.SetActive(true);
                }

            }
            if (wallType.HasFlag(WallType.left))
            {
              if (x + 1 < gridmap.gridArray.GetLength(0) && gridmap.gridArray[x + 1, z] != null)
                {
                    gridmap.gridArray[x+1, z ].gameObject.transform.Find("left").gameObject.SetActive(true);
                    two.transform.Find("right").gameObject.SetActive(true);
                }
            }
            if (wallType.HasFlag(WallType.right))
            {
               if (x - 1 >= 0 && gridmap.gridArray[x - 1, z] != null)
                {
                    gridmap.gridArray[x-1, z ].gameObject.transform.Find("right").gameObject.SetActive(true);
                    two.transform.Find("left").gameObject.SetActive(true);
                }
            }
            if (wallType.HasFlag(WallType.back))
            {
            if (z + 1 < gridmap.gridArray.GetLength(1) && gridmap.gridArray[x, z +1] != null)
                {
                    gridmap.gridArray[x, z + 1].gameObject.transform.Find("back").gameObject.SetActive(true);
                    two.transform.Find("forward").gameObject.SetActive(true);
                }
            }
        }
    }
   /* private void WallCheckOver(WallType wallType, int x, int z)
    {



        if (wallType.HasFlag(WallType.forward))
        {
            if (z + 1 < gridmap.gridArray.GetLength(1) && gridmap.gridArray[x, z + 1] != null)
            {
                if (gridmap.gridArray[x, z] != null)              
                gridmap.gridArray[x, z].transform.Find("forward").gameObject.SetActive(false);
                else
                {
                    gameObjectSave.TryGetValue(new Vector2(x, z), out GameObject gameObject);
                    gameObject.transform.Find("forward").gameObject.SetActive(false);
                }
                gridmap.gridArray[x, z + 1].transform.Find("back").gameObject.SetActive(false);
            }
        }
        else
        {
            if (gridmap.gridArray[x, z] != null)
                gridmap.gridArray[x, z].transform.Find("forward").gameObject.SetActive(true);
            else
            {
                gameObjectSave.TryGetValue(new Vector2(x, z), out GameObject gameObject);
                gameObject.transform.Find("forward").gameObject.SetActive(true);
            }

            if (z + 1 < gridmap.gridArray.GetLength(1) && gridmap.gridArray[x, z + 1] == null)
            {

                // gridmap.gridArray[x, z + 1].transform.Find("back").gameObject.SetActive(true);
                gameObjectSave.TryGetValue(new Vector2(x, z + 1),out GameObject gameObject);
                gameObject.transform.Find("back").gameObject.SetActive(true);
            }

        }
        if (wallType.HasFlag(WallType.left))
        {
            if (x - 1 >= 0 && gridmap.gridArray[x - 1, z] != null)
            {
                if (gridmap.gridArray[x, z] != null)
                    gridmap.gridArray[x, z].transform.Find("left").gameObject.SetActive(false);
                else
                {
                    gameObjectSave.TryGetValue(new Vector2(x, z), out GameObject gameObject);
                    gameObject.transform.Find("left").gameObject.SetActive(false);
                }
                gridmap.gridArray[x - 1, z].transform.Find("right").gameObject.SetActive(false);
            }

        }
        else
        {
            if (gridmap.gridArray[x, z] != null)
                gridmap.gridArray[x, z].transform.Find("left").gameObject.SetActive(true);
            else
            {
                gameObjectSave.TryGetValue(new Vector2(x, z), out GameObject gameObject);
                gameObject.transform.Find("left").gameObject.SetActive(true);
            }
            if (x - 1 >= 0 && gridmap.gridArray[x - 1, z] == null)
            {
               // gridmap.gridArray[x - 1, z].transform.Find("right").gameObject.SetActive(true); 
                gameObjectSave.TryGetValue(new Vector2(x-1, z), out GameObject gameObject);
                gameObject.transform.Find("right").gameObject.SetActive(true);
            }

        }
        if (wallType.HasFlag(WallType.right))
        {
            if (x + 1 < gridmap.gridArray.GetLength(0) && gridmap.gridArray[x + 1, z] != null)
            {
                if (gridmap.gridArray[x, z] != null)
                    gridmap.gridArray[x, z].transform.Find("right").gameObject.SetActive(false);
                else
                {
                    gameObjectSave.TryGetValue(new Vector2(x, z), out GameObject gameObject);
                    gameObject.transform.Find("right").gameObject.SetActive(false);
                }
                gridmap.gridArray[x + 1, z].transform.Find("left").gameObject.SetActive(false);
            }

        }
        else
        {
            if (gridmap.gridArray[x, z] != null)
                gridmap.gridArray[x, z].transform.Find("right").gameObject.SetActive(true);
            else
            {
                gameObjectSave.TryGetValue(new Vector2(x, z), out GameObject gameObject);
                gameObject.transform.Find("right").gameObject.SetActive(true);
            }
            if (x + 1 < gridmap.gridArray.GetLength(0) && gridmap.gridArray[x + 1, z] == null)
            {
                // gridmap.gridArray[x + 1, z].transform.Find("left").gameObject.SetActive(true);
                gameObjectSave.TryGetValue(new Vector2(x+1, z ), out GameObject gameObject);
                gameObject.transform.Find("left").gameObject.SetActive(true);
            }

        }
        if (wallType.HasFlag(WallType.back))
        {
            if (z - 1 >= 0 && gridmap.gridArray[x, z - 1] != null)
            {
                if (gridmap.gridArray[x, z] != null)
                    gridmap.gridArray[x, z].transform.Find("back").gameObject.SetActive(false);
                else
                {
                    gameObjectSave.TryGetValue(new Vector2(x, z), out GameObject gameObject);
                    gameObject.transform.Find("back").gameObject.SetActive(false);
                }
                gridmap.gridArray[x, z - 1].transform.Find("forward").gameObject.SetActive(false);

            }

        }
        else
        { if (gridmap.gridArray[x, z] != null)
            gridmap.gridArray[x, z].transform.Find("back").gameObject.SetActive(true);
            else
            {
                gameObjectSave.TryGetValue(new Vector2(x, z), out GameObject gameObject);
                gameObject.transform.Find("back").gameObject.SetActive(true);
            }
            if (z - 1 >= 0 && gridmap.gridArray[x, z - 1] == null)
            {
                //gridmap.gridArray[x, z - 1].transform.Find("forward").gameObject.SetActive(true);
                gameObjectSave.TryGetValue(new Vector2(x, z -1), out GameObject gameObject);
                gameObject.transform.Find("forward").gameObject.SetActive(true);
            }

        }




    }*/

    /* private void WallCheck(string a, string b, GameObject cube)
     {
         string[] save = new string[] { a, b };
         int x;
         int z;
         gridmap.GetGridXZ(cube.transform.position, out x, out z);
         foreach (var word in save)
         {
             if (word.Contains("forward"))
             {
                 if (z + 1 < gridmap.gridArray.GetLength(1))
                 {
                     if (gridmap.gridArray[x, z + 1] != null)
                     {
                         gridmap.gridArray[x, z].transform.Find(word).gameObject.SetActive(false);
                     }
                 }


             }
             else if (word.Contains("left"))
             {
                 if (x - 1 >= 0)
                 {
                     if (gridmap.gridArray[x - 1, z] != null)
                     {
                         gridmap.gridArray[x, z].transform.Find(word).gameObject.SetActive(false);
                     }
                 }

             }
             else if (word.Contains("right"))
             {
                 if (x + 1 < gridmap.gridArray.GetLength(0))
                 {
                     if (gridmap.gridArray[x + 1, z] != null)
                     {
                         gridmap.gridArray[x, z].transform.Find(word).gameObject.SetActive(false);
                     }
                 }

             }
             else if (word.Contains("back"))
             {
                 if (z - 1 >= 0)
                 {
                     if (gridmap.gridArray[x, z - 1] != null)
                     {
                         gridmap.gridArray[x, z].transform.Find(word).gameObject.SetActive(false);
                     }
                 }

             }
         }

     }*/
}
