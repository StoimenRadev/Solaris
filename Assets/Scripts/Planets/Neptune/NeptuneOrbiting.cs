using UnityEngine;

public class NeptuneOrbiting : MonoBehaviour
{
    [Header("Orbit Reference")]
    public OrbitPathNeptune orbitPath;

    [Header("Orbiting Speed")]
    public float orbitSpeed = 0.5f; // radians per second

    private float theta = 0f; // current angle along orbit

    void Start()
    {
        if (!orbitPath)
        {
            Debug.LogError("OrbitPath not assigned!");
            return;
        }

        // Automatically place planet at perihelion (closest point)
        theta = 0f; // theta = 0 corresponds to perihelion
        UpdatePosition();
    }

    void Update()
    {
        if (!orbitPath) return;

        // Advance theta
        theta += orbitSpeed * Time.deltaTime;
        theta %= 2f * Mathf.PI;

        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (!orbitPath) return;

        // Planet position in XZ plane
        float x = orbitPath.a * Mathf.Cos(theta);
        float z = orbitPath.b * Mathf.Sin(theta);

        transform.position = new Vector3(x, 0f, z);
    }
}
