using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : Singleton<TestGrid>
{
    public int x;
    public int z;
    public float celllong;
    public Vector3 Position;
    private PathFinder pathtest;
    public int targetX;
    public int targetZ;
    public GameObject cube;
    private Gridmap<GameObject> gridmap;
    private void Start()
    {
        //Gridmap<bool> gridmap = new Gridmap<bool>(5,5,7f,Position);//这个里面填什么类型其实都不影响

         pathtest = new PathFinder(x,z,celllong);
        List<PathNode> path = pathtest.FindPath(0, 0, targetX, targetZ);
       
        if (path != null)
        {
           for(int i=0;i<path.Count-1;i++)
            {
                //CubeCreate(path[i].x, path[i].z);
                Vector3 save = pathtest.GetGrid().GetGridCenter(path[i].x, path[i].z);
                Vector3 save2 = pathtest.GetGrid().GetGridCenter(path[i+1].x, path[i+1].z);
                Debug.DrawLine(save,save2,Color.blue,500f);
            }
        }   

    }



    private void Update()
    {
      
    }

}
