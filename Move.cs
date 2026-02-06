// Represents a single move that consists of a row/column pair. A struct is used
// because a move is a simple value with no behavior beyond storing coordinates,
// which is clearer than passing separate values.
public struct Move
{
    public int Row { get; }
    public int Col { get; }

    public Move(int row, int col)
    {
        Row = row;
        Col = col;
    }
}