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
        // Find the parent GameObject in the scene or create it if it doesn't exist
        GameObject mapPath = GameObject.Find("MapPath");
        if (mapPath == null)
        {
            mapPath = new GameObject("MapPath");
        }

        foreach (Vector2Int cell in pathCells)
        {
            int sideVal = pathGen.GetCellSideVal(cell.x, cell.y);
            GameObject pathTile = pathCellObjects[sideVal].cellPrefab;
            GameObject pathTileCell = Instantiate(pathTile, new Vector3(cell.x, 0, cell.y), Quaternion.identity);
            pathTileCell.transform.Rotate(0f, pathCellObjects[sideVal].yRotation, 0f, Space.Self);
            
            // Set the instantiated object as a child of the mapPath
            pathTileCell.transform.parent = mapPath.transform;
            
            yield return new WaitForSeconds(pathPlaceSpeed);
        }
        yield return null;
    }

    private IEnumerator LayMapCells()
    {
        GameObject mapParent = GameObject.Find("MapParent");
        if (mapParent == null)
        {
            mapParent = new GameObject("MapParent");
        }

        for (int y = gridHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (pathGen.IsCellFree(x, y))
                {
                    GameObject instantiatedObject;
                    int randomAssetPick = Random.Range(0, sceneryCellObjects.Length);
                    instantiatedObject = Instantiate(
                         sceneryCellObjects[3].cellPrefab,
                         new Vector3(x, 0f, y),
                         Quaternion.identity
                     );
                    instantiatedObject.transform.parent = mapParent.transform;
                    if (pathGen.IsNearPath(x, y))
                    {
                        instantiatedObject = Instantiate(
                            sceneryCellObjects[randomAssetPick].cellPrefab,
                            new Vector3(x, 0f, y),
                            Quaternion.identity
                        );
                    }
                    instantiatedObject.transform.parent = mapParent.transform;
                    yield return new WaitForSeconds(scenePlaceSpeed);
                }
            }
        }
        yield return null;
    }
}
