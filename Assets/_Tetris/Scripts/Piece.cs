using UnityEngine;
using System;

public class Piece : MonoBehaviour
{
    public Board board { get; set; }
    public TetrominoData data { get; set; }
    public Vector3Int[] cells { get; set; }
    public Vector3Int position { get; set; }
    public int rotationIndex { get; set; }

    public float stepDelay = 1f;
    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float moveTime;
    private float lockTime;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.data = data;
        this.board = board;
        this.position = position;
        this.rotationIndex = 0;

        this.stepTime = Time.time + this.stepDelay;
        this.moveTime = Time.time + this.moveDelay;
        this.lockTime = 0f;

        if (this.cells == null) {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < this.cells.Length; i++) {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    // Update is called once per frame
    private void Update()
    {
        this.board.Clear(this);
        this.lockTime += Time.deltaTime;

        if (Time.time > this.stepTime) {
            Step();
        }

        if (EventManager.current.actionLeft)
        {
            Move(Vector2Int.left);
            EventManager.current.actionLeft = false;
        }

        if (EventManager.current.actionRight)
        {
            Move(Vector2Int.right);
            EventManager.current.actionRight = false;
        }

        if (EventManager.current.actionRotate)
        {
            Rotate(1);
            EventManager.current.actionRotate = false;
        }

        if (EventManager.current.actionHardDrop)
        {
            HardDrop();
            EventManager.current.actionHardDrop = false;
        }

        this.board.Set(this);
    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        Move(Vector2Int.down);

        if (this.lockTime >= this.lockDelay) {
            Lock();
        }
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down)) {
            continue;
        }

        Lock();
    }

    private void Lock()
    {
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        if (valid)
        {
            this.position = newPosition;
            this.moveTime = Time.time + this.moveDelay;
            this.lockTime = 0f; // reset
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex;

        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = Data.RotationMatrix;

        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    // "I" and "O" are rotated from an offset center point
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

            if (Move(translation)) {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0) {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min) {
            return max - (min - input) % (max - min);
        } else {
            return min + (input - min) % (max - min);
        }
    }

}
