using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Gridmap<TGridObject> 

{
    public int width;
    public int height;
    public TGridObject[,] gridArray;
    public float celllong;
    public Vector3 pointPosition;
 public Gridmap(int width,int height,float celllong,Vector3 pointPosition,Func<Gridmap<TGridObject>, int, int, TGridObject> createGrid)//Func是委托功能
    {
        this.width = width;
        this.height = height;
        this.celllong = celllong;
        this.pointPosition = pointPosition;
        gridArray = new TGridObject[width, height];
        //此处是在绘制观测用的网格，但只画了每个点向上的竖线和向右的横线
        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int z = 0; z < gridArray.GetLength(1); z++)
            {
                gridArray[x, z] = createGrid(this, x, z);//此处存了一个泛型进数组
                Debug.Log(x + " ," + z);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.green, 500f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.green, 500f);
                Debug.DrawLine(GetGridCenter(x, z), Vector3.up + GetGridCenter(x, z), Color.red, 100f);//绘制网格中心点

            }
        }
        //网格中不完整的部分在这里补齐
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.green, 500f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.green, 500f);
    }
    public Vector3 GetGridCenter(int x,int z)
    {
        return GetWorldPosition(x, z) + new Vector3(celllong / 2f,0, celllong / 2f); 
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    public float GetCelllong()
    {
        return celllong;
    }
    public Vector3 GetWorldPosition(int x,int z)//根据网格坐标转化世界坐标值 
    {
        return new Vector3(x,0,z) * celllong + pointPosition;
    }

    public void GetGridXZ(Vector3 WorldPosition,out int x,out int z)//根据世界坐标值转换对应的网格坐标
    {
        x = Mathf.FloorToInt((WorldPosition - pointPosition).x / celllong);
        z = Mathf.FloorToInt((WorldPosition - pointPosition).z / celllong);
    }

    public void SetValue(int x,int z, TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            gridArray[x, z] = value;
        }
      //这里差一段代码，记得回去看下P1;

    }
    public void SetValue(Vector3 WorldPosition, TGridObject value)
    {
        int x, z;
        GetGridXZ(WorldPosition, out x, out z);
        SetValue(x, z, value);

    }

    public TGridObject GetValue(int x,int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
           return gridArray[x, z] ;
        }
        else
        {
            return default(TGridObject);
        }
    }
    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, z;
        GetGridXZ(worldPosition, out x, out z);
        return GetValue(x, z);
    }
}
