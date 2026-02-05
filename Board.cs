using System;

public class Board
{
    private char[,] _grid = new char[3, 3];

    public Board()
    {
        Reset();
    }

    public void Reset()
    {
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                _grid[r, c] = ' ';
            }
        }
    }

    public bool IsInBounds(int row, int col)
    {
        return row >= 0 && row < 3 && col >= 0 && col < 3;
    }

    public bool IsEmpty(int row, int col)
    {
        return _grid[row, col] == ' ';
    }

    public void Place(int row, int col, char symbol)
    {
        _grid[row, col] = symbol;
    }

    public bool IsFull()
    {
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (_grid[r, c] == ' ')
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool HasWinner(char symbol)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_grid[i, 0] == symbol && _grid[i, 1] == symbol && _grid[i, 2] == symbol)
            {
                return true;
            }

            if (_grid[0, i] == symbol && _grid[1, i] == symbol && _grid[2, i] == symbol)
            {
                return true;
            }
        }

        if (_grid[0, 0] == symbol && _grid[1, 1] == symbol && _grid[2, 2] == symbol)
        {
            return true;
        }

        if (_grid[0, 2] == symbol && _grid[1, 1] == symbol && _grid[2, 0] == symbol)
        {
            return true;
        }

        return false;
    }

    public void Draw()
    {
        Console.WriteLine("   1   2   3");
        Console.WriteLine($"1  {_grid[0, 0]} | {_grid[0, 1]} | {_grid[0, 2]}");
        Console.WriteLine("  ---+---+---");
        Console.WriteLine($"2  {_grid[1, 0]} | {_grid[1, 1]} | {_grid[1, 2]}");
        Console.WriteLine("  ---+---+---");
        Console.WriteLine($"3  {_grid[2, 0]} | {_grid[2, 1]} | {_grid[2, 2]}");
    }
}