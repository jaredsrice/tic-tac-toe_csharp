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
        // Resets the board at the start of each match. Ensures a clean board. 
        _board.Reset();

        while (true)
        {
            Console.WriteLine();
            _board.Draw();
            Console.WriteLine();

            Console.WriteLine($"{_currentPlayer}'s turn ({_currentSymbol})");
            Console.WriteLine("Enter row and column from 1 to 3.");

            Move move = GetValidMove();

            // Applies validated move to the board. 
            _board.Place(move.Row, move.Col, _currentSymbol);

            // Checks for a win or tie immediately after a move, before switching turns. Ensures correct game stats for players. 
            if (_board.HasWinner(_currentSymbol))
            {
                Console.WriteLine();
                _board.Draw();
                Console.WriteLine();
                Console.WriteLine($"{_currentPlayer} wins!");

                // Symbols are attached to Player 1 and Player 2 in the constructor, so the current symbol determines the game result. 
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

            // Converts user input (1-3) into 0-2 so it matches the board array. 
            row -= 1;
            col -= 1;

            // Validation loop to keep prompting until a player selects an in-bounds empty position. 
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

            // Reads input as a string first, then attempts to parse into an int so invalid entries don't cause a crash.
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int value))
            {
                return value;
            }

            Console.WriteLine("Invalid input. Enter a number.");
        }
    }

    // Switches turns by updating both the player and symbol together to keep in sync.  
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