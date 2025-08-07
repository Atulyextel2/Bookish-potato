// Presentation/AnimationManager.cs
using DG.Tweening;

public class AnimationManager
{
    public AnimationManager(CardFactory factory, GameController ctrl)
    {
        factory.OnCardCreated += view =>
            view.card.OnFlipped += _ => AnimateFlip(view, ctrl);
    }

    void AnimateFlip(CardView view, GameController ctrl)
    {
        ctrl.OnFlipAnimationComplete(view.card);
    }
}