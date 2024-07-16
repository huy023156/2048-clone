using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TileCell : MonoBehaviour
{
    private Vector2Int coordinates;

    private Tile tile;

    public void SetCoordinate(int x, int y) {
        coordinates = new Vector2Int(x, y);
    }

    public void SetTile(Tile tile)
    {
        this.tile = tile;
    }

    public Vector2Int GetCoordinates() => coordinates;

    public bool IsEmpty() => tile == null;

    public bool IsOccupied() => tile != null;

    public Tile GetTile()
    {
        return tile; 
    }
}
