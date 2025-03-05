using UnityEngine;
using UnityEngine.Tilemaps;

/**
* BoardManager class -- manages the game board.

* NOTE: this class should not be used for gameplay design. Ideally,
* this class only contains board information and the rest is
* handled by the GameManager.

* Functionality to add:
* - collision/map tracking
* - pathfinding?
* - tilemap interactions
*/
public class BoardManager : MonoBehaviour
{
    //tracks the underlying tilemap that we work from
    public Tilemap gameTilemap;
    //tracks the player prefab for spawning
    public PlayerController playerPrefab;
    //tracks the players in the game
    public PlayerController[] players;
    private float cellSize;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        players = new PlayerController[2];
        //spawn the players
        players[0] = Instantiate(playerPrefab);
        players[0].spawn(this, new Vector2Int(0, 0));
        players[1] = Instantiate(playerPrefab);
        players[1].spawn(this, new Vector2Int(1, 3));
        cellSize = gameTilemap.cellSize.x;
    }

    // detectSelected returns whatever the current cell has in it.
    // If a player is in the cell, it returns the player, otherwise it returns null.
    public PlayerController detectSelected(Vector2Int cell){
        for(int i = 0; i < players.Length; i++){
            if(players[i]  == null){
                continue;
            }
            if(players[i].getGridPosition() == cell){
                return players[i];
            }
        }
        
        return null;
    }

    //Given a cell x,y, return the world position of the center of that cell
    public Vector3 cellToWorld(Vector2Int cell)
    {
        Vector3Int cellPos = new Vector3Int(cell.x, cell.y, 0);
        Vector3 cellCenter = gameTilemap.GetCellCenterWorld(cellPos);
        float x = cellCenter.x;
        float y = cellCenter.y;
        //z-level is offset to ensure player is above the tilemap
        return new Vector3(x, y, -2);
    }

    //Given a click, return the cell that was clicked
    public Vector2Int clickToCell(Vector3 click)
    {
        Vector3Int cell = gameTilemap.WorldToCell(click);
        return new Vector2Int((int) cell.x, (int) cell.y);
    }

    //Given a world position, return the cell that was clicked
    public Vector2Int worldToCell(Vector3 click)
    {
        Vector3Int cell = gameTilemap.WorldToCell(click);
        return new Vector2Int((int) cell.x, (int) cell.y);
    }

    //isTurnComplete returns true if all players have completed their turn
    public bool isTurnComplete(){
        for(int i = 0; i < players.Length; i++){
            if(players[i] == null){
                continue;
            }
            if(!players[i].isTurnComplete()){
                return false;
            }
        }
        return true;
    }
}
