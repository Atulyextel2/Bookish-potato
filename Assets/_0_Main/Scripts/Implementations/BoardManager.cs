// BoardManager.cs
using UnityEngine;
using System.Linq;

public class BoardManager
{
    readonly ILayoutStrategy _layout;
    readonly CardFactory _factory;
    readonly ICardDataProvider _dp;

    public BoardManager(ICardDataProvider dp, CardFactory factory, ILayoutStrategy layout)
    {
        _layout = layout;
        _factory = factory;
        _dp = dp;
    }

    public void SetupBoard(int rows, int cols, RectTransform container)
    {
        var cells = _layout.CalculateCells(rows, cols, container);
        var allCards = _dp.GetCardData();
        var deck = allCards.OrderBy(_ => Random.value).Take(rows * cols).ToList();

        for (int i = 0; i < deck.Count; i++)
        {
            var r = cells[i];
            var cardView = _factory.CreateCard(deck[i], r.position, container);
            cardView.GetComponent<RectTransform>().sizeDelta = r.size;
        }
    }
}