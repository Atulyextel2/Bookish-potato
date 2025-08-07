public class WaitingForFlipState : IGameState
{
    // readonly GameStateMachine _m;
    // public WaitingForFlipState(GameStateMachine m) => _m = m;
    // public void Enter() => _m.ClearSelection();
    // public void HandleFlip(Card card)
    // {
    //     if (card.IsFaceUp) return;
    //     card.Flip();
    //     _m.AddSelection(card);
    //     if (_m.IsSelectionComplete())
    //         _m.TransitionTo(new CheckingMatchState(_m));
    // }
    // public void CheckForMatch() { }
}