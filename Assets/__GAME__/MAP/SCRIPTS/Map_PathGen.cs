using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_PathGen
{
    private int width;
    private int height;
    private List<Vector2Int> pathCells;


    public Map_PathGen(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public List<Vector2Int> GeneratePath()
    {
        pathCells = new List<Vector2Int>();
        
        int y = (int)(height / 2);
        int x = 0;

        while (x < width)
        {
            pathCells.Add(new Vector2Int(x, y));
            bool validMove = false;

            while (!validMove)
            {
                int move = Random.Range(0, 3);

                if (move == 0 || x % 2 == 0  || x > (width - 2))
                {
                    x++;
                    validMove = true;
                }
                else if (move == 1 && IsCellFree(x, y + 1) && y < (height-3))
                {
                    y++;
                    validMove = true;
                }
                else if (move == 2 && IsCellFree(x, y - 1) && y > 2)
                {
                    y--;
                    validMove = true;
                }
            }
        }

        return (pathCells);
    }

    public bool IsCellFree(int x , int y)
    {
        return !pathCells.Contains(new Vector2Int(x, y));
    }

    public bool IsCellTaken(int x, int y)
    {
        return pathCells.Contains(new Vector2Int(x, y));
    }

    public int GetCellSideVal(int x, int y)
    {
        int ret = 0;

        if (IsCellTaken(x, y - 1))
            ret += 1;
        if (IsCellTaken(x - 1, y))
            ret += 2;
        if (IsCellTaken(x + 1, y))
            ret += 4;
        if (IsCellTaken(x, y + 1))
            ret += 8;

        return ret;
    }
}
