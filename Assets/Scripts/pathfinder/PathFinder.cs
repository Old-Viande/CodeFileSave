using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private const int MOVE_STRAIGHT_COST = 10;//const �ǳ���
    private const int MOVE_DIAGONAL_COST = 14;   
    public Gridmap<PathNode> grid;
    private List<PathNode> openNodes;
    private List<PathNode> closedNodes;
 
 public PathFinder(int width,int height,float celllong = 1f,Vector3 worldPosition = default)//�˴��ǹ��캯��
    {
       
        grid = new Gridmap<PathNode>(width, height, celllong, worldPosition,  delegate (Gridmap<PathNode> grid, int x, int z) { return new PathNode(grid, x, z); }); //ί����һ�������ļ�д
    }
    
public Gridmap<PathNode> GetGrid()
    {
        return grid;
    }
    public List<PathNode> FindPath(int startX, int startZ, int endX, int endZ) //���뿪ʼ������������Լ�Ŀ������������
    {
        PathNode startNode = grid.GetValue(startX, startZ);
        PathNode endNode = grid.GetValue(endX, endZ);
        openNodes = new List<PathNode> { startNode };
        closedNodes = new List<PathNode>();
    
        if (grid.GetWidth() > 0&&grid.GetHeight()>0)
        {
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int z = 0; z < grid.GetHeight(); z++)
                {
                    PathNode pathNode = grid.GetValue(x, z);
                    pathNode.g = int.MaxValue;
                    pathNode.Getf();
                    pathNode.lastNode = null;
                }
            }
        }
        else if (grid.GetWidth() > 0 && grid.GetHeight() < 0)
        {
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int z = -1; z > grid.GetHeight()-1; z--)
                {
                    PathNode pathNode = grid.GetValue(x, z);
                    pathNode.g = int.MaxValue;
                    pathNode.Getf();
                    pathNode.lastNode = null;
                }
            }
        }
        else if (grid.GetWidth() < 0 && grid.GetHeight() < 0)
        {
            for (int x = -1; x > grid.GetWidth()-1; x--)
            {
                for (int z = -1; z > grid.GetHeight()-1; z--)
                {
                    PathNode pathNode = grid.GetValue(x, z);
                    pathNode.g = int.MaxValue;
                    pathNode.Getf();
                    pathNode.lastNode = null;
                }
            }
        }
        else if (grid.GetWidth() < 0 && grid.GetHeight() > 0)
        {
            for (int x = -1; x > grid.GetWidth()-1; x--)
            {
                for (int z = 0; z < grid.GetHeight(); z++)
                {
                    PathNode pathNode = grid.GetValue(x, z);
                    pathNode.g = int.MaxValue;
                    pathNode.Getf();
                    pathNode.lastNode = null;
                }
            }
        }
        startNode.g = 0;
        startNode.h = GetDistanceCost(startNode,endNode);
        startNode.Getf();


        while (openNodes.Count > 0)//ѭ�������еĿ����߸���
        {
            PathNode currentNode = GetCurrentNode(openNodes);
            if (currentNode == endNode)
            {
                return GetPath(endNode);
            }

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            foreach(PathNode aroundNode in AroundNodes(currentNode))//�����Χ�Ľڵ�
            {
                if (closedNodes.Contains(aroundNode)) continue;
                if (!aroundNode.canWalk) continue;
                int tryGetGCost = currentNode.g + GetDistanceCost(currentNode, aroundNode);
                if (tryGetGCost < aroundNode.g)
                {
                    aroundNode.lastNode = currentNode;
                    aroundNode.g = tryGetGCost;
                    aroundNode.h = GetDistanceCost(aroundNode, endNode);
                    aroundNode.Getf();

                    if (!openNodes.Contains(aroundNode))
                    {
                        openNodes.Add(aroundNode);
                    }
                }
            }            
        }
        return new List<PathNode>();
    }

    private void WalkCheck(int x, int z, ref List<PathNode> aroundList)
    {
        PathNode node = GetNode(x, z);
        if (node.canWalk)
        {
            aroundList.Add(node);
        }
    }
    private List<PathNode> AroundNodes(PathNode currentnode)//ѭ����ǰ�ڵ���Χ�Ľڵ�
        //ctrl+h �滻
    {
        List<PathNode> aroundList = new List<PathNode>();
        if (currentnode.x - 1 >= 0)
        {
            WalkCheck(currentnode.x - 1, currentnode.z, ref aroundList);
            if (currentnode.z - 1 >= 0) WalkCheck(currentnode.x - 1, currentnode.z - 1, ref aroundList);
            // Left Up
            if (currentnode.z + 1 < grid.GetHeight()) WalkCheck(currentnode.x - 1, currentnode.z + 1, ref aroundList);
        }
        if (currentnode.x + 1 < grid.GetWidth())
        {
            // Right
            WalkCheck(currentnode.x + 1, currentnode.z, ref aroundList);
            // Right Down
            if (currentnode.z - 1 >= 0) WalkCheck(currentnode.x + 1, currentnode.z - 1, ref aroundList);
            // Right Up
            if (currentnode.z + 1 < grid.GetHeight()) WalkCheck(currentnode.x + 1, currentnode.z + 1, ref aroundList);
        }
        // Up
        if (currentnode.z + 1 < grid.GetWidth()) WalkCheck(currentnode.x, currentnode.z + 1, ref aroundList);
        // Down
        if (currentnode.z - 1 >= 0) WalkCheck(currentnode.x, currentnode.z - 1, ref aroundList);
        return aroundList;
    }

    public PathNode GetNode(int x,int z)
    {
        return grid.GetValue(x, z);
    }
    private List<PathNode> GetPath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.lastNode != null)
        {
            path.Add(currentNode.lastNode);
            currentNode = currentNode.lastNode;
        }
        path.Reverse();//��һ�����ڷ�תList��˳��
        return path;
    }

    private int GetDistanceCost(PathNode a,PathNode b)//��һ����������뻨��
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int zDistance = Mathf.Abs(a.z - b.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetCurrentNode(List<PathNode> pathnodelist)//�ҳ���ǰ�ڵ㣬Ҳ����F��С���Ǹ���
    {
        PathNode lowestF = pathnodelist[0];
        for(int i = 0; i < pathnodelist.Count; i++)
        {
            if (pathnodelist[i].f < lowestF.f)
            {
                lowestF = pathnodelist[i];
            }
        }
        return lowestF;
    }
    
}
