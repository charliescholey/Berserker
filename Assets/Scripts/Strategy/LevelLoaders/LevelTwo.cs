using UnityEngine;
using System.Collections.Generic;

public class LevelTwo : MonoBehaviour
{
    public GameManager level;
    public BoardManager map;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //PLAYER SPAWN LOCATIONS
        Vector2Int[] playerSpawnLocations = {
            new Vector2Int(-5, -2),
            new Vector2Int(-4, -2),
            new Vector2Int(-3, -2),
            new Vector2Int(-4, -3)
        };

        map.setSpawnLocations(playerSpawnLocations);

        //ENEMY SPAWN LOCATIONS
        Vector2Int[] enemySpawnLocations = {
            new Vector2Int(3, 0),
            new Vector2Int(-9, 1),
            new Vector2Int(6, 3),
        };
        level.enemySpawnLocations = enemySpawnLocations;

        level.spawnEnemies(enemySpawnLocations);

        //WALLS
        List<Vector2Int> walls = new List<Vector2Int>();
        walls.AddRange(generateBox(new Vector2Int(-8, 3), new Vector2Int(0, -1))); //BUILDING
        walls.AddRange(generateBox(new Vector2Int(-1, -3), new Vector2Int(0, -4))); //CAR
        walls.AddRange(generateBox(new Vector2Int(-8, -3), new Vector2Int(-6, -3))); //FENCE1
        walls.AddRange(generateBox(new Vector2Int(3, -3), new Vector2Int(5, -3))); //FENCE2
        walls.AddRange(generateBox(new Vector2Int(8, 3), new Vector2Int(9, -5))); //TREEBOX

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
