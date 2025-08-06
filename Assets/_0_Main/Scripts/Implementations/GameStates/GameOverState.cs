// Domain/GameOverState.cs
public class GameOverState : IGameState
{
    readonly GameStateMachine _m;
    public GameOverState(GameStateMachine m) => _m = m;
    public void Enter()
    {
        _m.PlayGameOver(); // plays GameOver sound
    }
    public void HandleFlip(Card c) { }
    public void CheckForMatch() { }
}