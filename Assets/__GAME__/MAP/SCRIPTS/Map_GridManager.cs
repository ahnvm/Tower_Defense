using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_GridManager : MonoBehaviour
{
    #region definations
    [SerializeField]
    private int gridWidth = 16;
    [SerializeField]
    private int gridHeight = 8;
    [SerializeField]
    private int minPathLength = 30;
    [SerializeField]
    private float pathPlaceSpeed = 0.2f;
    [SerializeField]
    private float scenePlaceSpeed = 0.1f;

    public Map_GridCellObj[] pathCellObjects;
    public Map_GridCellObj[] sceneryCellObjects;

    private Map_PathGen pathGen;
    #endregion

    void Start()
    {
        pathGen = new Map_PathGen(gridWidth, gridHeight);
        List<Vector2Int> pathCells = pathGen.GeneratePath();
        
        int pathSize = pathCells.Count;
        while (pathSize < minPathLength) 
        {
            pathCells = pathGen.GeneratePath();
            pathSize = pathCells.Count;
        }
        StartCoroutine(CreateGrid(pathCells));  
    }

    IEnumerator CreateGrid(List<Vector2Int> pathCells) 
    {
        yield return StartCoroutine(LayPathCells(pathCells));
        yield return StartCoroutine(LayMapCells());
    }

    private IEnumerator LayPathCells(List<Vector2Int> pathCells)
    {
        foreach (Vector2Int cell in pathCells)
        {
            int sideVal = pathGen.GetCellSideVal(cell.x , cell.y);
            GameObject pathTile = pathCellObjects[sideVal].cellPrefab;
            GameObject pathTileCell = Instantiate(pathTile, new Vector3(cell.x, 0, cell.y), Quaternion.identity);
            pathTileCell.transform.Rotate(0f, pathCellObjects[sideVal].yRotation, 0f, Space.Self);
            yield return new WaitForSeconds(pathPlaceSpeed);
        }
        yield return null;
    }

    private IEnumerator LayMapCells()
    {
        for (int y = gridHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (pathGen.IsCellFree(x, y))
                {
                    int randomAssetPick = Random.Range(0, sceneryCellObjects.Length);
                    Instantiate(sceneryCellObjects[randomAssetPick].cellPrefab, new Vector3(x, 0f, y), Quaternion.identity);
                    yield return new WaitForSeconds(scenePlaceSpeed);
                }
            }
        }
        yield return null;
    }
}
