using UnityEngine;
using System;

public class SimulationTimeController : MonoBehaviour
{
    [Header("Simulation Settings")]
    public bool isPaused = false;
    public float timeRate = 1f; // seconds simulated per real second

    private DateTime realStartTime;       // system start time
    private DateTime simulatedStartTime;  // simulation start time

    [HideInInspector]
    public DateTime simulatedTime;        // current simulated time

    private void Awake()
    {
        realStartTime = DateTime.Now;
        simulatedStartTime = realStartTime; // start from real current time
        simulatedTime = simulatedStartTime;
    }

    private void Update()
    {
        if (isPaused) return;

        // Real seconds elapsed scaled by timeRate
        float delta = Time.unscaledDeltaTime * timeRate;

        // Advance simulated time
        simulatedTime = simulatedTime.AddSeconds(delta);

        // Update all connected TimeDisplays
        TimeDisplay[] timeDisplays = FindObjectsOfType<TimeDisplay>();
        foreach (var td in timeDisplays)
        {
            if (td != null) td.OverrideTime(simulatedTime);
        }

        // Update all connected DateDisplays
        DateDisplay[] dateDisplays = FindObjectsOfType<DateDisplay>();
        foreach (var dd in dateDisplays)
        {
            if (dd != null) dd.OverrideDate(simulatedTime);
        }
    }

    public void PauseResume()
    {
        isPaused = !isPaused;
    }

    public void ResetToRealTime()
    {
        simulatedTime = DateTime.Now;
        simulatedStartTime = simulatedTime;
    }

    public void SetTimeRate(float rate)
    {
        timeRate = rate;
    }
}