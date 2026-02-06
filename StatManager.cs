using System;
using System.Collections.Generic;
using System.IO;

public class StatManager
{
    private string _filePath;
    private Dictionary<string, PlayerStats> _players;

    public StatManager(string filePath)
    {
        _filePath = filePath;
        // Use a case-insensitive dictionary so player tags are treated the same regardless of case.
        _players = new Dictionary<string, PlayerStats>(StringComparer.OrdinalIgnoreCase);
    }

    public bool LoadStats()
    {
        // Clear existing data before loading to prevent duplicates if LoadStats is called again. 
        _players.Clear();

        // If file doesn't exist yet, return false so the program can start with an empty leaderboard. 
        if (!File.Exists(_filePath))
        {
            return false;
        }

        string[] lines = File.ReadAllLines(_filePath);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            // Skips header line if it exists so it won't parse as player data. 
            if (line.StartsWith("Name,", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string[] parts = line.Split(',');

            // Skips malformed lines that don't have expected formatting. Prevents crashes from bad data. 
            if (parts.Length != 4)
            {
                continue;
            }

            string playerName = parts[0].Trim();

            // Use TryParse here to ensure invalid numeric data doesn't cause a crash. Parsing fail = line skipped. 
            if (!int.TryParse(parts[1].Trim(), out int wins))
            {
                continue;
            }

            if (!int.TryParse(parts[2].Trim(), out int losses))
            {
                continue;
            }

            if (!int.TryParse(parts[3].Trim(), out int ties))
            {
                continue;
            }

            PlayerStats stats = new PlayerStats(playerName, wins, losses, ties);
            _players[playerName] = stats;
        }

        return true;
    }

    public void SaveStats()
    {
        List<string> lines = new List<string>();

        // Writes a header to keep the CSV readable and consistent. 
        lines.Add("Name,Wins,Losses,Ties");
        
        foreach (PlayerStats player in _players.Values)
        {
            lines.Add($"{player.Name},{player.Wins},{player.Losses},{player.Ties}");
        }

        File.WriteAllLines(_filePath, lines);
    }

    // Updates both player stats based on game result. 
    public void UpdateStats(string player1, string player2, GameResult result)
    {
        PlayerStats p1 = GetOrCreatePlayer(player1);
        PlayerStats p2 = GetOrCreatePlayer(player2);

        if (result == GameResult.Player1Win)
        {
            p1.RecordWin();
            p2.RecordLoss();
        }
        else if (result == GameResult.Player2Win)
        {
            p2.RecordWin();
            p1.RecordLoss();
        }
        else
        {
            p1.RecordTie();
            p2.RecordTie();
        }
    }

    public List<PlayerStats> GetLeaderboard()
    {
        List<PlayerStats> leaderboard = new List<PlayerStats>(_players.Values);

        // Sorts by win rate first, then wins, then by name for stable ordering. 
        leaderboard.Sort((a, b) =>
        {
            int winRateCompare = b.WinRate.CompareTo(a.WinRate);
            if (winRateCompare != 0)
            {
                return winRateCompare;
            }

            int winsCompare = b.Wins.CompareTo(a.Wins);
            if (winsCompare != 0)
            {
                return winsCompare;
            }

            return string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
        });

        return leaderboard;
    }

    // Helper method to retrieve an existing player record or create a new one if a tag doesn't exist. 
    private PlayerStats GetOrCreatePlayer(string name)
    {
        if (_players.ContainsKey(name))
        {
            return _players[name];
        }

        PlayerStats created = new PlayerStats(name);
        _players[name] = created;
        return created;
    }
}