using System;

public class ScoreManager
{
    public int Matches { get; private set; }
    public int Tries { get; private set; }
    public event Action<int, int> OnScoreChanged;
    public event Action<string> OnLeaderBoardChanged;

    public void SetInitial(int matches, int tries)
    {
        Matches = matches;
        Tries = tries;
        //OnScoreChanged?.Invoke(Matches, Tries);
    }

    public void SetUpLeaderBoard(GameProgress gameProgress)
    {
        if (gameProgress != null)
        {

        }
    }

    public void RecordResult(bool isMatch)
    {
        Tries++;
        if (isMatch) Matches++;
        OnScoreChanged?.Invoke(Matches, Tries);
    }
}