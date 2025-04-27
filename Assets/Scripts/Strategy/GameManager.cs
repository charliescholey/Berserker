using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

/**
* GameManager class -- handles gameplay logic.
*/

public class GameManager : MonoBehaviour
{
    public TileHighlighter tileHighlighter;
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
    //enemy spawn locations to be edited per-level
    public Vector2Int[] enemySpawnLocations;

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
        

    }

    void Start() {
        players[0].spawn(boardManager, boardManager.spawnLocations[0], "Warrior", actionDatabase);
        players[1].spawn(boardManager, boardManager.spawnLocations[1], "Mage", actionDatabase);
        players[2].spawn(boardManager, boardManager.spawnLocations[2], "Rogue", actionDatabase);
        players[3].spawn(boardManager, boardManager.spawnLocations[3], "Paladin", actionDatabase);

        OrderedCharacter[] characters = players.Cast<OrderedCharacter>().Concat(enemies.Cast<OrderedCharacter>()).ToArray();

        foreach (OrderedCharacter oc in characters)
        {
            addHPTracker(oc);
        }

        actionBTNManager = Instantiate(actionBTNManagerPrefab);
    }

    //Spawn enemies. Likely to be updated later.
    public void spawnEnemies(Vector2Int[] locs){
        enemies = new EnemyController[locs.Length];
        for (int i = 0; i < locs.Length; i++)
        {
            enemies[i] = Instantiate(enemyPrefab);
            enemies[i].spawn(boardManager, locs[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

        OrderedCharacter[] characters = players.Cast<OrderedCharacter>().Concat(enemies.Cast<OrderedCharacter>()).ToArray();
        boardManager.setPlayers(characters);

        //get where the mouse is in the world
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (hasSelected)
            {
                //if a player is selected, move them to the clicked cell
                Vector2Int cell = boardManager.clickToCell(worldPoint);
                player.moveToCell(cell);
                tileHighlighter.ClearHighlights();
                hasSelected = false;
                player.setSelected(false);
                player = null;

                actionBTNManager.destroy();

            }
            else
            {
                //if no player is selected, select the player in the clicked cell
                Vector2Int cell = boardManager.clickToCell(worldPoint);
                Debug.Log("Clicked cell: " + cell);


                OrderedCharacter tempPlayer = boardManager.detectSelected(cell);
                if (tempPlayer != null)
                {
                    if (tempPlayer.GetType() == playerPrefab.GetType())
                    {   
                        player = (CharacterController)boardManager.detectSelected(cell);
                        //player is selected
                        hasSelected = true;
                        player.setSelected(true);
                        tileHighlighter.ClearHighlights();

                        Vector2Int[] moveRange = player.getMovementRange();
                        if(player.hasMoved == false){
                            tileHighlighter.HighlightTiles(moveRange);
                        }
                        
                        if (player.hasActed == false)
                        {
                            actionBTNManager.Create(player, UI, tileHighlighter);
                        }
                    }
                    else
                    {
                        if(player != null){
                            //right now, enemy would be the one clicked
                            player.setSelected(false);
                            player = null;

                            actionBTNManager.destroy();
                        }
                        
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            //if the right mouse button is clicked, deselect the player
            hasSelected = false;
            player.setSelected(false);
            tileHighlighter.ClearHighlights();
            actionBTNManager.destroy();
        }

        processEndTurn();

        //check for end of section
        bool enemiesAlive = false;
        foreach (OrderedCharacter oc in enemies)
        {
            if(oc != null)
            {
                enemiesAlive = true;
                break;
            }
        }
        if (!enemiesAlive){
            Transition.Instance.onKill();
        }

        bool playersAlive = false;
        foreach (OrderedCharacter oc in players)
        {
            if(oc != null)
            {
                playersAlive = true;
                break;
            }
        }
        if (!enemiesAlive){
            Transition.Instance.onKill();
        }
        if(!playersAlive)
        {
            Transition.Instance.onDeath();
        }
    }

    void processEndTurn()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null) continue;
            if (!players[i].isTurnComplete())
            {
                return;
            }
        }
        takeEnemyAction();
        //reset highlights
        hasSelected = false;
        tileHighlighter.ClearHighlights();
        actionBTNManager.destroy();

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null) continue;
            players[i].resetTurn();
        }
    }

    void takeEnemyAction()
    {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy == null) continue;
            if (enemy.hp <= 0) continue;
            CharacterController nearestPlayer = null;
            int nearestDistance = int.MaxValue;

            foreach (CharacterController player in players)
            {
                if (player == null || player.hp <= 0) continue;
                int distance = enemy.getDist(player.gridPosition);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPlayer = player;
                }
            }

            if (nearestPlayer == null) continue;

            if (nearestDistance == 1)
            {
                nearestPlayer.TakeDamage(enemy.baseAttack);
                Debug.Log(enemy.name + " attacks " + nearestPlayer.name + " for " + enemy.baseAttack + " damage!");
            }
            else
            {
                Vector2Int enemyPos = enemy.gridPosition;
                Vector2Int playerPos = nearestPlayer.gridPosition;
                Vector2Int moveTarget = enemyPos;
                if (Mathf.Abs(playerPos.x - enemyPos.x) > Mathf.Abs(playerPos.y - enemyPos.y))
                {
                    moveTarget.x += (playerPos.x > enemyPos.x) ? 1 : -1;
                }
                else
                {
                    moveTarget.y += (playerPos.y > enemyPos.y) ? 1 : -1;
                }
                if (boardManager.checkCell(moveTarget))
                {
                    enemy.moveToCell(moveTarget);
                }
                if (enemy.getDist(nearestPlayer.gridPosition) == 1)
                {
                    nearestPlayer.TakeDamage(enemy.baseAttack);
                    Debug.Log(enemy.name + " attacks " + nearestPlayer.name + " for " + enemy.baseAttack + " damage!");
                }
            }
        }
    }


    // Spawns players given their locations
    // At the moment, each player is spawned with the same actions.
    // This will need to be updated to reflect the actual actions and locations of the players.
    void spawnPlayers(Vector2Int[] playerSpawnLocations)
    {
        List<Action> actions = actionDatabase.GetAllActions();

        int i = 0;
        foreach (CharacterController oc in players)
        {
            oc.spawn(boardManager, playerSpawnLocations[i], new[] { actions[1], actions[2] });
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

