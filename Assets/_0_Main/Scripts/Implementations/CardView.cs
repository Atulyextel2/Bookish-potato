using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public Card card { get; private set; }
    [SerializeField] private Image cardImage;

    public void Initialize(Card card, Sprite face, Sprite back)
    {
        if (card != null)
        {
            this.card = card;
            card.OnFlipped += c =>
            {
                cardImage.sprite = c.IsFaceUp ? face : back;
            };
            cardImage.sprite = back;
        }
    }
}