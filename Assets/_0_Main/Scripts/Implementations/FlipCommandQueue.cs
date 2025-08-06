using System.Collections.Generic;

public class FlipCommand
{
    public Card Card { get; }
    public FlipCommand(Card c) => Card = c;
}

public class FlipCommandQueue
{
    readonly Queue<FlipCommand> _q = new Queue<FlipCommand>();
    public bool HasNext => _q.Count > 0;
    public void Enqueue(FlipCommand cmd) => _q.Enqueue(cmd);
    public FlipCommand Dequeue() => _q.Dequeue();
}