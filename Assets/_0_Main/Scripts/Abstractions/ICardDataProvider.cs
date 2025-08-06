using System.Collections.Generic;

public interface ICardDataProvider
{
    IReadOnlyList<CardData> AllCards { get; }
}