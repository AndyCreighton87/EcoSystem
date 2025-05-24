using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Terrain Generation")]
    [SerializeField] private List<TileVisual> tileVisuals;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private GameObject bushPrefab;
    [SerializeField, Range(0, 1.0f)] private float noiseScale = 0.1f;
    [SerializeField, Range(0, 1.0f)] private float waterThreshold = 0.4f;
    [SerializeField, Range(0, 1.0f)] private float treeSpawnChance = 0.2f;
    [SerializeField, Range(0, 1.0f)] private float bushSpawnChance = 0.2f; 

    [Header("Animation")]
    [SerializeField] private float nextTileDelay = 0.1f;
    [SerializeField] private float tileDropDuration = 0.1f;
    [SerializeField] private float tileStartingOffset = 1.0f;

    private List<GameObject> spawnedTiles = new List<GameObject>();
    private List<GameObject> spawnedProps = new List<GameObject>();

    private void Start() {
        GenerateTerrain();
        SpawnProps();

        StartAnimation();
    }

    #region TILES

    private void GenerateTerrain() {
        Grid grid = Grid.Instance;

        if (grid == null) {
            Debug.LogError($"Attempting to generate terrain, but Grid is null. Something has gone terribly, terribly wrong");
            return;
        }

        CreateTileFoundation(grid);
        DoTileVariation(grid);
        SpawnTiles(grid);
    }

    private void CreateTileFoundation(Grid grid) {
        int maxTries = 10;
        int attempt = 0;
        int minConnectedWaterTiles = 15;

        do {
            float noiseOffsetX = Random.Range(0f, 10f);
            float noiseOffsetY = Random.Range(0f, 10f);

            int waterCount = 0;

            for (int x = 0; x < grid.Width; x++) {
                for (int y = 0; y < grid.Height; y++) {
                    float noise = Mathf.PerlinNoise((x * noiseScale) + noiseOffsetX, (y * noiseScale) + noiseOffsetY);
                    var tileType = noise < waterThreshold ? TileType.Water : TileType.Grass;
                    grid.Tiles[x, y].TileType = tileType;

                    if (tileType == TileType.Water) waterCount++;
                }
            }

            attempt++;

        } while (CountConnectedWaterTiles(grid) < minConnectedWaterTiles && attempt < maxTries);
    }

    private void DoTileVariation(Grid grid) {
        DoSand();
        DoDeepWater();

        void DoSand() {
            List<Tile> sandTilesToUpdate = new List<Tile>();
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

            foreach (Tile tile in sandTilesToUpdate) {
                tile.TileType = TileType.Sand;
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

    private int CountConnectedWaterTiles(Grid grid) {
        int width = grid.Width;
        int height = grid.Height;
        bool[,] visited = new bool[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (grid.Tiles[x, y].TileType == TileType.Water) {
                    return FloodFill(grid, x, y, visited);
                }
            }
        }

        return 0;
    }

    private int FloodFill(Grid grid, int startX, int startY, bool[,] visited) {
        int count = 0;
        int width = grid.Width;
        int height = grid.Height;

        Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
        queue.Enqueue((startX, startY));
        visited[startX, startY] = true;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        while (queue.Count > 0) {
            var (x, y) = queue.Dequeue();
            count++;

            for (int i = 0; i < 4; i++) {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (nx >= 0 && nx < width && ny >= 0 && ny < height) {
                    if (!visited[nx, ny] && grid.Tiles[nx, ny].TileType == TileType.Water) {
                        visited[nx, ny] = true;
                        queue.Enqueue((nx, ny));
                    }
                }
            }
        }

        return count;
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

                TileVisual visual = Instantiate(prefab, pos, Quaternion.identity, transform);
                visual.gameObject.SetActive(false);
                spawnedTiles.Add(visual.gameObject);
            }
        }
    }

    #endregion

    #region PROPS

    private void SpawnProps() {
        Grid grid = Grid.Instance;
        List<Tile> emptyTiles;

        RefreshEmptyTiles();

        if (emptyTiles.Count <= 0) {
            Debug.LogWarning($"Attempting to spawn props, however all tiles are filled. Returning, no props will be spawned.");
            return;
        }

        DoBushes();
        DoTrees();

        void DoBushes() {
            foreach (Tile tile in emptyTiles) {
                if (tile.TileType == TileType.Grass && Random.value < bushSpawnChance) {
                    SpawnProp(tile, bushPrefab);
                }
            }

            RefreshEmptyTiles();
        }

        void DoTrees() {
            foreach (Tile tile in emptyTiles) {
                if (tile.TileType == TileType.Grass && Random.value < treeSpawnChance) {
                    SpawnProp(tile, treePrefab);
                }
            }

            RefreshEmptyTiles();
        }

        void RefreshEmptyTiles() {
            emptyTiles = grid.GetAllEmptyTiles();
        }
    }

    private void SpawnProp(Tile tile, GameObject prefab) {
        GameObject prop = Instantiate(prefab, tile.Position, Quaternion.identity, transform);
        prop.SetActive(false);
        spawnedProps.Add(prop);
        tile.SetTileObject(prop);
    }

    #endregion

    #region ANIMATION
    private void StartAnimation() {
        StartCoroutine(StartAnimation_Coroutine());
    }

    private IEnumerator StartAnimation_Coroutine() {
        foreach (var tile in spawnedTiles.Shuffled()) {
            DoAnimation(tile);
            yield return new WaitForSeconds(nextTileDelay);
        }

        foreach (var tree in spawnedProps.Shuffled()) {
            DoAnimation(tree);
            yield return new WaitForSeconds(nextTileDelay);
        }
    }

    private void DoAnimation(GameObject obj) {
        obj.SetActive(true);

        Transform tileTrans = obj.transform;
        Vector3 tilePos = tileTrans.position;

        Vector3 newPos = new Vector3(tilePos.x, tilePos.y + tileStartingOffset, tilePos.z);
        tileTrans.position = newPos;

        obj.transform.DOMoveY(tilePos.y, tileDropDuration).SetEase(Ease.OutBounce);
    }

    #endregion

    public enum PropType {
        Tree,
        Bush
    }
}
