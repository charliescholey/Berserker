using UnityEngine;
using System.Collections.Generic;

public class Map3Scene1 : MonoBehaviour
{
    public GameManager level;
    public BoardManager map;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //PLAYER SPAWN LOCATIONS
        Vector2Int[] playerSpawnLocations = {
            new Vector2Int(-9, -3),
            new Vector2Int(-9, -1),
            new Vector2Int(-9, 1),
            new Vector2Int(-9, 3)
        };

        map.setSpawnLocations(playerSpawnLocations);

        //ENEMY SPAWN LOCATIONS
        Vector2Int[] enemySpawnLocations = {
            new Vector2Int(-6,1),
            new Vector2Int(-6,-2),
            new Vector2Int(2,1),
            new Vector2Int(2,-2),
            new Vector2Int(6,0),
        };
        level.enemySpawnLocations = enemySpawnLocations;

        level.spawnEnemies(enemySpawnLocations);

        // WALLS
        // for this scene only making walls for the house we are in
        List<Vector2Int> walls = new List<Vector2Int>();

        // First wall on top
        walls.AddRange(generateBox(new Vector2Int(-10, 4), new Vector2Int(9, 4)));

        //wall on right 
        walls.AddRange(generateBox(new Vector2Int(9, 3), new Vector2Int(9, -5)));

        //left top vertical wall 
        walls.AddRange(generateBox(new Vector2Int(-5, 3), new Vector2Int(-5, 1)));

        //left bottom vertical wall 
        walls.AddRange(generateBox(new Vector2Int(-5, -2), new Vector2Int(-5, -3)));

        //bottom wall 
        walls.AddRange(generateBox(new Vector2Int(-5, -4), new Vector2Int(8, -4)));

        //desk
        walls.AddRange(generateBox(new Vector2Int(4, 0), new Vector2Int(5, 0)));



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
