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
 public Gridmap(int width,int height,float celllong,Vector3 pointPosition,Func<Gridmap<TGridObject>, int, int, TGridObject> createGrid)//Func��ί�й���
    {
        this.width = width;
        this.height = height;
        this.celllong = celllong;
        this.pointPosition = pointPosition;
        gridArray = new TGridObject[width, height];
        //�˴����ڻ��ƹ۲��õ����񣬵�ֻ����ÿ�������ϵ����ߺ����ҵĺ���
        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int z = 0; z < gridArray.GetLength(1); z++)
            {
                gridArray[x, z] = createGrid(this, x, z);//�˴�����һ�����ͽ�����
                Debug.Log(x + " ," + z);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.green, 500f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.green, 500f);
                Debug.DrawLine(GetGridCenter(x, z), Vector3.up + GetGridCenter(x, z), Color.red, 100f);//�����������ĵ�

            }
        }
        //�����в������Ĳ��������ﲹ��
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
    public Vector3 GetWorldPosition(int x,int z)//������������ת����������ֵ 
    {
        return new Vector3(x,0,z) * celllong + pointPosition;
    }

    public void GetGridXZ(Vector3 WorldPosition,out int x,out int z)//������������ֵת����Ӧ����������
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
      //�����һ�δ��룬�ǵû�ȥ����P1;

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
