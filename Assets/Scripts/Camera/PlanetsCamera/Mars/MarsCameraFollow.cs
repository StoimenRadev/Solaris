using UnityEngine;

public class MarsCameraFollow : MonoBehaviour
{
    [Header("Planet to Follow")]
    public Transform target;

    [Header("Camera Settings")]
    public float distance = 150f;   // how far behind the planet
    public float height = 50f;      // how high above the planet
    public float smoothSpeed = 2f;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate offset based on distance + height
        Vector3 offset = (-target.forward * distance) + (Vector3.up * height);

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        transform.LookAt(target);
    }
}
