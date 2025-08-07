using System;
using UnityEngine;

public interface IInputProvider
{
    event Action<Ray> OnFlipRequest;
    void Enable(); void Disable();
}
