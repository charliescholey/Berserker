using UnityEngine;
using System.Collections.Generic;

public class Map3Scene4 : MonoBehaviour
{
    public GameManager level;
    public BoardManager map;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //PLAYER SPAWN LOCATIONS
        Vector2Int[] playerSpawnLocations = {
            new Vector2Int(-3, -5),
            new Vector2Int(-1, -5),
            new Vector2Int(1, -5),
            new Vector2Int(3, -5)
        };

        map.setSpawnLocations(playerSpawnLocations);

        //ENEMY SPAWN LOCATIONS
        Vector2Int[] enemySpawnLocations = {
            new Vector2Int(-2,-3),
            new Vector2Int(-1,-3),
            new Vector2Int(1,-3),
            new Vector2Int(2,-3),
        };
        level.enemySpawnLocations = enemySpawnLocations;

        level.spawnEnemies(enemySpawnLocations);

        // WALLS
        // for this scene only making walls for the house we are in
        List<Vector2Int> walls = new List<Vector2Int>();

        // First wall on left 
        walls.AddRange(generateBox(new Vector2Int(-10, -1), new Vector2Int(-3, -1)));

        // First wall on right 
        walls.AddRange(generateBox(new Vector2Int(3, -1), new Vector2Int(9, -1)));

        //small vertical wall left
        walls.AddRange(generateBox(new Vector2Int(-2, -1), new Vector2Int(-2, -2)));

        //small vertical wall left
        walls.AddRange(generateBox(new Vector2Int(2, -1), new Vector2Int(2, -2)));

        //vertical wall on right
        walls.AddRange(generateBox(new Vector2Int(4, 4), new Vector2Int(4, 0)));

        //small vertical wall left
        walls.AddRange(generateBox(new Vector2Int(-3, 2), new Vector2Int(-3, 1)));


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
