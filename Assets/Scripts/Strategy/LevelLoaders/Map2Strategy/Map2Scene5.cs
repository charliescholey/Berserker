using UnityEngine;
using System.Collections.Generic;

public class Map2Scene5 : MonoBehaviour
{
    public GameManager level;
    public BoardManager map;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //PLAYER SPAWN LOCATIONS
        Vector2Int[] playerSpawnLocations = {
            new Vector2Int(9, 0),
            new Vector2Int(9, -1),
            new Vector2Int(8, -1),
            new Vector2Int(8, 0)
        };

        map.setSpawnLocations(playerSpawnLocations);

        //ENEMY SPAWN LOCATIONS
        Vector2Int[] enemySpawnLocations = {
            new Vector2Int(-4,3),
            new Vector2Int(1,3),
            new Vector2Int(6,3),
            new Vector2Int(-8,-4),
            new Vector2Int(-4,-4),
            new Vector2Int(1,-4),
            
        };
        level.enemySpawnLocations = enemySpawnLocations;

        level.spawnEnemies(enemySpawnLocations);

        // WALLS
        List<Vector2Int> walls = new List<Vector2Int>();

        // First wall on top
        walls.AddRange(generateBox(new Vector2Int(-10, 4), new Vector2Int(9, 4)));

        // Second wall on bottom
        walls.AddRange(generateBox(new Vector2Int(-10, -5), new Vector2Int(9, -5)));

        //leftmost wall
        walls.AddRange(generateBox(new Vector2Int(9, 3), new Vector2Int(9, -4)));

        //bottom right wall
        walls.AddRange(generateBox(new Vector2Int(9, -2), new Vector2Int(9, -4)));

        //top right wall
        walls.AddRange(generateBox(new Vector2Int(9, 3), new Vector2Int(9, 1)));

        //none of this code is wokring ask charlie why
        // //cell doors top(horizontal walls)
        // walls.AddRange(generateBox(new Vector2Int(-9,1), new Vector2Int(-8,1)));

        // walls.AddRange(generateBox(new Vector2Int(-5,1), new Vector2Int(-3,1)));

        // walls.AddRange(generateBox(new Vector2Int(0,1), new Vector2Int(2,1)));

        // walls.AddRange(generateBox(new Vector2Int(5,1), new Vector2Int(7,1)));

        // //cell doors bottom(horizontal walls)
        // walls.AddRange(generateBox(new Vector2Int(-9,-2), new Vector2Int(-8,-2)));

        // walls.AddRange(generateBox(new Vector2Int(-5,-2), new Vector2Int(-3,-2)));

        // walls.AddRange(generateBox(new Vector2Int(0,-2), new Vector2Int(2,-2)));

        // walls.AddRange(generateBox(new Vector2Int(5,-2), new Vector2Int(7,-2)));

        // //vetical cell walls top
        // walls.AddRange(generateBox(new Vector2Int(-6,3), new Vector2Int(-6,1)));

        // walls.AddRange(generateBox(new Vector2Int(-1,3), new Vector2Int(-1,1)));

        // walls.AddRange(generateBox(new Vector2Int(4,3), new Vector2Int(4,1)));

        // //vetical cell walls bottom
        // walls.AddRange(generateBox(new Vector2Int(-6,-2), new Vector2Int(7,-4)));

        // walls.AddRange(generateBox(new Vector2Int(-1,-2), new Vector2Int(-1,-4)));

        // walls.AddRange(generateBox(new Vector2Int(4,-2), new Vector2Int(4,-4)));


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
