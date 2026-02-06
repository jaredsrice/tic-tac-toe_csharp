public class PlayerStats
{
    public string Name { get; private set; }

    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int Ties { get; private set; }

    // Derived from existing stats instead of being stored separately. 
    public int GamesPlayed
    {
        get
        {
            return Wins + Losses + Ties;
        }
    }

    // WinRate is calculated on demand to ensure it reflects the most current stats. Also guards against division by zero.
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


    // Methods used to update stats while preventing direct modification from outside the class.
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