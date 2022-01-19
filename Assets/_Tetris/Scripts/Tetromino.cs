using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I, J, L, O, S, T, Z
}

[System.Serializable]
public struct TetrominoData
{
    public Tile tile;
    public Tetromino tetromino;

    public Vector2Int[] cells { get; set; }
    public Vector2Int[,] wallKicks { get; set; }

    public void Initialize()
    {
        this.cells = Data.Cells[this.tetromino];
        this.wallKicks = Data.WallKicks[this.tetromino];
    }

}
