using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    IInputProvider _input;
    FlipCommandQueue _queue;
    GameStateMachine _fsm;
    float _delay;
    bool _waiting = false;
    IAudioService _audio;


    public void Initialize(IInputProvider input, FlipCommandQueue queue, GameStateMachine fsm, float flipDelay, IAudioService audio)
    {
        _input = input;
        _queue = queue;
        _fsm = fsm;
        _delay = flipDelay;
        _audio = audio;

        _input.Enable();
        _input.OnFlipRequest += HandleRay;

        _fsm.OnMatchChecked += (isMatch, cards) =>
        {
            Debug.Log("!@#  _fsm.OnMatchChecked " + isMatch);
            if (!isMatch)
            {
                _audio.Play(SoundType.Mismatch);
                StartCoroutine(FlipBack(cards));
            }
            else
            {
                _audio.Play(SoundType.Match);
            }
        };
        //_fsm.GameOver += () => _input.Disable();
    }

    void HandleRay(Ray ray)
    {

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity) && hit.collider.TryGetComponent<CardView>(out var view))
        {
            _queue.Enqueue(new FlipCommand(view.card));
        }
    }

    void Update()
    {
        if (!_waiting && _queue.HasNext)
        {
            var cmd = _queue.Dequeue();
            _audio.Play(SoundType.Flip);
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

    private IEnumerator DelayedMatch()
    {
        yield return new WaitForSeconds(_delay);
        _fsm.CheckForMatch();
        _waiting = false;
    }

    private IEnumerator FlipBack(List<Card> cards)
    {
        yield return new WaitForSeconds(_delay);

        foreach (var c in cards)
        {
            _audio.Play(SoundType.Flip);
            c.Flip();
        }
    }
}