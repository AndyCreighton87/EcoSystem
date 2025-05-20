using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private List<TileVisual> tileVisuals;
    [SerializeField] private float noiseScale = 0.1f;
    [SerializeField] private float waterThreshold = 0.4f;

    private List<Vector2Int> Cardinal = new List<Vector2Int>() {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    private void Start() {
        GenerateTerrain();
    }

    private void GenerateTerrain() {
        Grid grid = Grid.Instance;

        if (grid == null) {
            Debug.LogError($"Attempting to generate terrain, but Grid is null. Something has gone terribly, terribly wrong");
            return;
        }

        for (int x = 0; x < grid.Width; x++) {
            for (int y = 0; y < grid.Height; y++) {
                float noise = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);

                grid.Tiles[x, y].TileType = noise < waterThreshold ? TileType.Water : TileType.Grass;
            }
        }

        DoTileVariation(grid);
        SpawnTiles(grid);
    }

    private void DoTileVariation(Grid grid) {
        // TODO: Some issues here:
        // The Sand Grass isn't spawning.
        // Deep water looks really good with a neighbour range of 1
        // Thick grass does not


        DoSand();
        DoSandGrass();
        DoDeepWaterAndThickGrass();


        void DoSand() {
            for (int x = 0; x < grid.Width; x++) {
                for (int y = 0; y < grid.Height; y++) {
                    if (grid.Tiles[x, y].TileType == TileType.Water) {
                        foreach (var dir in Cardinal) {
                            int nx = x + dir.x;
                            int ny = y + dir.y;
                            if (CheckInBounds(nx, ny, grid.Width, grid.Height) && grid.Tiles[nx, ny].TileType == TileType.Grass) {
                                grid.Tiles[nx, ny].TileType = TileType.Sand;
                            }
                        }
                    }
                }
            }
        }

        void DoSandGrass() {
            for (int x = 0; x < grid.Width; x++) {
                for (int y = 0; y < grid.Height; y++) {
                    if (grid.Tiles[x, y].TileType == TileType.Grass) {
                        foreach (var dir in Cardinal) {
                            int nx = x + dir.x;
                            int ny = y + dir.y;
                            if (CheckInBounds(nx, ny, grid.Width, grid.Height) && grid.Tiles[nx, ny].TileType == TileType.Sand) {
                                grid.Tiles[nx, ny ].TileType = TileType.SandGrass;
                            }
                        }
                    }
                }
            }
        }

        void DoDeepWaterAndThickGrass() {
            List<Tile> tilesToUpdate = new List<Tile>();

            for (int x = 1; x < grid.Width - 1; x++) {
                for (int y = 1; y < grid.Height - 1; y++) {
                    Tile tile = grid.Tiles[x, y];
                    if (tile.TileType != TileType.Water && tile.TileType != TileType.Grass) {
                        continue;
                    }

                    if (AllNeighboursMatch(tile)) {
                        tilesToUpdate.Add(tile);
                    }
                }
            }

            foreach (var tile in tilesToUpdate) {
                tile.TileType = tile.TileType == TileType.Water ? TileType.DeepWater : TileType.ThickGrass;
            }
        }

        bool AllNeighboursMatch(Tile tile) {
            var neighbours = grid.GetNeighbours(tile, 3);
            return neighbours.All(t => t.TileType == tile.TileType);
        }
    }

    private bool CheckInBounds(int x, int y, int width, int height) {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    private void SpawnTiles(Grid grid) {
        for (int x = 0; x < grid.Width; x++) {
            for (int y = 0; y < grid.Height; y++) {
                Tile tile = grid.Tiles[x, y];
                TileVisual prefab = tileVisuals.FirstOrDefault(t => t.TileType == tile.TileType);
                if (prefab == null) {
                    Debug.LogError($"Tile at {x},{y}. Visual not found.");
                    return;
                }
                Instantiate(prefab, tile.Position, Quaternion.identity, transform);
            }
        }
    }


}
