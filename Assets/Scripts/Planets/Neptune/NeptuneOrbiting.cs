using UnityEngine;

public class NeptuneOrbiting : MonoBehaviour
{
    [Header("Orbit Reference")]
    public OrbitPathNeptune orbitPath;

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