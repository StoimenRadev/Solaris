using UnityEngine;

public class EarthRotation : MonoBehaviour
{
    [Header("Rotation Speed (degrees per second)")]
    public float rotationSpeed = 0.0001f;

    void Start()
    {
        // Earth's axial tilt (23.26°)
        transform.rotation = Quaternion.Euler(23.26f, 0f, 0f);
    }

    void Update()
    {
        // Prograde rotation = COUNTERCLOCKWISE when viewed from north
        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime / 5, Space.Self);
    }
}
