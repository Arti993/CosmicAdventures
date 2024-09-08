public class LeaderboardPlayer
{
    public LeaderboardPlayer(string name, int score, int rank)
    {
        Name = name;
        Score = score;
        Rank = rank;
    }
    
    public string Name { get; private set; }
    public int Score { get; private set; }
    public int Rank { get; private set; }
}
