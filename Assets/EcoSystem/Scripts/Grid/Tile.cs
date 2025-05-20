using UnityEngine;

public class Tile
{
    public Vector3 Position;

    public int GridX;
    public int GridY;

    public TileType TileType;

    public Tile(Vector3 _Position, int _GridX, int _GridY) {
        Position = _Position;
        GridX = _GridX;
        GridY = _GridY;
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
