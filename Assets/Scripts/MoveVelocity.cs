using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : Singleton<MoveVelocity>
{
    // Start is called before the first frame update
    private Vector3 movePosition;
    private List<PathNode> paths;
    private Vector3 pointPosition;
    private int celllong;
    private int minX, minZ, roomcell;
    private PathFinder pathFinder;
    private GameObject Object;
    private Vector3 targetPoint;
    private int i;
    private bool moveFinish=true;
    private int endX, endZ;
    
    public void SetMoveData( int X, int Z,int celllong,GameObject Object, List<PathNode> paths,int endX,int endZ)
    {
        if (moveFinish)
        {
            this.Object = Object;
            minX = X * celllong;
            minZ = Z * celllong;
            this.paths = paths;
            pathFinder = new PathFinder(minX, minZ);
            i = 0;
            this.endX = endX;
            this.endZ = endZ;
           // moveFinish = false;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (paths != null)
        {   
            int pathIndex = paths.ToArray().Length;
            targetPoint = pathFinder.GetGrid().GetGridCenter(paths[i].x, paths[i].z);
            if(i < pathIndex-1)
            {
                //if (Mathf.FloorToInt((targetPoint - Object.transform.position).magnitude) != 0)
                //{
                //    Object.transform.position = Vector3.MoveTowards(Object.transform.position, targetPoint, 3f*Time.deltaTime);
                //    /*Object.transform.LookAt(targetPoint);
                //    Object.transform.Translate(Vector3.forward * Time.deltaTime);*/
                //}
                //else
                //{
                //    i++; 
                //}
                                   
                if (Mathf.CeilToInt((targetPoint - Object.transform.position).magnitude) != 0)//如果目标未移动到指定坐标，则持续移动，将指定的坐标点减去目标坐标点后进行数值化得到距离，然后按照最小值进行取整
                {
                    Object.transform.position = Vector3.MoveTowards(Object.transform.position, targetPoint, 3f * Time.deltaTime);                   
                    Object.transform.LookAt(targetPoint);
                }
                else
                {
                    i++;
                }                
            } else
            {
                i = pathIndex-1 ;
                Object.transform.position = Vector3.MoveTowards(Object.transform.position, pathFinder.GetGrid().GetGridCenter(endX,endZ), 3f * Time.deltaTime);
                Object.transform.LookAt(pathFinder.GetGrid().GetGridCenter(endX, endZ));
                //moveFinish = true;
            }
        }
    }
}
