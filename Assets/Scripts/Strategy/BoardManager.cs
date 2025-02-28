using UnityEngine;
using UnityEngine.Tilemaps;


public class BoardManager : MonoBehaviour
{
    public Tilemap gameTilemap;

    public int width;
    public int height;
    public PlayerController playerPrefab;
    private PlayerController player;
    private float cellSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Instantiate(playerPrefab);
        player.spawn(this, new Vector2Int(width/2, height/2));
        cellSize = gameTilemap.cellSize.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Given a cell x,y, return the world position of the center of that cell
    public Vector3 cellToWorld(Vector2Int cell)
    {
        //this is maybe the dirtiest code I've ever written
        float x = cell.x * cellSize;
        float y = cell.y * cellSize;
        //the tilemap is still offset maybe?
        y += cellSize;
        return new Vector3(x, y, -2);
    }

    //Given a click, return the cell that was clicked
    public Vector2Int clickToCell(Vector3 click)
    {
        Vector3Int cell = gameTilemap.WorldToCell(click);
        return new Vector2Int(cell.x, cell.y);
    }

    //Given a world position, return the cell that was clicked
    public Vector2Int worldToCell(Vector3 click)
    {
        int x = (int) click.x;
        int y = (int) click.y;
        x %= (int) cellSize;
        y %= (int) cellSize;
        x *= (int)cellSize;
        y *= (int)cellSize;
        return new Vector2Int(y, x);
    }
}
