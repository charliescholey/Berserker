using UnityEngine;

/*  
        *** PLAN ***

        objects to load in:
            i. enemies
                a) Section 1 - Front Door Guards: (-14, 4) (-14, 1)
                b) Section 2 - Fire Guards: (-6, -9) (-4, -7) (-2, -9) (-4, -11)
                c) Section 3 - Chest Guards: (7, -9) (16, -9)
                d) Section 4 - Boss Area Guards: (3, 1) (3, -2)
                e) Section 5 - Dining Hall Guards: (2 ,11) (4, 11) (7, 7) (9, 7) (13, 7) (13, 11)


            ii. chest @ (16, -13) (-5, 14)
            iii. boss @ (11, 2)
            iv. players @   (-19, -10) (-18, -10)
                            (-19, -11) (-18, -11))

        if this is the first time we load it - load in all enemies & chests

        if it is loaded from a strategy section - load without the guards from that section - only way to get 
        back into stealth from strategy is to kill all the guards


        fields:
            i. parent game object for each enemy section
            ii. list of chest/items status - true = taken, false = not yet taken
            iii. list of player positions - saved when a player gets into an enemy's FOV, so when they finish the
                strategy section they get spawned back into the same spot

    */


public class Map1 : MonoBehaviour
{
    // PLAYER SPAWN LOCATIONS
    Vector2Int[] playerSpawnLocations = {
        new Vector2Int(-19, -11),
        new Vector2Int(-19, -9),
        new Vector2Int(-18, -11),
        new Vector2Int(-18, -9)
    };

    // ENEMY SPAWN LOCATIONS

    // both are left rotating enemies
    Vector2Int[] sector1FrontDoorEnemySpawnLocations = {
        new Vector2Int(-14, 4),
        new Vector2Int(-14, 1)
    };

    Vector2Int[] sector2FireEnemySpawnLocations = {
        new Vector2Int(-8, -5),
        new Vector2Int(-6, -4),
        new Vector2Int(-5, -6),
        new Vector2Int(-7, -7)
    };

    Vector2Int[] sector3ChestGuards = {
        new Vector2Int(7, -7),
        new Vector2Int(12, -7)
    };
    Vector2Int[] sector4BossAreaGuards = {
        new Vector2Int(1, 5),
        new Vector2Int(1, 0)
    };
    Vector2Int[] sector5DiningHallGuards = {
        new Vector2Int(-2, 14),
        new Vector2Int(2, 14),
        new Vector2Int(7, 14),
        new Vector2Int(5, 13),
        new Vector2Int(10, 15),
        new Vector2Int(10, 11)
    };
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject enemy1 = Resources.Load<GameObject>("Enemy");
        if (enemy1 != null)
        {
            for (int i = 0; i < 6; i++) {
                Instantiate(enemy1, new Vector3(sector5DiningHallGuards[i].x, sector5DiningHallGuards[i].y, -5), Quaternion.identity);
                Debug.Log("loaded!");
            }
            
        }
        else
        {
            Debug.LogError("Failed to load enemy1!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
