using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    [SerializeField]
    private Tile tilePrefab;
    private TileGrid grid;
    private List<Tile> tiles;
    private List<TileStateSO> tileStateList;

    public bool IsBusy {  get; private set; } 
    private HashSet<Tile> mergedTiles; 

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
        tileStateList = Resources.Load<TileStateListSO>("TileStateListSO").list;
        mergedTiles = new HashSet<Tile>();
    }

    private void Start()
    {
        CreateTile();
        CreateTile();
    }

    public void HandleInput()
    {
        if (IsBusy) return; 

        int width = grid.Rows[0].Cells.Count();
        int height = grid.Rows.Count();

        if (Input.GetKeyDown(KeyCode.W))
        {
            IsBusy = true;
            mergedTiles.Clear(); 
            for (int y = 1; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    TileCell tileCell = grid.Rows[y].Cells[x];
                    if (tileCell.IsOccupied())
                    {
                        Move(tileCell.GetTile(), GameManager.Direction.Up);
                    }
                }
            }

            CreateTile();
            CreateTile();
            IsBusy = false;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            IsBusy = true;
            mergedTiles.Clear(); 
            for (int y = height - 2; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    TileCell tileCell = grid.Rows[y].Cells[x];
                    if (tileCell.IsOccupied())
                    {
                        Move(tileCell.GetTile(), GameManager.Direction.Down);
                    }
                }
            }

            CreateTile();
            CreateTile();
            IsBusy = false;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            IsBusy = true;
            mergedTiles.Clear(); 
            for (int x = 1; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    TileCell tileCell = grid.Rows[y].Cells[x];
                    if (tileCell.IsOccupied())
                    {
                        Move(tileCell.GetTile(), GameManager.Direction.Left);
                    }
                }
            }

            CreateTile();
            CreateTile();
            IsBusy = false;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            IsBusy = true;
            mergedTiles.Clear(); 
            for (int x = width - 2; x >= 0; x--)
            {
                for (int y = 0; y < height; y++)
                {
                    TileCell tileCell = grid.Rows[y].Cells[x];
                    if (tileCell.IsOccupied())
                    {
                        Move(tileCell.GetTile(), GameManager.Direction.Right);
                    }
                }
            }

            CreateTile();
            CreateTile();
            IsBusy = false;
        }
    }

    private void CreateTile()
    {
        IsBusy = true;
        TileCell tileCell = GetRandomTileCellToSpawnTile();

        if (tileCell != null)
        {
            Tile tile = Instantiate(tilePrefab, grid.transform);
            tile.SetTileCell(tileCell);
            tile.SetState(tileStateList[0]);
            tileCell.SetTile(tile);
            tile.transform.position = tileCell.transform.position;
            tiles.Add(tile);

            tile.transform.localScale = Vector3.zero;
            LeanTween.scale(tile.gameObject, Vector3.one * 1.2f, 0.2f).setOnComplete(() =>
            {
                LeanTween.scale(tile.gameObject, Vector3.one, 0.1f).setOnComplete(() =>
                {
                    IsBusy = false;
                });
            });
        }
    }

    private TileCell GetRandomTileCellToSpawnTile()
    {
        System.Random random = new System.Random();

        int randomIndex = random.Next(0, grid.Cells.Count());

        int originRandomIndex = randomIndex;

        while (true)
        {
            if (grid.Cells[randomIndex].IsEmpty())
            {
                return grid.Cells[randomIndex];
            }

            randomIndex++;
            if (randomIndex >= grid.Cells.Count())
            {
                randomIndex = 0;
            }

            if (randomIndex == originRandomIndex)
                return null;
        }
    }

    private void Move(Tile tile, GameManager.Direction direction)
    {
        TileCell adjacentTileCell = GetAdjacentTile(tile, direction);

        if (adjacentTileCell != null)
        {
            if (adjacentTileCell.IsOccupied())
            {
                Tile adjacentTile = adjacentTileCell.GetTile();
                TileStateSO adjacentTileState = adjacentTile.GetState();

                // merge logic
                IsBusy = true;

                if (adjacentTileState == tile.GetState() && !mergedTiles.Contains(tile) && !mergedTiles.Contains(adjacentTile))
                {
                    int nextStateIndex = tileStateList.IndexOf(adjacentTileState) + 1;

                    if (nextStateIndex < tileStateList.Count)
                    {
                        tile.GetTileCell().SetTile(null);
                        tiles.Remove(tile);
                        Destroy(tile.gameObject);

                        LeanTween.scale(adjacentTile.gameObject, Vector3.zero, 0.2f).setOnComplete(() =>
                        {
                            adjacentTile.SetState(tileStateList[nextStateIndex]);
                            LeanTween.scale(adjacentTile.gameObject, Vector3.one, 0.2f);
                            IsBusy = false;
                        });

                        mergedTiles.Add(adjacentTile); // Mark the tile as merged
                    }
                }
            }
            else
            {
                adjacentTileCell.SetTile(tile);
                tile.GetTileCell().SetTile(null);
                tile.SetTileCell(adjacentTileCell);
                float moveSpeed = 0.05f;
                LeanTween.move(tile.gameObject, adjacentTileCell.transform.position, moveSpeed).setOnComplete(() =>
                {
                    IsBusy = false;
                });
                Move(tile, direction);
            }
        }
    }

    private TileCell GetAdjacentTile(Tile tile, GameManager.Direction direction)
    {

        switch (direction)
        {
            case GameManager.Direction.Up:
                Vector2Int tileXY = tile.GetTileCell().GetCoordinates();

                if (tileXY.y <= 0) return null;

                TileCell adjacentTileCell = grid.Rows[tileXY.y - 1].Cells[tileXY.x];

                return adjacentTileCell;
            case GameManager.Direction.Down:
                tileXY = tile.GetTileCell().GetCoordinates();

                if (tileXY.y >= grid.GetHeight() - 1) return null;

                adjacentTileCell = grid.Rows[tileXY.y + 1].Cells[tileXY.x];

                return adjacentTileCell;
            case GameManager.Direction.Left:
                tileXY = tile.GetTileCell().GetCoordinates();

                if (tileXY.x <= 0) return null;

                adjacentTileCell = grid.Rows[tileXY.y].Cells[tileXY.x - 1];

                return adjacentTileCell;
            case GameManager.Direction.Right:
                tileXY = tile.GetTileCell().GetCoordinates();

                if (tileXY.x >= grid.GetWidth() - 1) return null;

                adjacentTileCell = grid.Rows[tileXY.y].Cells[tileXY.x + 1];

                return adjacentTileCell;
        }

        return null;
    }

    public bool IsGameOverCheck()
    {
        // Check if all tiles are occupied
        if (tiles.Count < 16)
        {
            return false;
        }

        // Check for possible merges
        for (int y = 0; y < grid.Rows.Count(); y++)
        {
            for (int x = 0; x < grid.Rows[y].Cells.Count(); x++)
            {
                TileCell tileCell = grid.Rows[y].Cells[x];
                if (tileCell.IsOccupied())
                {
                    Tile tile = tileCell.GetTile();
                    if (CanMerge(tile, GameManager.Direction.Up) ||
                        CanMerge(tile, GameManager.Direction.Down) ||
                        CanMerge(tile, GameManager.Direction.Left) ||
                        CanMerge(tile, GameManager.Direction.Right))
                    {
                        return false;
                    }
                }
            }
        }

        // If no merges are possible and all tiles are occupied, game is over
        return true;
    }


    private bool CanMerge(Tile tile, GameManager.Direction direction)
    {
        TileCell adjacentTileCell = GetAdjacentTile(tile, direction);

        if (adjacentTileCell != null && adjacentTileCell.IsOccupied())
        {
            Tile adjacentTile = adjacentTileCell.GetTile();
            return tile.GetState() == adjacentTile.GetState();
        }

        return false;
    }

    public void RestartGame()
    {
        foreach (Tile tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        tiles.Clear();
        mergedTiles.Clear();

        for (int y = 0; y < grid.Rows.Count(); y++)
        {
            for (int x = 0; x < grid.Rows[y].Cells.Count(); x++)
            {
                TileCell tileCell = grid.Rows[y].Cells[x];

                tileCell.SetTile(null);
            }
        }


        CreateTile();
        CreateTile();
    }
}
