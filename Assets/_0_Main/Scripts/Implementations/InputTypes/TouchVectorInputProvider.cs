// 3) Touch (Mobile)
using System;
using UnityEngine;

public class TouchVectorInputProvider : MonoBehaviour, IInputProvider
{
    public event Action<Ray> OnFlipRequest;
    void Update()
    {
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            var touchPos = Input.touches[0].position;
            Ray r = Camera.main.ScreenPointToRay(touchPos);
            OnFlipRequest?.Invoke(r);
        }
    }
    public void Enable() => this.enabled = true;
    public void Disable() => this.enabled = false;
}
