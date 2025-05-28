using UnityEngine;

public class Tile : IHeapItem<Tile>
{
    // Grid
    public Vector3 Position;
    public int GridX;
    public int GridY;

    // Pathfinding
    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;
    public bool walkable = true;
    public Tile parent;

    // Heap
    private int heapIndex;
    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    // Tile
    public TileType TileType;

    private GameObject tileObj = null;
    public GameObject TileObj => tileObj;
    public bool isEmpty => tileObj == null;

    public Tile(Vector3 _Position, int _GridX, int _GridY) {
        Position = _Position;
        GridX = _GridX;
        GridY = _GridY;
    }

    public void SetTileObject(GameObject _tileObj) {
        if (!isEmpty) {
            Debug.LogError($"Attempted to place {_tileObj.name} in tile {GridX}, {GridY}. However, {tileObj.name} already exists there. Returning.");
            return;
        }

        tileObj = _tileObj;
    }

    public int CompareTo(Tile other) {
        int compare = fCost.CompareTo(other.fCost);

        if (compare == 0) {
            compare = hCost.CompareTo(other.hCost);
        }

        return -compare;
    }
}

public enum TileType {
    Grass,
    ThickGrass,
    Water,
    DeepWater,
    Sand,
    SandGrass
}
