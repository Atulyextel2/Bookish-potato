// 3) Touch (Mobile)
using System;
using UnityEngine;

public class TouchVectorInputProvider : MonoBehaviour, IInputProvider
{
    public event Action<Vector2> OnFlipRequest;
    void Update()
    {
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            var worldPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            OnFlipRequest?.Invoke(worldPos);
        }
    }
    public void Enable()  => enabled = true;
    public void Disable() => enabled = false;
}
