using UnityEngine;

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
    private PlayerController player;

    //tracks the player prefab for spawning
    public PlayerController playerPrefab;
    //tracks the enemy prefab for spawning
    public EnemyController enemyPrefab;
    //tracks the players in the game
    public PlayerController[] players;


    private EnemyController enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        players = new PlayerController[2];
        //spawn the players
        players[0] = Instantiate(playerPrefab);
        players[0].spawn(boardManager, new Vector2Int(0, 0));
        players[1] = Instantiate(playerPrefab);
        players[1].spawn(boardManager, new Vector2Int(1, 3));

        enemy = Instantiate(enemyPrefab);
        enemy.spawn(boardManager, new Vector2Int(3, 3));

    }

    // Update is called once per frame
    void Update()
    {
        //get where the mouse is in the world
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0)){
            if(hasSelected){
                //if a player is selected, move them to the clicked cell
                Vector2Int cell = boardManager.clickToCell(worldPoint);
                player.moveToCell(cell);
                hasSelected = false;
                player.setSelected(false);
                player = null;
                enemy.moveToCell(new Vector2Int(enemy.gridPosition.x + 1, enemy.gridPosition.y));
            }else{
                //if no player is selected, select the player in the clicked cell
                Vector2Int cell = boardManager.clickToCell(worldPoint);
                player = boardManager.detectSelected(cell, players);
                if(player != null){
                    hasSelected = true;
                    player.setSelected(true);
                }
            }
        }

        if(Input.GetMouseButtonDown(1)){
            //if the right mouse button is clicked, deselect the player
            hasSelected = false;
            player.setSelected(false);
        }

    }
}
