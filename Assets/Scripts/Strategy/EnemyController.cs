using UnityEngine;

public class EnemyController : OrderedCharacter
{
    int moveRange = 2;
    private bool hasMoved = false;

    public override void spawn(BoardManager bm, Vector2Int cell)
    {
        boardManager = bm;
        gridPosition = cell;
        transform.position = boardManager.cellToWorld(cell);
    }

    public override bool isTurnComplete()
    {
        return true;
    }

    public override void moveToCell(Vector2Int cell)
    {
        if(getDist(cell) > moveRange){
            return;
        }
        if(gridPosition.x == cell.x && gridPosition.y == cell.y){
            return;
        }
        gridPosition = cell;
        transform.position = boardManager.cellToWorld(cell);
        hasMoved = true;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp = 20;
    }
}
