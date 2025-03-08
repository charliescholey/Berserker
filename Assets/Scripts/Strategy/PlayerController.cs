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
    private int moveRange = 3;
    private Action[] actions;

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

    public override bool isTurnComplete()
    {   //temp solve until combat is implemented
        return hasMoved;
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

    public Vector2Int getGridPosition()
    {
        return gridPosition;
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

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        hp = 100;
    }
}
