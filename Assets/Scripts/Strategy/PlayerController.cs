using UnityEngine;

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
