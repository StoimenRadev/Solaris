using UnityEngine;

public class VenusOrbiting : MonoBehaviour
{
    [Header("Orbit Reference")]
    public OrbitPathVenus orbitPath;

    void Start()
    {
        if (!orbitPath)
        {
            Debug.LogError("OrbitPathEarth not assigned!");
            return;
        }

        float day = orbitPath.manualDayOfYear;
        if (orbitPath.timeDisplay != null)
            day = orbitPath.timeDisplay.DayOfYear;

        orbitPath.UpdatePlanetPosition(day);
    }

    void Update()
    {
        if (!orbitPath) return;

        float day = orbitPath.manualDayOfYear;
        if (orbitPath.timeDisplay != null)
            day = orbitPath.timeDisplay.DayOfYear;

        orbitPath.UpdatePlanetPosition(day);
    }
}