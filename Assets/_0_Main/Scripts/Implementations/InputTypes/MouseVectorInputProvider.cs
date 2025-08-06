using System;
using UnityEngine;

public class MouseVectorInputProvider : MonoBehaviour, IInputProvider
{
    public event Action<Vector2> OnFlipRequest;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnFlipRequest?.Invoke(worldPos);
        }
    }
    public void Enable()  => enabled = true;
    public void Disable() => enabled = false;
}