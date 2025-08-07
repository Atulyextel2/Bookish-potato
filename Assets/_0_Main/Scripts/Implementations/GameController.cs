using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    IInputProvider _input;
    FlipCommandQueue _queue;
    GameStateMachine _fsm;
    float _delay;
    bool _waiting = false;

    public void Initialize(IInputProvider input, FlipCommandQueue queue, GameStateMachine fsm, float flipDelay)
    {
        _input = input;
        _queue = queue;
        _fsm = fsm;
        _delay = flipDelay;

        _input.Enable();
        _input.OnFlipRequest += pos =>
        {
            var hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider?.TryGetComponent<CardView>(out var v) == true)
                _queue.Enqueue(new FlipCommand(v.card));
        };
    }

    void Update()
    {
        if (!_waiting && _queue.HasNext)
        {
            var cmd = _queue.Dequeue();
            cmd.Card.Flip();
            _fsm.OnCardFlipped(cmd.Card);
            if (_fsm.CurrentState is CheckingMatchState)
                _waiting = true;
        }
    }

    public void OnFlipAnimationComplete(Card card)
    {
        if (_fsm.CurrentState is CheckingMatchState)
            StartCoroutine(DelayedMatch());
        else
            _waiting = false;
    }

    IEnumerator DelayedMatch()
    {
        yield return new WaitForSeconds(_delay);
        _fsm.CheckForMatch();
        _waiting = false;
    }
}