using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid : MonoBehaviour {
    public static Grid Instance { get; private set; }

    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 1.0f;

    private Tile[,] tiles;

    public int Width => width;
    public int Height => height;
    public Tile[,] Tiles => tiles;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        GenerateGrid();
    }

    private void GenerateGrid() {
        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Vector3 worldPos = new Vector3(x * cellSize, 0.0f, y * cellSize);
                Tile tile = new Tile(worldPos, x, y);
                tiles[x, y] = tile;
            }
        }
    }

    public Tile GetTileAtWorldPosition(Vector3 worldPos) {
        int x = Mathf.FloorToInt(worldPos.x / cellSize);
        int y = Mathf.FloorToInt(worldPos.z / cellSize);

        if (x >= 0 && x < width && y >= 0 && y < height) {
            return tiles[x, y];
        }

        Debug.LogError($"Attempted to get tile at position {worldPos}, however this position appears to be out of bounds. Returning null.");
        return null;
    }

    public HashSet<Tile> GetNeighbours(Tile _node, int _rangeX = 1, int _rangeY = 1) {
        HashSet<Tile> neighbours = new HashSet<Tile>();

        for (int x = -_rangeX; x <= _rangeX; x++) {
            for (int y = -_rangeY; y <= _rangeY; y++) {

                if (x == 0 && y == 0) {
                    continue;
                }

                int checkX = _node.GridX + x;
                int checkY = _node.GridY + y;

                if (checkX >= 0 && checkX < width &&
                    checkY >= 0 && checkY < height) {
                    neighbours.Add(tiles[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public List<Tile> GetAllEmptyTiles() {
        List<Tile> emptyTiles = new List<Tile>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (tiles[x, y].isEmpty) {
                    emptyTiles.Add(tiles[x, y]);
                }
            }
        }

        return emptyTiles;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                Vector3 position = new Vector3(x * cellSize, 0, z * cellSize);
                Gizmos.DrawWireCube(position, new Vector3(cellSize, 0.1f, cellSize));
            }
        }
    }
}
