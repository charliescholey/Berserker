using UnityEngine;

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
public class PlayerController : OrderedCharacter
{
    private SpriteRenderer m_SpriteRenderer;
    private bool isSelected = false;
    private bool hasMoved = false;
    private bool hasActed = false;
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
        return hasMoved && hasActed;
    }

    public override void takeAction(Action action)
    {
        if(hasActed){
            return;
        }
        hasActed = true;
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

    public void toggleHighlight(){
        if(isSelected){
            m_SpriteRenderer.color = Color.cyan;
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

    public void TakeDamage(int damage){
        int finalDamage = Mathf.Max(damage - baseDefense, 1);
        currentHealth -= finalDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //not sure how we are going to go about having thr characters die but here is a function for it
    private void Die(){ 

    }
}
