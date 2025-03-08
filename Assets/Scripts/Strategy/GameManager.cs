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
    public OrderedCharacter[] characters;


    private EnemyController enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characters = new OrderedCharacter[3];
        //spawn the players
        characters[0] = Instantiate(playerPrefab);
        characters[0].spawn(boardManager, new Vector2Int(0, 0));
        characters[1] = Instantiate(playerPrefab);
        characters[1].spawn(boardManager, new Vector2Int(1, 3));

        characters[2] = Instantiate(enemyPrefab);
        characters[2].spawn(boardManager, new Vector2Int(3, 0));

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
                //move enemy
                characters[2].moveToCell(new Vector2Int(characters[2].gridPosition.x + 1, characters[2].gridPosition.y));
            }else{
                //if no player is selected, select the player in the clicked cell
                Vector2Int cell = boardManager.clickToCell(worldPoint);
                player = (PlayerController) boardManager.detectSelected(cell, characters);
                if(player != null){
                    if(player.GetType() == typeof(PlayerController)){
                        //player is selected
                        hasSelected = true;
                        player.setSelected(true);
                    }else{
                        //right now, enemy would be the one clicked
                        player = null;
                    }
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
