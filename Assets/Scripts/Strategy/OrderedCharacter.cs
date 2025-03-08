using UnityEngine;

public abstract class OrderedCharacter : MonoBehaviour
{
    protected BoardManager boardManager;
    public Vector2Int gridPosition;
    public int hp;

    public abstract void spawn(BoardManager bm, Vector2Int cell);

    public abstract bool isTurnComplete();

    public abstract void moveToCell(Vector2Int cell);


    public int getDist(Vector2Int cell)
    {
        //return manhattan distance from provided cell
        return Mathf.Abs(cell.x - gridPosition.x) + Mathf.Abs(cell.y - gridPosition.y);
    }

}
