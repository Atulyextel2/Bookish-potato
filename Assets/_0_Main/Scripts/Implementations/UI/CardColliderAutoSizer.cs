using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(BoxCollider))]
public class RectTransformColliderFitter : MonoBehaviour
{
    [Tooltip("Depth of the collider along the Z axis (front-to-back).")]
    [SerializeField] private float colliderDepth = 0.1f;

    void Awake()
    {
        FitCollider();
    }

#if UNITY_EDITOR
    // In the Editor, update live when you tweak RectTransforms
    void OnValidate()
    {
        FitCollider();
    }
#endif

    private void FitCollider()
    {
        var rt = GetComponent<RectTransform>();
        var bc = GetComponent<BoxCollider>();
        if (rt == null || bc == null) return;

        // rt.rect is in *local* space, centered on pivot
        Rect r = rt.rect;

        // Match width & height exactly, leave z as a thin slab
        bc.size = new Vector3(r.width, r.height, colliderDepth);
        bc.center = new Vector3(r.center.x, r.center.y, bc.center.z);
    }
}