using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "GridCell" , menuName = "Tower Def/Grid Cell")]
public class Map_GridCellObj : ScriptableObject
{
    public enum CellType { Path, Ground}

    public CellType cellType;
    public GameObject cellPrefab;
    public int yRotation;
}
