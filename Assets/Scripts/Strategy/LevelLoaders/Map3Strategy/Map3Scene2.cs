using UnityEngine;
using System.Collections.Generic;

public class Map3Scene2 : MonoBehaviour
{
    public GameManager level;
    public BoardManager map;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
          //PLAYER SPAWN LOCATIONS
        Vector2Int[] playerSpawnLocations = {
            new Vector2Int(9, 1),
            new Vector2Int(9, 0),
            new Vector2Int(9, -1),
            new Vector2Int(9, -2)
        };

        map.setSpawnLocations(playerSpawnLocations);

        //ENEMY SPAWN LOCATIONS
        Vector2Int[] enemySpawnLocations = {
            new Vector2Int(7,1),
            new Vector2Int(7,-2),
            new Vector2Int(-2,-3),
            new Vector2Int(2,-3),
            new Vector2Int(-2,0),
            new Vector2Int(3,0),
        };
        level.enemySpawnLocations = enemySpawnLocations;

        level.spawnEnemies(enemySpawnLocations);

        // WALLS
        // for this scene only making walls for the house we are in
        List<Vector2Int> walls = new List<Vector2Int>();

        // First wall on bottom
        walls.AddRange(generateBox(new Vector2Int(-10, -4), new Vector2Int(9, -4)));

        // left wall 
        walls.AddRange(generateBox(new Vector2Int(-10, 1), new Vector2Int(-10, -3)));

        // small left top wall 
        walls.AddRange(generateBox(new Vector2Int(-10, 2), new Vector2Int(-9, 2)));

        // rest of top wall
        walls.AddRange(generateBox(new Vector2Int(-6, 2), new Vector2Int(5, 2)));

        //top right wall
        walls.AddRange(generateBox(new Vector2Int(6, 2), new Vector2Int(6, 1)));

        // bottom right wall
        walls.AddRange(generateBox(new Vector2Int(6, -2), new Vector2Int(6, -3)));

        // tables

        walls.AddRange(generateBox(new Vector2Int(-7, -1), new Vector2Int(-5, -1)));

        walls.AddRange(generateBox(new Vector2Int(-3, -1), new Vector2Int(-1, -1)));

        walls.AddRange(generateBox(new Vector2Int(1, -1), new Vector2Int(3, -1)));


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
