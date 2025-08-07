// BoardManager.cs
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class BoardManager
{
    readonly ILayoutStrategy _layout;
    readonly CardFactory _factory;
    readonly ICardDataProvider _dp;
    readonly int _matchGroupSize;

    public BoardManager(ICardDataProvider dp, CardFactory factory, ILayoutStrategy layout, int matchGroupSize)
    {
        _layout = layout;
        _factory = factory;
        _dp = dp;
        _matchGroupSize = matchGroupSize;
    }

    public void SetupBoard(int rows, int cols, RectTransform container)
    {
        int totalCells = rows * cols;

        if (totalCells % _matchGroupSize != 0)
            Debug.LogError($"Grid {rows}×{cols} = {totalCells} cells, " +
                $"not divisible by matchGroupSize {_matchGroupSize}.");

        int groupCount = totalCells / _matchGroupSize;

        var allData = _dp.GetCardData().ToList();
        if (allData.Count == 0)
            Debug.LogError("No CardData available in provider.");

        var shuffledData = allData
            .OrderBy(_ => UnityEngine.Random.value)
            .ToList();

        var selected = new List<CardData>(groupCount);
        for (int i = 0; selected.Count < groupCount; i++)
        {
            selected.Add(shuffledData[i % shuffledData.Count]);
        }

        var deck = selected
            .SelectMany(data => Enumerable.Repeat(data, _matchGroupSize))
            .OrderBy(_ => UnityEngine.Random.value)
            .ToList();

        Rect[] cells = _layout.CalculateCells(rows, cols, container);
        for (int i = 0; i < deck.Count; i++)
        {
            var data = deck[i];
            var cell = cells[i];

            var view = _factory.CreateCard(data, Vector2.zero, container);
            var rt = view.GetComponent<RectTransform>();

            rt.SetParent(container, worldPositionStays: false);

            rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);

            rt.anchoredPosition = new Vector2(cell.x, cell.y);
            rt.sizeDelta = new Vector2(cell.width, cell.height);
        }
    }
}