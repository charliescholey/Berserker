using UnityEngine;
using System.Collections.Generic;

public abstract class OrderedCharacter : MonoBehaviour
{
    protected BoardManager boardManager;
    public Vector2Int gridPosition;
    public int hp;
    protected int moveRange;

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

    public Vector2Int[] getMovementRange()
    {
        // Use BFS to calculate movement range while avoiding walls
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        List<Vector2Int> cells = new List<Vector2Int>();

        queue.Enqueue(gridPosition);
        visited.Add(gridPosition);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            cells.Add(current);

            foreach (Vector2Int direction in new Vector2Int[] {
            new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(0, 1), new Vector2Int(0, -1)
            })
            {
            Vector2Int neighbor = current + direction;

            if (!visited.Contains(neighbor) &&
                getDist(neighbor) <= moveRange &&
                boardManager.checkCell(neighbor))
            {
                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
            }
        }

        return cells.ToArray();
    }


    // Action Processing

    //get all related cells for an action
    public Vector2Int[] processActionRange(Action action)
    {
        Vector2Int[] cells;
        switch (action.target)
        {
            case Action.TargetType.SELF:
                return new Vector2Int[] { gridPosition };
            case Action.TargetType.RADIUS:
                cells = new Vector2Int[(action.range * 2 + 1) * (action.range * 2 + 1)];
                int index = 0;
                for (int i = -action.range; i <= action.range; i++)
                {
                    for (int j = -action.range; j <= action.range; j++)
                    {
                        cells[index] = new Vector2Int(gridPosition.x + i, gridPosition.y + j);
                        index++;
                    }
                }
                return cells;
            case Action.TargetType.TARGETDIST:
                cells = new Vector2Int[4];
                cells[0] = new Vector2Int(gridPosition.x + action.range, gridPosition.y);
                cells[1] = new Vector2Int(gridPosition.x - action.range, gridPosition.y);
                cells[2] = new Vector2Int(gridPosition.x, gridPosition.y + action.range);
                cells[3] = new Vector2Int(gridPosition.x, gridPosition.y - action.range);
                return cells;
            case Action.TargetType.LINE:
                cells = new Vector2Int[action.range * 4];
                for (int i = 0; i < action.range; i++)
                {
                    cells[i] = new Vector2Int(gridPosition.x + i, gridPosition.y);
                }
                for (int i = 0; i < action.range; i++)
                {
                    cells[i] = new Vector2Int(gridPosition.x - i, gridPosition.y);
                }
                for (int i = 0; i < action.range; i++)
                {
                    cells[i] = new Vector2Int(gridPosition.x, gridPosition.y + i);
                }
                for (int i = 0; i < action.range; i++)
                {
                    cells[i] = new Vector2Int(gridPosition.x, gridPosition.y - i);
                }
                return cells;
        }
        return new Vector2Int[] { gridPosition };
    }

    public void processActionEffect(Action action, Vector2Int cell)
    {
        float odds = action.effectChance;
        float randomValue = Random.Range(0.0f, 1.0f);
        if (randomValue < odds)
        {
            switch (action.effect)
            {
                case Action.EffectType.BURN:
                    Debug.Log("Burned");
                    break;
                case Action.EffectType.POISON:
                    Debug.Log("Poisoned");
                    break;
                case Action.EffectType.PARALYSIS:
                    Debug.Log("Paralyzed");
                    break;
                case Action.EffectType.SLEEP:
                    Debug.Log("Put to sleep");
                    break;
            }
        }
        else
        {
            Debug.Log("Effect failed");
        }

    }

    public void processAction(Action action)
    {
        //TODO
    }
}
