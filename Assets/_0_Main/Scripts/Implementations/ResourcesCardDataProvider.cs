using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesCardDataProvider : ICardDataProvider
{
    private IReadOnlyList<CardData> _allCardDataList;
    public IReadOnlyList<CardData> GetCardData()
    {
        if (_allCardDataList == null)
            _allCardDataList = Resources.LoadAll<CardData>("CardData_Set_1").ToList();
        return _allCardDataList;
    }
}