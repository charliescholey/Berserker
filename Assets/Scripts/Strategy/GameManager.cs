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
    private CharacterController player;

    public ActionBTNController actionBTNPrefab;
    private ActionBTNController actionBTNController;

    //canvas for overlay rendering
    public Canvas UI;
    public GameObject HPTextPrefab;

    //tracks the player prefab for spawning
    public CharacterController playerPrefab;
    //tracks the enemy prefab for spawning
    public EnemyController enemyPrefab;
    //tracks the players in the game
    public OrderedCharacter[] characters;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characters = new OrderedCharacter[3];
        //spawn the characters
        characters[0] = Instantiate(playerPrefab);
        characters[0].spawn(boardManager, new Vector2Int(0, 0));
        characters[1] = Instantiate(playerPrefab);
        characters[1].spawn(boardManager, new Vector2Int(1, 3));

        characters[2] = Instantiate(enemyPrefab);
        characters[2].spawn(boardManager, new Vector2Int(3, 0));

        //give every character an HP tracker
        for(int i = 0; i < characters.Length; i++){
            GameObject newhptext = Instantiate(HPTextPrefab);
            newhptext.transform.SetParent(UI.transform);
            newhptext.GetComponent<HPTextController>().setCharacter(characters[i]);
        }
        
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
                if(actionBTNController != null){
                    actionBTNController.destroy();
                }
            }else{
                //if no player is selected, select the player in the clicked cell
                Vector2Int cell = boardManager.clickToCell(worldPoint);
                player = (CharacterController) boardManager.detectSelected(cell, characters);
                if(player != null){
                    if(player.GetType() == typeof(CharacterController)){
                        //player is selected
                        hasSelected = true;
                        player.setSelected(true);
                        if(player.hasActed == false){
                            actionBTNController = Instantiate(actionBTNPrefab);
                            actionBTNController.transform.SetParent(UI.transform);
                            actionBTNController.setAction(actionDatabase.actions[0], player);
                        }
                    }else{
                        //right now, enemy would be the one clicked
                        player.setSelected(false);
                        player = null;
                        if(actionBTNController != null){
                            actionBTNController.destroy();
                        }
                    }
                }
            }
        }

        if(Input.GetMouseButtonDown(1)){
            //if the right mouse button is clicked, deselect the player
            hasSelected = false;
            player.setSelected(false);
        }

        //list characters to end turn
        CharacterController[] players = new CharacterController[] { (CharacterController) characters[0], (CharacterController) characters[1] };
        processEndTurn(players);
    }

    void processEndTurn(CharacterController[] players){
        for(int i = 0; i < players.Length; i++){
            if(!players[i].isTurnComplete()){
                Debug.Log("Player " + i + " has not completed their turn");
                return;
            }
        }
        takeEnemyAction();
        for(int i = 0; i < players.Length; i++){
            players[i].resetTurn();
        }
    }

    void takeEnemyAction(){
        //move enemy towards player
        Vector2Int playerPos = characters[0].gridPosition;
        Vector2Int enemyPos = characters[2].gridPosition;
        if(playerPos.x > enemyPos.x){
            characters[2].moveToCell(new Vector2Int(enemyPos.x + 1, enemyPos.y));
        }else if(playerPos.x < enemyPos.x){
            characters[2].moveToCell(new Vector2Int(enemyPos.x - 1, enemyPos.y));
        }else if(playerPos.y > enemyPos.y){
            characters[2].moveToCell(new Vector2Int(enemyPos.x, enemyPos.y + 1));
        }else if(playerPos.y < enemyPos.y){
            characters[2].moveToCell(new Vector2Int(enemyPos.x, enemyPos.y - 1));
        }
    }

    //Executes the attack action.
    public void ExecuteAttack(CharacterController attacker, Action attackAction)
    {
        // Determine damage from the action's power defaulting to 10 if no action is provided.
        int damage = attackAction != null ? attackAction.power : 10;

        Vector2Int attackerPos = attacker.gridPosition;

        Vector2Int[] adjacentCells = GetAdjacentCells(attackerPos);

        // Loop through all characters in the game.
        foreach (OrderedCharacter oc in characters)
        {
            if (!(oc is EnemyController))
                continue;

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

}

