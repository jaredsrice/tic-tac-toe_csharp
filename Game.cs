using System;

public class Game
{
    private readonly Board _board;
    private readonly string _player1;
    private readonly string _player2;

    private string _currentPlayer;
    private char _currentSymbol;

    public Game(string player1, string player2)
    {
        _player1 = player1;
        _player2 = player2;

        _board = new Board();

        _currentPlayer = _player1;
        _currentSymbol = 'X';
    }

    public GameResult Play()
    {
        _board.Reset();

        while (true)
        {
            Console.WriteLine();
            _board.Draw();
            Console.WriteLine();

            Console.WriteLine($"{_currentPlayer}'s turn ({_currentSymbol})");
            Console.WriteLine("Enter row and column from 1 to 3.");

            Move move = GetValidMove();
            _board.Place(move.Row, move.Col, _currentSymbol);

            if (_board.HasWinner(_currentSymbol))
            {
                Console.WriteLine();
                _board.Draw();
                Console.WriteLine();
                Console.WriteLine($"{_currentPlayer} wins!");

                if (_currentSymbol == 'X')
                {
                    return GameResult.Player1Win;
                }
                else
                {
                    return GameResult.Player2Win;
                }
            }

            if (_board.IsFull())
            {
                Console.WriteLine();
                _board.Draw();
                Console.WriteLine();
                Console.WriteLine("It's a tie!");
                return GameResult.Tie;
            }

            SwitchTurn();
        }
    }

    private Move GetValidMove()
    {
        while (true)
        {
            int row = PromptInt("Row (1-3): ");
            int col = PromptInt("Col (1-3): ");

            row -= 1;
            col -= 1;

            if (!_board.IsInBounds(row, col))
            {
                Console.WriteLine("Out of bounds. Try again.");
                continue;
            }

            if (!_board.IsEmpty(row, col))
            {
                Console.WriteLine("That spot is taken. Try again.");
                continue;
            }

            return new Move(row, col);
        }
    }

    private int PromptInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int value))
            {
                return value;
            }

            Console.WriteLine("Invalid input. Enter a number.");
        }
    }

    private void SwitchTurn()
    {
        if (_currentSymbol == 'X')
        {
            _currentSymbol = 'O';
            _currentPlayer = _player2;
        }
        else
        {
            _currentSymbol = 'X';
            _currentPlayer = _player1;
        }
    }
}