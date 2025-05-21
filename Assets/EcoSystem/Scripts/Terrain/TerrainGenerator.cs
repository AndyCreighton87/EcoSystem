using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private List<TileVisual> tileVisuals;
    [SerializeField] private float noiseScale = 0.1f;
    [SerializeField] private float waterThreshold = 0.4f;

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
        DoSand();
        DoDeepWater();

        void DoSand() {
            List<Tile> sandTilesToUpdate = new List<Tile>();
            List<Tile> sandGrassTilesToUpdate = new List<Tile>();

            foreach (Tile tile in grid.Tiles) {
                if (tile.TileType != TileType.Grass) {
                    continue;
                }

                foreach (Tile neighbor in grid.GetNeighbours(tile)) {
                    if (neighbor.TileType == TileType.Water) {
                        sandTilesToUpdate.Add(tile);
                        break;
                    }
                }
            }

            foreach (Tile sandTile in sandTilesToUpdate) {
                foreach (Tile neighbor in grid.GetNeighbours(sandTile)) {
                    if (neighbor.TileType == TileType.Grass && !sandTilesToUpdate.Contains(neighbor)) {
                        sandGrassTilesToUpdate.Add(neighbor);
                    }
                }
            }

            foreach (Tile tile in sandTilesToUpdate) {
                tile.TileType = TileType.Sand;
            }

            foreach (Tile tile in sandGrassTilesToUpdate) {
                tile.TileType = TileType.SandGrass;
            }

        }

        void DoDeepWater() {
            List<Tile> tilesToUpdate = new List<Tile>();

            for (int x = 1; x < grid.Width - 1; x++) {
                for (int y = 1; y < grid.Height - 1; y++) {
                    Tile tile = grid.Tiles[x, y];
                    if (tile.TileType != TileType.Water) {
                        continue;
                    }

                    if (AllNeighboursMatch(tile)) {
                        tilesToUpdate.Add(tile);
                    }
                }
            }

            foreach (var tile in tilesToUpdate) {
                tile.TileType = TileType.DeepWater;
            }
        }

        bool AllNeighboursMatch(Tile tile) {
            var neighbours = grid.GetNeighbours(tile);
            return neighbours.All(t => t.TileType == tile.TileType);
        }
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

                Vector3 pos = tile.Position;
                if (tile.TileType == TileType.Water || tile.TileType == TileType.DeepWater) {
                    pos.y -= 0.1f;
                }

                Instantiate(prefab, pos, Quaternion.identity, transform);
            }
        }
    }


}
