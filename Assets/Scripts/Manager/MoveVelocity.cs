using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : Singleton<MoveVelocity>
{
    // Start is called before the first frame update
    //private Vector3 movePosition;
    private List<PathNode> paths;
   // private Vector3 pointPosition;
   // private int celllong;
   // private int minX, minZ, roomcell;
    //private PathFinder pathFinder;
    private GameObject Object;
    private Vector3 targetPoint;
    private int i;
    public bool moveFinish=true;
    private int pathIndex;

    public void SetMoveData(GameObject Object, List<PathNode> paths)
    {
        if (moveFinish)
        {
            this.Object = Object;         
            this.paths = paths;         
            i = 0;           
            moveFinish = false;
            pathIndex = paths.ToArray().Length;
        }        
    }
    public void EnemyMoveData(GameObject Object, List<PathNode> paths,int moveStep)
    {
        this.Object = Object;
        this.paths = paths;
        i = 0;
        moveFinish = false;
        pathIndex = moveStep;
    }

    // Update is called once per frame
    void Update()
    {
        if (paths != null)
        {
            GridManager.Instance.pathFinder.GetGrid().GetGridXZ(Object.transform.position, out int px, out int pz);//在移动前把当前位置的节点跟改为可以行走
            GridManager.Instance.pathFinder.GetGrid().GetValue(px, pz).canWalk = true;
                   
            if(i <= pathIndex-1)
            {
                targetPoint = GridManager.Instance.pathFinder.GetGrid().GetGridCenter(paths[i].x, paths[i].z);
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
                //Object.transform.position = Vector3.MoveTowards(Object.transform.position, pathFinder.GetGrid().GetGridCenter(endX,endZ), 3f * Time.deltaTime);
               // Object.transform.LookAt(pathFinder.GetGrid().GetGridCenter(endX, endZ));
                if(pathIndex>0)
                    GridManager.Instance.pathFinder.GetGrid().GetValue(paths[pathIndex-1].x, paths[pathIndex-1].z).canWalk = false;
                moveFinish = true;
            }
        }
    }
}
