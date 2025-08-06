using System.Collections.Generic;

public class CardViewRegistry
{
    readonly List<CardView> _all = new List<CardView>();
    public IEnumerable<CardView> All => _all;
    public void Register(CardView v) => _all.Add(v);
}