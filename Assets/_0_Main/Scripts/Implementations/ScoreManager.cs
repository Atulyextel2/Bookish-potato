using System;

public class ScoreManager
{
    public int Matches { get; private set; }
    public int Tries { get; private set; }
    public event Action<int, int> OnScoreChanged;

    public void SetInitial(int matches, int tries)
    {
        Matches = matches;
        Tries = tries;
        //OnScoreChanged?.Invoke(Matches, Tries);
    }

    public void RecordResult(bool isMatch)
    {
        Tries++;
        if (isMatch) Matches++;
        OnScoreChanged?.Invoke(Matches, Tries);
    }
}