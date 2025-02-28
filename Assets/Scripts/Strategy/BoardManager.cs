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

    public Vector3 cellToWorld(Vector2Int cell)
    {
        //this is maybe the dirtiest code I've ever written
        float x = cell.x * cellSize;
        x += cellSize / 2;
        x = -x;
        float y = cell.y * cellSize;
        y += cellSize / 2;
        y = -y;
        return new Vector3(x, y, -2);
    }
}
