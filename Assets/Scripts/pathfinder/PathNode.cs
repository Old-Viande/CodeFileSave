using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    private Gridmap<PathNode> grid;
    public int x;
    public int z;
    public bool canWalk =false;


    public int g;//����㵽��ǰλ�þ���
    public int h;//���յ㵽��ǰλ�þ���
    public int f;//Ȩ�أ�Խ��Խ����

    public PathNode lastNode;//Ϊ���ҵ�����·
    //ctrl + r�������Ǽ���������
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
