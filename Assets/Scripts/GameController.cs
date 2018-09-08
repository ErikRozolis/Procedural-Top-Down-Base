using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    List<GameObject> alertedEnemies;

    public List<GameObject> enemyList;
    public Tilemap floorMap;
    public Tilemap wallMap;
    public Tilemap decorationMap;
    public RandomTile floor;
    public RuleTile brickWall;
    public RuleTile walkwayWall;
    public RuleTile stairs;
    public RuleTile rug;

    Transform tileMapGrid;
    int enemyConcentration;
    int roomCount;

    //List<WallBoundary> roomBounds;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        tileMapGrid = GameObject.Find("TileMapGrid").transform;
        enemyConcentration = 5;
        roomCount = 10;
        alertedEnemies = new List<GameObject>();
        GenerateFloor();
        GenerateWalls();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddAlertedEnemy(GameObject enemy)
    {
        alertedEnemies.Add(enemy);
    }

    public void StartFight()
    {
        foreach(GameObject obj in alertedEnemies)
        {
            Destroy(obj);
        }
    }

    void SpawnEnemies()
    {

        for (int x = floorMap.cellBounds.xMin; x <= floorMap.cellBounds.xMax; x++)
        {
            for (int y = floorMap.cellBounds.yMin; y <= floorMap.cellBounds.yMax; y++)
            {
                Vector3Int vector = new Vector3Int(x, y, 0);
                if(vector.magnitude > 15)
                {
                    TileBase tile = floorMap.GetTile(vector);
                    if (tile != null)
                    {
                        if (Random.Range(0, 1000) < enemyConcentration)
                            SpawnEnemy(vector);
                    }
                }
            }
        }
                    
    }

    void SpawnEnemy(Vector3Int vector)
    {
        GameObject enemy = Instantiate(enemyList[Random.Range(0, enemyList.Count)], new Vector3(vector.x, vector.y, vector.z), Quaternion.identity);
    }

    void GenerateFloor()
    {
        floorMap.ClearAllTiles();
        wallMap.ClearAllTiles();
        Vector2Int currentPos = new Vector2Int(0, 0);
        for (int i = 0; i < roomCount; i++)
        {
            Vector2Int hallwayDir = new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
            int hallwayDistance = 15 * Random.Range(2, 4);

            int randInt = Random.Range(1, 5);
            if (randInt < 4)
            {
                int randWidth = Random.Range(8, 16);
                int randHeight = Random.Range(8, 16);
                GenerateFloorRectangle(randWidth, randHeight, currentPos);
            }
            if (randInt >= 4)
            {
                int randRadius = Random.Range(5, 10);
                GenerateFloorCircle(randRadius, currentPos);
            }
            currentPos = GenerateFloorHallway(hallwayDir, hallwayDistance, currentPos);
        }
    }

    void GenerateFloorCircle(int radius, Vector2Int centerPoint)
    {
        radius++;
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if ((x * x + y * y) < (radius * radius))
                {
                    Vector3Int tilePos = new Vector3Int(centerPoint.x + x, centerPoint.y + y, 0);
                    floorMap.SetTile(tilePos, floor);
                }
            }
        }
    }

    void GenerateFloorRectangle(int width, int height, Vector2Int centerPoint)
    {
        int startX = centerPoint.x - width / 2;
        int startY = centerPoint.y - height / 2;
        int endX = startX + width;
        int endY = startY + height;
        //roomBounds.Add(new WallBoundary(startX, startY, endX-1, endY-1));
        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                floorMap.SetTile(tilePos, floor);
                //roomBounds.Add(new BoundsInt(centerPoint.x + startX, centerPoint.y + startY , 0, width, height, 0));
            }
        }
    }

    Vector2Int GenerateFloorHallway(Vector2Int direction, int distance, Vector2Int startingPoint)
    {
        Vector2Int hallwayCenter = startingPoint + direction * (distance / 2);
        Vector2Int travel = direction * distance;
        int width = (Mathf.Abs(travel.x) < 3 ? 3 : Mathf.Abs(travel.x));
        int height = (Mathf.Abs(travel.y) < 3 ? 3 : Mathf.Abs(travel.y));
        GenerateFloorRectangle(width, height, hallwayCenter);
        return startingPoint + travel;
    }

    void GenerateWalls()
    {
        //Iterate through all tiles one at a time
        for (int x = floorMap.cellBounds.xMin; x <= floorMap.cellBounds.xMax; x++)
        {
            for (int y = floorMap.cellBounds.yMin; y <= floorMap.cellBounds.yMax; y++)
            {
                Vector3Int vector = new Vector3Int(x, y, 0);
                TileBase tile = floorMap.GetTile(vector);
                //only do things if there isn't already a tile there
                if (tile != null)
                {
                    for (int nx = x - 1; nx <= x + 1; nx++)
                    {
                        for (int ny = y - 1; ny <= y + 1; ny++)
                        {
                            var nearbyTile = floorMap.GetTile(new Vector3Int(nx, ny, 0));
                            if (nearbyTile == null)
                                wallMap.SetTile(new Vector3Int(nx, ny, 0), walkwayWall);
                        }
                    }
                }
            }
        }
    }

    bool CheckForEmptyTile(Vector3Int vector)
    {
        return floorMap.GetTile(vector) == null && wallMap.GetTile(vector) == null;
    }
}
