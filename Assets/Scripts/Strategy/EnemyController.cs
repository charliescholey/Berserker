using UnityEngine;

public class EnemyController : OrderedCharacter
{
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public int currentHealth;

    public override void spawn(BoardManager bm, Vector2Int cell)
    {
        moveRange = 2;
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
    }

    public override void takeAction(Action action)
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp = 30;
        baseAttack = 50;
    }
       public override void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }
    public override void Die()
     {
        // to deal with dying, again not sure how we are dealing with it
        Debug.Log($"{gameObject.name} died.");
        //SaveFileManager.CurrentPlayerData.exp += 150; // give 10 XP on enemy death
        Destroy(gameObject);
    }
}
