using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [Header("Camera Transform (set in scene)")]
    public Transform startTransform;

    private void Start()
    {
        if (startTransform != null)
        {
            // Set camera exactly where you place it in the editor
            transform.position = startTransform.position;
            transform.rotation = startTransform.rotation;
        }
    }

    private void Update()
    {
        // Camera is fully static — nothing happens here
    }
}
