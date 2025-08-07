using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    IInputProvider _input;
    FlipCommandQueue _queue;
    GameStateMachine _fsm;
    float _delay;
    bool _processing;

    public void Initialize(IInputProvider input, FlipCommandQueue queue, GameStateMachine fsm, float flipDelay)
    {
        _input = input;
        _queue = queue;
        _fsm = fsm;
        _delay = flipDelay;
        _processing = false;

        _input.Enable();
        _input.OnFlipRequest += HandleRay;

        _fsm.OnGroupReady += HandleOnGroupReady;
        _fsm.OnGameOver += () => _input.Disable();
    }

    private void HandleRay(Ray ray)
    {
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity) && hit.collider.TryGetComponent<CardView>(out var view))
        {
            _queue.Enqueue(new FlipCommand(view.card));
        }
    }
    private void HandleOnGroupReady(List<Card> group)
    {
        StartCoroutine(DelayedCheck(group));
    }
    private void Update()
    {
        if (!_processing && _queue.HasNext)
        {
            FlipCommand cmd = _queue.Dequeue();
            _processing = true;
            _fsm.HandleCardFlip(cmd.Card);
        }
    }

    public void OnFlipAnimationComplete(Card card)
    {
        _processing = false;

        if (card.IsFaceUp)
        {
            _fsm.HandleFlipCompleted(card);
        }
    }

    private IEnumerator DelayedCheck(List<Card> group)
    {
        yield return new WaitForSeconds(_delay);
        bool isMatch = group.TrueForAll(c => c.MatchId == group[0].MatchId);
        _fsm.HandleMatchResult(isMatch, group);
    }
}