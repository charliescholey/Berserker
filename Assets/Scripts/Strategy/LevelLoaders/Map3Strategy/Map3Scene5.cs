using UnityEngine;
using System.Collections.Generic;

public class Map3Scene5 : MonoBehaviour
{
    public GameManager level;
    public BoardManager map;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //PLAYER SPAWN LOCATIONS
        Vector2Int[] playerSpawnLocations = {
            new Vector2Int(-9, 3),
            new Vector2Int(-9, 1),
            new Vector2Int(-9, -1),
            new Vector2Int(-9, -3)
        };

        map.setSpawnLocations(playerSpawnLocations);

        //ENEMY SPAWN LOCATIONS
        Vector2Int[] enemySpawnLocations = {
            new Vector2Int(-7,-2),
            new Vector2Int(-7,1),
        };
        level.enemySpawnLocations = enemySpawnLocations;

        level.spawnEnemies(enemySpawnLocations);

        // WALLS
        // for this scene only making walls for the house we are in
        List<Vector2Int> walls = new List<Vector2Int>();

        // First wall on bottom big one
        walls.AddRange(generateBox(new Vector2Int(-2, -5), new Vector2Int(9, -5)));

        //top left wall
        walls.AddRange(generateBox(new Vector2Int(-6, 4), new Vector2Int(-6, 1)));

        //bottom left wall
        walls.AddRange(generateBox(new Vector2Int(-6, -2), new Vector2Int(-6, -5)));

        //tiny wall
        walls.AddRange(generateBox(new Vector2Int(5, -5), new Vector2Int(5, -5)));

        //invisible outside wall
        walls.AddRange(generateBox(new Vector2Int(-11, 4), new Vector2Int(-11, -5)));



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
