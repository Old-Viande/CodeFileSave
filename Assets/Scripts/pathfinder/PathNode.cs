using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Gridmap<PathNode> grid;
    public int x;
    public int z;
    public bool canWalk = true;

    [System.Flags]
    public enum Direction
    {
        Up = 1 << 0,
        Right = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3,
    }
    public Direction m_Direction;

    public int g;//����㵽��ǰλ�þ���
    public int h;//���յ㵽��ǰλ�þ���
    public int f;//Ȩ�أ�Խ��Խ����

    public PathNode lastNode;//Ϊ���ҵ�����·
    //ctrl + r�������Ǽ���������
    public PathNode(Gridmap<PathNode> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;

        int x1 = x < 0 ? (7 - (-x) % 7) : (x % 7);
        int z1 = z < 0 ? (7 - (-z) % 7) : (z % 7);
        int direction = 15;
        if (x1 == 0 && z1 != 3)
            direction -= 8;
        else if (x1 == 6 && z1 != 3)
            direction -= 2;
        if (z1 == 0 && x1 != 3)
            direction -= 4;
        else if (z1 == 6 && x1 != 3)
            direction -= 1;
        m_Direction = (Direction)direction;
    }

    public bool CheckDirection(Direction direction)
    {
        return m_Direction.HasFlag(direction);
    }

    public void Getf()
    {
        f = g + h;
    }
}
