using System.Collections.Generic;

public interface ICardDataProvider
{
    IReadOnlyList<CardData> GetCardData();
}