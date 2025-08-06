public class CardTile : Tile, IFlippable, IMatchable
{
    public string MatchId { get; }
    public bool IsFaceUp { get; private set; }
    public event Action<Card> OnFlipped;

    public Card(string matchId) => MatchId = matchId;
    public void Flip() 
    {
        IsFaceUp = !IsFaceUp;
        OnFlipped?.Invoke(this);
    }
}