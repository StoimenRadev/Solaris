using UnityEngine;

public class FixedUI : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        // Store the initial position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Reset position and rotation every frame
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
