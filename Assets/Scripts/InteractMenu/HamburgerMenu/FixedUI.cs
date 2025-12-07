using UnityEngine;

[ExecuteAlways]
public class FixedUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 startAnchoredPosition;
    private Quaternion startLocalRotation;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            startAnchoredPosition = rectTransform.anchoredPosition;
            startLocalRotation = rectTransform.localRotation;
        }
        else
        {
            Debug.LogWarning("FixedUI attached to a non-UI object. Consider using the original world position version.");
        }
    }

    void LateUpdate()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = startAnchoredPosition;
            rectTransform.localRotation = startLocalRotation;
        }
        else
        {
            // Fallback for non-UI objects
            transform.position = startAnchoredPosition;
            transform.rotation = startLocalRotation;
        }
    }
}
