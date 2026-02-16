using UnityEngine;
using TMPro;
using System;

public class TimeDisplay : MonoBehaviour
{
    private TextMeshProUGUI buttonText;

    // UTC at simulation start
    private DateTime startUtc;

    // Real system time at simulation start
    private DateTime startSystemTime;

    // Timer to update once per second
    private float timer = 0f;

    void Awake()
    {
        // Get the TMP_Text from child automatically
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        // Capture simulation start UTC
        startUtc = DateTime.UtcNow;

        // Capture real system time at launch
        startSystemTime = DateTime.Now;

        // Display initial time immediately
        UpdateTimeDisplay();
    }

    void Update()
    {
        // Accumulate unscaled delta time (ignores Time.timeScale)
        timer += Time.unscaledDeltaTime;

        // Only update once per second
        if (timer < 1f) return;

        timer = 0f;

        // Calculate elapsed real-world time
        TimeSpan elapsed = DateTime.Now - startSystemTime;

        // Advance simulated UTC
        DateTime simulatedUtc = startUtc.AddSeconds(elapsed.TotalSeconds);

        // Convert to local time
        DateTime localTime = simulatedUtc.ToLocalTime();

        // Display in 12-hour format with uppercase AM/PM
        buttonText.text = localTime.ToString("hh:mm:ss tt").ToUpper();
    }

    // Optional: call this if you want to force update immediately
    public void UpdateTimeDisplay()
    {
        TimeSpan elapsed = DateTime.Now - startSystemTime;
        DateTime simulatedUtc = startUtc.AddSeconds(elapsed.TotalSeconds);
        DateTime localTime = simulatedUtc.ToLocalTime();
        buttonText.text = localTime.ToString("hh:mm:ss tt").ToUpper();
    }
}
