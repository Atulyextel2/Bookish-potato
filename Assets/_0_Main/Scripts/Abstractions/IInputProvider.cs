using System;
using UnityEngine;

public interface IInputProvider
{
    event Action<Vector2> OnFlipRequest;
    void Enable(); void Disable();
}
