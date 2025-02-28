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
*/

public class PlayerController : MonoBehaviour
{
    private BoardManager boardManager;
    private Vector2Int gridPosition;

    public void spawn(BoardManager bm, Vector2Int cell)
    {
        boardManager = bm;
        moveToCell(cell);
    }

    private void moveToCell(Vector2Int cell)
    {
        gridPosition = cell;
        transform.position = boardManager.cellToWorld(cell);
    }

    // Update is called once per frame
    void Update()
    {
        //move character when clicked (TO BE MOVED ELSEWHERE... EVENTUALLY)
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)){
            Vector2Int cell = boardManager.clickToCell(worldPoint);
            moveToCell(cell);
        }
        
    }
}
