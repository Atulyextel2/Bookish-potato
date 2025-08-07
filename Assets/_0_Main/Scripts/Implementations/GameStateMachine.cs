using System;
using System.Collections.Generic;
using System.Diagnostics;

public class GameStateMachine
{
    readonly int _matchGroupSize;
    readonly int _totalGroups;
    int _matchedGroups;

    readonly IAudioService _audio;
    readonly ScoreManager _score;

    readonly List<Card> _currentGroup = new List<Card>();

    public event Action<List<Card>> OnGroupReady;
    public event Action<bool, List<Card>> OnMatchResult;
    public event Action OnGameOver;

    public GameStateMachine(IAudioService audio, ScoreManager score, int matchGroupSize, int totalGroups)
    {
        _audio = audio;
        _score = score;
        _matchGroupSize = matchGroupSize;
        _totalGroups = totalGroups;
        _matchedGroups = 0;
    }

    public void HandleCardFlip(Card card)
    {
        if (card.IsFaceUp) return;
        FlipCard(card);
    }

    private void FlipCard(Card card)
    {
        _audio.Play(SoundType.Flip);
        card.Flip();
    }

    public void HandleFlipCompleted(Card card)
    {
        UnityEngine.Debug.Log("GFSM HandleFlipCompleted " + card);
        _currentGroup.Add(card);
        if (_currentGroup.Count == _matchGroupSize)
        {
            OnGroupReady?.Invoke(new List<Card>(_currentGroup));
        }
    }

    public void HandleMatchResult(bool isMatch, List<Card> group)
    {
        _audio.Play(isMatch ? SoundType.Match : SoundType.Mismatch);
        _score.RecordResult(isMatch);
        OnMatchResult?.Invoke(isMatch, group);
        if (isMatch) _matchedGroups++;
        else
        {
            foreach (Card card in _currentGroup)
            {
                card?.Flip();
            }
        }
        if (_matchedGroups >= _totalGroups)
        {
            _audio.Play(SoundType.GameOver);
            OnGameOver?.Invoke();
        }
    }
}