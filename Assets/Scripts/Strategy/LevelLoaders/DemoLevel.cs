using UnityEngine;

public class DemoLevel : MonoBehaviour
{
    public GameManager level;
    public BoardManager map;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //PLAYER SPAWN LOCATIONS
        Vector2Int[] playerSpawnLocations = {
            new Vector2Int(0, 0),
            new Vector2Int(1, 3),
            new Vector2Int(2, 0),
            new Vector2Int(3, 3)
        };

        map.setSpawnLocations(playerSpawnLocations);

        //ENEMY SPAWN LOCATIONS
        Vector2Int[] enemySpawnLocations = {
            new Vector2Int(3, 0)
        };
        level.enemySpawnLocations = enemySpawnLocations;

        //WALLS
        Vector2Int[] walls = {
            new Vector2Int(-1, 2),
            new Vector2Int(-2, 2),
            new Vector2Int(-2, 1),
            new Vector2Int(-2, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1)
        };
        map.setWalls(walls);

    }
}
