using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Gridmap<TGridObject>

{
    public int maxWidth;
    public int minWidth;
    public int maxHeight;
    public int minHeight;
    //public TGridObject[,] gridArray;
    public Dictionary<Vector2, TGridObject> gridDictionary = new Dictionary<Vector2, TGridObject>();
    public int xArray;
    public int zArray;
    public float celllong;
    public int maxX, minX, maxZ, minZ;
    public Vector3 pointPosition;
    public Gridmap(int maxwidth, int maxheight,int minwidth ,int minheight,float celllong, Vector3 pointPosition, Func<Gridmap<TGridObject>, int, int, TGridObject> createGrid)//Func��ί�й���
    {
       
        this.maxWidth = maxwidth;
        this.minWidth=minwidth;
        this.maxHeight=maxheight;
        this.minHeight=minheight;
        this.celllong = celllong;
        this.pointPosition = pointPosition;
        //gridArray = new TGridObject[width, height];
        // xArray = Mathf.Abs(width);
        //zArray = Mathf.Abs(height);
        //�˴����ڻ��ƹ۲��õ����񣬵�ֻ����ÿ�������ϵ����ߺ����ҵĺ���
        /* for(int x = 0; x < gridArray.GetLength(0); x++)
         {
             for(int z = 0; z < gridArray.GetLength(1); z++)
             {
                 gridArray[x, z] = createGrid(this, x, z);//�˴�����һ�����ͽ�����
                 Debug.Log(x + " ," + z);
                 Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.green, 500f);
                 Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.green, 500f);
                 Debug.DrawLine(GetGridCenter(x, z), Vector3.up + GetGridCenter(x, z), Color.red, 100f);//�����������ĵ�

             }
         }*/
        /* if (width > 0 && height > 0)
         {
             for (int x = 0; x < xArray; x++)
             {
                 for (int z = 0; z < zArray; z++)
                 {
                     //Debug.Log(x + " ," + z);
                     GetRangeIndex(x, z);
                     gridDictionary.Add(new Vector2(x, z), createGrid(this, x, z));
                     Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.green, 500f);
                     Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.green, 500f);
                     Debug.DrawLine(GetGridCenter(x, z), Vector3.up + GetGridCenter(x, z), Color.red, 100f);
                 }
             }
         }
         else
         if (width < 0 && height > 0)
         {
             for (int x = 1; x < xArray + 1; x++)
             {
                 for (int z = 0; z < zArray; z++)
                 {

                     if (width < 0)
                         x = -x;
                     if (height < 0)
                         z = -z;
                     //Debug.Log(x + " ," + z);
                     GetRangeIndex(x, z);
                     gridDictionary.Add(new Vector2(x, z), createGrid(this, x, z));
                     Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.green, 500f);
                     Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.green, 500f);
                     Debug.DrawLine(GetGridCenter(x, z), Vector3.up + GetGridCenter(x, z), Color.red, 100f);
                     if (width < 0)
                         x = -x;
                     if (height < 0)
                         z = -z;
                 }
             }
         }
         else
         if (width < 0 && height < 0)
         {
             for (int x = 1; x < xArray + 1; x++)
             {
                 for (int z = 1; z < zArray + 1; z++)
                 {

                     if (width < 0)
                         x = -x;
                     if (height < 0)
                         z = -z;
                     //Debug.Log(x + " ," + z);
                     GetRangeIndex(x, z);
                     gridDictionary.Add(new Vector2(x, z), createGrid(this, x, z));
                     Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.green, 500f);
                     Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.green, 500f);
                     Debug.DrawLine(GetGridCenter(x, z), Vector3.up + GetGridCenter(x, z), Color.red, 100f);
                     if (width < 0)
                         x = -x;
                     if (height < 0)
                         z = -z;
                 }
             }
         }
         else
             if (width > 0 && height < 0)
         {
             for (int x = 0; x < xArray; x++)
             {
                 for (int z = 1; z < zArray + 1; z++)
                 {

                     if (width < 0)
                         x = -x;
                     if (height < 0)
                         z = -z;
                     //Debug.Log(x + " ," + z);
                     GetRangeIndex(x, z);
                     gridDictionary.Add(new Vector2(x, z), createGrid(this, x, z));
                     Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.green, 500f);
                     Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.green, 500f);
                     Debug.DrawLine(GetGridCenter(x, z), Vector3.up + GetGridCenter(x, z), Color.red, 100f);
                     if (width < 0)
                         x = -x;
                     if (height < 0)
                         z = -z;
                 }
             }
         }*/
        for (int x = minWidth; x < maxWidth; x++)
        {
            for (int z = minHeight ; z < maxHeight; z++)
            {
                GetRangeIndex(x, z);
                gridDictionary.Add(new Vector2(x, z), createGrid(this, x, z));
                //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.green, 500f);
                //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.green, 500f);
                //Debug.DrawLine(GetGridCenter(x, z), Vector3.up + GetGridCenter(x, z), Color.red, 100f);
            }
        }
    }

    private void GetRangeIndex(int x,int z)
    {
        if (maxX == 0)        
            maxX = x;
        if (maxZ == 0)
            maxZ = z;

        if (x > maxX)
        {
            maxX = x;
        }else if (x < minX)
        {
            minX = x;
        }

        if (z > maxZ)
        {
            maxZ = z;
        }
        else if (z < minZ)
        {
            minZ = z;
        }
    }
    public Vector3 GetGridCenter(int x, int z)
    {
        return GetWorldPosition(x, z) + new Vector3(celllong / 2f, 0, celllong / 2f);
    }

    public void GetGridRangePoints(int x,int z,out int maxX,out int  minX,  out int maxZ,out int minZ)
    {
        minX = x;
        minZ = z;
        maxX = x + 1;
        maxZ = z + 1;
       
    }
   /* public int GetWidth()
    {
        eturn width;
    }*/
   /* public int GetHeight()
    {
        return height;
    }*/
    public float GetCelllong()
    {
        return celllong;
    }
    public Vector3 GetWorldPosition(int x, int z)//������������ת����������ֵ 
    {
        return new Vector3(x, 0, z) * celllong + pointPosition;
    }

    public void GetGridXZ(Vector3 WorldPosition, out int x, out int z)//������������ֵת����Ӧ����������
    {
        x = Mathf.FloorToInt((WorldPosition - pointPosition).x / celllong);//mathf.floortoint ��������ȡ����
        z = Mathf.FloorToInt((WorldPosition - pointPosition).z / celllong);
    }

    /* public void SetValue(int x,int z, TGridObject value)
     {
         if (x >= 0 && z >= 0 && x < width && z < height)
         {
             gridArray[x, z] = value;
         }
       //�����һ�δ��룬�ǵû�ȥ����P1;

     }*/
   /* public void SetValue(int x, int z, TGridObject value)//�ֵ�汾����ֵ,Ҳ���������������������µĸ���(�ɰ汾)
    {
        if (!gridDictionary.ContainsKey(new Vector2(x, z)))
        {
            if (width > 0 && height > 0 && x < width && z < height)
            {
                gridDictionary.Add(new Vector2(x, z), value);
                GetRangeIndex(x, z);
            }
            else if (width > 0 && height < 0 && x < width && z >= height)
            {
                gridDictionary.Add(new Vector2(x, z), value);
                GetRangeIndex(x, z);
            }
            else if (width < 0 && height < 0 && x >= width && z >= height)
            {
                gridDictionary.Add(new Vector2(x, z), value);
                GetRangeIndex(x, z);
            }
            else if (width < 0 && height > 0 && x >= width && z < height)
            {
                gridDictionary.Add(new Vector2(x, z), value);
                GetRangeIndex(x, z);
            }
        }
        else
        {
            gridDictionary.Remove(new Vector2(x, z));
            if (width > 0 && height > 0 && x < width && z < height)
            {
                gridDictionary.Add(new Vector2(x, z), value);
            }
            else if (width > 0 && height < 0 && x < width && z >= height)
            {
                gridDictionary.Add(new Vector2(x, z), value);
            }
            else if (width < 0 && height < 0 && x >= width && z >= height)
            {
                gridDictionary.Add(new Vector2(x, z), value);
            }
            else if (width < 0 && height > 0 && x >= width && z < height)
            {
                gridDictionary.Add(new Vector2(x, z), value);
            }
        }


    }*/
    public void SetValue(int x, int z, TGridObject value)//�ֵ�汾����ֵ,Ҳ���������������������µĸ���(�°汾)
    {
        if (!gridDictionary.ContainsKey(new Vector2(x, z)))
        {
                gridDictionary.Add(new Vector2(x, z), value);
                GetRangeIndex(x, z);            
        }
        else
        {
            
            gridDictionary.Remove(new Vector2(x, z));           
            gridDictionary.Add(new Vector2(x, z), value);            
        }
       
            //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z+1), Color.green, 500f);
            //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x+1,z), Color.green, 500f);
            //Debug.DrawLine(GetGridCenter(x, z), Vector3.up + GetGridCenter(x, z), Color.red, 100f);
            
        
    }
    public void SetValue( Vector3 WorldPosition, TGridObject value)
    {
        int x, z;
        GetGridXZ(WorldPosition, out x, out z);
        SetValue(x, z, value);

    }

    /* public TGridObject GetValue(int x,int z)
     {
         if (x >= 0 && z >= 0 && x < width && z < height)
         {
            return gridArray[x, z] ;
         }
         else
         {
             return default(TGridObject);
         }
     }*/
    /* public TGridObject GetValue(int x, int z)//�ֵ�汾�Ķ�ȡ����(�ɰ汾)
     {
         TGridObject Value;
         if (width > 0 && height > 0 && x < width && z < height)
         {
             gridDictionary.TryGetValue(new Vector2(x, z), out Value);
             return Value;
         }
         else
         if (width > 0 && height < 0 && x < width && z >= height)
         {
             gridDictionary.TryGetValue(new Vector2(x, z), out Value);
             return Value;
         }
         else
         if (width < 0 && height < 0 && x >= width && z >= height)
         {
             gridDictionary.TryGetValue(new Vector2(x, z), out Value);
             return Value;
         }
         else
         if (width < 0 && height > 0 && x >= width && z < height)
         {
             gridDictionary.TryGetValue(new Vector2(x, z), out Value);
             return Value;
         }
         else
         {
             return default(TGridObject);
         }

     */
    public TGridObject GetValue(int x, int z)//�ֵ�汾�Ķ�ȡ����(�°汾)
    {
        TGridObject Value;

        if(gridDictionary.TryGetValue(new Vector2(x, z), out Value))
        return Value;
        else
        return default(TGridObject);
    }
    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, z;
        GetGridXZ(worldPosition, out x, out z);
        return GetValue(x, z);

    }
    
}
