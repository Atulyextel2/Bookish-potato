using UnityEngine;
using System;

public class CardFactory
{
    public event Action<CardView> OnCardCreated;
    readonly GameObject _cardPrefab;

    // Prefab is injected here
    public CardFactory(GameObject cardPrefab)
    {
        _cardPrefab = cardPrefab;
    }

    public CardView CreateCard(CardData data, Vector2 pos, RectTransform parent)
    {
        var go = UnityEngine.Object.Instantiate(_cardPrefab, parent);
        go.transform.localPosition = pos;

        var model = new Card(data.matchId);
        var view = go.GetComponent<CardView>();
        view.Initialize(model, data.faceSprite, data.backSprite);

        OnCardCreated?.Invoke(view);
        return view;
    }
}