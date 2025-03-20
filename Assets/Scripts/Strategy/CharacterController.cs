using System.Collections.Generic;
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

    // HWE Code Start
    public Action[] GetActions() {
        return actions;
    }
    // HWE Code End
}
