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
        view.transform
            .DORotate(new UnityEngine.Vector3(0, 180, 0), 0.3f)
            .OnComplete(() => ctrl.OnFlipAnimationComplete(view.card));
    }
}