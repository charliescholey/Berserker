using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

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
    private float cellSize;

    private OrderedCharacter[] players;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cellSize = gameTilemap.cellSize.x;
    }

    public void setPlayers(OrderedCharacter[] players){
        this.players = players;
    }

    // detectSelected returns whatever the current cell has in it.
    // If a player is in the cell, it returns the player, otherwise it returns null.
    public OrderedCharacter detectSelected(Vector2Int cell){
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

    public bool checkCell(Vector2Int cell){
        //I'm going to treat these values as basically hardcoded for now
        if(cell.x < -11 || cell.x > 10){
            return false;
        }
        if(cell.y < -5 || cell.y > 4){
            return false;
        }
        //check for player collisions
        if(detectSelected(cell) != null){
            return false;
        }
        return true;
    }

    public Vector2Int[] checkCells(Vector2Int[] cells){
        List<Vector2Int> validCells = new List<Vector2Int>();
        for(int i = 0; i < cells.Length; i++){
            if(checkCell(cells[i])){
                validCells.Add(cells[i]);
            }
        }
        Vector2Int[] returnCells = validCells.ToArray();
        
        return returnCells;
    }

    //isTurnComplete returns true if all players have completed their turn
    //TODO move into gameManager
    /*public bool isTurnComplete(){
        for(int i = 0; i < players.Length; i++){
            if(players[i] == null){
                continue;
            }
            if(!players[i].isTurnComplete()){
                return false;
            }
        }
        return true;
    }*/
}
