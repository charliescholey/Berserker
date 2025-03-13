using UnityEngine;

public abstract class OrderedCharacter : MonoBehaviour
{
    protected BoardManager boardManager;
    public Vector2Int gridPosition;
    public int hp;
    public HPTextController hpTextController;

    // to deal with dying, again not sure how we are dealing with it
    public virtual void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }

    public abstract void spawn(BoardManager bm, Vector2Int cell);

    public abstract bool isTurnComplete();

    public abstract void moveToCell(Vector2Int cell);

    public abstract void takeAction(Action action);

    public abstract void TakeDamage(int damage);


    public int getDist(Vector2Int cell)
    {
        //return manhattan distance from provided cell
        return Mathf.Abs(cell.x - gridPosition.x) + Mathf.Abs(cell.y - gridPosition.y);
    }

    public Vector2Int getGridPosition()
    {
        return gridPosition;
    }

}
