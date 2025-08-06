using System.Collections.Generic;
using UnityEngine;

public class ResourcesCardDataProvider : ICardDataProvider
{
    public IReadOnlyList<CardData> AllCards { get; }
    public ResourcesCardDataProvider()
    {
        AllCards = Resources.LoadAll<CardData>("CardData");
    }
}