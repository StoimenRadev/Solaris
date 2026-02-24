using UnityEngine;
using TMPro;
using System;

public class TimeDisplay : MonoBehaviour
{
    private TextMeshProUGUI buttonText;
    private DateTime startUtc;
    private DateTime startSystemTime;
    private float timer = 0f;

    [Header("Options")]
    public bool showText = true;

    // Exposed values for other scripts
    public float DayOfYear { get; private set; }      // For orbit calculations
    public float FractionOfDay { get; private set; }  // 0..1

    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        startUtc = DateTime.UtcNow;
        startSystemTime = DateTime.Now;
        UpdateTimeValues();
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer < 1f) return; // Update once per second
        timer = 0f;

        UpdateTimeValues();
    }

    private void UpdateTimeValues()
    {
        // Calculate elapsed time since start
        TimeSpan elapsed = DateTime.Now - startSystemTime;

        // Simulated UTC time
        DateTime simulatedUtc = startUtc.AddSeconds(elapsed.TotalSeconds);

        // Convert to local time
        DateTime localTime = simulatedUtc.ToLocalTime();

        // Fraction of the day (0..1)
        FractionOfDay = (localTime.Hour + localTime.Minute / 60f + localTime.Second / 3600f) / 24f;

        // Full day of year including fraction
        DayOfYear = localTime.DayOfYear + FractionOfDay;

        // Update UI text if enabled
        if (showText && buttonText != null)
        {
            buttonText.text = localTime.ToString("HH:mm:ss");
        }
    }

    /// <summary>
    /// Overrides the current time with a new DateTime value.
    /// Updates orbit-related values and UI text if enabled.
    /// </summary>
    public void OverrideTime(DateTime newTime)
    {
        DateTime localTime = newTime.ToLocalTime();

        FractionOfDay = (localTime.Hour + localTime.Minute / 60f + localTime.Second / 3600f) / 24f;
        DayOfYear = localTime.DayOfYear + FractionOfDay;

        if (showText && buttonText != null)
            buttonText.text = localTime.ToString("HH:mm:ss");
    }

    /// <summary>
    /// Force update of time values externally.
    /// </summary>
    public void ForceUpdate()
    {
        UpdateTimeValues();
    }
}