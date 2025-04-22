using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

/**
 * PlayerController class -- manages the player-controlled characters.
 
 * NOTE: this class should not be used for gameplay design. Ideally,
 * this class only contains player information and the rest is
 * handled by the GameManager.

 * Functionality to add:
 * - track when turn has been completed
 * - set player sprite
 * - set player stats (from JSON eventually)
 * - attacking & all other actions
 */
public class CharacterController : OrderedCharacter
{
    private SpriteRenderer m_SpriteRenderer;
    private bool isSelected = false;
    public bool hasMoved = false;
    public bool hasActed = false;
    private int moveRange = 3;
    private Action[] actions;
    //basic stats of a character that can be changed 
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public int baseMovementRange;
    public int currentHealth;
    //this is for if we decide to have class specific chcaracters
    public string characterClass; 
    // One unique ability specific to each character
    public string uniqueAbility;

    [SerializeField] private Grid grid;

    public override void spawn(BoardManager bm, Vector2Int cell)
    {
        boardManager = bm;
        gridPosition = cell;
        transform.position = boardManager.cellToWorld(cell);
    }
    
    public void spawn(BoardManager bm, Vector2Int cell, Action[] acts)
    {
        boardManager = bm;
        gridPosition = cell;
        transform.position = boardManager.cellToWorld(cell);
        actions = acts;
    }

    public void resetTurn(){
        hasMoved = false;
        hasActed = false;
        isSelected = false;
    }

    public override bool isTurnComplete()
    {   //temp solve until combat is implemented
        if(hasMoved && hasActed){
            return true;
        }
        return false;
    }

    public override void takeAction(Action action)
    {
        hasActed = true;
        Debug.Log("Player has taken action: " + action.name);
    }

    public Vector2Int[] getMovementRange(){
        //handle as arraylist for dynamic sizing
        List<Vector2Int> cells = new List<Vector2Int>();
        for(int i = -moveRange; i <= moveRange; i++){
            for(int j = -moveRange; j <= moveRange; j++){
                //uses existing manhattan distance function from OrderedCharacter
                if(getDist(new Vector2Int(gridPosition.x + i, gridPosition.y + j)) <= moveRange){
                    cells.Add(new Vector2Int(gridPosition.x + i, gridPosition.y + j));
                }
            }
        }
        
        //handle return as array
        return cells.ToArray();
    }

    public override void moveToCell(Vector2Int cell)
    {
        if(getDist(cell) > moveRange){
            return;
        }
        if(gridPosition.x == cell.x && gridPosition.y == cell.y){
            return;
        }
        if(hasMoved){
            return;
        }
        gridPosition = cell;
        transform.position = boardManager.cellToWorld(cell);
        hasMoved = true;
    }

    public void toggleHighlight(){
        if(isSelected){
            if(hasMoved){
                m_SpriteRenderer.color = Color.red;
            }else{
                m_SpriteRenderer.color = Color.cyan;
            }
        }else{
            m_SpriteRenderer.color = Color.white;
        }
    }

    public void setSelected(bool selected){
        isSelected = selected;
        toggleHighlight();
    }

    void Start(){
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        hp = 100;
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
        gameObject.SetActive(false);
    }

    public Action[] GetActions() {
        return actions;
    }

    // takes in the position that the player wants to move to, returns true if there is a wall at that tile, false otherwise
    private bool IsWallAtTarget(Vector2 inputDelta) {
        // compute target position
        Vector2 desiredPos = (Vector2)transform.position + inputDelta;

        // convert to cell coordinates
        Vector3Int cell = grid.WorldToCell(desiredPos);

        // get cell center
        Vector3 cellCenter3 = grid.GetCellCenterWorld(cell);
        Vector2 cellCenter2 = new Vector2(cellCenter3.x, cellCenter3.y);

        // check there is an object tagged wall at that point
        Collider2D hit = Physics2D.OverlapPoint(cellCenter2);
        return hit != null && hit.CompareTag("wall");
    }
}
