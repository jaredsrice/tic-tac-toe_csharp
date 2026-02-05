public class PlayerStats
{
    public string Name { get; private set; }

    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int Ties { get; private set; }

    public int GamesPlayed
    {
        get
        {
            return Wins + Losses + Ties;
        }
    }

    public double WinRate
    {
        get
        {
            if (GamesPlayed == 0)
            {
                return 0.0;
            }

            return (double)Wins / GamesPlayed;
        }
    }

    public PlayerStats(string name)
    {
        Name = name;
        Wins = 0;
        Losses = 0;
        Ties = 0;
    }

    public PlayerStats(string name, int wins, int losses, int ties)
    {
        Name = name;
        Wins = wins;
        Losses = losses;
        Ties = ties;
    }

    public void RecordWin()
    {
        Wins++;
    }

    public void RecordLoss()
    {
        Losses++;
    }

    public void RecordTie()
    {
        Ties++;
    }
}