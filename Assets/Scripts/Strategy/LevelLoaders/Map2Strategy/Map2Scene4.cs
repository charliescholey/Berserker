using UnityEngine;
using System.Collections.Generic;

public class Map2Scene4 : MonoBehaviour
{
    public GameManager level;
    public BoardManager map;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //PLAYER SPAWN LOCATIONS
        Vector2Int[] playerSpawnLocations = {
            new Vector2Int(8, 3),
            new Vector2Int(8, 2),
            new Vector2Int(8, 1),
            new Vector2Int(8, 0)
        };

        map.setSpawnLocations(playerSpawnLocations);

        //ENEMY SPAWN LOCATIONS
        Vector2Int[] enemySpawnLocations = {
            new Vector2Int(3,3),
            new Vector2Int(5,3),
            
        };
        level.enemySpawnLocations = enemySpawnLocations;

        level.spawnEnemies(enemySpawnLocations);

        // WALLS
        List<Vector2Int> walls = new List<Vector2Int>();

        // wall on top
        walls.AddRange(generateBox(new Vector2Int(-10, 4), new Vector2Int(8, 4)));

        // bigger right wall 
        walls.AddRange(generateBox(new Vector2Int(9, 0), new Vector2Int(9, -5)));

        //smaller right wall
        walls.AddRange(generateBox(new Vector2Int(9, 4), new Vector2Int(9, 3)));

        //left wall
        walls.AddRange(generateBox(new Vector2Int(1, 3), new Vector2Int(1, -1)));

        //bottom wall
        walls.AddRange(generateBox(new Vector2Int(4, -1), new Vector2Int(8, -1)));

        //bench
        walls.AddRange(generateBox(new Vector2Int(3, 2), new Vector2Int(5, 2)));

        map.setWalls(walls.ToArray());

        level.enable();
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
