using System;
using UnityEngine;

public class MouseVectorInputProvider : MonoBehaviour, IInputProvider
{
    public event Action<Ray> OnFlipRequest;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            OnFlipRequest?.Invoke(r);
        }
    }
    public void Enable() => this.enabled = true;
    public void Disable() => this.enabled = false;
}