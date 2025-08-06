using System.Collections.Generic;
using System.Linq;

public class GameStateMachine
{
    IGameState _current;
    readonly List<Card> _selection = new List<Card>();
    readonly IAudioService _audio;
    readonly ScoreManager _score;
    readonly int _matchGroupSize;
    readonly int _totalGroups;
    int _matchedGroups;

    public IGameState CurrentState => _current;

    public GameStateMachine(
        IAudioService audio,
        ScoreManager score,
        int matchGroupSize,
        int totalGroups)
    {
        _audio = audio;
        _score = score;
        _matchGroupSize = matchGroupSize;
        _totalGroups = totalGroups;
        TransitionTo(new WaitingForFlipState(this));
    }

    public void OnCardFlipped(Card c) => _current.HandleFlip(c);
    public void CheckForMatch() => _current.CheckForMatch();

    public void TransitionTo(IGameState next)
    {
        _current = next;
        _current.Enter();
    }

    // Selection helpers
    internal void AddSelection(Card c) => _selection.Add(c);
    internal bool IsSelectionComplete() => _selection.Count == _matchGroupSize;
    internal bool IsMatch() => _selection.All(c => c.MatchId == _selection[0].MatchId);
    internal void ClearSelection() => _selection.Clear();

    // Called by CheckingMatchState
    internal void RecordResult(bool isMatch)
    {
        _score.RecordResult(isMatch);
        if (isMatch) _matchedGroups++;

        if (_matchedGroups >= _totalGroups)
            TransitionTo(new GameOverState(this));
        else
            TransitionTo(new WaitingForFlipState(this));
    }

    internal void PlayGameOver()
    {
        _audio.Play(SoundType.GameOver);
    }
}