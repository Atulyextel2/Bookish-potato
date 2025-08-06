using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    private Card card;
    [SerializeField] private Image cardFrontImage, cardBackImage;

    public void Initialize(Card card, Sprite face, Sprite back)
    {
        if (card != null)
        {
            this.card = card;
        }

        if (cardFrontImage != null)
        {
            if (face != null)
            {
                cardFrontImage.sprite = face;
            }
        }

        if (cardBackImage != null)
        {
            if (back != null)
            {
                cardBackImage.sprite = back;
            }
        }
    }
}