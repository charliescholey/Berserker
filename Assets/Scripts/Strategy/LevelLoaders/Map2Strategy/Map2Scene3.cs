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
            new Vector2Int(-6, 1),
            new Vector2Int(-4, 1),
            new Vector2Int(-6, -1),
            new Vector2Int(-4, -1)
        };

        map.setSpawnLocations(playerSpawnLocations);

        //ENEMY SPAWN LOCATIONS
        Vector2Int[] enemySpawnLocations = {
            new Vector2Int(-1,-2),
            new Vector2Int(-1,1),
            
        };
        level.enemySpawnLocations = enemySpawnLocations;

        level.spawnEnemies(enemySpawnLocations);

        // WALLS
        List<Vector2Int> walls = new List<Vector2Int>();

        // First wall on top
        walls.AddRange(generateBox(new Vector2Int(0, 4), new Vector2Int(0, 1)));

        // Second wall on bottom
        walls.AddRange(generateBox(new Vector2Int(0, -2), new Vector2Int(0, -5)));

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
