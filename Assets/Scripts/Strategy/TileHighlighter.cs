using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileHighlighter : MonoBehaviour
{
    public Tilemap highlightTilemap;
    public TileBase movementTile;
    public TileBase attackTile;

    public void HighlightTiles(IEnumerable<Vector2Int> cells, bool isAttack = false)
    {
        foreach (var cell in cells)
        {
            var tile = isAttack ? attackTile : movementTile;
            highlightTilemap.SetTile((Vector3Int)cell, tile);
        }
    }

    public void ClearHighlights()
    {
        highlightTilemap.ClearAllTiles();
    }
}