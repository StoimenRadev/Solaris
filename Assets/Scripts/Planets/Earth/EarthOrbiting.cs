using UnityEngine;

public class EarthOrbiting : MonoBehaviour
{
    [Header("Orbit Reference")]
    public OrbitPathEarth orbitPath;

    [Header("Orbiting Speed")]
    public float orbitSpeed = 1.99e-7f; // radians per second

    private float theta = 0f; // current angle along orbit

    void Start()
    {
        if (!orbitPath)
        {
            Debug.LogError("OrbitPath not assigned!");
            return;
        }

        // Automatically place planet at perihelion
        theta = 0f;
        UpdatePosition();
    }

    void Update()
    {
        if (!orbitPath) return;
        float day = orbitPath.manualDayOfYear;
        if (orbitPath.timeDisplay != null)
            day = orbitPath.timeDisplay.DayOfYear;

        orbitPath.UpdatePlanetPosition(day);
    }

    void UpdatePosition()
    {
        if (!orbitPath) return;

        transform.position = orbitPath.GetPosition(theta);
    }
}
