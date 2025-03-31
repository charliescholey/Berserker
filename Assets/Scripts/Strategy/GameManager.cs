using UnityEngine;
using System;
using System.Linq;

/**
* GameManager class -- handles gameplay logic.
*/

public class GameManager : MonoBehaviour
{
    //tracks whether the player has an object selected
    private bool hasSelected = false;
    //link to boardmanager
    public BoardManager boardManager;
    //link to action database
    public ActionDatabase actionDatabase;
    //tracks active player
    private CharacterController player;

    public ActionBTNManager actionBTNManagerPrefab;

    private ActionBTNManager actionBTNManager;

    //canvas for overlay rendering
    public Canvas UI;
    public GameObject HPTextPrefab;

    //tracks the player prefab for spawning
    public CharacterController playerPrefab;
    //tracks the enemy prefab for spawning
    public EnemyController enemyPrefab;
    //tracks the players in the game
    public CharacterController[] players;
    public EnemyController[] enemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        players = new CharacterController[4];

        //spawn the characters
        players[0] = Instantiate(playerPrefab);
        players[1] = Instantiate(playerPrefab);
        players[2] = Instantiate(playerPrefab);
        players[3] = Instantiate(playerPrefab);
        /*
        Use the following two commented out lines instead of the spawnPlayers() function to test different actions.
        spawnPlayers() currently gives all the players the same actions.
        */

        players[0].spawn(boardManager, new Vector2Int(0, 0), "Warrior", actionDatabase);
        players[1].spawn(boardManager, new Vector2Int(1, 3), "Mage", actionDatabase);
        players[2].spawn(boardManager, new Vector2Int(2, 0), "Rogue", actionDatabase);
        players[3].spawn(boardManager, new Vector2Int(3, 3), "Paladin", actionDatabase);

        enemies = new EnemyController[1];
        enemies[0] = Instantiate(enemyPrefab);
        enemies[0].spawn(boardManager, new Vector2Int(3, 0));

        //give every character an HP tracker
        Array.ForEach(players, x => addHPTracker(x));
        Array.ForEach(enemies, x => addHPTracker(x));

        actionBTNManager = Instantiate(actionBTNManagerPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        //get where the mouse is in the world
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (hasSelected)
            {
                //if a player is selected, move them to the clicked cell
                Vector2Int cell = boardManager.clickToCell(worldPoint);
                player.moveToCell(cell);
                hasSelected = false;
                player.setSelected(false);
                player = null;

                actionBTNManager.destroy();

            }
            else
            {
                //if no player is selected, select the player in the clicked cell
                Vector2Int cell = boardManager.clickToCell(worldPoint);

                OrderedCharacter[] characters = players.Cast<OrderedCharacter>().Concat(enemies.Cast<OrderedCharacter>()).ToArray();

                player = (CharacterController)boardManager.detectSelected(cell, characters);
                if (player != null)
                {
                    if (player.GetType() == typeof(CharacterController))
                    {
                        //player is selected
                        hasSelected = true;
                        player.setSelected(true);
                        if (player.hasActed == false)
                        {
                            actionBTNManager.Create(player, UI);
                        }
                    }
                    else
                    {
                        //right now, enemy would be the one clicked
                        player.setSelected(false);
                        player = null;

                        actionBTNManager.destroy();
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            //if the right mouse button is clicked, deselect the player
            hasSelected = false;
            player.setSelected(false);
        }

        processEndTurn();
    }

    void processEndTurn()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].isTurnComplete())
            {
                return;
            }
        }
        takeEnemyAction();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].resetTurn();
        }
    }

    void takeEnemyAction()
    {
        //move enemy towards player
        Vector2Int playerPos = players[0].gridPosition;
        Vector2Int enemyPos = enemies[0].gridPosition;
        if (playerPos.x > enemyPos.x)
        {
            enemies[0].moveToCell(new Vector2Int(enemyPos.x + 1, enemyPos.y));
        }
        else if (playerPos.x < enemyPos.x)
        {
            enemies[0].moveToCell(new Vector2Int(enemyPos.x - 1, enemyPos.y));
        }
        else if (playerPos.y > enemyPos.y)
        {
            enemies[0].moveToCell(new Vector2Int(enemyPos.x, enemyPos.y + 1));
        }
        else if (playerPos.y < enemyPos.y)
        {
            enemies[0].moveToCell(new Vector2Int(enemyPos.x, enemyPos.y - 1));
        }
    }

    //Executes the attack action.
    public void ExecuteAttack(CharacterController attacker, Action attackAction)
    {
        // Determine damage from the action's power defaulting to 10 if no action is provided.
        int damage = attackAction != null ? attackAction.power : 10;

        Vector2Int attackerPos = attacker.gridPosition;

        Vector2Int[] adjacentCells = GetAdjacentCells(attackerPos);

        // Loop through all enemies in the game.
        foreach (OrderedCharacter oc in enemies)
        {
            bool isAdjacent = false;

            foreach (Vector2Int cell in adjacentCells)
            {
                if (oc.gridPosition == cell)
                {
                    isAdjacent = true;
                    break;
                }
            }

            // If the enemy is adjacent, apply damage and log the event.
            if (isAdjacent)
            {
                oc.TakeDamage(damage);
                //this my best attempt at logging 
                Debug.Log(oc.gameObject.name + " took " + damage + " damage from " + attacker.gameObject.name);
            }
        }

        // Mark the attacker as having taken their action.
        attacker.hasActed = true;
    }


    //Helper method to get the four adjacent cells.
    Vector2Int[] GetAdjacentCells(Vector2Int center)
    {
        return new Vector2Int[]
        {
            new Vector2Int(center.x + 1, center.y),       // East
            new Vector2Int(center.x - 1, center.y),       // West
            new Vector2Int(center.x, center.y + 1),       // North
            new Vector2Int(center.x, center.y - 1),       // South
            new Vector2Int(center.x + 1, center.y + 1),   // Northeast
            new Vector2Int(center.x + 1, center.y - 1),   // Southeast
            new Vector2Int(center.x - 1, center.y + 1),   // Northwest
            new Vector2Int(center.x - 1, center.y - 1)    // Southwest
        };
    }

    // Spawns players given their locations
    // At the moment, each player is spawned with the same actions.
    // This will need to be updated to reflect the actual actions and locations of the players.
    void spawnPlayers(Vector2Int[] playerSpawnLocations)
    {
        int i = 0;
        foreach (CharacterController oc in players)
        {
            oc.spawn(boardManager, playerSpawnLocations[i], new[] { actionDatabase.actions[1], actionDatabase.actions[2] });
            i++;
        }
    }

    void addHPTracker(OrderedCharacter oc)
    {
        GameObject hpText = Instantiate(HPTextPrefab);
        hpText.transform.SetParent(UI.transform);
        HPTextController hpTextController = hpText.GetComponent<HPTextController>();
        hpTextController.setCharacter(oc);
    }

}

