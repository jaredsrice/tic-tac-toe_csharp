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
        _players = new Dictionary<string, PlayerStats>(StringComparer.OrdinalIgnoreCase);
    }

    public bool LoadStats()
    {
        _players.Clear();

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

            if (line.StartsWith("Name,", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string[] parts = line.Split(',');

            if (parts.Length != 4)
            {
                continue;
            }

            string playerName = parts[0].Trim();

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
        lines.Add("Name,Wins,Losses,Ties");
        
        foreach (PlayerStats player in _players.Values)
        {
            lines.Add($"{player.Name},{player.Wins},{player.Losses},{player.Ties}");
        }

        File.WriteAllLines(_filePath, lines);
    }

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

    private PlayerStats GetOrCreatePlayer(string name)
    {
        if (_players.TryGetValue(name, out PlayerStats existing))
        {
            return existing;
        }

        PlayerStats created = new PlayerStats(name);
        _players[name] = created;
        return created;
    }
}