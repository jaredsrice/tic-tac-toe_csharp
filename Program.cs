using System;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine();
        Console.WriteLine("Welcome to Tic-Tac-Toe!");
        Console.WriteLine();

        // Load saved player data at startup so we can have a persistent leaderboard
        StatManager stats = new StatManager("player_stats.csv");
        bool loaded = stats.LoadStats();

        if (loaded)
        {
            Console.WriteLine("Player statistics loaded successfully.");
        }
        else
        {
            Console.WriteLine("No existing player statistics found. Starting fresh.");
        }

        bool running = true;

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. Start New Game");
            Console.WriteLine("2. View Leaderboard");
            Console.WriteLine("3. Quit");
            Console.WriteLine();
            Console.Write("Select an option (1-3): ");

            string input = (Console.ReadLine() ?? "").Trim();

            if (input == "1")
            {
                Console.WriteLine();

                // Fixed-length player tags match a retro arcade style and keep the leaderboard aligned and consistent. 
                string player1 = PromptPlayerTag("Player 1 tag (3 letters/numbers): ");
                string player2 = PromptPlayerTag("Player 2 tag (3 letters/numbers): ");

                // Prevents same tag for both players (optional)
                while (player2 == player1)
                {
                    Console.WriteLine("Player 2 tag must be different from Player 1 tag.");
                    player2 = PromptPlayerTag("Player 2 tag (3 letters/numbers): ");
                }

                Game game = new Game(player1, player2);
                GameResult result = game.Play();

                // Save stats immediately after each completed game to keep the leaderboard up to date. 
                stats.UpdateStats(player1, player2, result);
                stats.SaveStats();

                Console.WriteLine();
                Console.WriteLine("Stats saved.");
                Pause();
            }
            else if (input == "2")
            {
                Console.WriteLine();
                List<PlayerStats> leaderboard = stats.GetLeaderboard();

                if (leaderboard.Count == 0)
                {
                    Console.WriteLine("No stats yet. Play a game first.");
                }
                else
                {
                    PrintLeaderboard(leaderboard);
                }

                Pause();
            }
            else if (input == "3")
            {
                running = false;
                Console.WriteLine();
                Console.WriteLine("Thanks for playing!");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Invalid option. Please enter 1, 2, or 3.");
                Pause();
            }
        }
    }

    private static string PromptPlayerTag(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            // Trim and force uppercase so tags are stored consistently. 
            string input = (Console.ReadLine() ?? "").Trim().ToUpper();

            if (input.Length != 3)
            {
                Console.WriteLine("Tag must be exactly 3 characters.");
                continue;
            }

            bool allValid = true;

            // Restricts tags to letters and numbers to keep storage and display simple. 
            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    allValid = false;
                    break;
                }
            }

            if (!allValid)
            {
                Console.WriteLine("Only letters and numbers are allowed.");
                continue;
            }

            return input;
        }
    }

    private static void PrintLeaderboard(List<PlayerStats> leaderboard)
    {
        Console.WriteLine("Leaderboard");
        Console.WriteLine("----------------------------");
        Console.WriteLine("RANK TAG  W  L  T");

        int rank = 1;

        foreach (PlayerStats p in leaderboard)
        {
            // Fixed-width formatting to keep columns aligned in the console. 
            Console.WriteLine("{0,4} {1,3} {2,2} {3,2} {4,2}",
                rank,
                p.Name,
                p.Wins,
                p.Losses,
                p.Ties
            );

            rank++;
        }

        Console.WriteLine("----------------------------");
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.Write("Press Enter to continue...");
        Console.ReadLine();
    }
}