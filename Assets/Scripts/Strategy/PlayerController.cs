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
 */

public class PlayerController : MonoBehaviour
{
    private BoardManager boardManager;
    private Vector2Int gridPosition;
    private SpriteRenderer m_SpriteRenderer;
    private bool isSelected = false;

    public void spawn(BoardManager bm, Vector2Int cell)
    {

        boardManager = bm;
        moveToCell(cell);
        //DON'T. ASK. IDK. WHY. THIS. WORKS.
        gridPosition.y -= 1;
    }

    public void moveToCell(Vector2Int cell)
    {
        gridPosition = cell;
        transform.position = boardManager.cellToWorld(cell);
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
    }
}
