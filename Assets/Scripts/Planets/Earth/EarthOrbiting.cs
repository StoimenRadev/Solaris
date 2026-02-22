using UnityEngine;

public class EarthOrbiting : MonoBehaviour
{
    [Header("Orbit Reference")]
    public OrbitPathEarth orbitPath;

    [Header("Start Offset")]
    [Tooltip("Day offset to start planet at a different position in orbit")]
    [Range(0f, 365f)]
    public float startDayOffset = 0f;

    private Transform planet; // the planet we control

    void Start()
    {
        if (!orbitPath)
        {
            Debug.LogError("OrbitPath not assigned!");
            return;
        }

        planet = orbitPath.GetPlanetInstance();
    }

    void Update()
    {
        if (!orbitPath || planet == null) return;

        // Current day of year + offset
        float day = orbitPath.manualDayOfYear + startDayOffset;

        // Wrap around year
        day %= orbitPath.planetYearLength;

        // Update planet position along orbit
        orbitPath.UpdatePlanetPosition(day);
    }
}