using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    private Gridmap<PathNode> grid;
    public int x;
    public int z;
    public bool canWalk =false;


    public int g;//从起点到当前位置距离
    public int h;//从终点到当前位置距离
    public int f;//权重，越低越优先

    public PathNode lastNode;//为了找到来的路
    //ctrl + r按两下是集体重命名
    public PathNode(Gridmap<PathNode> grid,int x,int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }
    public void Getf()
    {
        f = g + h;
    }
}
