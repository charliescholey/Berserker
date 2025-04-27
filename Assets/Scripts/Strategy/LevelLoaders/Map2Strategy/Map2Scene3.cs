using UnityEngine;
using System.Collections.Generic;

public class Map2Scene3 : MonoBehaviour
{
    public GameManager level;
    public BoardManager map;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //PLAYER SPAWN LOCATIONS
        Vector2Int[] playerSpawnLocations = {
            new Vector2Int(1, -1),
            new Vector2Int(2, -1),
            new Vector2Int(3, -1),
            new Vector2Int(4, -1)
        };

        map.setSpawnLocations(playerSpawnLocations);

        //ENEMY SPAWN LOCATIONS
        Vector2Int[] enemySpawnLocations = {
            new Vector2Int(-2,-1),
            new Vector2Int(6,-1),
            new Vector2Int(2,-4),
            
        };
        level.enemySpawnLocations = enemySpawnLocations;

        level.spawnEnemies(enemySpawnLocations);

        // WALLS
        List<Vector2Int> walls = new List<Vector2Int>();

        // First wall on top left
        walls.AddRange(generateBox(new Vector2Int(-4, 0), new Vector2Int(1, 0)));

        // Second wall on top right 
        walls.AddRange(generateBox(new Vector2Int(4, 0), new Vector2Int(8, 0)));

        //bottom wall
        walls.AddRange(generateBox(new Vector2Int(-10, -5), new Vector2Int(9, -5)));

        //vertical wall left
        walls.AddRange(generateBox(new Vector2Int(-5, 0), new Vector2Int(-5, -4)));

        //vertical wall right
        walls.AddRange(generateBox(new Vector2Int(9, 4), new Vector2Int(9, -4)));

        //benches
        walls.AddRange(generateBox(new Vector2Int(-3, -2), new Vector2Int(-1, -2)));

        walls.AddRange(generateBox(new Vector2Int(6, -2), new Vector2Int(8, -2)));

        map.setWalls(walls.ToArray());
    }


    List<Vector2Int> generateBox(Vector2Int topLeft, Vector2Int bottomRight)
    {
        Debug.Log("Generating box from " + topLeft + " to " + bottomRight);
        List<Vector2Int> box = new List<Vector2Int>();
        for (int x = topLeft.x; x <= bottomRight.x; x++)
        {
            for (int y = topLeft.y; y >= bottomRight.y; y--)
            {
                box.Add(new Vector2Int(x, y));
            }
        }
        Debug.Log("Box generated with " + box.Count + " tiles.");
        return box;
    }
}
